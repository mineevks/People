using System;
using System.ComponentModel.DataAnnotations;

namespace People.Models.V1.CommonModels
{
    public class CitizenCsv
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Snils { get; set; }

        public string Inn { get; set; }

        public string /*DateTime*/ DateOfBirth { get; set; }

        public string /*DateTime?*/ DateOfDeath { get; set; }

    }
}
