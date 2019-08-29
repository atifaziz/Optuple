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

namespace Optuple.RegularExpressions
{
    using System;
    using System.Text.RegularExpressions;
    using static OptionModule;

    static partial class RegexExtensions
    {
        public static (bool Success, T Value) ToOption<T>(this T group) where T : Group
            => group == null ? throw new ArgumentNullException(nameof(group))
             : group.Success ? Some(group)
             : default;
    }
}
