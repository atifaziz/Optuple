namespace Optuple.Tests
{
    using System;

    static class BreakingAction
    {
        public static readonly Action OfNone = () => throw new NotImplementedException();
        public static Action<T> Of<T>() => delegate { throw new NotImplementedException(); };
    }
}
