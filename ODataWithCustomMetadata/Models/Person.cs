using System;
using System.ComponentModel.DataAnnotations;

namespace ODataWithCustomMetadata.Models
{
    public class Person
    {
        [Required]
        [MinLength(3, ErrorMessage = "Very short")]
        [MaxLength(30, ErrorMessage = "Very long")]

        public String FirstName { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Very short")]
        [MaxLength(30, ErrorMessage = "Very long")]
        public String LastName { get; set; }

        [Range(18, 150, ErrorMessage = "Age is not in range 18..150")]
        public Int32 Age { get; set; }
    }
}