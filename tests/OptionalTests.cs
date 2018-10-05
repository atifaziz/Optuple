namespace JustNothing.Linq.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using NUnit.Framework;
    using Case = System.Boolean;

    [TestFixture]
    public class OptionalTests
    {
        static T Fail<T>()
        {
            Assert.Fail("Binding semantic error!");
            return default; // never reaches here
        }

        [Test]
        public void Select()
        {
            var result =
                from n in Option.Some(42)
                select new string((char) n, n);

            var stars = new string('*', 42);
            Assert.That(result, Is.EqualTo(Option.Some(stars)));
        }

        [Test]
        public void SelectNone()
        {
            var result =
                from n in Option.None<int>()
                select Fail<string>();

            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void WhereSomeMeetsCondition()
        {
            var some = Option.Some(42);

            var result =
                from n in some
                where n > 0
                select n - n;

            Assert.That(result, Is.EqualTo(Option.Some(0)));
        }

        [Test]
        public void WhereSomeFailsCondition()
        {
            var result =
                from n in Option.Some(42)
                where n < 0
                select Fail<int>();

            Assert.That(result, Is.EqualTo(Option.None<int>()));
        }

        [Test]
        public void WhereNone()
        {
            var none = Option.None<int>();

            var result =
                from n in none
                where Fail<bool>()
                select n;

            Assert.That(result, Is.EqualTo(none));
        }

        [Test]
        public void SelectManySimple()
        {
            var result = Option.Some(42).SelectMany(n => Option.Some(new string((char) n, n)));
            var stars = new string('*', 42);
            Assert.That(result, Is.EqualTo(Option.Some(stars)));
        }

        [Test]
        public void SelectMany()
        {
            var result =
                from n in Option.Some(42)
                from s in Option.Some(new string((char) n, n))
                select string.Concat(n, s);

            var stars = new string('*', 42);
            Assert.That(result, Is.EqualTo(Option.Some("42" + stars)));
        }

        [Test]
        public void SelectManyNone()
        {
            var result =
                from n in Option.None<int>()
                from s in Fail<(Case, string)>()
                select Fail<string>();

            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void SelectManySomeAndNone()
        {
            var result =
                from n in Option.Some(42)
                from s in Option.None<string>()
                select Fail<string>();

            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void Cast()
        {
            var result =
                from int n in Option.Some((object) 42)
                select n;

            Assert.That(result, Is.EqualTo(Option.Some(42)));
        }

        [Test]
        public void CastInvalid()
        {
            Assert.Throws<InvalidCastException>(() =>
            {
                var _ =
                    from string s in Option.Some((object) 42)
                    select s;
            });
        }

        [Test]
        public void CastNone()
        {
            var result =
                from int _ in Option.None<DateTime>()
                select Fail<string>();

            Assert.That(result, Is.EqualTo(Option.None<string>()));
        }

        [Test]
        public void AllSomeTrue()
        {
            var result = Option.Some(42).All(n => n > 0);
            Assert.That(result, Is.True);
        }

        [Test]
        public void AllSomeFalse()
        {
            var result = Option.Some(42).All(n => n < 0);
            Assert.That(result, Is.False);
        }

        [Test]
        public void AllNone()
        {
            var result = Option.None<int>().All(_ => Fail<bool>());
            Assert.That(result, Is.True);
        }

        [Test]
        public void ToArraySome()
        {
            var result = Option.Some(42).ToArray();
            Assert.That(result, Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void ToArrayNone()
        {
            var result = Option.None<int>().ToArray();
            Assert.That(result, Is.EqualTo(new int[0]));
        }

        [Test]
        public void ToListSome()
        {
            var result = Option.Some(42).ToList();
            Assert.That(result, Is.EqualTo(new List<int> { 42 }));
        }

        [Test]
        public void ToListNone()
        {
            var result = Option.None<int>().ToList();
            Assert.That(result, Is.EqualTo(new List<int>()));
        }

        [Test]
        public void ToEnumerableSome()
        {
            var result = Option.Some(42).ToEnumerable();
            Assert.That(result, Is.EqualTo(new[] { 42 }));
        }

        [Test]
        public void ToEnumerableNone()
        {
            var result = Option.None<int>().ToList();
            Assert.That(result, Is.EqualTo(Enumerable.Empty<int>()));
        }
    }
}
