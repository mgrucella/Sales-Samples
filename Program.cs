using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InRule.Runtime;

namespace HelloWorld
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter your name:");
            var name = Console.ReadLine();

            //Xml input
            var input = String.Format("<Test><Name>{0}</Name></Test>", name);

            ExecuteBusinessRules(input);

            Console.Read();
        }

        static void ExecuteBusinessRules(string input)
        {
            //Option 1: Retreive rule application from the catalog
            // - URL is for the catalog service
            //var ruleAppRef = new CatalogRuleApplicationReference("http://localhost/InRuleCatalogService/Service.svc", "HelloWorld", "admin", "password");
            
            //Option 2: use a file based rule application
            // - Fully qualified path to the *.ruleappx file
            var ruleAppRef = new FileSystemRuleApplicationReference(@"HelloWorld.ruleappx");

            using (var session = new RuleSession(ruleAppRef))
            {
                //You cam set logging options. By default, all logging is turned off
                //session.Settings.LogOptions = EngineLogOptions.RuleTrace | EngineLogOptions.StateChanges;

                //You have 3 options for passing in data:
                // 1. Xml
                // 2. Json
                // 3. .NET object instance
                //In all 3 situations, it must map to the data structure (Entities) in the rule application. 
                var entity = session.CreateEntity("Test", input);

                session.ApplyRules();

                //Post rule execution, you can get the output state:
                // 1. Xml
                // 2. Json
                //If you passed in a .NET object instance, you don't need to do anything as the rule engine will operate on it in memory.
                var outputXml = entity.GetXml();
                Console.WriteLine(outputXml);

                var outputJson = entity.GetJson();
                Console.WriteLine(outputJson);

                //You can get a list of Notifications...
                //session.GetNotifications();

                //You can get a list of Validations...
                //session.GetValidations();


            }
        }
    }
}
