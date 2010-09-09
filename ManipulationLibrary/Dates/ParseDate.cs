// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 4.0
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
using Microsoft.Xrm.Sdk.Workflow;
using Microsoft.Xrm.Sdk;

namespace ManipulationLibrary.Dates
{
    [WorkflowActivity("Parse Date", "Date Utilities")]
    public sealed class ParseDate : CodeActivity
    {

        public static int GetWeek(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
        }

        protected override void Execute(CodeActivityContext executionContext)
        {
            var am = false;
            var date = Date.Get<DateTime>(executionContext);
            int hour = date.Hour;
            
            Year.Set(executionContext, date.Year);
            Month.Set(executionContext, date.Month);
            Day.Set(executionContext, date.Day);
            Hour24.Set(executionContext, date.Hour);
            
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
            DayOfWeekString.Set(executionContext,date.DayOfWeek.ToString());
            DayOfYear.Set(executionContext, date.DayOfYear);
            Week.Set(executionContext, GetWeek(date));
        }

        [Input("Date")]
        [Default("1900-01-01T00:00:00Z")]
        public InArgument<DateTime> Date { get; set; }
        
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

        [Output("Month")]
        public OutArgument<int> Month { get; set; }

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