// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		ConvertToUTC.cs
//  Summary:	This workflow activity converts a local datetime to UTC date time
//  License:    MsPL - Microsoft Public License
// ==================================================================================
using System;
using System.Activities;
using ManipulationLibrary.Helpers;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Dates
{
    public sealed class ConvertToUTC : CodeActivity
    {
        protected override void Execute(CodeActivityContext eContext)
        {
            // Setup
            var context = eContext.GetExtension<IWorkflowContext>();
            var serviceFactory = eContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(context.UserId);


            var codeString = TimeZoneCodeString.Get(eContext);
            int code;

            if (String.IsNullOrWhiteSpace(codeString) ||
                !Int32.TryParse(codeString, out code))
            {
                var settings = UserSettings.GetUserSettings(service, context.UserId);
                code = (int)settings.Attributes["timezonecode"];
            }
            var req = new UtcTimeFromLocalTimeRequest {TimeZoneCode = code, LocalTime = LocalDateTime.Get(eContext)};

            var resp = (UtcTimeFromLocalTimeResponse) service.Execute(req);
            if (resp == null) return;
            UTCDateTime.Set(eContext,resp.UtcTime);
        }

        [Input("Time Zone Code (Leave Blank to use User's Time Zone)")]
        public InArgument<string> TimeZoneCodeString { get; set; }

        [Input("Local DateTime")]
        [Default("1950-01-01T00:00:00Z")]
        public InArgument<DateTime> LocalDateTime { get; set; }

        [Output("UTC DateTime")]
        public OutArgument<DateTime> UTCDateTime { get; set; }
    }
}
