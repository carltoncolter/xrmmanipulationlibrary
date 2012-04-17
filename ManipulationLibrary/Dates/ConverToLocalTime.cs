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
    public sealed class ConvertToLocalTime : CodeActivity
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
            var req = new LocalTimeFromUtcTimeRequest {TimeZoneCode = code, UtcTime = UTCDateTime.Get(eContext)};

            var resp = (LocalTimeFromUtcTimeResponse)service.Execute(req);
            if (resp == null) return;
            LocalDateTime.Set(eContext, resp.LocalTime);
        }

        [Input("Time Zone Code (Leave Blank to use User's Time Zone)")]
        public InArgument<string> TimeZoneCodeString { get; set; }

        [Input("UTC DateTime")]
        [Default("1950-01-01T00:00:00Z")]
        public InArgument<DateTime> UTCDateTime { get; set; }

        [Output("Local DateTime")]
        public OutArgument<DateTime> LocalDateTime { get; set; }
    }
}
