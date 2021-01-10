using System;
using System.ComponentModel.DataAnnotations;
using People.Models.V1.CommonModels;

namespace People.Models.Sql
{
    public class SearchRequestParsed
    {
        public Guid? Guid { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public long? Snils { get; set; }

        public long? Inn { get; set; }

        public DateTime? DateOfBirthStart { get; set; }

        public DateTime? DateOfBirthEnd { get; set; }

        public DateTime? DateOfDeathStart { get; set; }

        public DateTime? DateOfDeathEnd { get; set; }

    }
}
