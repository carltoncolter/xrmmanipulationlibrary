// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 9.0
//  File:		EntityInfo.cs
//  Summary:	This workflow activity gets the id and logical name of the context 
//              entity as well as builds a url to reference the entity
//  License:    MsPL - Microsoft Public License
// ==================================================================================
using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Crm
{
    public sealed class EntityInfo : CodeActivity
    {
        public const string URLFormat = @"{0}/main.aspx?etn={1}&pagetype=entityrecord&id=%7B{2}%7D";

        protected override void Execute(CodeActivityContext eContext)
        {
            var lookup = LookupAttribute.Get(eContext);
            var orgurl = OrgUrl.Get(eContext);
            var context = eContext.GetExtension<IWorkflowContext>();
            var serviceFactory = eContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var reference = GetReference(service, lookup, new EntityReference(context.PrimaryEntityName, context.PrimaryEntityId));
            
            // Update Reference Information
            if (reference == null) return;
            ID.Set(eContext, reference.Id.ToString());
            LogicalName.Set(eContext, reference.LogicalName);

            // Update URL Information
            if (string.IsNullOrWhiteSpace(orgurl)) return;

            var url = CreateUrl(orgurl, reference);
            URL.Set(eContext, url);
            HtmlLink.Set(eContext, CreateHtmlLink(service, reference, url, LinkText.Get(eContext)));
        }

        private static string CreateUrl(string orgurl, EntityReference reference)
        {
            return String.Format(URLFormat, orgurl, reference.LogicalName, reference.Id);
        }

        private static string CreateHtmlLink(IOrganizationService service, EntityReference reference, string url, string linktext)
        {
            if (string.IsNullOrWhiteSpace(linktext))
            {
                linktext = GetReferenceName(service, reference);
                if (string.IsNullOrWhiteSpace(linktext))
                {
                    return string.Empty;
                }
            }

            return String.Format(@"<a href='{0}'>{1}</a>", url, linktext);
        }

        private static string GetReferenceName(IOrganizationService service, EntityReference reference)
        {
            if (String.IsNullOrWhiteSpace(reference.Name))
            {
                var attr = GetPrimaryAttribute(service, reference);
                if (String.IsNullOrWhiteSpace(attr)) return null;
                var entity = service.Retrieve(reference.LogicalName, reference.Id, new ColumnSet(attr));
                if (entity == null) return null;
                return entity[attr].ToString();
            }
            return reference.Name;
        }

        private static string GetPrimaryAttribute(IOrganizationService service, EntityReference reference)
        {
            var req = new RetrieveEntityRequest
                          {
                              LogicalName = reference.LogicalName,
                          };
            var resp = (RetrieveEntityResponse)service.Execute(req);
            return resp == null ? null : resp.EntityMetadata.PrimaryNameAttribute;
        }

        private static EntityReference GetReference(IOrganizationService service, string lookup, EntityReference @default)
        {
            if (string.IsNullOrWhiteSpace(lookup))
            {
                return @default;
            }

            var entity = service.Retrieve(@default.LogicalName, @default.Id, new ColumnSet(lookup));
            if (entity == null) return null;
            return (EntityReference)entity[lookup];
        }

        [Output("ID")]
        public OutArgument<string> ID { get; set; }

        [Output("LogicalName")]
        public OutArgument<string> LogicalName { get; set; }

        [Output("URL")]
        public OutArgument<string> URL { get; set; }

        [Output("HtmlLink")]
        public OutArgument<string> HtmlLink { get; set; }

        [Input("Lookup Attribute Name (Leave Blank for This Entity)")]
        public InArgument<string> LookupAttribute { get; set; }

        [Input("URL of the CRM Organization (e.g. 'https://myorg.crm.dynamics.com' or 'http://server/org'")]
        public InArgument<string> OrgUrl { get; set; }

        [Input("Link Text (Default is the entity's primary field)")]
        public InArgument<string> LinkText { get; set; }
    }
}