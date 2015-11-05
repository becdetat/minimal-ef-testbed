using System;
using System.Linq;

namespace MinimalEF
{
    public static partial class StringExtensions
    {
        public static bool IsRoughly(this string input, params string[] matches)
        {
            return matches.Any(match => input.Trim().Equals(match.Trim(), StringComparison.InvariantCultureIgnoreCase));
        }
    }
}