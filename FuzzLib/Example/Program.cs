using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using FuzzLib.Binary;
using FuzzLib.Data;
using FuzzLib.Functions;
using FuzzLib.Parser;
using FuzzLib.TemplateInvokerMethods;

namespace Example
{
    class Program
    {
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
            var data = converter.ToData(xml);

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
    }
}
