using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;

namespace PlaygroundDemo
{
    /// <summary>
    ///     Combining multiple rules: Make sure the return type is "Result"...
    /// </summary>
    internal static class StringValidator
    {
        public static Result<string> Validate(string s)
        {
            var errors = new List<string>();

            if (s.StartsWith("a")) errors.Add("a is illegal as first letter");

            if (s.EndsWith("a")) errors.Add("a is illegal as last letter");

            return errors.Any()
                ? Result.Fail<string>(errors.Aggregate((x, y) => x + "," + y))
                : Result.Ok(s);
        }
    }
}