// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		GetUserSettings.cs
//  Summary:	This workflow activity gets the user settings
//  License:    MsPL - Microsoft Public License
// ==================================================================================

using System;
using System.Activities;
using System.Globalization;
using ManipulationLibrary.Helpers;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Crm
{
    public sealed class GetUserSettings : CodeActivity
    {
        protected override void Execute(CodeActivityContext eContext)
        {
            // Read Variables
            int precision = DecimalPrecision.Get(eContext);

            // Setup
            var context = eContext.GetExtension<IWorkflowContext>();
            var serviceFactory = eContext.GetExtension<IOrganizationServiceFactory>();
            var service = serviceFactory.CreateOrganizationService(null);
            ITracingService tracer = eContext.GetExtension<ITracingService>();

        
            tracer.Trace("Retrieving User Settings");
            // Read Settings
            var settings = UserSettings.GetUserSettings(service, context.UserId);
            tracer.Trace("User Settings Retrieved");


            tracer.Trace("Retrieving User Setting Attributes");
            var uilang = GetEntityValue<int>(tracer, settings, "uilanguageid");
            var currencyPrecision = GetEntityValue<int>(tracer, settings, "currencydecimalprecision");
            var currencyFormatCode = GetEntityValue<int>(tracer, settings, "currencyformatcode");
            var currencySymbol = GetEntityValue<string>(tracer, settings, "currencysymbol");
            var numberGroupFormat = GetEntityValue<string>(tracer, settings, "numbergroupformat");
            var negativeCurrencyFormatCode = GetEntityValue<int>(tracer, settings, "negativecurrencyformatcode");
            var negativeFormatCode = GetEntityValue<int>(tracer, settings, "negativeformatcode");
            var decimalSymbol = GetEntityValue<string>(tracer, settings, "decimalsymbol");
            var numberSeparator = GetEntityValue<string>(tracer, settings, "numberseparator");
            var businessUnitId = GetEntityValue<Guid>(tracer, settings, "businessunitid");
            var timeFormatString = GetEntityValue<string>(tracer, settings, "timeformatstring");
            var dateFormatString = GetEntityValue<string>(tracer, settings, "dateformatstring");
            var timeZoneCode = GetEntityValue<int>(tracer, settings, "timezonecode");
            var localeid = GetEntityValue<int>(tracer, settings, "localeid");

            // Calculate Fields
            tracer.Trace("Calculating Fields");
            var uiCulture = new CultureInfo(uilang);

            var currencyFormat = NumberFormat.GetFormatString(decimalSymbol, numberSeparator,
                                                              currencyPrecision, numberGroupFormat,
                                                              negativeCurrencyFormatCode, true,
                                                              currencySymbol,
                                                              currencyFormatCode);

            var numberFormat = NumberFormat.GetFormatString(decimalSymbol, numberSeparator,
                                                            precision, numberGroupFormat,
                                                            negativeFormatCode);

            var businessUnit = new EntityReference("businessunit", businessUnitId);

            // Store Results
            tracer.Trace("Storing Results");
            Locale.Set(eContext,localeid);
            BusinessUnit.Set(eContext, businessUnit);
            CurrencySymbol.Set(eContext, currencySymbol);
            CurrencyFormatString.Set(eContext, currencyFormat);
            NumberFormatString.Set(eContext, numberFormat);
            DateFormatString.Set(eContext, dateFormatString);
            TimeFormatString.Set(eContext, timeFormatString);
            TimeZoneCode.Set(eContext,timeZoneCode);
            UILanguageCode.Set(eContext, uilang);
            UILanguageName.Set(eContext, uiCulture.DisplayName);
            UILanguageEnglishName.Set(eContext, uiCulture.EnglishName);
        }

        public T GetEntityValue<T>(ITracingService tracer, Entity entity, string fieldname)
        {
            tracer.Trace("Retrieving field {0}",fieldname);
            return (T) entity[fieldname];
        }


        [Output("Business Unit")]
        [ReferenceTarget("businessunit")]
        public OutArgument<EntityReference> BusinessUnit { get; set; }

        [Output("Currency Symbol")]
        public OutArgument<string> CurrencySymbol { get; set; }

        [Output("Currency Format String")]
        public OutArgument<string> CurrencyFormatString { get; set; }

        [Output("Number Format String")]
        public OutArgument<string> NumberFormatString { get; set; }

        [Output("Time Format String")]
        public OutArgument<string> TimeFormatString { get; set; }

        [Output("Time Zone Code")]
        public OutArgument<int> TimeZoneCode { get; set; }

        [Output("Locale ID")]
        public OutArgument<int> Locale { get; set; }

        [Output("Date Format String")]
        public OutArgument<string> DateFormatString { get; set; }

        [Output("UI Language Code")]
        public OutArgument<int> UILanguageCode { get; set; }

        [Output("UI Language Name")]
        public OutArgument<string> UILanguageName { get; set; }

        [Output("UI Language Name in English")]
        public OutArgument<string> UILanguageEnglishName { get; set; }

        [Input("Decimal Precision")]
        [Default("2")]
        public InArgument<int> DecimalPrecision { get; set; }


    }
}
