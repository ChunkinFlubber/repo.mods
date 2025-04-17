using System;
using System.Collections.Generic;
using System.Text;

namespace PostLevelSummaryPlus.Helpers
{
    public static class NumberFormatter
    {
        public static string FormatToK(float value)
        {
            if (value >= 1000)
            {
                return $"{value / 1000:0.#}k";
            }

            return value.ToString("0");
        }
    }
}
