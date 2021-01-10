using System;
using System.ComponentModel.DataAnnotations;
using People.Models.V1.CommonModels;

namespace People.Models.V1.Requests
{
    public class RqGetCitizens
    {
        [Required]
        public SearchRequest SearchRequest { get; set; }
    }

}
