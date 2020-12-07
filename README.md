# LogSplit
[![Build Status](https://travis-ci.org/WickedFlame/LogSplit.svg?branch=master)](https://travis-ci.org/WickedFlame/LogSplit)
[![Build status](https://ci.appveyor.com/api/projects/status/5xsg81nvy8xwval0?svg=true)](https://ci.appveyor.com/project/chriswalpen/logsplit)

Split string lines into objects

Lies are split by providing a property with the propertyname wrapped in %{...} followed by a delimeter. 
The delimeter in the following is the whitespace.
```csharp
var parser = new Parser("%{first} %{second}");
var result = parser.Parse("Word1 Word2");

result["first"].Should().Be("Word1");
result["second"].Should().Be("Word2");
```
Map to a Type
```csharp
var parser = new Parser("%{first} %{second}");
var result = parser.Parse<WordType>("Word1 Word2");

result.First.Should().Be("Word1");
result.Second.Should().Be("Word2");
```

The splitting delimeters have to be identifieable. The line is split at the next possible delimeter

```csharp
var parser = new Parser("%{date} [%{level}] [%{pc}] %{message:len(*)}");
var result = parser.Parse("01.01.2020 [INFO] [PC-NAME] The log message");

result[0].Should().BeEquivalentTo(new { Key = "date", Value = "01.01.2020" });
result[1].Should().BeEquivalentTo(new { Key = "level", Value = "INFO" });
result[2].Should().BeEquivalentTo(new { Key = "pc", Value = "PC-NAME" });
result[3].Should().BeEquivalentTo(new { Key = "message", Value = "The log message" });
```

String extensions
```csharp
var result = "01.01.2020 [INFO] [PC-NAME] The log message".Parse("%{date} [%{level}] [%{pc}] %{message:len(*)}");

var logItem = "01.01.2020 [INFO] [PC-NAME] The log message".Parse<LogItem>("%{Date} [%{Level}] [%{Pc}] %{Message:len(*)}");
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

### Result
Items in the Result can be accessed by an int indexer or by providing the string key in the indexer. 
The int indexer returnes th part while the key indexer provides the value
```csharp
var parser = new Parser("%{date} [%{level}] [%{pc}] %{message:len(*)}");
var result = parser.Parse("01.01.2020 [INFO] [PC-NAME] The log message");

result[0].Should().BeEquivalentTo(new { Key = "date", Value = "01.01.2020" });
result["date"].Should().Be("01.01.2020");
```