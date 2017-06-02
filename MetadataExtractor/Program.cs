using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Newtonsoft.Json.Linq;
using ValidationMetadataExtractor;

namespace MetadataProvider
{
    class Program
    {
        static void Main(string[] args)
        {
            ObjectMetadata metadata = new ObjectMetadata(typeof(Person));

            String jsonMetadata = JObject.FromObject(metadata).ToString();

            Console.WriteLine(jsonMetadata);
            
            new MetadataExporter().ExportMetadata<Person>();
        }
    }

    class MetadataExporter
    {
        public void ExportMetadata<T>()
        {
            ObjectMetadata metadata = new ObjectMetadata(typeof(T));
            String jsonMetadata = JObject.FromObject(metadata).ToString();
            using (var f = File.CreateText($"..\\..\\..\\{typeof(T).Name}.metadata.json"))
            {
                f.WriteLine(jsonMetadata);
            }
        }
    }

    public class Person
    {
        [Required(AllowEmptyStrings = true, ErrorMessage = "asd")]
        [MaxLength(30)]
        public String Name { get; set; }

        [Required]
        [MaxLength(30)]
        public String LastName { get; set; }

        [Range(0, 150)]
        public Int32 Age { get; set; }
    }
}