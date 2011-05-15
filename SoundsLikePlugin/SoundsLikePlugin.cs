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

        public void Execute(IServiceProvider serviceProvider)
        {
            //Extract the tracing service
            var tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            if (tracingService == null)
                throw new InvalidPluginExecutionException("Failed to retrieve the tracing service.");

            // Obtain the execution context from the service provider.
            var context = (IPluginExecutionContext) serviceProvider.GetService(typeof(IPluginExecutionContext));

            // Only process Create and Update messages
            if (context.MessageName != "Create" && context.MessageName != "Update") return;

            // Only process if there is a target entity
            if (!context.InputParameters.Contains("Target") || 
               (!(context.InputParameters["Target"] is Entity)))
            {
                return;
            }

            // Get the target entity
            var entity = (Entity) context.InputParameters["Target"];

            // Codify for each setting
            foreach (var setting in Configuration.Settings)
            {
                // Continue if the Target does not contain the field to encode, no need to re-encode
                // something that wasn't changed
                if (!entity.Contains(setting.Source))
                {
                    // If DisableUpdate is specified, then don't allow someone to manually 
                    // update the coded text
                    if (setting.DisableUpdate && entity.Contains(setting.Target))
                    {
                        entity.Attributes.Remove(setting.Target);
                    }
                    continue;
                }

                // Get the text to encode
                var text = entity.GetAttributeValue<string>(setting.Source);
                
                // Codify the string using the method specified
                string codifiedText;
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

                // Add or update the codified attribute);
                if (entity.Attributes.ContainsKey(setting.Target))
                {
                    entity.Attributes[setting.Target] = codifiedText;
                } else
                {
                    entity.Attributes.Add(setting.Target, codifiedText);
                }
            }
        }
    }
}