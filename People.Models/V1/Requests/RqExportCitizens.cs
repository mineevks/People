using System;
using System.Collections.Generic;

namespace People.Models.V1.Requests
{
    public class RqExportCitizens
    {
        public List<Guid> CitizenGuids { get; set; }
    }
}
