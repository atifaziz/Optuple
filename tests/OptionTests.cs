namespace JustNothing.Tests
{
    using NUnit.Framework;
    using Case = Option.Case;

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
            Assert.That(x.IsSome(), Is.True);
            Assert.That(x.Value, Is.EqualTo(3));
        }

        [Test]
        public void ToOptionWithTupleIsNone()
        {
            var x = (false, 3).ToOption();
            Assert.That(x.IsNone(), Is.True);
        }

        [Test]
        public void ToOptionWithNullableNonNull()
        {
            int? i = 3;
            var x = i.ToOption();
            Assert.That(x.IsSome(), Is.True);
        }

        [Test]
        public void ToOptionWithNullableNull()
        {
            int? i = null;
            var x = i.ToOption();
            Assert.That(x.IsNone(), Is.True);
        }

        [Test]
        public void ToOptionWithNonNullReference()
        {
            string s = "aeiou";
            var x = s.ToOption();
            Assert.That(x.IsSome(), Is.True);
        }

        [Test]
        public void ToOptionWithNullReference()
        {
            string s = null;
            var x = s.ToOption();
            Assert.That(x.IsNone(), Is.True);
        }

        [Test]
        public void From()
        {
            var x = Option.From(true, 3);
            Assert.That(x.IsSome(), Is.True);
        }

        [Test]
        public void FromNone()
        {
            var x = Option.From(false, 3);
            Assert.That(x.IsNone(), Is.True);
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
        public void SomeWhen()
        {
            int? i = 3;
            var x = Option.SomeWhen(i, e => e.HasValue);
            Assert.That(x.IsSome());
        }

        [Test]
        public void SomeWhen2()
        {
            int? i = null;
            var x = Option.SomeWhen(i, e => e.HasValue);
            Assert.That(x.IsNone());
        }

        [Test]
        public void NoneWhen()
        {
            int? i = 3;
            var x = Option.NoneWhen(i, e => !e.HasValue);
            Assert.That(x.IsSome());
        }

        [Test]
        public void NoneWhen2()
        {
            int? i = null;
            var x = Option.NoneWhen(i, e => !e.HasValue);
            Assert.That(x.IsNone());
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
            some.Match(x => Assert.That(x, Is.EqualTo(2)),
                       () => Assert.Fail());
        }

        [Test]
        public void MatchNoneWithAction()
        {
            var none = Option.None<int>();
            none.Match(x => Assert.Fail(),
                       () => Assert.Pass());
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
    }
}
