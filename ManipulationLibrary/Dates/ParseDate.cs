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
using System.Workflow.ComponentModel;
using System.Workflow.Activities;
using Microsoft.Crm.Workflow;
using Microsoft.Crm.Sdk;

namespace ManipulationLibrary.Dates
{
    [CrmWorkflowActivity("Parse Date", "Date Utilities")]
    public partial class ParseDate : SequenceActivity
    {

        public static int GetWeek(DateTime date)
        {
            CultureInfo ciCurr = CultureInfo.CurrentCulture;
            return ciCurr.Calendar.GetWeekOfYear(date, CalendarWeekRule.FirstFourDayWeek, System.DayOfWeek.Monday);
        }

        protected override ActivityExecutionStatus Execute(ActivityExecutionContext executionContext)
        {
            var date = DateTime.Parse(Date.Value);

            Year = new CrmNumber(date.Year);
            Month = new CrmNumber(date.Month);
            Day = new CrmNumber(date.Day);
            Hour24 = new CrmNumber(date.Hour);
            
            if (date.Hour >= 12)
            {
                IsPM = new CrmBoolean(true);
                IsAM = new CrmBoolean(false); 
                Hour = new CrmNumber(date.Hour - 12);
            }
            else
            {
                IsPM = new CrmBoolean(false);
                IsAM = new CrmBoolean(true);
                Hour = new CrmNumber(date.Hour);
            }

            Minute = new CrmNumber(date.Minute);
            DayOfWeek = new CrmNumber((int)date.DayOfWeek);
            DayOfWeekString = date.DayOfWeek.ToString();
            DayOfYear = new CrmNumber(date.DayOfYear);
            Week = new CrmNumber(GetWeek(date));

            return ActivityExecutionStatus.Closed;
        }

        public static DependencyProperty DateProperty = DependencyProperty.Register("Date", typeof(CrmDateTime), typeof(ParseDate));
        [CrmInput("Date")]
        [CrmDefault("1900-01-01T00:00:00Z")]
        public CrmDateTime Date
        {
            get { return (CrmDateTime)GetValue(DateProperty); }
            set { SetValue(DateProperty, value); }
        }

        public static DependencyProperty DayOfWeekProperty = DependencyProperty.Register("DayOfWeek", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Day Of Week")]
        public CrmNumber DayOfWeek
        {
            get { return (CrmNumber)GetValue(DayOfWeekProperty); }
            set { SetValue(DayOfWeekProperty, value); }
        }

        public static DependencyProperty DayOfWeekStringProperty = DependencyProperty.Register("DayOfWeekString", typeof(String), typeof(ParseDate));
        [CrmOutput("Day Of Week (Text)")]
        public String DayOfWeekString
        {
            get { return (String)GetValue(DayOfWeekStringProperty); }
            set { SetValue(DayOfWeekStringProperty, value); }
        }

        public static DependencyProperty DayOfYearProperty = DependencyProperty.Register("DayOfYear", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Day Of Year")]
        public CrmNumber DayOfYear
        {
            get { return (CrmNumber)GetValue(DayOfYearProperty); }
            set { SetValue(DayOfYearProperty, value); }
        }

        public static DependencyProperty WeekProperty = DependencyProperty.Register("Week", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Week Number")]
        public CrmNumber Week
        {
            get { return (CrmNumber)GetValue(WeekProperty); }
            set { SetValue(WeekProperty, value); }
        }

        public static DependencyProperty YearProperty = DependencyProperty.Register("Year", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Year")]
        public CrmNumber Year
        {
            get { return (CrmNumber)GetValue(YearProperty); }
            set { SetValue(YearProperty, value); }
        }

        public static DependencyProperty MonthProperty = DependencyProperty.Register("Month", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Month")]
        public CrmNumber Month
        {
            get { return (CrmNumber)GetValue(MonthProperty); }
            set { SetValue(MonthProperty, value); }
        }

        public static DependencyProperty DayProperty = DependencyProperty.Register("Day", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Day")]
        public CrmNumber Day
        {
            get { return (CrmNumber)GetValue(DayProperty); }
            set { SetValue(DayProperty, value); }
        }

        public static DependencyProperty HourProperty = DependencyProperty.Register("Hour", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Hour (12-Hour-Clock)")]
        public CrmNumber Hour
        {
            get { return (CrmNumber)GetValue(HourProperty); }
            set { SetValue(HourProperty, value); }
        }

        public static DependencyProperty Hour24Property = DependencyProperty.Register("Hour24", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Hour (24-Hour-Clock)")]
        public CrmNumber Hour24
        {
            get { return (CrmNumber)GetValue(Hour24Property); }
            set { SetValue(Hour24Property, value); }
        }

        public static DependencyProperty MinuteProperty = DependencyProperty.Register("Minute", typeof(CrmNumber), typeof(ParseDate));
        [CrmOutput("Minute")]
        public CrmNumber Minute
        {
            get { return (CrmNumber)GetValue(MinuteProperty); }
            set { SetValue(MinuteProperty, value); }
        }

        public static DependencyProperty IsAMProperty = DependencyProperty.Register("IsAM", typeof(CrmBoolean), typeof(ParseDate));
        [CrmOutput("AM")]
        public CrmBoolean IsAM
        {
            get { return (CrmBoolean)GetValue(IsAMProperty); }
            set { SetValue(IsAMProperty, value); }
        }

        public static DependencyProperty IsPMProperty = DependencyProperty.Register("IsPM", typeof(CrmBoolean), typeof(ParseDate));
        [CrmOutput("PM")]
        public CrmBoolean IsPM
        {
            get { return (CrmBoolean)GetValue(IsPMProperty); }
            set { SetValue(IsPMProperty, value); }
        }
 
    }
}