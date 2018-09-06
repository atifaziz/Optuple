namespace JustNothing.Tests
{
    using NUnit.Framework;
    using Case = Option.Case;
    using System;

    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void Some()
        {
            var (t, x) = Option.Some(42);
            Assert.That(t, Is.EqualTo(Case.Some));
            Assert.That(x, Is.EqualTo(42));
        }

        [Test]
        public void None()
        {
            var (t, x) = Option.None<int>();
            Assert.That(t, Is.EqualTo(Case.None));
            Assert.That(x, Is.Zero);
        }

        [Test]
        public void IsSome()
        {
            var x = Option.Some(42);
            Assert.That(x.IsSome(), Is.True);
        }

        [Test]
        public void SomeIsNotNone()
        {
            var x = Option.Some(42);
            Assert.That(x.IsNone(), Is.False);
        }

        [Test]
        public void IsNone()
        {
            var x = Option.None<int>();
            Assert.That(x.IsNone(), Is.True);
        }

        [Test]
        public void NoneIsNotSome()
        {
            var x = Option.None<int>();
            Assert.That(x.IsSome(), Is.False);
        }

        [Test]
        public void ToOptionWithTupleIsSome()
        {
            var x = (true, 42).ToOption();
            Assert.That(x, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void ToOptionWithTupleIsNone()
        {
            var x = (false, 42).ToOption();
            Assert.That(x, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void ToOptionWithNullableNonNull()
        {
            int? i = 42;
            var x = i.ToOption();
            Assert.That(x, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void ToOptionWithNullableNull()
        {
            int? i = null;
            var x = i.ToOption();
            Assert.That(x, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void From()
        {
            var x = Option.From(true, 42);
            Assert.That(x, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void FromNone()
        {
            var x = Option.From(false, 42);
            Assert.That(x, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void Flagged()
        {
            var (some, x) = Option.Some(42).Flagged();
            Assert.That(some, Is.True);
            Assert.That(x, Is.EqualTo(42));
        }

        [Test]
        public void FlaggedNone()
        {
            var (some, x) = Option.None<int>().Flagged();
            Assert.That(some, Is.False);
            Assert.That(x, Is.Zero);
        }

        [Test]
        public void SomeWhenNullableIsNonNull()
        {
            int? i = 42;
            var x = Option.SomeWhen(i, e => e.HasValue);
            Assert.That(x, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void SomeWhenNullableIsNull()
        {
            int? i = null;
            var x = Option.SomeWhen(i, e => e.HasValue);
            Assert.That(x, Is.EqualTo(Option.None<int?>()));
        }

        [Test]
        public void NoneWhenNullableIsNonNull()
        {
            int? i = 42;
            var x = Option.NoneWhen(i, e => !e.HasValue);
            Assert.That(x, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void NoneWhenNullableIsNull()
        {
            int? i = null;
            var x = Option.NoneWhen(i, e => !e.HasValue);
            Assert.That(x, Is.EqualTo(Option.None<int?>()));
        }

        [Test]
        public void MatchSome()
        {
            var some = Option.Some(3);
            var result = some.Match(x => "foobar".Substring(x), () => "none");
            Assert.That(result, Is.EqualTo("bar"));
        }

        [Test]
        public void MatchNone()
        {
            var none = Option.None<int>();
            var result = none.Match(x => "foobar".Substring(x), () => "foobar");
            Assert.That(result, Is.EqualTo("foobar"));
        }

        [Test]
        public void MatchSomeWithAction()
        {
            var some = Option.Some(42);
            some.Match(x => Assert.That(x, Is.EqualTo(42)), Assert.Fail);
        }

        [Test]
        public void MatchNoneWithAction()
        {
            var none = Option.None<int>();
            none.Match(x => Assert.Fail(), Assert.Pass);
        }

        [Test]
        public void DoSome()
        {
            var some = Option.Some(42);
            var actual = 0;
            some.Do(x => actual = x);
            Assert.That(actual, Is.EqualTo(42));
        }

        [Test]
        public void DoNone()
        {
            var none = Option.None<int>();
            none.Do(x => Assert.Fail());
            Assert.Pass();
        }

        [Test]
        public void BindSome()
        {
            var some = Option.Some(3);
            var result = some.Bind(x => Option.Some("foobar".Substring(x)));
            Assert.That(result, Is.EqualTo(Option.Some("bar")));
        }

        [Test]
        public void BindNone()
        {
            var none = Option.None<int>();
            var result = none.Bind(x => Option.Some("foobar".Substring(x)));
            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void MapSome()
        {
            var some = Option.Some(42);
            var result = some.Map(x => (char) x);
            Assert.That(result, Is.EqualTo(Option.Some('*')));
        }

        [Test]
        public void MapNone()
        {
            var none = Option.None<int>();
            var result = none.Map(x => (char) x);
            Assert.That(result, Is.EqualTo(none));
        }

        [Test]
        public void GetSome()
        {
            var some = Option.Some(42);
            Assert.That(some.Get(), Is.EqualTo(42));
        }

        [Test]
        public void GetNone()
        {
            var none = Option.None<int>();
            Assert.Throws<ArgumentException>(() => none.Get());
        }

        [Test]
        public void OrDefaultSome()
        {
            var some = Option.Some(42);
            Assert.That(some.OrDefault(), Is.EqualTo(42));
        }
        [Test]
        public void OrDefaultNone()
        {
            var none = Option.None<int>();
            Assert.That(none.OrDefault(), Is.Zero);
        }

        [Test]
        public void OrSome()
        {
            var some = Option.Some(42);
            Assert.That(some.Or(-42), Is.EqualTo(42));
        }

        [Test]
        public void OrNone()
        {
            var none = Option.None<int>();
            Assert.That(none.Or(42), Is.EqualTo(42));
        }

        [Test]
        public void CountSome()
        {
            Assert.That(Option.Some(42).Count(), Is.EqualTo(1));
        }

        [Test]
        public void CountNone()
        {
            Assert.That(Option.None<int>().Count(), Is.Zero);
        }

        [Test]
        public void ExistsSome()
        {
            var some = Option.Some(42);
            Assert.That(some.Exists(x => x > 0), Is.True);
            Assert.That(some.Exists(x => x < 0), Is.False);
        }

        [Test]
        public void ExistsNone()
        {
            var some = Option.None<int>();
            Assert.That(some.Exists(x => x < 0), Is.False);
            Assert.That(some.Exists(x => x > 0), Is.False);
        }

        [Test]
        public void FilterSome()
        {
            var some = Option.Some(42);
            var result1 = some.Filter(x => x > 0);
            var result2 = some.Filter(x => x < 0);
            Assert.That(result1, Is.EqualTo(some));
            Assert.That(result2, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void FilterNone()
        {
            var none = Option.None<int>();
            var result1 = none.Filter(x => x > 42);
            var result2 = none.Filter(x => x < 42);
            Assert.That(result1, Is.EqualTo(none));
            Assert.That(result2, Is.EqualTo(none));
        }

        [Test]
        public void ToNullableSome()
        {
            var some = Option.Some(42);
            var result = some.ToNullable();
            Assert.That(result, Is.EqualTo(42));
        }

        [Test]
        public void ToNullableNone()
        {
            var none = Option.None<int>();
            var result = none.ToNullable();
            Assert.That(result, Is.Null);
        }

        public class CompareTo
        {
            static void LessThan<T>((Case, T) lesser, (Case, T) greater)
            {
                Assert.That(lesser.CompareTo(greater), Is.LessThan(0));
                Assert.That(greater.CompareTo(lesser), Is.GreaterThan(0));
            }

            static void EqualTo<T>((Case, T) left, (Case, T) right)
            {
                Assert.That(left.CompareTo(right), Is.Zero);
                Assert.That(right.CompareTo(left), Is.Zero);
            }

            [Test]
            public void ValueTypes()
            {
                var none = Option.None<int>();
                var some1 = Option.Some(1);
                var some2 = Option.Some(2);

                LessThan(none, some1);
                LessThan(some1, some2);

                EqualTo(none, none);
                EqualTo(some1, some1);
            }

            [Test]
            public void Comparables()
            {
                var none = Option.None<string>();
                var someNull = Option.Some<string>(null);
                var some1 = Option.Some("1");
                var some2 = Option.Some("2");

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
                var none = Option.None<object>();
                var someNull = Option.Some<object>(null);
                var some1 = Option.Some(new object());
                var some2 = Option.Some(new object());

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
