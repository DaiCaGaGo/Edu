using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CMS.XuLy
{
    public static class Common
    {
        public static string NullIfWhiteSpace(this string value)
        {
            if (String.IsNullOrWhiteSpace(value)) { return null; }
            return value;
        }
        public static double ConvertToDouble(this string value)
        {
            if (value == null)
            {
                return 0;
            }
            else
            {
                double OutVal;
                double.TryParse(value, out OutVal);

                if (double.IsNaN(OutVal) || double.IsInfinity(OutVal))
                {
                    return 0;
                }
                return OutVal;
            }
        }

        /// <summary>
        /// to normalize lower
        /// </summary>
        public static string ToNormalizeLower(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToLower().Normalize(NormalizationForm.FormKD).Replace("  ", " ");
            return value;
        }

        /// <summary>
        /// to normalize lower replace 2 space by 1 space
        /// </summary>
        public static string ToNormalizeLowerRelace(this string value)
        {
            if (!string.IsNullOrEmpty(value))
                return value.Trim().ToLower().Normalize(NormalizationForm.FormKD).Replace("  ", " ");
            return value;
        }
    }
}