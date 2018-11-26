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
    using Case = System.Boolean;

    static partial class Optional
    {
        public static (Case Case, TResult Value) Select<T, TResult>(this (Case, T) option, Func<T, TResult> selector) =>
            option.Map(selector);

        public static (Case Case, T Value) Where<T>(this (Case, T) option, Func<T, bool> predicate) =>
            option.Filter(predicate);

        public static (Case Case, TResult Value) SelectMany<T, TResult>(this (Case, T) first, Func<T, (Case, TResult)> secondSelector) =>
            first.Bind(secondSelector);

        public static (Case Case, TResult Value) SelectMany<TFirst, TSecond, TResult>(this (Case, TFirst) first, Func<TFirst, (Case, TSecond)> secondSelector, Func<TFirst, TSecond, TResult> resultSelector) =>
            first.Bind(x => secondSelector(x).Map(y => resultSelector(x, y)));

        public static (Case Case, T Value) Cast<T>(this (Case, object) option) =>
            from x in option
            select (T) x;

        public static bool All<T>(this (Case, T) option, Func<T, bool> predicate) =>
            option.Match(predicate, () => true);

        public static T[] ToArray<T>(this (Case, T) option) =>
            option.Match(x => new[] { x }, () => EmptyArray<T>.Value);

        public static List<T> ToList<T>(this (Case, T) option) =>
            option.Match(x => new List<T> { x }, () => new List<T>());

        static class EmptyArray<T>
        {
            public static readonly T[] Value = new T[0];
        }
    }
}
