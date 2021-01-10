using System.Collections.Generic;
using People.Models.Sql;
using People.Models.V1.CommonModels;

namespace People.Models.V1.Responses
{
    public class RpGetCitizens
    {
        public List<CitizenSql> Citizens { get; set; }
    }
}
