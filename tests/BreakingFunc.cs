namespace Optuple.Tests
{
    using System;

    static class BreakingFunc
    {
        public static Func<T> Of<T>() => () => throw new NotImplementedException();
        public static Func<T, TResult> Of<T, TResult>() => delegate { throw new NotImplementedException(); };
        public static Func<T1, T2, TResult> Of<T1, T2, TResult>() => delegate { throw new NotImplementedException(); };
    }
}