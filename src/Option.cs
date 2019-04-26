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

namespace Optuple
{
    using System;
    using System.Collections.Generic;

    static partial class Option
    {
        public static (bool HasValue, T Value) Some<T>(T value) => (true, value);
        public static (bool HasValue, T Value) None<T>() => default;

        public static (bool HasValue, T Value) From<T>(bool isSome, T value) => isSome ? Some(value) : None<T>();
        public static (bool HasValue, T Value) From<T>((bool, T) option) => option.ToOption();

        public static (bool HasValue, T Value) ToOption<T>(this (bool, T) option) =>
            option switch { var (f, v) when f => Some(v), _ => None<T>() };

        public static (bool HasValue, T Value) ToOption<T>(this T? value) where T : struct =>
            value is T x ? Some(x) : None<T>();

        public static (bool HasValue, T Value) Flagged<T>(this (bool, T Value) option) =>
            option.IsSome() ? (true, option.Value) : default;

        public static bool IsSome<T>(this (bool, T) option) => option switch { var (f, _) when f => true , _ => false };
        public static bool IsNone<T>(this (bool, T) option) => option switch { var (f, _) when f => false, _ => true  };

        public static (bool HasValue, T Value) SomeWhen<T>(T value, Func<T, bool> predicate) => predicate(value) ? Some(value) : None<T>();
        public static (bool HasValue, T Value) NoneWhen<T>(T value, Func<T, bool> predicate) => predicate(value) ? None<T>() : Some(value);

        public static TResult Match<T, TResult>(this (bool, T) option, Func<T, TResult> some, Func<TResult> none) =>
            option switch { var (f, v) when f => some(v), _ => none() };

        public static void Match<T>(this (bool, T) option, Action<T> some, Action none)
        { if (option.IsSome()) { var (_, v) = option; some(v); } else none(); }

        public static void Do<T>(this (bool, T) option, Action<T> some) =>
            option.Match(some, delegate { });

        public static (bool HasValue, TResult Value) Bind<T, TResult>(this (bool, T) first, Func<T, (bool, TResult)> function) =>
            first switch { var (f, v) when f => function(v), _ => None<TResult>() };

        public static (bool HasValue, TResult Value) Map<T, TResult>(this (bool, T) option, Func<T, TResult> mapper) =>
            option.Bind(x => Some(mapper(x)));

        public static T Get<T>(this (bool, T) option) =>
            option switch { var (f, v) when f => v, _ => throw new ArgumentException(nameof(option)) };

        public static T OrDefault<T>(this (bool, T) option) =>
            option.Or(default);

        public static T Or<T>(this (bool, T) option, T none) =>
            option switch { var (f, v) when f => v, _ => none };

        public static int Count<T>(this (bool, T) option) => option.IsSome() ? 1 : 0;

        public static bool Exists<T>(this (bool, T) option, Func<T, bool> predicate) =>
            option switch { var (f, v) when f => predicate(v), _ => false };

        public static (bool HasValue, T Value) Filter<T>(this (bool, T) option, Func<T, bool> predicate) =>
            option.Bind(x => predicate(x) ? Some(x) : None<T>());

        public static T? ToNullable<T>(this (bool, T) option) where T : struct =>
            option switch { var (f, v) when f => (T?) v, _ => null };

        public static IEnumerable<T> ToEnumerable<T>(this (bool, T) option) =>
            option.Match(Seq, System.Linq.Enumerable.Empty<T>);

        static IEnumerable<T> Seq<T>(T x) { yield return x; }
    }
}
