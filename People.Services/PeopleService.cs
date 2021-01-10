using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using People.Models.Settings;
using People.Models.Sql;
using People.Models.V1.CommonModels;
using People.Models.V1.Requests;
using People.MSSql;
using Utilities;


//using Standard.XmlClasses.Input;

namespace People.Services
{
    public interface IPeopleService
    {
        Task<Guid> AddCitizen(CitizenSql citizenSql);

        Task AddCitizens(List<CitizenSql> citizensSql);

        Task<bool> DeleteCitizen(Guid guid);

        Task UpdateCitizen(CitizenSql citizenSql);

        Task<List<CitizenSql>> GetCitizens(SearchRequestParsed searchRequestParsed);

        Task GenerateCitizens(int numberOfCitizens);

        Task<byte[]> GetCsvResult(List<CitizenSql> citizenSql);

        void ValidateCitizenCsv(CitizenCsv citizenCsv);
    }

    public class PeopleService:IPeopleService
    {
        private readonly WSettings _wSettings;
        protected PeopleDbContext Context { get; set; }

        public PeopleService(
            PeopleDbContext context,
            IOptions<WSettings> wSettings
            )
        {
            _wSettings = wSettings.Value;
            Context = context;
        }


        public async Task<Guid> AddCitizen(CitizenSql citizenSql)
        {
            //citizenSql.Guid = new Guid();
            var rrr = Context.Citizens.AddAsync(citizenSql);
            await Context.SaveChangesAsync();

            return citizenSql.Guid;
        }

        public async Task AddCitizens(List<CitizenSql> citizensSql)
        {
            //citizenSql.Guid = new Guid();
            var rrr = Context.Citizens.AddRangeAsync(citizensSql);
            await Context.SaveChangesAsync();
        }

        public async Task<bool> DeleteCitizen(Guid guid)
        {
            var isRecordExist = Context.Citizens.Any(x => x.Guid == guid);
            if (!isRecordExist)
            {
                return false;
            }

            var citizen = new CitizenSql() { Guid = guid };
            Context.Citizens.Attach(citizen);
            Context.Citizens.Remove(citizen);
            await Context.SaveChangesAsync();

            return true;
        }


        public async Task UpdateCitizen(CitizenSql citizenSql)
        {
            Context.Update(citizenSql);
            await Context.SaveChangesAsync();
        }


        public async Task<List<CitizenSql>> GetCitizens(SearchRequestParsed searchRequestParsed)
        {
            //validate input

            IQueryable<CitizenSql> citizens = Context.Set<CitizenSql>().AsQueryable();

            if (searchRequestParsed.Guid != null && searchRequestParsed.Guid != Guid.Empty)
                citizens = citizens.Where(x => x.Guid == searchRequestParsed.Guid);

            if (searchRequestParsed.Inn != null)
                citizens = citizens.Where(x => x.Inn == searchRequestParsed.Inn);

            if (searchRequestParsed.Snils != null)
                citizens = citizens.Where(x => x.Snils == searchRequestParsed.Snils);

            if (!string.IsNullOrEmpty(searchRequestParsed.Name))
                citizens = citizens.Where(x => x.Name == searchRequestParsed.Name);
            if (!string.IsNullOrEmpty(searchRequestParsed.Patronymic))
                citizens = citizens.Where(x => x.Patronymic == searchRequestParsed.Patronymic);

            if (!string.IsNullOrEmpty(searchRequestParsed.Surname))
                citizens = citizens.Where(x => x.Surname == searchRequestParsed.Surname);

            if (searchRequestParsed.DateOfBirthStart != null && searchRequestParsed.DateOfBirthEnd != null)
                citizens = citizens.Where(x => x.DateOfBirth > searchRequestParsed.DateOfBirthStart && x.DateOfBirth < searchRequestParsed.DateOfBirthEnd);

            if (searchRequestParsed.DateOfDeathStart != null && searchRequestParsed.DateOfDeathEnd != null)
                citizens = citizens.Where(x => x.DateOfBirth > searchRequestParsed.DateOfBirthStart && x.DateOfBirth < searchRequestParsed.DateOfBirthEnd);

            return await citizens.AsNoTracking().ToListAsync();
        }


        public async Task GenerateCitizens(int numberOfCitizens)
        {
            for (var i = 1; i <= numberOfCitizens; i++)
            {
                var rrr = Context.Citizens.AddAsync(GetRandomCitizenSql());
            }

            await Context.SaveChangesAsync();
        }


        private CitizenSql GetRandomCitizenSql()
        {
            var citizen = new CitizenSql()
            {
                Guid = Guid.NewGuid(),
                Name = StringConverter.GetRandomString(10),
                Patronymic = StringConverter.GetRandomString(10),
                Surname = StringConverter.GetRandomString(10),
                //Inn = 
                //Snils = 
                DateOfBirth = StringConverter.RandomDay(1850),
            };

            if (citizen.DateOfBirth.Year > 100)
            {
                var dateOfDeath = StringConverter.RandomDay(1950);
                if (dateOfDeath.Year > citizen.DateOfBirth.Year)
                {
                    citizen.DateOfDeath = dateOfDeath;
                }
            }

            return citizen;
        }




        public async Task<byte[]> GetCsvResult(List<CitizenSql> citizensSql)
        {
            byte[] result;
            await using var memoryStream = new MemoryStream();
            await using var streamWriter = new StreamWriter(memoryStream);
            await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

            csvWriter.WriteHeader<CitizenCsv>();
            await csvWriter.NextRecordAsync();

            foreach (var citizenSql in citizensSql)
            {
                var citizenCsv = CitizenConverter.FromSqlToCsv(citizenSql);
                csvWriter.WriteRecord<CitizenCsv>(citizenCsv);
                await csvWriter.NextRecordAsync();
            }
            await streamWriter.FlushAsync();
            result = memoryStream.ToArray();
            return result;


            /*await csvWriter.WriteRecordsAsync<CitizenSql>(citizensSql);
            await streamWriter.FlushAsync();
            result = memoryStream.ToArray();
            return result;*/
        }


        /*public Task ImportCsv(List<CitizenSql> citizenSql)
        {

        }*/


        public void ValidateCitizenCsv(CitizenCsv citizenCsv)
        {
            if (string.IsNullOrEmpty(citizenCsv.Name) || citizenCsv.Name.Length > 30)
            {
                throw new ArgumentException($"Name not valid: {citizenCsv.Name}");
            }
            if (string.IsNullOrEmpty(citizenCsv.Surname) || citizenCsv.Surname.Length > 30)
            {
                throw new ArgumentException($"Surname not valid: {citizenCsv.Surname}");
            }
            if (string.IsNullOrEmpty(citizenCsv.Patronymic) || citizenCsv.Patronymic.Length > 30)
            {
                throw new ArgumentException($"Patronymic not valid: {citizenCsv.Patronymic}");
            }

            if (!string.IsNullOrEmpty(citizenCsv.Inn) && !ValidationsCollection.Validations.IsValidInnForIndividual(citizenCsv.Inn))
            {
                throw new ArgumentException($"Inn not valid: {citizenCsv.Inn}");
            }

            if (!string.IsNullOrEmpty(citizenCsv.Snils) && !ValidationsCollection.Validations.IsValidSnils(StringConverter.GetNumbers(citizenCsv.Snils)))
            {
                throw new ArgumentException($"Snils not valid: {citizenCsv.Snils}");
            }
        }


    }
}
