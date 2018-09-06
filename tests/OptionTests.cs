namespace JustNothing.Tests
{
    using NUnit.Framework;
    using Case = Option.Case;
    using System;
    using System.Collections.Generic;

    [TestFixture]
    public class OptionTests
    {
        [Test]
        public void Some()
        {
            var x = Option.Some(42);
            Assert.That(x.Case, Is.EqualTo(Case.Some));
            Assert.That(x.Value, Is.EqualTo(42));
        }

        [Test]
        public void None()
        {
            var x = Option.None<int>();
            Assert.That(x.Case, Is.EqualTo(Case.None));
            Assert.That(x.Value, Is.EqualTo(default(int)));
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
            var x = (true, 3).ToOption();
            Assert.That(x, Is.EqualTo(Option.Some(3)));
        }

        [Test]
        public void ToOptionWithTupleIsNone()
        {
            var x = (false, 3).ToOption();
            Assert.That(x, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void ToOptionWithNullableNonNull()
        {
            int? i = 3;
            var x = i.ToOption();
            Assert.That(x, Is.EqualTo(Option.Some(3)));
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
            var x = Option.From(true, 3);
            Assert.That(x, Is.EqualTo(Option.Some(3)));
        }

        [Test]
        public void FromNone()
        {
            var x = Option.From(false, 3);
            Assert.That(x, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void Flagged()
        {
            var x = (Case.Some, 3).Flagged();
            Assert.That(x.HasValue, Is.True);
            Assert.That(x.Value, Is.EqualTo(3));
        }

        [Test]
        public void FlaggedNone()
        {
            var x = (Case.None, 3).Flagged();
            Assert.That(x.HasValue, Is.False);
            Assert.That(x.Value, Is.EqualTo(default(int)));
        }

        [Test]
        public void SomeWhenNullableIsNonNull()
        {
            int? i = 3;
            var x = Option.SomeWhen(i, e => e.HasValue);
            Assert.That(x, Is.EqualTo(Option.Some(3)));
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
            int? i = 3;
            var x = Option.NoneWhen(i, e => !e.HasValue);
            Assert.That(x, Is.EqualTo(Option.Some(3)));
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
            var some = Option.Some(2);
            var result = some.Match(x => "aeiou".Substring(x), () => "aeiou");
            Assert.That(result, Is.EqualTo("iou"));
        }

        [Test]
        public void MatchNone()
        {
            var none = Option.None<int>();
            var result = none.Match(x => "aeiou".Substring(x), () => "aeiou");
            Assert.That(result, Is.EqualTo("aeiou"));
        }

        [Test]
        public void MatchSomeWithAction()
        {
            var some = Option.Some(2);
            some.Match(x => Assert.That(x, Is.EqualTo(2)), Assert.Fail);
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
            var some = Option.Some(2);
            some.Do(x => Assert.That(x, Is.EqualTo(2)));
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
            var some = Option.Some(2);
            var result = some.Bind(x => Option.Some("aeiou".Substring(x)));
            Assert.That(result, Is.EqualTo(Option.Some("iou")));
        }

        [Test]
        public void BindNone()
        {
            var none = Option.None<int>();
            var result = none.Bind(x => Option.Some("aeiou".Substring(x)));
            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void MapSome()
        {
            var some = Option.Some(97);
            var result = some.Map(x => (char) x);
            Assert.That(result, Is.EqualTo(Option.Some('a')));
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
            var some = Option.Some(2);
            Assert.That(some.Get(), Is.EqualTo(2));
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
            var some = Option.Some(2);
            Assert.That(some.OrDefault(), Is.EqualTo(2));
        }
        [Test]
        public void OrDefaultNone()
        {
            var none = Option.None<int>();
            Assert.That(none.OrDefault(), Is.EqualTo(0));
        }

        [Test]
        public void OrSome()
        {
            var some = Option.Some(2);
            Assert.That(some.Or(1), Is.EqualTo(2));
        }

        [Test]
        public void OrNone()
        {
            var none = Option.None<int>();
            Assert.That(none.Or(1), Is.EqualTo(1));
        }

        [Test]
        public void CountSome()
        {
            Assert.That(Option.Some(2).Count(), Is.EqualTo(1));
        }

        [Test]
        public void CountNone()
        {
            Assert.That(Option.None<int>().Count(), Is.EqualTo(0));
        }

        [Test]
        public void ExistsSome()
        {
            var some = Option.Some(2);
            Assert.That(some.Exists(x => x < 3), Is.True);
            Assert.That(some.Exists(x => x >= 3), Is.False);
        }

        [Test]
        public void ExistsNone()
        {
            var some = Option.None<int>();
            Assert.That(some.Exists(x => x < 3), Is.False);
            Assert.That(some.Exists(x => x >= 3), Is.False);
        }

        [Test]
        public void FilterSome()
        {
            var some = Option.Some(2);
            var result1 = some.Filter(x => x < 3);
            var result2 = some.Filter(x => x >= 3);
            Assert.That(result1, Is.EqualTo(some));
            Assert.That(result2, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void FilterNone()
        {
            var none = Option.None<int>();
            var result1 = none.Filter(x => x < 3);
            var result2 = none.Filter(x => x >= 3);
            Assert.That(result1, Is.EqualTo(none));
            Assert.That(result2, Is.EqualTo(none));
        }

        [Test]
        public void ToNullableSome()
        {
            var some = Option.Some(2);
            var result = some.ToNullable();
            Assert.That(result, Is.EqualTo(2));
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
                Assert.That(left.CompareTo(right), Is.EqualTo(0));
                Assert.That(right.CompareTo(left), Is.EqualTo(0));
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
                var none = Option.None<Dictionary<string, string>>();
                var someNull = Option.Some<Dictionary<string, string>>(null);
                var some1 = Option.Some(new Dictionary<string, string>());
                var some2 = Option.Some(new Dictionary<string, string>());

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
