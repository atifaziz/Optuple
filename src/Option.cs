#region Copyright 2018 Atif Aziz. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace JustNothing
{
    using System;
    using System.Collections.Generic;

    static partial class Option
    {
        public enum Case { None, Some }

        public static (Case Case, T Value) Some<T>(T value) => (Case.Some, value);
        public static (Case Case, T Value) None<T>() => (Case.None, default(T));

        public static (Case Case, T Value) From<T>(bool isSome, T value) => isSome ? Some(value) : None<T>();

        public static (Case Case, T Value) ToOptional<T>(this (bool HasValue, T Value) option) =>
            option.HasValue ? Some(option.Value) : None<T>();

        public static (Case Case, T Value) ToOptional<T>(this T value) =>
            value == null ? None<T>() : Some(value);

        public static (bool HasValue, T Value) Flagged<T>(this (Case Case, T Value) option) =>
            option.IsSome() ? (true, option.Value) : (false, default(T));

        public static bool IsSome<T>(this (Case Case, T Value) option) => option.Case == Case.Some;
        public static bool IsNone<T>(this (Case Case, T Value) option) => option.Case == Case.None;

        public static (Case Case, T Value) SomeWhen<T>(T value, Func<T, bool> predicate) => predicate(value) ? Some(value) : None<T>();
        public static (Case Case, T Value) NoneWhen<T>(T value, Func<T, bool> predicate) => predicate(value) ? None<T>() : Some(value);

        public static TResult Match<T, TResult>(this (Case Case, T Value) option, Func<T, TResult> some, Func<TResult> none) =>
            option.IsSome() ? some(option.Value) : none();

        public static void Match<T>(this (Case Case, T Value) option, Action<T> some, Action none)
        { if (option.IsSome()) some(option.Value); else none(); }

        public static void Do<T>(this (Case Case, T Value) option, Action<T> some) =>
            option.Match(some, delegate { });

        public static (Case Case, TResult Value) Bind<T, TResult>(this (Case Case, T Value) first, Func<T, (Case, TResult)> function) =>
            first.IsSome() ? function(first.Value) : None<TResult>();

        public static (Case Case, TResult Value) Map<T, TResult>(this (Case Case, T Value) option, Func<T, TResult> mapper) =>
            option.Bind(x => Some(mapper(x)));

        public static T Get<T>(this (Case Case, T Value) option) =>
            option.IsSome() ? option.Value : throw new ArgumentException(nameof(option));

        public static T OrDefault<T>(this (Case Case, T Value) option) =>
            option.Or(default(T));

        public static T Or<T>(this (Case Case, T Value) option, T none) =>
            option.IsSome() ? option.Value : none;

        public static int Count<T>(this (Case, T) option) => option.IsSome() ? 1 : 0;

        public static bool Exists<T>(this (Case Case, T Value) option, Func<T, bool> predicate) =>
            option.IsSome() && predicate(option.Value);

        public static (Case Case, T Value) Filter<T>(this (Case, T) option, Func<T, bool> predicate) =>
            option.Bind(x => predicate(x) ? Some(x) : None<T>());

        public static T? ToNullable<T>(this (Case Case, T Value) option) where T : struct =>
            option.IsSome() ? (T?) option.Value : null;

        // LINQ support

        public static (Case Case, TResult Value) Select<T, TResult>(this (Case, T) option, Func<T, TResult> selector) =>
            option.Map(selector);

        public static (Case Case, T Value) Where<T>(this (Case, T) option, Func<T, bool> predicate) =>
            option.Filter(predicate);

        public static (Case Case, TResult Value) SelectMany<T, TResult>(this (Case, T) first, Func<T, (Case, TResult)> secondSelector) =>
            first.Bind(secondSelector);

        public static (Case Case, TResult Value) SelectMany<TFirst, TSecond, TResult>(this (Case, TFirst) first, Func<TFirst, (Case, TSecond)> secondSelector, Func<TFirst, TSecond, TResult> resultSelector) =>
            first.Bind(x => secondSelector(x).Map(y => resultSelector(x, y)));

        public static (Case Case, TResult Value) Cast<T, TResult>(this (Case, T) option) =>
            from x in option
            select (TResult) (object) x;

        public static bool All<T>(this (Case, T) option, Func<T, bool> predicate) =>
            option.Match(predicate, () => true);

        public static T[] ToArray<T>(this (Case, T) option) =>
            option.Match(x => new[] { x }, () => EmptyArray<T>.Value);

        public static List<T> ToList<T>(this (Case, T) option) =>
            option.Match(x => new List<T> { x }, () => new List<T>());

        public static IEnumerable<T> ToEnumerable<T>(this (Case, T) option) =>
            option.Match(Seq, System.Linq.Enumerable.Empty<T>);

        static IEnumerable<T> Seq<T>(T x) { yield return x; }

        static class EmptyArray<T>
        {
            public static readonly T[] Value = new T[0];
        }
    }
}
