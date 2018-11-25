# Optuple

Optuple is a .NET Standard library that enables a tuple of Boolean and some
type (`T`), i.e. `(bool, T)`, to have the same semantics as an option type
found in most functional languages.

An option type is a discriminated union that either represents the absence of
a value (_none_) or the value that's present (_some_). For example, F# has
such a type that is defined as:

```f#
type Option<T> = | None | Some of T
```

Optuple, however, does not define any such type. Instead it primarily supplies
extension methods for any `(bool, T)` (like `Match`, `Map` and more) to be
treated like an option type. Suppose a value `x` of type `T`, then
`(true, x)` will be treated like `Some x` and `(false, _)` will
be treated like `None`. Note that in the _none case_, when the first
element is `false`, Optuple completely ignores and discards the second
element.

A library that wants to expose optional values needs no dependency on Optuple.
It can just expose `(bool, T)` for some type `T`. The client of the library
can use Optuple independently to get &ldquo;optional semantics&rdquo;.


## Usage

### Using the library

To use Optuple simply import the following namespace:

```c#
using Optuple;
```

An auxiliary namespace is also provided:

```c#
using Optuple.Linq; // Linq query syntax support
```

### Creating optional values

The most basic way to create optional values is to use the static `Option`
class:

```c#
var none = Option.None<int>();
var some = Option.Some(42);
```

Similarly, a more general extension method is provided, allowing a specified
predicate:

```c#
string str = "foo";
var none = Option.SomeWhen(str, s => s == "bar"); // Return None if predicate is violated
var none = Option.NoneWhen(str, s => s == "foo"); // Return None if predicate is satisfied
```

Clearly, optional values are conceptually quite similar to nullables. Hence, a
method is provided to convert a nullable into an optional value:

```c#
int? nullableWithoutValue = null;
int? nullableWithValue = 2;
var none = nullableWithoutValue.ToOption();
var some = nullableWithValue.ToOption();
```

### Retrieving values

When retrieving values, Optuple forces you to consider both cases (that is
if a value is present or not).

Firstly, it is possible to check if a value is actually present:

```c#
var isSome = option.IsSome(); //returns true if a value is present
var isNone = option.IsNone(); //returns true if a value is not present
```

If you want to check if an option `option` satisfies some predicate, you can
use the`Exists` method.

```c#
var isGreaterThanHundred = option.Exists(val => val > 100);
```

The most basic way to retrieve a value from an `Option<T>` is the following:

```c#
// Returns the value if present, or otherwise an alternative value (42)
var value = option.Or(42);
```

Similarly, the `OrDefault` function simply retrieves the default value for
a given type:

```c#
var none = Option.None<int>();
var value = none.OrDefault(); // Value 0
```

```c#
var none = Option.None<string>();
var value = none.OrDefault(); // null
```


In more elaborate scenarios, the `Match` method evaluates a specified
function:

```c#
// Evaluates one of the provided functions and returns the result
var value = option.Match(x => x + 1, () => 42);

// Or written in a more functional style (pattern matching)
var value = option.Match(
  some: x => x + 1,
  none: () => 42
);
```

There is a similar `Match` function to simply induce side-effects:

```c#
// Evaluates one of the provided actions
option.Match(x => Console.WriteLine(x), () => Console.WriteLine(42));

// Or pattern matching style as before
option.Match(
  some: x => Console.WriteLine(x),
  none: () => Console.WriteLine(42)
);
```

### Transforming and filtering values

A few extension methods are provided to safely manipulate optional values.

The `Map` function transforms the inner value of an option. If no value is
present none is simply propagated:

```c#
var none = Option.None<int>();
var stillNone = none.Map(x => x + 10);

var some = Option.Some(42);
var somePlus10 = some.Map(x => x + 10);
```

Finally, it is possible to perform filtering. The `Filter` function returns
none, if the specified predicate is not satisfied. If the option is already
none, it is simply returned as is:

```c#
var none = Option.None<int>();
var stillNone = none.Filter(x => x > 10);

var some = Option.Some(10);
var stillSome = some.Filter(x => x == 10);
var none = some.Filter(x => x != 10);
```

### Enumerating options

[comment]: # (Move somewhere?!)

Although options deliberately don't act as enumerables, you can easily convert
an option to an enumerable by calling the `ToEnumerable()` method:

```c#
var enumerable = option.ToEnumerable();
```

### Working with LINQ query syntax

Optuple supports LINQ query syntax, to make the above transformations
somewhat cleaner.

To use LINQ query syntax you must import the following namespace:

```c#
using Optuple.Linq;
```

This allows you to do fancy stuff such as:

```c#
var personWithGreenHair =
  from person in FindPersonById(10)
  from hairstyle in GetHairstyle(person)
  from color in ParseStringToColor("green")
  where hairstyle.Color == color
  select person;
```

In general, this closely resembles a sequence of calls to `FlatMap` and
`Filter`. However, using query syntax can be a lot easier to read in complex
cases.

### Equivalence and comparison

Two optional values are equal if the following is satisfied:

* The two options have the same type
* Both are none, both contain null values, or the contained values are equal
