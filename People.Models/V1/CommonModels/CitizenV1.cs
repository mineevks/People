using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace People.Models.V1.CommonModels
{
    public class CitizenV1
    {

        public Guid Guid { get; set; }

        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        [Required]
        [StringLength(30)]
        public string Surname { get; set; }

        [Required]
        [StringLength(30)]
        public string Patronymic { get; set; }

        //[Required]
        public string Snils { get; set; }

        //[Required]
        public string Inn { get; set; }


        [Required]
        
        //[JsonConverter(typeof(DateTimeOffsetConverter))]

        public DateTime DateOfBirth { get; set; }

        //[JsonConverter(typeof(DateTimeOffsetConverter))]

        public DateTime? DateOfDeath { get; set; }

    }
}
