using System;
using System.Collections.Generic;
using System.Text;

namespace ReadReceipt.Utility
{
    public static class FilterRules
    {
        public static bool IsReceitItem(string text)
        {
            var stext = text.Trim().ToLower();
            if (stext.StartsWith("%") == false)
                return true;
            stext = stext.Replace('-', ' ');
            stext = stext.Replace('*', ' ');
            stext = stext.Replace('+', ' ');
            stext = stext.Trim();
            return string.IsNullOrWhiteSpace(stext);
        }

        public static string TrimforValue(string valueText)
        {
            return valueText
                .Replace('X', ' ')
                .Replace('x', ' ')
                .Replace('*', ' ')
                .Replace('-', ' ')
                .Replace('+', ' ')
                .Trim();
        }
    }
}