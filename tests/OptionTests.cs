namespace Optuple.Tests
{
    using System;
    using NUnit.Framework;
    using _ = System.Object;
    using static OptionModule;

    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void SomeValue()
        {
            var (t, x) = Some(42);
            Assert.That(t, Is.True);
            Assert.That(x, Is.EqualTo(42));
        }

        [Test]
        public void SomeNullReference()
        {
            var (t, x) = Some((object) null);
            Assert.That(t, Is.True);
            Assert.That(x, Is.Null);
        }

        [Test]
        public void SomeNullValue()
        {
            var (t, x) = Some((int?) null);
            Assert.That(t, Is.True);
            Assert.That(x, Is.Null);
        }

        [Test]
        public void None()
        {
            var (t, x) = None<int>();
            Assert.That(t, Is.False);
            Assert.That(x, Is.Zero);
        }

        [Test]
        public void IsSome()
        {
            var result = Some(42);
            Assert.That(result.IsSome(), Is.True);
        }

        [Test]
        public void SomeIsNotNone()
        {
            var result = Some(42);
            Assert.That(result.IsNone(), Is.False);
        }

        [Test]
        public void IsNone()
        {
            var result = None<int>();
            Assert.That(result.IsNone(), Is.True);
        }

        [Test]
        public void NoneIsNotSome()
        {
            var result = None<int>();
            Assert.That(result.IsSome(), Is.False);
        }

        [Test]
        public void ToOptionWithNullableNonNull()
        {
            int? i = 42;
            var result = i.ToOption();
            Assert.That(result, Is.EqualTo(Some(42)));
        }

        [Test]
        public void ToOptionWithNullableNull()
        {
            int? i = null;
            var result = i.ToOption();
            Assert.That(result, Is.EqualTo(None<int>()));
        }

        [Test]
        public void From()
        {
            var result = Option.From(true, 42);
            Assert.That(result, Is.EqualTo(Some(42)));
        }

        [Test]
        public void FromNone()
        {
            var result = Option.From(false, 42);
            Assert.That(result, Is.EqualTo(None<int>()));
        }

        [Test]
        public void SomeWhenWithNullPredicate()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                SomeWhen(42, null));
            Assert.That(e.ParamName, Is.EqualTo("predicate"));
        }

        [Test]
        public void SomeWhenWithTrueCondition()
        {
            var result = SomeWhen(42, n => n > 0);
            Assert.That(result, Is.EqualTo(Some(42)));
        }

        [Test]
        public void SomeWhenWithFalseCondition()
        {
            var result = SomeWhen(42, n => n < 0);
            Assert.That(result, Is.EqualTo(None<int>()));
        }

        [Test]
        public void NoneWhenWithNullPredicate()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                NoneWhen(42, null));
            Assert.That(e.ParamName, Is.EqualTo("predicate"));
        }

        [Test]
        public void NoneWhenNullableIsNonNull()
        {
            var result = NoneWhen(42, n => n < 0);
            Assert.That(result, Is.EqualTo(Some(42)));
        }

        [Test]
        public void NoneWhenNullableIsNull()
        {
            var result = NoneWhen(42, n => n > 0);
            Assert.That(result, Is.EqualTo(None<int>()));
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void MatchWithNullSomeFunction(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Match(null, BreakingFunc.Of<_>()));
            Assert.That(e.ParamName, Is.EqualTo("some"));
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void MatchWithNullNoneFunction(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Match(BreakingFunc.Of<_, _>(), null));
            Assert.That(e.ParamName, Is.EqualTo("none"));
        }

        [Test]
        public void MatchSome()
        {
            var some = Some(3);
            var result = some.Match(i => "foobar".Substring(i), () => "none");
            Assert.That(result, Is.EqualTo("bar"));
        }

        [Test]
        public void MatchNone()
        {
            var none = None<int>();
            var result = none.Match(i => "foobar".Substring(i), () => "foobar");
            Assert.That(result, Is.EqualTo("foobar"));
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void MatchWithNullSomeAction(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Match(null, BreakingAction.OfNone));
            Assert.That(e.ParamName, Is.EqualTo("some"));
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void MatchWithNullNoneAction(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Match(BreakingAction.Of<int>(), null));
            Assert.That(e.ParamName, Is.EqualTo("none"));
        }

        [Test]
        public void MatchSomeWithAction()
        {
            var some = Some(42);
            some.Match(x => Assert.That(x, Is.EqualTo(42)), Assert.Fail);
        }

        [Test]
        public void MatchNoneWithAction()
        {
            var none = None<int>();
            none.Match(x => Assert.Fail(), Assert.Pass);
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void DoWithNullAction(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Do(null));
            Assert.That(e.ParamName, Is.EqualTo("some"));
        }

        [Test]
        public void DoSome()
        {
            var some = Some(42);
            var result = 0;
            some.Do(x => result = x);
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void DoNone()
        {
            var none = None<int>();
            none.Do(x => Assert.Fail());
            Assert.Pass();
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void BindWithNullFunction(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Bind<int, _>(null));
            Assert.That(e.ParamName, Is.EqualTo("function"));
        }

        [Test]
        public void BindSome()
        {
            var some = Some(3);
            var result = some.Bind(i => Some("foobar".Substring(i)));
            Assert.That(result, Is.EqualTo(Some("bar")));
        }

        [Test]
        public void BindNone()
        {
            var none = None<int>();
            var result = none.Bind(i => Some("foobar".Substring(i)));
            Assert.That(result, Is.EqualTo(None<string>()));
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void MapWithNullMapper(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Map<_, _>(null));
            Assert.That(e.ParamName, Is.EqualTo("mapper"));
        }

        [Test]
        public void MapSome()
        {
            var some = Some(42);
            var result = some.Map(n => new string((char) n, n));
            var stars = new string('*', 42);
            Assert.That(result, Is.EqualTo(Some(stars)));
        }

        [Test]
        public void MapNone()
        {
            var none = None<int>();
            var result = none.Map(n => new string((char) n, n));
            Assert.That(result, Is.EqualTo(None<string>()));
        }

        [Test]
        public void GetSome()
        {
            var some = Some(42);
            Assert.That(some.Get(), Is.EqualTo(42));
        }

        [Test]
        public void GetNone()
        {
            var none = None<int>();
            Assert.Throws<ArgumentException>(() => none.Get());
        }

        [Test]
        public void OrDefaultSome()
        {
            var some = Some(42);
            Assert.That(some.OrDefault(), Is.EqualTo(42));
        }

        [Test]
        public void OrDefaultNone()
        {
            var none = None<int>();
            Assert.That(none.OrDefault(), Is.Zero);
        }

        [Test]
        public void OrSome()
        {
            var some = Some(42);
            Assert.That(some.Or(-42), Is.EqualTo(42));
        }

        [Test]
        public void OrNone()
        {
            var none = None<int>();
            Assert.That(none.Or(42), Is.EqualTo(42));
        }

        [Test]
        public void CountSome()
        {
            Assert.That(Some(42).Count(), Is.EqualTo(1));
        }

        [Test]
        public void CountNone()
        {
            Assert.That(None<int>().Count(), Is.Zero);
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void ExistsWithNullPredicate(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Exists(null));
            Assert.That(e.ParamName, Is.EqualTo("predicate"));
        }

        [Test]
        public void ExistsSome()
        {
            var some = Some(42);
            Assert.That(some.Exists(x => x > 0), Is.True);
            Assert.That(some.Exists(x => x < 0), Is.False);
        }

        [Test]
        public void ExistsNone()
        {
            var some = None<int>();
            Assert.That(some.Exists(x => x < 0), Is.False);
            Assert.That(some.Exists(x => x > 0), Is.False);
        }

        [TestCase(true, 42)]
        [TestCase(false, 0)]
        public void FilterWithNullPredicate(bool f, int v)
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Option.From(f, v).Filter(null));
            Assert.That(e.ParamName, Is.EqualTo("predicate"));
        }

        [Test]
        public void FilterSome()
        {
            var some = Some(42);
            var result1 = some.Filter(x => x > 0);
            var result2 = some.Filter(x => x < 0);
            Assert.That(result1, Is.EqualTo(some));
            Assert.That(result2, Is.EqualTo(None<int>()));
        }

        [Test]
        public void FilterNone()
        {
            var none = None<int>();
            var result1 = none.Filter(x => x > 42);
            var result2 = none.Filter(x => x < 42);
            Assert.That(result1, Is.EqualTo(none));
            Assert.That(result2, Is.EqualTo(none));
        }

        [Test]
        public void ToNullableSome()
        {
            var some = Some(42);
            var result = some.ToNullable();
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void ToNullableNone()
        {
            var none = None<int>();
            var result = none.ToNullable();
            Assert.That(result, Is.Null);
        }

        // Credit & inspiration:
        // https://github.com/nlkl/Optional/blob/fa0160d995af60e8378c28005d810ec5b74f2eef/src/Optional.Tests/MaybeTests.cs#L137-L202
        // Copyright (c) 2014 Nils Lück
        // The MIT License (MIT)
        // https://github.com/nlkl/Optional/blob/fa0160d995af60e8378c28005d810ec5b74f2eef/LICENSE

        public class CompareTo
        {
            static void LessThan<T>((bool, T) lesser, (bool, T) greater)
            {
                Assert.That(lesser.CompareTo(greater), Is.LessThan(0));
                Assert.That(greater.CompareTo(lesser), Is.GreaterThan(0));
            }

            static void EqualTo<T>((bool, T) left, (bool, T) right)
            {
                Assert.That(left.CompareTo(right), Is.Zero);
                Assert.That(right.CompareTo(left), Is.Zero);
            }

            [Test]
            public void ValueTypes()
            {
                var none = None<int>();
                var some1 = Some(1);
                var some2 = Some(2);

                LessThan(none, some1);
                LessThan(some1, some2);

                EqualTo(none, none);
                EqualTo(some1, some1);
            }

            [Test]
            public void Comparables()
            {
                var none = None<string>();
                var someNull = Some<string>(null);
                var some1 = Some("1");
                var some2 = Some("2");

                LessThan(none, some1);
                LessThan(none, someNull);
                LessThan(someNull, some1);
                LessThan(some1, some2);

                EqualTo(none, none);
                EqualTo(someNull, someNull);
                EqualTo(some1, some1);
            }

            [Test]
            public void NonComparables()
            {
                var none = None<object>();
                var someNull = Some<object>(null);
                var some1 = Some(new object());
                var some2 = Some(new object());

                Assert.Throws<ArgumentException>(() => some1.CompareTo(some2));
                Assert.Throws<ArgumentException>(() => some2.CompareTo(some1));

                LessThan(none, some1);
                LessThan(none, someNull);
                LessThan(someNull, some1);

                EqualTo(none, none);
                EqualTo(someNull, someNull);
                EqualTo(some1, some1);
            }
        }
    }
}
