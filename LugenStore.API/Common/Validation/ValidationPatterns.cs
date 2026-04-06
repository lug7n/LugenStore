using System.Text.RegularExpressions;

namespace LugenStore.API.Common.Validation;

public static class ValidationPatterns
{
    public static readonly Regex NameRegex = new Regex(
        @"^[\p{L}\p{N}]+(?:[ \-'\(\):&][\p{L}\p{N}]+)*$", 
        RegexOptions.Compiled
    );
}
