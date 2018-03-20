# fuzzlib
fuzzlib template engine. productive, lightweight, flexible
## template
```javascript
<table border="0" cellpadding="0" cellspacing="0" width="610" align="center">
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> User name: {%username%} </font><br/>
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> order id: {%orderid%} </font><br/>
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> order price: {%sum%} </font><br/>
    <font size="5" style="font-size: 20px" face="arial, sans-serif" color="#834dad"> order date: {%date%} </font><br/>
    
    <div style="font-size: 20px" face="arial, sans-serif">
        <ul>
            {%for:cars%}
                <li>{%=cars.id%}</li>
                <li>{%=cars.model%}</li>
                <li>{%=cars.hp%}</li>
                <li>{%=cars.color%}</li>
                <li>{%=cars.price%}</li>
                <li>{%RemoveLink({%local%},{%=cars.id%},{%orderid%})%}</li>
            {%endfor%}
        </ul>
        <br />
        cars ids:
        <ul>
            {%for:cars%}
                <li>username:{%username%}, car id:{%=cars.id%}</li>
            {%endfor%}
        </ul>
        <br />
        {%HelloWorld()%}
    </div>
</table>
```
## data xml
```xml
<item>
    <username>Britney Spears</username>
    <orderid>123456</orderid>
    <sum>120000$</sum>
    <date>15.03.2018</date>
    <local>ru</local>
    <cars>
        <param>
            <id>1</id>
            <model>Audi</model>
            <hp>170</hp>
            <color>black</color>
            <price>65000$</price>                
        </param>
        <param>
            <id>2</id>
            <model>BMW</model>
            <hp>130</hp>
            <color>blue</color>
            <price>55000$</price>     
        </param>
    </cars>
</item>
```
## data dictionary
```javascript
new Dictionary<string, object>
{
    {"username", "Britney Spears"},
    {"orderid", "123456"},
    {"sum", "120000$"},
    {"date", "15.03.2018"},
    {"local", "ru"},
    {
        "cars", new List<Dictionary<string, object>>
        {
            new Dictionary<string, object>
            {
                {"cars.id", "1"},
                {"cars.model", "Audi"},
                {"cars.hp", "170"},
                {"cars.color", "black"},
                {"cars.price", "65000$"}
            },
            new Dictionary<string, object>
            {
                {"cars.id", "2"},
                {"cars.model", "BMW"},
                {"cars.hp", "130"},
                {"cars.color", "blue"},
                {"cars.price", "55000$"}
            }
        }
    }
};
```
## variables
```javascript
global variables
    {%username%}
    {%orderid%} 
    {%sum%} 
    {%date%} 


for loop variables
    {%for:cars%}        - start loop
        {%=cars.id%}
        {%=cars.model%}
        {%=cars.hp%}
        {%=cars.color%}
        {%=cars.price%}
    {%endfor%}          - end loop


custom methods
    {%RemoveLink({%local%},{%=cars.id%},{%orderid%})%}
    {%HelloWorld()%}
```

## contracts
```javascript
IDataConverter - data converter contract 

To use custom functions in the template
Implementation ITemplateInvokerMethod
Use [InvokerMethod("HelloWorld")]. "HelloWorld" - function name for template.

IInvokeMethodsMapper - custom methods mapper contract
```

## example
https://github.com/sbabiv/fuzzlib/tree/master/FuzzLib/Example
```C#
static void Main(string[] args)
{
    var tmplFilePath = Path.Combine(Environment.CurrentDirectory, @"Statics\Template.txt");
    var dataFilePath = Path.Combine(Environment.CurrentDirectory, @"Statics\Data.xml");

    //read template 
    var content = File.ReadAllText(tmplFilePath);
    //read message
    var xml = XDocument.Load(dataFilePath).Element("item");

    //convert message to Dictionary
    var converter = new LinqDataConverter();
    //var data = converter.ToData(xml);
    var data = GetData();

    //get custom methods
    var methodsMapper = new InvokeMethodsMapper();
    var parser = new TemplateParser(new FunctionsContainer());
    //set custom methods for parser
    parser.SetMethods(methodsMapper.GetMethods().ToArray());

    //create and compile template 
    var factory = new BinaryTemplatesFactory(parser);
    var template = factory.Compile(content, true);
    template.SetHandlers(() => methodsMapper.GetInvokers());

    //render content
    var output = template.Render(data);

    Console.WriteLine(output);
    Console.ReadLine();
}

private static Dictionary<string, object> GetData()
{
    return new Dictionary<string, object>
    {
        {"username", "Britney Spears"},
        {"orderid", "123456"},
        {"sum", "120000$"},
        {"date", "15.03.2018"},
        {"local", "ru"},
        {
            "cars", new List<Dictionary<string, object>>
            {
                new Dictionary<string, object>
                {
                    {"cars.id", "1"},
                    {"cars.model", "Audi"},
                    {"cars.hp", "170"},
                    {"cars.color", "black"},
                    {"cars.price", "65000$"}
                },
                new Dictionary<string, object>
                {
                    {"cars.id", "2"},
                    {"cars.model", "BMW"},
                    {"cars.hp", "130"},
                    {"cars.color", "blue"},
                    {"cars.price", "55000$"}
                }
            }
        }
    };
}
```

## Benchmark
https://github.com/sbabiv/fuzzlib/tree/master/FuzzLib/Benchmark     
Render Parallel.For     
Intel Core i7-2600K CPU 3.4GHz 8 cores      

```javascript
count: 10
Fuzz Elapsed ticks: 80252, millisecond: 24
Razor Elapsed ticks: 1361884, millisecond: 408
===================
count: 100
Fuzz Elapsed ticks: 1147, millisecond: 0
Razor Elapsed ticks: 88250, millisecond: 26
===================
count: 1000
Fuzz Elapsed ticks: 9373, millisecond: 2
Razor Elapsed ticks: 590320, millisecond: 177
===================
count: 10000
Fuzz Elapsed ticks: 77192, millisecond: 23
Razor Elapsed ticks: 6056490, millisecond: 1817
===================
count: 100000
Fuzz Elapsed ticks: 719481, millisecond: 215
Razor Elapsed ticks: 58974754, millisecond: 17701
===================
count: 1000000
Fuzz Elapsed ticks: 7517282, millisecond: 2256
Razor Elapsed ticks: 652056494, millisecond: 195719
===================
```
**template**
```javascript
Fuzzlib tmpl

<table border=0 cellpadding=0 cellspacing=0 width=610 align=center>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> User name: {%username%} </font><br/>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order id: {%orderid%} </font><br/>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order price: {%sum%} </font><br/>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order date: {%date%} </font><br/>
	
	{%for:cars%}
		<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car id: {%cars.id%} </font><br/>
		<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car price: {%cars.price%} </font><br/>
	{%endfor%}
</table>

RazorEngine tmpl

<table border=0 cellpadding=0 cellspacing=0 width=610 align=center>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> User name: @Model.username </font><br/>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order id: @Model.orderid </font><br/>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order price: @Model.sum </font><br/>
	<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order date: @Model.date </font><br/>

	@foreach (var item in Model.cars) {
		<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car id: @item.id </font><br/>
		<font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car price: @item.price </font><br/>
	}
</table>

