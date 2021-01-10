using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using People.Models.Sql;
using People.Models.V1.CommonModels;
using People.Models.V1.Requests;

namespace Utilities
{
    public static class CitizenConverter
    {

        public static CitizenV1 FromSqlToApiV1(CitizenSql citizenSql)
        {
            var citizenV1 = new CitizenV1
            {
                Inn = citizenSql.Inn.ToString(),
                Name = citizenSql.Name,
                Surname = citizenSql.Surname,
                Patronymic = citizenSql.Patronymic,
                Snils = citizenSql.Snils.ToString(),
                DateOfBirth = citizenSql.DateOfBirth,
                DateOfDeath = citizenSql.DateOfDeath,
                Guid = citizenSql.Guid
            };

            return citizenV1;
        }

        public static CitizenSql FromApiV1ToSql(CitizenV1 citizenV1)
        {
            var citizenSql = new CitizenSql
            {
                Name = citizenV1.Name,
                Surname = citizenV1.Surname,
                Patronymic = citizenV1.Patronymic,
                DateOfBirth = citizenV1.DateOfBirth,
                DateOfDeath = citizenV1.DateOfDeath,
            };

            if (!string.IsNullOrEmpty(citizenV1.Snils))
            {
                var snilsNumbersString = StringConverter.GetNumbers(citizenV1.Snils);
                citizenSql.Snils = long.Parse(snilsNumbersString);
            }

            if (!string.IsNullOrEmpty(citizenV1.Inn))
            {
                var innNumbersString = StringConverter.GetNumbers(citizenV1.Inn);
                citizenSql.Inn = long.Parse(innNumbersString);
            }

            return citizenSql;
        }

        public static CitizenSql FromCsvToSql(CitizenCsv citizenCsv)
        {
            var provider = CultureInfo.InvariantCulture;

            var citizenSql = new CitizenSql
            {
                Name = citizenCsv.Name,
                Surname = citizenCsv.Surname,
                Patronymic = citizenCsv.Patronymic,
            };

            citizenSql.DateOfBirth = DateTime.ParseExact(citizenCsv.DateOfBirth, "yyyy'-'MM'-'dd", provider);
            if (!string.IsNullOrEmpty(citizenCsv.DateOfDeath))
            {
                citizenSql.DateOfDeath = DateTime.ParseExact(citizenCsv.DateOfDeath, "yyyy'-'MM'-'dd", provider);
            }

            if (!string.IsNullOrEmpty(citizenCsv.Snils))
            {
                var snilsNumbersString = StringConverter.GetNumbers(citizenCsv.Snils);
                citizenSql.Snils = long.Parse(snilsNumbersString);
            }

            if (!string.IsNullOrEmpty(citizenCsv.Inn))
            {
                var innNumbersString = StringConverter.GetNumbers(citizenCsv.Inn);
                citizenSql.Inn = long.Parse(innNumbersString);
            }

            return citizenSql;
        }


        public static CitizenCsv FromSqlToCsv(CitizenSql citizenSql)
        {
            var provider = CultureInfo.InvariantCulture;

            var citizenCsv = new CitizenCsv
            {
                Name = citizenSql.Name,
                Surname = citizenSql.Surname,
                Patronymic = citizenSql.Patronymic,
                DateOfBirth = citizenSql.DateOfBirth.ToString("yyyy'-'MM'-'dd", provider)
            };

            if (citizenSql.Snils != null)
            {
                citizenCsv.Snils = citizenSql.Snils.Value.ToString();
            }
            if (citizenSql.Inn != null)
            {
                citizenCsv.Inn = citizenSql.Inn.Value.ToString();
            }
            if (citizenSql.DateOfDeath != null)
            {
                citizenCsv.DateOfDeath = citizenSql.DateOfDeath.Value.ToString("yyyy'-'MM'-'dd", provider);
            }

            return citizenCsv;
        }


        public static SearchRequestParsed FromSearchRequestToSearchRequestParsed(SearchRequest searchRequest)
        {
            var searchRequestParsed = new SearchRequestParsed
            {
                Guid = searchRequest.Guid,
                Name = searchRequest.Name,
                Surname = searchRequest.Surname,
                Patronymic = searchRequest.Patronymic,
                DateOfBirthStart = searchRequest.DateOfBirthStart,
                DateOfBirthEnd = searchRequest.DateOfBirthEnd,
                DateOfDeathStart = searchRequest.DateOfDeathStart,
                DateOfDeathEnd = searchRequest.DateOfDeathEnd,
            };

            if (!string.IsNullOrEmpty(searchRequest.Snils))
            {
                var snilsNumbersString = StringConverter.GetNumbers(searchRequest.Snils);
                searchRequestParsed.Snils = long.Parse(snilsNumbersString);
            }

            if (!string.IsNullOrEmpty(searchRequest.Inn))
            {
                var innNumbersString = StringConverter.GetNumbers(searchRequest.Inn);
                searchRequestParsed.Inn = long.Parse(innNumbersString);
            }

            return searchRequestParsed;
        }

    }
}
