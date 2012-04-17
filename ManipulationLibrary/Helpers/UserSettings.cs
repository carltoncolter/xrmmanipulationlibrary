// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		UserSettings.cs
//  Summary:	This helper gets the user settings of a user running a workflow.
//  License:    MsPL - Microsoft Public License
// ==================================================================================

using System;
using System.Activities;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
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
            return service.Retrieve("usersettings", userid, new ColumnSet(true));
        }
    }
}
