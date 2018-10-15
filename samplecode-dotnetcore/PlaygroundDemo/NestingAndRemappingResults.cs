using CSharpFunctionalExtensions;

namespace PlaygroundDemo
{
    internal static class NestingAndRemappingResults
    {
        // "foo", "bar" -> "foo"
        public static Result<string> CheckBothAndReturnFirst(string first, string second)
        {
            var foo = StringValidator.Validate(first)
                .OnSuccess(x => StringValidator.Validate(second).OnSuccess(y => x));
            return foo;
        }

        // "foo", "bar" -> "bar"
        public static Result<string> CheckBothAndReturnSecond(string first, string second)
        {
            var foo = StringValidator.Validate(first)
                .OnSuccess(x => StringValidator.Validate(second).OnSuccess(y => y));
            return foo;
        }
    }
}