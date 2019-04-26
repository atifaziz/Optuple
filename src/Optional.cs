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

namespace Optuple.Linq
{
    using System;
    using System.Collections.Generic;

    static partial class Optional
    {
        public static (bool HasValue, TResult Value) Select<T, TResult>(this (bool, T) option, Func<T, TResult> selector) =>
            option.Map(selector);

        public static (bool HasValue, T Value) Where<T>(this (bool, T) option, Func<T, bool> predicate) =>
            option.Filter(predicate);

        public static (bool HasValue, TResult Value) SelectMany<T, TResult>(this (bool, T) first, Func<T, (bool, TResult)> secondSelector) =>
            first.Bind(secondSelector);

        public static (bool HasValue, TResult Value) SelectMany<TFirst, TSecond, TResult>(this (bool, TFirst) first, Func<TFirst, (bool, TSecond)> secondSelector, Func<TFirst, TSecond, TResult> resultSelector) =>
            first.Bind(x => secondSelector(x).Map(y => resultSelector(x, y)));

        public static (bool HasValue, T Value) Cast<T>(this (bool, object) option) =>
            from x in option
            select (T) x;

        public static bool All<T>(this (bool, T) option, Func<T, bool> predicate) =>
            option.Match(predicate, () => true);

        public static T[] ToArray<T>(this (bool, T) option) =>
            option.Match(x => new[] { x }, () => EmptyArray<T>.Value);

        public static List<T> ToList<T>(this (bool, T) option) =>
            option.Match(x => new List<T> { x }, () => new List<T>());

        static class EmptyArray<T>
        {
            public static readonly T[] Value = new T[0];
        }
    }
}
