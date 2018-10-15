using CSharpFunctionalExtensions;
using FluentAssertions;
using Xunit;
using static PlaygroundDemo.EnsuringResults;
using static PlaygroundDemo.GettingFailureBackOnTrack;
using static PlaygroundDemo.NestingAndRemappingResults;

namespace PlaygroundDemo
{
    public class Tests
    {
        [Fact]
        public void CheckBothAndReturnFirst_return_first_when_valid_should_work() 
            => CheckBothAndReturnFirst("x", "b").Value.Should().Be("x");

        [Fact]
        public void CheckBothAndReturnSecond_return_second_when_valid_should_work() 
            => CheckBothAndReturnSecond("x", "b").Value.Should().Be("b");

        [Theory]
        [InlineData("blub", "b", "must contain")]
        [InlineData("bla", "b", "illegal as last letter")]
        [InlineData("abla", "b", "illegal as first letter")]
        [InlineData("blax", "b", "blax")]
        public void CheckMultipleConditions_should_work(string s1, string s2, string expected)
        {
            var result = CheckFirstStringContainsBlaAndBothStringsAreValid(s1, s2)
                .OnBoth(x => x.IsSuccess ? x.Value : x.Error);

            result.Should().Contain(expected);
        }

        [Fact]
        public void BackOnTrack_test() => BackOnTrack("a").IsFailure.Should().BeTrue();


        [Fact]
        public void CombineDemo()
        {
            var kundeResult = Kunde.CheckKunde("");
            
            var mailResult = MailAddress.CheckMail("");

            var resultCombine = Result.Combine(",", kundeResult, mailResult);
            resultCombine.Error.Should().Be("ups1,ups2");

            var foo = resultCombine.OnSuccess(() => kundeResult);
            var bar = resultCombine.OnSuccess(() => mailResult);
        }


        class Kunde
        {
            public string Name { get; set; }

            public static Result<Kunde> CheckKunde(string name)
            {
                return string.IsNullOrWhiteSpace(name)
                    ? Result.Fail<Kunde>("ups1")
                    : Result.Ok(new Kunde {Name = name});
            }
        }

        class MailAddress
        {
            public string Name { get; set; }

            public static Result<MailAddress> CheckMail(string name)
            {
                return string.IsNullOrWhiteSpace(name)
                    ? Result.Fail<MailAddress>("ups2")
                    : Result.Ok(new MailAddress {Name = name});
            }
        }
    }
}