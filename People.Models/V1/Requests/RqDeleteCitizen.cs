using System;
using System.ComponentModel.DataAnnotations;

namespace People.Models.V1.Requests
{
    public class RqDeleteCitizen
    {
        [Required]
        public Guid? Guid { get; set; }

    }
}
