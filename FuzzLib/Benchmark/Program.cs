using FuzzLib.Binary;
using FuzzLib.Functions;
using FuzzLib.Parser;
using FuzzLib.TemplateInvokerMethods;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        public class Car
        {
            public string id;
            public string price;
        }

        static void Main(string[] args)
        {
            var razor = new Razor();
            var fuzz = new Fuzz();
            
            int count = 1;
            for (int i = 0; i < 6; i++)
            {
                count *= 10;
                Console.WriteLine($"count: {count}");

                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                Parallel.For(0, count, f =>
                {
                    fuzz.Render(new Dictionary<string, object>
                    {
                        { "username", Guid.NewGuid().ToString() },
                        { "orderid", Guid.NewGuid().ToString() },
                        { "sum", Guid.NewGuid().ToString() },
                        { "date", Guid.NewGuid().ToString() },
                        {"cars", new List<Dictionary<string, object>>
                            {
                                new Dictionary<string, object>
                                {
                                    { "cars.id", Guid.NewGuid().ToString() },
                                    { "cars.price", Guid.NewGuid().ToString() },
                                },
                                new Dictionary<string, object>
                                {
                                    { "cars.id", Guid.NewGuid().ToString() },
                                    { "cars.price", Guid.NewGuid().ToString() },
                                }
                            }
                        }
                    });
                });
                stopWatch.Stop();
                Console.WriteLine($"Fuzz Elapsed ticks: {stopWatch.ElapsedTicks}, millisecond: {stopWatch.ElapsedMilliseconds}");

                stopWatch.Restart();
                Parallel.For(0, count, f =>
                {
                    var cars = new List<Car>
                    {
                        new Car {id = Guid.NewGuid().ToString(), price = Guid.NewGuid().ToString() },
                        new Car {id = Guid.NewGuid().ToString(), price = Guid.NewGuid().ToString() }
                    };

                    razor.Render(new
                    {
                        username = Guid.NewGuid().ToString(),
                        orderid = Guid.NewGuid().ToString(),
                        sum = Guid.NewGuid().ToString(),
                        date = Guid.NewGuid().ToString(),
                        cars = cars
                    });
                });
                stopWatch.Stop();
                Console.WriteLine($"Razor Elapsed ticks: {stopWatch.ElapsedTicks}, millisecond: {stopWatch.ElapsedMilliseconds}");
                Console.WriteLine($"===================");
            }

            Console.ReadLine();
        }
    }
    
    public class Razor
    {
        private const string tmpl = @"<table border=0 cellpadding=0 cellspacing=0 width=610 align=center>
	                                    <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> User name: @Model.username </font><br/>
                                        <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order id: @Model.orderid </font><br/>
                                        <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order price: @Model.sum </font><br/>
	                                    <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order date: @Model.date </font><br/>

                                        @foreach (var item in Model.cars) {
                                            <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car id: @item.id </font><br/>
	                                        <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car price: @item.price </font><br/>
                                        }
                                    </table>";
        public Razor()
        {
            RazorEngine.Razor.Compile(tmpl, "razorTest");
        }

        public void Render(object model)
        {
            RazorEngine.Razor.Run("razorTest", model);
        }
    }

    public class Fuzz
    {
        private const string tmpl = @"<table border=0 cellpadding=0 cellspacing=0 width=610 align=center>
	                                    <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> User name: {%username%} </font><br/>
                                        <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order id: {%orderid%} </font><br/>
                                        <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order price: {%sum%} </font><br/>
	                                    <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> order date: {%date%} </font><br/>

                                        {%for:cars%}
                                            <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car id: {%=cars.id%} </font><br/>
	                                        <font size=5 style=font-size: 20px face=arial, sans-serif color=#834dad> car price: {%=cars.price%} </font><br/>
                                        {%endfor%}
                                    </table>";

        private BinaryTemplate _template;

        public Fuzz()
        {
            var methodsMapper = new InvokeMethodsMapper();
            var parser = new TemplateParser(new FunctionsContainer());
            //set custom methods for parser
            parser.SetMethods(methodsMapper.GetMethods().ToArray());

            //create and compile template 
            var factory = new BinaryTemplatesFactory(parser);
            _template = factory.Compile(tmpl, true);
            //_template.SetHandlers(() => methodsMapper.GetInvokers());
        }

        public void Render(Dictionary<string, object> model)
        {
            var str = _template.Render(model);
        }
    }
}
