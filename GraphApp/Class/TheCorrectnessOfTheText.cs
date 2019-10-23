﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GraphApp.Class
{
    class TheCorrectnessOfTheText
    {
        /// <summary>
        /// check that the text entered is correct
        /// </summary>

        private static readonly Regex _regex = new Regex("[^0-9.]+"); //regex that matches disallowed text
        public static async Task<bool> IsTextAllowed(string text) => !_regex.IsMatch(text);

        public static async Task<bool> IsAValueIntheRange(double min, double max, string text)
        {
            if (!double.TryParse(text, out double result))
                return false;
            if (min <= result && result <= max)
                return true;
            else
                return false;
        }
    }
}
