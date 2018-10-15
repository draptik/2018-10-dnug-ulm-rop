using CSharpFunctionalExtensions;

namespace PlaygroundDemo
{
    internal static class EnsuringResults
    {
        public static Result<string> CheckFirstStringContainsBlaAndBothStringsAreValid(string s1, string s2)
        {
            var result = StringValidator.Validate(s1)
                .Ensure(x => x.Contains("bla"), "must contain 'bla'")
                .OnSuccess(x => StringValidator.Validate(s2).OnSuccess(y => x));
            return result;
        }
    }
}