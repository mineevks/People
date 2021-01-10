using System.ComponentModel.DataAnnotations;
using People.Models.V1.CommonModels;

namespace People.Models.V1.Requests
{
    public class RqUpdateCitizen
    {

        [Required]
        public CitizenV1 Citizen { get; set; }

    }
}
