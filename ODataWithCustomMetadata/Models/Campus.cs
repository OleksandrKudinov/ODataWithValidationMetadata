using System;
using System.ComponentModel.DataAnnotations;

namespace ODataWithCustomMetadata.Models
{
    public class Campus
    {
        [Required(ErrorMessage = "Campus name is required")]
        public String CampusName { get; set; }

        [MaxLength(50, ErrorMessage = "Address is too long")]
        public String Address { get; set; }

        [MinLength(12)]
        [MaxLength(12)]
        public String Phone { get; set; }
    }
}