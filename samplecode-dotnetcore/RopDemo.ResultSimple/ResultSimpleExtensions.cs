using System;

namespace RopDemo.ResultSimple
{
    public static class ResultSimpleExtensions
    {
        public static ResultSimple<TK> OnSuccess<T, TK>(
            this ResultSimple<T> result, Func<T, ResultSimple<TK>> func)
        {
            if (result.IsFailure)
            {
                return ResultSimple.Fail<TK>(result.Error);
            }

            return func(result.Value);
        }

        public static ResultSimple<T> OnFailure<T>(
            this ResultSimple<T> result, Action<string> action)
        {
            if (result.IsFailure)
            {
                action(result.Error);
            }
                
            return result;
        }

        public static TK OnBoth<T, TK>(
            this ResultSimple<T> result, Func<ResultSimple<T>, TK> func)
        {
            return func(result);
        }
    }
}