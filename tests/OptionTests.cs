namespace JustNothing.Tests
{
    using NUnit.Framework;
    using Case = Option.Case;
    using Linq;

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
    }
}
