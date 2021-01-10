using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using People.Api.Filters;
using People.Api.Helpers;
using People.Models.Settings;
using People.Models.Sql;
using People.Models.V1.CommonModels;
using People.Models.V1.Requests;
using People.Models.V1.Responses;
using People.Services;
using Utilities;

namespace People.Api.Controllers.Api
{
    [Route("api/[controller]")]
    //[ValidateModel]
    [ApiController]
    public class CitizensController : Controller
    {
        private IPeopleService _peopleService;
        private readonly WSettings _wSettings;


        public CitizensController(
            IPeopleService peopleService,
            IOptions<WSettings> wSettings
        )
        {
            _peopleService = peopleService;
            _wSettings = wSettings.Value;
        }


        [HttpPost(nameof(AddCitizen))]
        public async Task<ActionResult<RpAddCitizen>> AddCitizen([FromBody] RqAddCitizen rqAddCitizen)
        {
            try
            {
                if (!string.IsNullOrEmpty(rqAddCitizen.Citizen.Inn) && !ValidationsCollection.Validations.IsValidInnForIndividual(rqAddCitizen.Citizen.Inn))
                {
                    return Json(ResponseHelper.ReturnBadRequest("Inn not valid"));
                }

                if (!string.IsNullOrEmpty(rqAddCitizen.Citizen.Snils) && !ValidationsCollection.Validations.IsValidSnils(StringConverter.GetNumbers(rqAddCitizen.Citizen.Snils)))
                {
                    return Json(ResponseHelper.ReturnBadRequest("Snils not valid"));
                }


                var citizenSql = new CitizenSql();
                try
                {
                    citizenSql = CitizenConverter.FromApiV1ToSql(rqAddCitizen.Citizen);
                }
                catch (Exception exception)
                {
                    LoggerStatic.Logger.Warn("Exception: " + exception);
                    return Json(ResponseHelper.ReturnBadRequest(exception.Message));
                }

                var guid = await _peopleService.AddCitizen(citizenSql);
                var rpAddCitizen = new RpAddCitizen
                {
                    Guid = guid
                };
                return Json(rpAddCitizen);
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Exception: " + exception);
                return Json(ResponseHelper.ReturnInternalServerError(exception.Message));
            }

        }

        [HttpPost(nameof(DeleteCitizen))]
        public async Task<ActionResult<RpDeleteCitizen>> DeleteCitizen([FromBody] RqDeleteCitizen rqDeleteCitizen)
        {
            try
            {
                var wasCitizenDeleted = await _peopleService.DeleteCitizen(rqDeleteCitizen.Guid.Value);
                if (!wasCitizenDeleted)
                {
                    return Json(ResponseHelper.ReturnBadRequest("Citizen's guid not found"));
                }

                var rpDeleteCitizen = new RpDeleteCitizen();
                return Json(rpDeleteCitizen);
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Exception: " + exception);
                return Json(ResponseHelper.ReturnInternalServerError(exception.Message));
            }

        }

        [HttpPost(nameof(UpdateCitizen))]
        public async Task<ActionResult<RpUpdateCitizen>> UpdateCitizen([FromBody] RqUpdateCitizen rqUpdateCitizen)
        {
            try
            {
                if (!string.IsNullOrEmpty(rqUpdateCitizen.Citizen.Inn) && !ValidationsCollection.Validations.IsValidInnForIndividual(rqUpdateCitizen.Citizen.Inn))
                {
                    return Json(ResponseHelper.ReturnBadRequest("Inn not valid"));
                }

                if (!string.IsNullOrEmpty(rqUpdateCitizen.Citizen.Snils) && !ValidationsCollection.Validations.IsValidSnils(StringConverter.GetNumbers(rqUpdateCitizen.Citizen.Snils)))
                {
                    return Json(ResponseHelper.ReturnBadRequest("Snils not valid"));
                }


                var citizenSql = new CitizenSql();
                try
                {
                    citizenSql = CitizenConverter.FromApiV1ToSql(rqUpdateCitizen.Citizen);
                    citizenSql.Guid = rqUpdateCitizen.Citizen.Guid;
                }
                catch (Exception exception)
                {
                    LoggerStatic.Logger.Warn("Exception: " + exception);
                    return Json(ResponseHelper.ReturnBadRequest(exception.Message));
                }

                await _peopleService.UpdateCitizen(citizenSql);

                var rpUpdateCitizen = new RpUpdateCitizen();
                return Json(rpUpdateCitizen);
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Exception: " + exception);
                return Json(ResponseHelper.ReturnInternalServerError(exception.Message));
            }

        }


        [HttpPost(nameof(GetCitizens))]
        public async Task<ActionResult<RpGetCitizens>> GetCitizens([FromBody] RqGetCitizens rqGetCitizens)
        {
            try
            {
                if (!string.IsNullOrEmpty(rqGetCitizens.SearchRequest.Inn) && !ValidationsCollection.Validations.IsValidInnForIndividual(rqGetCitizens.SearchRequest.Inn))
                {
                    return Json(ResponseHelper.ReturnBadRequest("Inn not valid"));
                }

                if (!string.IsNullOrEmpty(rqGetCitizens.SearchRequest.Snils) && !ValidationsCollection.Validations.IsValidSnils(StringConverter.GetNumbers(rqGetCitizens.SearchRequest.Snils)))
                {
                    return Json(ResponseHelper.ReturnBadRequest("Snils not valid"));
                }

                var searchRequestParsed = new SearchRequestParsed();
                try
                {
                    searchRequestParsed = CitizenConverter.FromSearchRequestToSearchRequestParsed(rqGetCitizens.SearchRequest);
                }
                catch (Exception exception)
                {
                    LoggerStatic.Logger.Warn("Exception: " + exception);
                    return Json(ResponseHelper.ReturnBadRequest(exception.Message));
                }

                var rpGetCitizens = new RpGetCitizens
                {
                    Citizens = await _peopleService.GetCitizens(searchRequestParsed)
                };
                //return Json(rpGetCitizens);
                return ControllersHelper.ReturnContentResult(SerializerJson.SerializeObjectToJsonString(rpGetCitizens));
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Exception: " + exception);
                return Json(ResponseHelper.ReturnInternalServerError(exception.Message));
            }
        }


        
        [HttpPost(nameof(ExportCsv))]
        public async Task<FileContentResult> ExportCsv([FromBody] RqGetCitizens rqGetCitizens)
        {
            try
            {
                if (!string.IsNullOrEmpty(rqGetCitizens.SearchRequest.Inn) && !ValidationsCollection.Validations.IsValidInnForIndividual(rqGetCitizens.SearchRequest.Inn))
                {
                    return File(new UTF8Encoding().GetBytes($"Exception: Inn not valid)"), "text/csv", "exception.txt");
                }

                if (!string.IsNullOrEmpty(rqGetCitizens.SearchRequest.Snils) && !ValidationsCollection.Validations.IsValidSnils(StringConverter.GetNumbers(rqGetCitizens.SearchRequest.Snils)))
                {
                    return File(new UTF8Encoding().GetBytes($"Exception: Snils not valid)"), "text/csv", "exception.txt");
                }

                var searchRequestParsed = new SearchRequestParsed();
                try
                {
                    searchRequestParsed = CitizenConverter.FromSearchRequestToSearchRequestParsed(rqGetCitizens.SearchRequest);
                }
                catch (Exception exception)
                {
                    LoggerStatic.Logger.Warn("Exception: " + exception);
                    return File(new UTF8Encoding().GetBytes($"Exception: {exception})"), "text/csv", "exception.txt");
                }

                var citizens = await _peopleService.GetCitizens(searchRequestParsed);
                var csvBytes = await _peopleService.GetCsvResult(citizens);

                return File(csvBytes, "text/csv", $"Citizens-{DateTime.Now:yyyy-MM-dd-H-mm-ss}.csv");
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Exception: " + exception);
                return File(new System.Text.UTF8Encoding().GetBytes($"Exception: {exception}"), "text/csv", "exception.txt");
            }
        }


        [HttpPost(nameof(ImportCsv))]
        public async Task<ActionResult<RpImportCitizens>> ImportCsv(IFormFile file)
        {
            try
            {
                if (file != null)
                {
                    var citizenSqls = new List<CitizenSql>();
                    try
                    {
                        citizenSqls = ReadCsv(file);
                    }
                    catch (Exception exception)
                    {
                        LoggerStatic.Logger.Warn($"Exception: {exception}");
                        var exceptionMessage = $"Exception: { exception.Message}";
                        if (exception.InnerException != null)
                        {
                            exceptionMessage += $" InnerException: { exception.InnerException.Message}";
                        }

                        return Json(ResponseHelper.ReturnBadRequest(exceptionMessage));
                    }

                    await _peopleService.AddCitizens(citizenSqls);

                    var rpImportCitizens = new RpImportCitizens
                    {
                        RecordsAdded = citizenSqls.Count
                    };
                    return Json(rpImportCitizens);
                    //return Json(ResponseHelper.ReturnSuccess());
                }

                return Json(ResponseHelper.ReturnBadRequest("file == null"));
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error($"Exception: {exception}");
                return Json(ResponseHelper.ReturnInternalServerError(exception.Message));
            }
            
        }


        


        private List<CitizenSql> ReadCsv(IFormFile file)
        {
            var citizenSqls = new List<CitizenSql>();

            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    /*csv.Configuration.HasHeaderRecord = true;
                    var records = csv.GetRecords<T>();
                    return records.ToList();*/
                    
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var citizenCsv = csv.GetRecord<CitizenCsv>();
                        _peopleService.ValidateCitizenCsv(citizenCsv);
                        citizenSqls.Add(CitizenConverter.FromCsvToSql(citizenCsv));
                    }

                    return citizenSqls;
                }
            }
        }






        [HttpPost(nameof(GenerateCitizens))]
        public async Task<ActionResult<RpGenerateCitizens>> GenerateCitizens([FromBody] RqGenerateCitizens rqGenerateCitizens)
        {
            try
            {
                await _peopleService.GenerateCitizens(rqGenerateCitizens.NumberOfCitizens.Value);

                var rpGenerateCitizens = new RpGenerateCitizens();
                return Json(rpGenerateCitizens);
            }
            catch (Exception exception)
            {
                LoggerStatic.Logger.Error("Exception: " + exception);
                return Json(ResponseHelper.ReturnInternalServerError(exception.Message));
            }
        }




    }

}
