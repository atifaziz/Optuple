namespace Optuple.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Collections;
    using MoreLinq;
    using NUnit.Framework;
    using static System.Linq.Enumerable;
    using static OptionModule;
    using Enumerable = Collections.EnumerableExtensions;

    [TestFixture]
    public class EnumerableTests
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

        public class Filter
        {
            [Test]
            public void NullSource()
            {
                var e = Assert.Throws<ArgumentNullException>(() =>
                    Enumerable.Filter<object>(null));
                Assert.That(e.ParamName, Is.EqualTo("options"));
            }

            [Test]
            public void Laziness()
            {
                new BreakingSequence<(bool, object)>().Filter();
            }

            [Test]
            public void Empty()
            {
                var result = Empty<(bool, object)>().Filter();
                Assert.That(result, Is.Empty);
            }

            [Test]
            public void AllNone()
            {
                var result = Repeat(Option.None<int>(), 10).Filter();
                Assert.That(result, Is.Empty);
            }

            [Test]
            public void AllSome()
            {
                var xs = Range(1, 10);
                var result = xs.Select(Some).Filter();
                Assert.That(result, Is.EqualTo(xs));
            }

            [Test]
            public void NoneAndSome()
            {
                var xs = Range(1, 10);
                var result = xs.Select(x => x % 2 == 0 ? Some(x) : default).Filter();
                Assert.That(result, Is.EqualTo(new[] { 2, 4, 6, 8, 10 }));
            }
        }

        public class ListAll
        {
            [Test]
            public void NullSource()
            {
                var e = Assert.Throws<ArgumentNullException>(() =>
                    Enumerable.ListAll<object>(null));
                Assert.That(e.ParamName, Is.EqualTo("options"));
            }

            [Test]
            public void Empty()
            {
                var (t, list) = Empty<(bool, object)>().ListAll();
                Assert.That(t, Is.True);
                Assert.That(list, Is.Empty);
            }

            [Test]
            public void AllNone()
            {
                var (t, list) = Repeat(Option.None<int>(), 10).ListAll();
                Assert.That(t, Is.False);
                Assert.That(list, Is.Null);
            }

            [Test]
            public void AllSome()
            {
                var xs = Range(1, 10);
                var (t, list) = xs.Select(Some).ListAll();
                Assert.That(t, Is.True);
                Assert.That(list, Is.EqualTo(xs));
            }

            [Test]
            public void NoneAndSome()
            {
                var xs =
                    MoreEnumerable.From(
                        () => Some(1),
                        () => Some(2),
                        () => Some(3),
                        None<int>,
                        BreakingFunc.Of<(bool, int)>());

                var (t, list) = xs.ListAll();
                Assert.That(t, Is.False);
                Assert.That(list, Is.Null);
            }
        }
    }
}
