using System.ComponentModel.DataAnnotations;
using People.Models.V1.CommonModels;

namespace People.Models.V1.Requests
{
    public class RqGenerateCitizens
    {
        [Required]
        public int? NumberOfCitizens { get; set; }
    }
}
