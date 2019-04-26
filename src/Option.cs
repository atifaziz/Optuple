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

        public static (bool HasValue, T Value) ToOption<T>(this (bool HasValue, T Value) option) =>
            option.HasValue ? Some(option.Value) : None<T>();

        public static (bool HasValue, T Value) ToOption<T>(this T? value) where T : struct =>
            value is T x ? Some(x) : None<T>();

        public static (bool HasValue, T Value) Flagged<T>(this (bool HasValue, T Value) option) =>
            option.IsSome() ? (true, option.Value) : (false, default);

        public static bool IsSome<T>(this (bool HasValue, T Value) option) => option.HasValue;
        public static bool IsNone<T>(this (bool HasValue, T Value) option) => !option.HasValue;

        public static (bool HasValue, T Value) SomeWhen<T>(T value, Func<T, bool> predicate) => predicate(value) ? Some(value) : None<T>();
        public static (bool HasValue, T Value) NoneWhen<T>(T value, Func<T, bool> predicate) => predicate(value) ? None<T>() : Some(value);

        public static TResult Match<T, TResult>(this (bool HasValue, T Value) option, Func<T, TResult> some, Func<TResult> none) =>
            option.IsSome() ? some(option.Value) : none();

        public static void Match<T>(this (bool HasValue, T Value) option, Action<T> some, Action none)
        { if (option.IsSome()) some(option.Value); else none(); }

        public static void Do<T>(this (bool HasValue, T Value) option, Action<T> some) =>
            option.Match(some, delegate { });

        public static (bool HasValue, TResult Value) Bind<T, TResult>(this (bool HasValue, T Value) first, Func<T, (bool, TResult)> function) =>
            first.IsSome() ? function(first.Value) : None<TResult>();

        public static (bool HasValue, TResult Value) Map<T, TResult>(this (bool HasValue, T Value) option, Func<T, TResult> mapper) =>
            option.Bind(x => Some(mapper(x)));

        public static T Get<T>(this (bool HasValue, T Value) option) =>
            option.IsSome() ? option.Value : throw new ArgumentException(nameof(option));

        public static T OrDefault<T>(this (bool HasValue, T Value) option) =>
            option.Or(default);

        public static T Or<T>(this (bool HasValue, T Value) option, T none) =>
            option.IsSome() ? option.Value : none;

        public static int Count<T>(this (bool, T) option) => option.IsSome() ? 1 : 0;

        public static bool Exists<T>(this (bool HasValue, T Value) option, Func<T, bool> predicate) =>
            option.IsSome() && predicate(option.Value);

        public static (bool HasValue, T Value) Filter<T>(this (bool, T) option, Func<T, bool> predicate) =>
            option.Bind(x => predicate(x) ? Some(x) : None<T>());

        public static T? ToNullable<T>(this (bool HasValue, T Value) option) where T : struct =>
            option.IsSome() ? (T?) option.Value : null;

        public static IEnumerable<T> ToEnumerable<T>(this (bool, T) option) =>
            option.Match(Seq, System.Linq.Enumerable.Empty<T>);

        static IEnumerable<T> Seq<T>(T x) { yield return x; }
    }
}
