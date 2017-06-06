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
            ObjectValidationMetadata validationMetadata = new ObjectValidationMetadata(typeof(Person));

            String jsonMetadata = JObject.FromObject(validationMetadata).ToString();

            Console.WriteLine(jsonMetadata);
            
            new MetadataExporter().ExportMetadata<Person>();
        }
    }

    class MetadataExporter
    {
        public void ExportMetadata<T>()
        {
            ObjectValidationMetadata validationMetadata = new ObjectValidationMetadata(typeof(T));
            String jsonMetadata = JObject.FromObject(validationMetadata).ToString();
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