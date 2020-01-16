# logsplit
Split string lines into objects


The splitting delimeters have to be identifieable


### Functions
len(COUNT)
The function len defines the length of the string that has to be parsed.

```
var parser = new Parser("%{fieldname:len(11)}");
var result = parser.Parse("this is a test");

result.Single().Should().BeEquivalentTo(new { Key = "fieldname", Value = "this is a t" });
````
