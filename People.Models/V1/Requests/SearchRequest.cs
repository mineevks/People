using System;
using System.ComponentModel.DataAnnotations;
using People.Models.V1.CommonModels;

namespace People.Models.V1.Requests
{
    public class SearchRequest
    {
        public Guid? Guid { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Patronymic { get; set; }

        public string Snils { get; set; }

        public string Inn { get; set; }

        public DateTime? DateOfBirthStart { get; set; }

        public DateTime? DateOfBirthEnd { get; set; }

        public DateTime? DateOfDeathStart { get; set; }

        public DateTime? DateOfDeathEnd { get; set; }

    }
}
