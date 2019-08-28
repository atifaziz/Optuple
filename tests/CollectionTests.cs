namespace Optuple.Collections.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using MoreLinq;
    using NUnit.Framework;
    using Optuple.Tests;
    using static System.Linq.Enumerable;
    using static OptionModule;
    using Enumerable = Enumerable;

    [TestFixture]
    public class OptionalTests
    {
        public class FirstOrNone
        {
            [Test]
            public void NullSource()
            {
                var e = Assert.Throws<ArgumentNullException>(() =>
                    Enumerable.FirstOrNone<object>(null));
                Assert.That(e.ParamName, Is.EqualTo("source"));
            }

            public class Sequence
            {
                [Test]
                public void Empty()
                {
                    var source = Empty<object>().Hide();
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = MoreEnumerable.From(() => 123);
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = MoreEnumerable.From(() => 123, BreakingFunc.Of<int>());
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }
            }

            public class List
            {
                [Test]
                public void Empty()
                {
                    var source = new BreakingList<object>(new List<object>());
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = new BreakingList<int>(new List<int> { 123 });
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = new BreakingList<int>(new List<int> { 123, 456 });
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }
            }

            public class ReadOnlyList
            {
                [Test]
                public void Empty()
                {
                    var source = new BreakingReadOnlyList<object>(new List<object>());
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = new BreakingReadOnlyList<int>(new List<int> { 123 });
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = new BreakingReadOnlyList<int>(new List<int> { 123, 456 });
                    var (t, x) = source.FirstOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }
            }

            public class Predicate
            {
                [Test]
                public void NullSource()
                {
                    var e = Assert.Throws<ArgumentNullException>(() =>
                        Enumerable.FirstOrNone(null, BreakingFunc.Of<object, bool>()));
                    Assert.That(e.ParamName, Is.EqualTo("source"));
                }

                [Test]
                public void NullPredicate()
                {
                    var e = Assert.Throws<ArgumentNullException>(() =>
                        Empty<object>().FirstOrNone(null));
                    Assert.That(e.ParamName, Is.EqualTo("predicate"));
                }

                [Test]
                public void Empty()
                {
                    var source = Empty<object>().Hide();
                    var (t, x) = source.FirstOrNone(BreakingFunc.Of<object, bool>());
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                public class One
                {
                    static readonly IEnumerable<int> Source = MoreEnumerable.From(() => 123);

                    [Test]
                    public void Match()
                    {
                        var (t, x) = Source.FirstOrNone(x => x > 100);
                        Assert.That(t, Is.True);
                        Assert.That(x, Is.EqualTo(123));
                    }

                    [Test]
                    public void NoneMatch()
                    {
                        var (t, x) = Source.FirstOrNone(_ => false);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }
                }

                public class Many
                {
                    static readonly IEnumerable<int> Source =
                        MoreEnumerable.From(() => 123, () => 456, () => 789);

                    [Test]
                    public void Match()
                    {
                        var (t, x) = Source.FirstOrNone(x => x >= 500);
                        Assert.That(t, Is.True);
                        Assert.That(x, Is.EqualTo(789));
                    }

                    [Test]
                    public void NoneMatch()
                    {
                        var (t, x) = Source.FirstOrNone(_ => false);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }
                }
            }
        }

        public class SingleOrNone
        {
            [Test]
            public void NullSource()
            {
                var e = Assert.Throws<ArgumentNullException>(() =>
                    Enumerable.SingleOrNone<object>(null));
                Assert.That(e.ParamName, Is.EqualTo("source"));
            }

            public class Sequence
            {
                [Test]
                public void Empty()
                {
                    var source = Empty<object>().Hide();
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = MoreEnumerable.From(() => 123);
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = MoreEnumerable.From(() => 123, () => 456, BreakingFunc.Of<object>());
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }
            }

            public class List
            {
                [Test]
                public void Empty()
                {
                    var source = new BreakingList<object>(new List<object>());
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = new BreakingList<int>(new List<int> { 123 });
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = new BreakingList<object>(new List<object> { 123, 456 });
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }
            }

            public class ReadOnlyList
            {
                [Test]
                public void Empty()
                {
                    var source = new BreakingReadOnlyList<object>(new List<object>());
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = new BreakingReadOnlyList<int>(new List<int> { 123 });
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = new BreakingReadOnlyList<object>(new List<object> { 123, 456 });
                    var (t, x) = source.SingleOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }
            }

            public class Predicate
            {
                [Test]
                public void NullSource()
                {
                    var e = Assert.Throws<ArgumentNullException>(() =>
                        Enumerable.SingleOrNone(null, BreakingFunc.Of<object, bool>()));
                    Assert.That(e.ParamName, Is.EqualTo("source"));
                }

                [Test]
                public void NullPredicate()
                {
                    var e = Assert.Throws<ArgumentNullException>(() =>
                        Empty<object>().SingleOrNone(null));
                    Assert.That(e.ParamName, Is.EqualTo("predicate"));
                }

                [Test]
                public void Empty()
                {
                    var source = Empty<object>().Hide();
                    var (t, x) = source.SingleOrNone(BreakingFunc.Of<object, bool>());
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                public class One
                {
                    static readonly IEnumerable<int> Source = MoreEnumerable.From(() => 123);

                    [Test]
                    public void NoneMatch()
                    {
                        var (t, x) = Source.SingleOrNone(_ => false);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }

                    [Test]
                    public void OneMatch()
                    {
                        var (t, x) = Source.SingleOrNone(x => x > 100);
                        Assert.That(t, Is.True);
                        Assert.That(x, Is.EqualTo(123));
                    }
                }

                public class Many
                {
                    static readonly IEnumerable<int> Source =
                        MoreEnumerable.From(() => 123, () => 456, () => 786);

                    [Test]
                    public void NoneMatch()
                    {
                        var (t, x) = Source.SingleOrNone(_ => false);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }

                    [Test]
                    public void OneMatch()
                    {
                        var (t, x) = Source.SingleOrNone(x => x > 500);
                        Assert.That(t, Is.True);
                        Assert.That(x, Is.EqualTo(786));
                    }

                    [Test]
                    public void ManyMatch()
                    {
                        var (t, x) =
                            Source.Concat(MoreEnumerable.From(BreakingFunc.Of<int>()))
                                  .SingleOrNone(x => x > 200);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }
                }
            }
        }

        public class LastOrNone
        {
            [Test]
            public void NullSource()
            {
                var e = Assert.Throws<ArgumentNullException>(() =>
                    Enumerable.LastOrNone<object>(null));
                Assert.That(e.ParamName, Is.EqualTo("source"));
            }

            public class Sequence
            {
                [Test]
                public void Empty()
                {
                    var source = Empty<object>().Hide();
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = MoreEnumerable.From(() => 123);
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = MoreEnumerable.From(() => 123, () => 456, () => 789);
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(789));
                }
            }

            public class List
            {
                [Test]
                public void Empty()
                {
                    var source = new BreakingList<object>(new List<object>());
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = new BreakingList<int>(new List<int> { 123 });
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = new BreakingList<int>(new List<int> { 123, 456, 789 });
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(789));
                }
            }

            public class ReadOnlyList
            {
                [Test]
                public void Empty()
                {
                    var source = new BreakingReadOnlyList<object>(new List<object>());
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                [Test]
                public void One()
                {
                    var source = new BreakingReadOnlyList<int>(new List<int> { 123 });
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(123));
                }

                [Test]
                public void Many()
                {
                    var source = new BreakingReadOnlyList<int>(new List<int> { 123, 456, 789 });
                    var (t, x) = source.LastOrNone();
                    Assert.That(t, Is.True);
                    Assert.That(x, Is.EqualTo(789));
                }
            }

            public class Predicate
            {
                [Test]
                public void NullSource()
                {
                    var e = Assert.Throws<ArgumentNullException>(() =>
                        Enumerable.LastOrNone(null, BreakingFunc.Of<object, bool>()));
                    Assert.That(e.ParamName, Is.EqualTo("source"));
                }

                [Test]
                public void NullPredicate()
                {
                    var e = Assert.Throws<ArgumentNullException>(() =>
                        Empty<object>().LastOrNone(null));
                    Assert.That(e.ParamName, Is.EqualTo("predicate"));
                }

                [Test]
                public void Empty()
                {
                    var source = Empty<object>().Hide();
                    var (t, x) = source.LastOrNone(BreakingFunc.Of<object, bool>());
                    Assert.That(t, Is.False);
                    Assert.That(x, Is.Null);
                }

                public class One
                {
                    static readonly IEnumerable<int> Source = MoreEnumerable.From(() => 123);

                    [Test]
                    public void Match()
                    {
                        var (t, x) = Source.LastOrNone(x => x > 100);
                        Assert.That(t, Is.True);
                        Assert.That(x, Is.EqualTo(123));
                    }

                    [Test]
                    public void NoneMatch()
                    {
                        var (t, x) = Source.LastOrNone(_ => false);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }
                }

                public class Many
                {
                    static readonly IEnumerable<int> Source =
                        MoreEnumerable.From(() => 123, () => 456, () => 786);

                    [Test]
                    public void Match()
                    {
                        var (t, x) = Source.LastOrNone(x => x < 500);
                        Assert.That(t, Is.True);
                        Assert.That(x, Is.EqualTo(456));
                    }

                    [Test]
                    public void NoneMatch()
                    {
                        var (t, x) = Source.LastOrNone(_ => false);
                        Assert.That(t, Is.False);
                        Assert.That(x, Is.Zero);
                    }
                }
            }
        }
    }
}