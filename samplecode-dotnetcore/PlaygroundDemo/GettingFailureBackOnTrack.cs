using CSharpFunctionalExtensions;

namespace PlaygroundDemo
{
    /// <summary>
    ///     This is NOT allowed in Railway Oriented Programming
    /// </summary>
    internal static class GettingFailureBackOnTrack
    {
        public static Result<string> BackOnTrack(string s1)
        {
            var result = Result.Ok(s1)
                .OnSuccess(x => StringValidator.Validate(s1).OnFailure(y => Result.Ok("a")))
                .OnSuccess(x => x);
            return result;
        }
    }
}