using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace People.Models.Sql
{
    public class CitizenSql
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Guid { get; set; }


        [StringLength(30)]
        public string Name { get; set; }

        [StringLength(30)]
        public string Surname { get; set; }

        [StringLength(30)]
        public string Patronymic { get; set; }

        public long? Snils { get; set; }

        public long? Inn { get; set; }


        //[JsonConverter(typeof(JsonConverter.DateTimeConverter))]
        public DateTime DateOfBirth { get; set; }

        
        public DateTime? DateOfDeath { get; set; }

    }


}
