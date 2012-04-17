using System;
using System.Activities;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Helpers
{
    public static class UserSettings
    {
        public static Entity GetUserSettings(CodeActivityContext executionContext)
        {
            var context = executionContext.GetExtension<IWorkflowContext>();
            var serviceFactory = executionContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);
            return GetUserSettings(service, context.UserId);
        }
        public static Entity GetUserSettings(IOrganizationService service, Guid userid)
        {
            var req = new RetrieveUserSettingsSystemUserRequest
            {
                ColumnSet = new Microsoft.Xrm.Sdk.Query.ColumnSet(true),
                EntityId = userid // user is the user object
            };

            var resp = (RetrieveUserSettingsSystemUserResponse)service.Execute(req);
            return resp == null ? null : resp.Entity;
        }
    }
}
