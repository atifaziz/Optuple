namespace JustNothing.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Linq;
    using NUnit.Framework;
    using Case = Option.Case;
    using JustNothing.Linq;

    [TestFixture]
    class OptionalTests
    {
        [Test]
        public void Select()
        {
            var result = from a in Option.Some(97)
                         select (char) a;
            Assert.That(result, Is.EqualTo(Option.Some('a')));
        }

        [Test]
        public void WhereSome()
        {
            var result = from a in Option.Some(97)
                         where a < 100
                         select a;
            Assert.That(result, Is.EqualTo(Option.Some('a')));
        }

        [Test]
        public void WhereNone()
        {
            var result = from a in Option.Some(97)
                         where a > 100
                         select a;
            Assert.That(result, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void SelectMany()
        {
            var result = from a in Option.Some(97)
                         from b in Option.Some((char) a)
                         select a.ToString() + b.ToString();
            Assert.That(result, Is.EqualTo(Option.Some("97a")));
        }

        [Test]
        public void SelectManyNoneAndSome()
        {
            var result = from a in Option.None<int>()
                         from b in Option.Some((char) a)
                         select a.ToString() + b.ToString();
            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void SelectManyNone()
        {
            var result = from a in Option.None<int>()
                         from b in Option.None<char>()
                         select a.ToString() + b.ToString();
            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void CastObjectToInt()
        {
            var result = from int x in Option.Some((object) 42)
                         select x;
            Assert.That(result, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void CastIntToString()
        {
            Assert.Throws<InvalidCastException>(() => {
                var _ = from string x in Option.Some((object) 42)
                        select x;
            });

        }

        [Test]
        public void AllSomeTrue()
        {
            var result = Option.Some(97).All(e => e < 100);
            Assert.That(result, Is.True);
        }

        [Test]
        public void AllSomeFalse()
        {
            var result = Option.Some(97).All(e => e >= 100);
            Assert.That(result, Is.False);
        }

        [Test]
        public void AllNone()
        {
            var result1 = Option.None<int>().All(e => e > 100);
            var result2 = Option.None<int>().All(e => e <= 100);
            Assert.That(result1, Is.True);
            Assert.That(result2, Is.True);
        }

        [Test]
        public void ToArraySome()
        {
            var result = Option.Some(97).ToArray();
            Assert.That(result, Is.EqualTo(new int[] { 97 }));
        }

        [Test]
        public void ToArrayNone()
        {
            var result = Option.None<int>().ToArray();
            Assert.That(result, Is.EqualTo(new int[] { }));
        }

        [Test]
        public void ToListSome()
        {
            var result = Option.Some(97).ToList();
            Assert.That(result, Is.EqualTo(new List<int>() { 97 }));
        }

        [Test]
        public void ToListNone()
        {
            var result = Option.None<int>().ToList();
            Assert.That(result, Is.EqualTo(new List<int>()));
        }
    }
}
