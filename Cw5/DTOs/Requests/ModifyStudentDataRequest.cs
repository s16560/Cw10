using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.DTOs.Requests
{
    public class ModifyStudentDataRequest
    {
        [Required]
        [RegularExpression("^s[0-9]+$")]
        public string IndexNumber { get; set; }

        [Required]      
        public string FirstName { get; set; }

        [Required]       
        public string LastName { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }
      
        [Required]
        public int IdEnrollment { get; set; }

    }
}
