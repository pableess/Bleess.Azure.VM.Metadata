using Bleess.Azure.VM.Metadata;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServiceCollection sc = new ServiceCollection();
            sc.AddAzureVmMetadataClient();

            var sp = sc.BuildServiceProvider();
            var client = sp.GetRequiredService<IVmMetadataClient>();

            Console.WriteLine("Versions");
            var versions = await client.GetVersions();
            Console.WriteLine(versions.DumpAsYaml());
            Console.WriteLine();

            Console.WriteLine("Instance");
            var data = await client.GetVmInstanceMetadata();
            Console.WriteLine(data.DumpAsYaml());
            Console.WriteLine();

            Console.WriteLine("Attested");
            var attestedData = await client.GetAttestedInstanceMetadata();
            Console.WriteLine(attestedData.DumpAsYaml());
            Console.WriteLine();


            Console.WriteLine("Events");
            var events = await client.GetScheduledEvents();
            events = await client.GetScheduledEvents(); // test caching
            Console.WriteLine(events.DumpAsYaml());

            if (events.Events?.Count > 0)
            {
                Console.WriteLine($"There are {events.Events.Count} scheduled events.  Would you like to start the events? [y/n]");

                var key = Console.ReadKey();
                if (key.KeyChar == 'y' || key.KeyChar == 'Y')
                {
                    await client.StartEvents(events.Events);
                    Console.WriteLine("Events started !");
                }
            }
        }
    }
    public static class YamlExtensions 
    {
        public static string DumpAsYaml(this object o)
        {
            if (o == null)
            {
                return null;
            }
            var stringBuilder = new StringBuilder();
            var serializer = new Serializer();
            serializer.Serialize(new IndentedTextWriter(new StringWriter(stringBuilder)), o);
            return stringBuilder.ToString();
        }
    }
}
