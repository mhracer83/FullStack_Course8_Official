using System.Text.RegularExpressions;

namespace SafeVault.Shared
{
    public static class InputSanitizer
    {
        public static string SanitizeInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            // Step 1: Remove unsafe characters
            input = Regex.Replace(input, @"[<>""';]", string.Empty);

            // Step 2: Remove SQL injection patterns
            // Remove single-line (--) and multi-line (/* */) comments
            input = Regex.Replace(input, @"--|/\*.*?\*/", " ", RegexOptions.Singleline);

            // Remove malicious characters: quotes (') and semicolons (;)
            input = input.Replace("'", string.Empty).Replace(";", string.Empty);

            bool hasLeadingSpace = input.StartsWith(" ");
            bool hasTrailingSpace = input.EndsWith(" ");

            // Step 3: Normalize spaces (replace multiple spaces with a single space)
            input = Regex.Replace(input, @"\s+", " ").Trim();

            if (hasLeadingSpace)
                input = " " + input;
            if (hasTrailingSpace)
                input = input + " ";

            return input;
        }
    }
}
