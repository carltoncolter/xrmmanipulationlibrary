using System;
using Microsoft.Xrm.Sdk;

namespace SoundsLike
{
    public class SoundsLikePlugin : IPlugin
    {
        public Config Configuration;

        public SoundsLikePlugin()
        { }

        public SoundsLikePlugin(string unsecureConfig, string secureConfig)
        {
            var configString = String.IsNullOrEmpty(unsecureConfig) ? secureConfig : unsecureConfig;
            Configuration = Config.Deserialize(configString);
        }

        private string GetString(ITracingService tracer, Entity primary, Entity secondary, string field, string prefix = "")
        {
            if (field == null || field.Length < 1)
            {
                return string.Empty;
            }
            tracer.Trace("Getting field {0}", field);
            if (primary.Attributes.ContainsKey(field))
            {
                return String.Format("{0}{1}", prefix, primary.GetAttributeValue<string>(field));
            }
            if (secondary.Attributes.ContainsKey(field))
            {
                return String.Format("{0}{1}", prefix, secondary.GetAttributeValue<string>(field));
            }
            return string.Empty;
        }

        public void Execute(IServiceProvider serviceProvider)
        {
            try
            {
                //Extract the tracing service
                var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
                if (tracingService == null)
                    throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

                // Obtain the execution context from the service provider.
                var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

                // Only process Create and Update messages
                if (context.MessageName != "Create" && context.MessageName != "Update")
                {
                    return;
                }
                tracingService.Trace("SoundsLikePlugin: Create or Update");
                

                // Only process if there is a target entity
                Entity pre = null;
                if (context.PreEntityImages.ContainsKey("pre"))
                {
                    pre = context.PreEntityImages["pre"];
                }

                if (!context.InputParameters.Contains("Target") ||
                   (!(context.InputParameters["Target"] is Entity)))
                {
                    tracingService.Trace("SoundsLikePlugin: No Target");
                    return;
                }
                else
                {
                    tracingService.Trace("SoundsLikePlugin: Target Found");
                }

                Entity entity = (Entity)context.InputParameters["Target"];



                // Codify for each setting
                foreach (var setting in Configuration.Settings)
                {
                    // Continue if the Target does not contain the field to encode, no need to re-encode
                    // something that wasn't changed
                    if (entity.Contains(setting.Target))
                    {
                        // If DisableUpdate is specified, then don't allow someone to manually 
                        // update the coded text
                        if (setting.DisableUpdate && entity.Contains(setting.Target))
                        {
                            entity.Attributes.Remove(setting.Target);
                        }
                    }

                    // Get the text to encode
                    tracingService.Trace("SoundsLikePlugin: Getting Text for Sources");
                    string text = String.Format("{0}{1}{2}",
                                    GetString(tracingService, entity, pre, setting.Source),
                                    GetString(tracingService, entity, pre, setting.Source2),
                                    GetString(tracingService, entity, pre, setting.Source3));
                    tracingService.Trace("SoundsLikePlugin: Source Text: {0}", text);

                    // Codify the string using the method specified
                    string codifiedText;

                    tracingService.Trace("SoundsLikePlugin: Codifying Text: {0}", text);
                    switch (setting.Method)
                    {
                        case CodificationMethod.Soundex:
                            codifiedText = Soundex.Codify(text, setting.MinLength, setting.MaxLength, false);
                            break;
                        case CodificationMethod.SoundexOriginal:
                            codifiedText = Soundex.Codify(text, setting.MinLength, setting.MaxLength, true);
                            break;
                        case CodificationMethod.Metaphone:
                            codifiedText = Metaphone.Codify(text, setting.MinLength, setting.MaxLength);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    tracingService.Trace("SoundsLikePlugin: Codified To: {0}", codifiedText);

                    // Add or update the codified attribute);
                    if (entity.Attributes.ContainsKey(setting.Target))
                    {
                        tracingService.Trace("SoundsLikePlugin: Setting Target Field: {0}", setting.Target);
                        entity.Attributes[setting.Target] = codifiedText;
                    }
                    else
                    {
                        tracingService.Trace("SoundsLikePlugin: Adding Target Field: {0}", setting.Target);
                        entity.Attributes.Add(setting.Target, codifiedText);
                    }

                    //throw new Exception("doh");
                }

            }
            catch (Exception ex)
            {
                throw new InvalidPluginExecutionException("SoundsLikePlugin Failed", ex);
            }
        }
    }
}