using System;
using System.ComponentModel.DataAnnotations;

namespace Cw5.DTOs.Requests
{
    public class PromoteStudentsRequest
    {

        [Required]
        public string Studies { get; set; }

        [Required]       
        public Nullable<int> Semester { get; set; }

    }
}
