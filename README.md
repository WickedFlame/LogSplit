# LogSplit
[![Build Status](https://travis-ci.org/WickedFlame/LogSplit.svg?branch=master)](https://travis-ci.org/WickedFlame/LogSplit)
[![Build status](https://ci.appveyor.com/api/projects/status/5xsg81nvy8xwval0?svg=true)](https://ci.appveyor.com/project/chriswalpen/logsplit)

Split string lines into objects

The splitting delimeters have to be identifieable

```csharp
var parser = new Parser("%{date} [%{level}] [%{pc}] %{message:len(*)}");
var result = parser.Parse("01.01.2020 [INFO] [PC-NAME] The log message");

result[0].Should().BeEquivalentTo(new { Key = "date", Value = "01.01.2020" });
result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
result[2].Should().BeEquivalentTo(new { Key = "pc", Value = "PC-NAME" });
result[3].Should().BeEquivalentTo(new { Key = "message", Value = "The log message" });
```

### Functions
len(COUNT)
The function len defines the length of the string that has to be parsed.

```csharp
var parser = new Parser("%{fieldname:len(11)}");
var result = parser.Parse("this is a test");

result.Single().Should().BeEquivalentTo(new { Key = "fieldname", Value = "this is a t" });
```

len(*)
Take the rest of the string
```csharp
var parser = new Parser("%{first} %{rest:len(*)}");
var result = parser.Parse("this is a test");

result.First().Should().BeEquivalentTo(new { Key = "first", Value = "this" });
result.First().Should().BeEquivalentTo(new { Key = "rest", Value = "is a test" });
```