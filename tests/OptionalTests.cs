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

    }
}
