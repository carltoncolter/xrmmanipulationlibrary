// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		ParseDate.cs
//  License:    MsPL - Microsoft Public License
//  Summary:	This workflow activity parses a date and was written by Carlton Colter.
//     Following Engage's idea of adding Date Utilities to the Manipulation Library,
//     I thought it may be a good idea if you could parse a date and get some of the
//     stranger values such as week number, day of year, day of the week, etc.
//
//  Todo:       Allow a format string or something to be entered as a parameter
//     to specify how the date is to be formated and then format it following those
//     requirements.
// ==================================================================================
using System;
using System.Globalization;
using System.Activities;
using ManipulationLibrary.Helpers;
using Microsoft.Xrm.Sdk.Workflow;

namespace ManipulationLibrary.Dates
{
    public sealed class ParseDate : CodeActivity
    {

        public static int GetWeek(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
        }

        private static CultureInfo GetCultureInfo(CodeActivityContext executionContext, int languageCode)
        {
            if (languageCode>0) return new CultureInfo(languageCode);
            var settings = UserSettings.GetUserSettings(executionContext);
            var uilang = (int) settings["uilanguageid"];
            return new CultureInfo(uilang);
        }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var dateString = DateString.Get<string>(executionContext);
            var dateFormat = DateFormat.Get<string>(executionContext);
            DateTime date;

            if (!String.IsNullOrWhiteSpace(dateString))
            {
                var provider = CultureInfo.InvariantCulture;

                if (!DateTime.TryParseExact(dateString, dateFormat, provider, DateTimeStyles.None, out date))
                {
                    date = Date.Get<DateTime>(executionContext);
                }
            }
            else
            {
                date = Date.Get<DateTime>(executionContext);
            }

            bool am;
            
            var hour = date.Hour;
            
            Year.Set(executionContext, date.Year);
            YearText.Set(executionContext, date.Year.ToString());
            Month.Set(executionContext, date.Month);
            Day.Set(executionContext, date.Day);
            Hour24.Set(executionContext, date.Hour);

            var cultureInfo = GetCultureInfo(executionContext, LanguageCode.Get<int>(executionContext));
            MonthText.Set(executionContext, date.ToString("MMMM", cultureInfo.DateTimeFormat));
            DayOfWeekString.Set(executionContext, date.ToString("dddd", cultureInfo.DateTimeFormat));


            if (date.Hour >= 12)
            {
                am = false;
                hour = date.Hour - 12;
            }
            else
            {
                am = true;
            }

            Hour.Set(executionContext, hour);
            IsAM.Set(executionContext, am);
            IsPM.Set(executionContext, !am);
            Minute.Set(executionContext, date.Minute);
            
            DayOfWeek.Set(executionContext, (int)date.DayOfWeek);
            DayOfYear.Set(executionContext, date.DayOfYear);
            Week.Set(executionContext, GetWeek(date));
        }

        [Input("Date")]
        [Default("1900-01-01T00:00:00Z")]
        public InArgument<DateTime> Date { get; set; }

        [Input("Language Code")]
        [Default("-1")]
        public InArgument<int> LanguageCode { get; set; }

        [Input("Date Format")]
        [Default("MM/dd/yyyy")]
        public InArgument<string> DateFormat { get; set; }

        [Input("Date String (Overrides Date)")]
        [Default("")]
        public InArgument<string> DateString { get; set; }

        [Output("Day Of Week")]
        public OutArgument<int> DayOfWeek { get; set; }
        
        [Output("Day Of Week (Text)")]
        public OutArgument<string> DayOfWeekString { get; set; }
        
        [Output("Day Of Year")]
        public OutArgument<int> DayOfYear { get; set; }

        [Output("Week Number")]
        public OutArgument<int> Week { get; set; }

        [Output("Year")]
        public OutArgument<int> Year { get; set; }

        [Output("Year (Text)")]
        public OutArgument<string> YearText { get; set; }

        [Output("Month")]
        public OutArgument<int> Month { get; set; }

        [Output("Month (Text)")]
        public OutArgument<string> MonthText { get; set; }

        [Output("Day")]
        public OutArgument<int> Day { get; set; }

        [Output("Hour (12-Hour-Clock)")]
        public OutArgument<int> Hour { get; set; }

        [Output("Hour (24-Hour-Clock)")]
        public OutArgument<int> Hour24 { get; set; }

        [Output("Minute")]
        public OutArgument<int> Minute { get; set; }

        [Output("AM")]
        public OutArgument<bool> IsAM { get; set; }

        [Output("PM")]
        public OutArgument<bool> IsPM { get; set; }
 
    }
}