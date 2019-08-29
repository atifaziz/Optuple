namespace Optuple.Tests
{
    using System;
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using RegularExpressions;
    using Regex = RegularExpressions.Regex;
    using SysRegex = System.Text.RegularExpressions.Regex;

    [TestFixture]
    public class RegexTests
    {
        [Test]
        public void NullThis()
        {
            var e = Assert.Throws<ArgumentNullException>(() =>
                Regex.ToOption<Group>(null));
            Assert.That(e.ParamName, Is.EqualTo("group"));
        }

        [Test]
        public void Success()
        {
            var match = SysRegex.Match("foo bar baz", @"\bbar\b");
            var (ms, m) = match.ToOption();
            Assert.That(ms, Is.True);
            Assert.That(m, Is.SameAs(match));
        }

        [Test]
        public void Mismatch()
        {
            var (t, m) = SysRegex.Match(string.Empty, "foo").ToOption();
            Assert.That(t, Is.False);
            Assert.That(m, Is.Null);
        }
    }
}
