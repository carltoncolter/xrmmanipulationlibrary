// ==================================================================================
//  Project:	Manipulation Library for Microsoft Dynamics CRM 2011
//  File:		NumberFormat.cs
//  Summary:	This helper is a re-implementation of fn_GetNumberFormatString SQL 
//              function used by CRM's Filtered Views
//  License:    MsPL - Microsoft Public License
// ==================================================================================

using System;

namespace ManipulationLibrary.Helpers
{
    public static class NumberFormat
    {
        public static string GetFormatString(string decimalSymbol, string numberSeparator, int precision, string numberGroupFormat, int negativeFormatCode, bool isCurrency = false, string currencySymbol = "", int currencyformatcode = 0)
        {
            var precisionString = GetPrecisionString(decimalSymbol, precision);
            var zeroNumberFormat = "0" + precisionString;
            var positiveNumberFormat = GetPositiveNumberFormat(numberSeparator, numberGroupFormat, precisionString);
            
            string negativeNumberFormat;
            if (isCurrency)
            {
                negativeNumberFormat = GetNegativeNumberFormat(negativeFormatCode, positiveNumberFormat, currencySymbol);
                positiveNumberFormat = UpdateNumberFormatForCurrency(currencyformatcode, currencySymbol, positiveNumberFormat);
                zeroNumberFormat = UpdateNumberFormatForCurrency(currencyformatcode, currencySymbol, zeroNumberFormat);
            }
            else
            {
                negativeNumberFormat = GetNegativeNumberFormat(negativeFormatCode, positiveNumberFormat);
            }

            return positiveNumberFormat + ";" + negativeNumberFormat + ";" + zeroNumberFormat;
        }

        private static string UpdateNumberFormatForCurrency(int currencyformatcode, string currencySymbol, string numberFormat)
        {
            switch (currencyformatcode)
            {
                case 0:
                    return "\"" + currencySymbol + "\"" + (char)8203 + numberFormat;
                case 1:
                    return numberFormat + (char)8203 + "\"" + currencySymbol + "\"";
                case 2:
                    return "\"" + currencySymbol + "\"" + (char)160 + numberFormat;
                case 3:
                    return numberFormat + (char)160 + "\"" + currencySymbol + "\"";
                default:
                    return "\"" + currencySymbol + "\"" + numberFormat;
            }
        }

        private static string GetNegativeNumberFormat(int negativeFormatCode, string positiveNumberFormat, string currencySymbol = null)
        {
            if (String.IsNullOrWhiteSpace(currencySymbol))
            {
                switch (negativeFormatCode)
                {
                    case 0:
                        return "(" + positiveNumberFormat + ")";
                    case 1:
                        return "-" + positiveNumberFormat;
                    case 2:
                        return "-" + (char)160 + positiveNumberFormat;
                    case 3:
                        return positiveNumberFormat + "-";
                    case 4:
                        return positiveNumberFormat + (char)160 + "-";
                    default:
                        return "(" + positiveNumberFormat + ")";
                }
            }

            switch (negativeFormatCode)
            {
                case 0:
                    return "(\"" + currencySymbol + "\"" + (char)8203 + positiveNumberFormat + ")";
                case 1:
                    return "-\"" + currencySymbol + "\"" + (char)8203 + positiveNumberFormat;
                case 2:
                    return "\"" + currencySymbol + "\"-" + positiveNumberFormat;
                case 3:
                    return "\"" + currencySymbol + "\"" + (char)8203 + positiveNumberFormat + "-";
                case 4:
                    return "(" + positiveNumberFormat + (char)8203 + "\"" + currencySymbol + "\")";
                case 5:
                    return "-" + positiveNumberFormat + (char)8203 + "\"" + currencySymbol + "\"";
                case 6:
                    return positiveNumberFormat + "-\"" + currencySymbol + "\"";
                case 7:
                    return positiveNumberFormat + (char)8203 + "\"" + currencySymbol + "\"-";
                case 8:
                    return "-" + positiveNumberFormat + (char)160 + "\"" + currencySymbol + "\"";
                case 9:
                    return "-\"" + currencySymbol + "\"" + (char)160 + positiveNumberFormat;
                case 10:
                    return positiveNumberFormat + (char)160 + "\"" + currencySymbol + "\"-";
                case 11:
                    return "\"" + currencySymbol + "\"" + (char)16 + positiveNumberFormat + "-";
                case 12:
                    return "\"" + currencySymbol + "\" -" + positiveNumberFormat;
                case 13:
                    return positiveNumberFormat + "- \"" + currencySymbol + "\"";
                case 14:
                    return "(\"" + currencySymbol + "\"" + (char)160 + positiveNumberFormat + ")";
                case 15:
                    return "(" + positiveNumberFormat + (char)160 + "\"" + currencySymbol + "\")";
                default:
                    return "(\"" + currencySymbol + "\"" + (char)8203 + positiveNumberFormat + ")";
            }
        }

        private static string GetPositiveNumberFormat(string separator, string numberGroupFormat, string precisionString)
        {
            string format;
            switch (numberGroupFormat)
            {
                case "3,0":
                    format = "##########{0}##0{1}";
                    break;
                case "3,2":
                    format = "##{0}##{0}##{0}##{0}##{0}##0{1}";
                    break;
                default:
                    format = "###{0}###{0}###{0}##0{1}";
                    break;
            }

            return String.Format(format, separator, precisionString);
        }

        private static string GetPrecisionString(string decimalSymbol, int precision)
        {
            var precisionString = String.Empty;
            if (precision > 0)
            {
                precisionString = decimalSymbol;
                precisionString.PadRight(precision, '0');
            }
            return precisionString;
        }
    }
}
