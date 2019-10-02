using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using starter_dotnet_core.Models;
using Twilio.TwiML;
using Twilio.TwiML.Voice;
using Twilio.Types;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Security;
using Newtonsoft.Json;
using System.IO;
using System.Collections.Generic;
using starter_dotnet_core.Services;
using System.Threading.Tasks;
using starter_dotnet_core.Wheels;

namespace starter_dotnet_core.Controllers
{
    public class HomeController : Controller
    {

        //Verfication String used to authertication requests [MWH]
        const string verficationString = "1A32AE8561A336CE0755DFD396DC8FB683AE5EF4EB420BBF9157C855CCB7AA41";
        
        private readonly ICosmosDbService _cosmosDbService;
        //Old list used for storing the new job [MWH]
        //private static List<Job> serverData = new List<Job>();


        public HomeController(ICosmosDbService dbService)
        {
            _cosmosDbService = dbService;
            //_fromPhoneNumber = new PhoneNumber(
            //    Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER")
            //);
        }

        public IActionResult Index()
        {
            return View();
        }

        //Handles the POST request. Creates a new job and stores it in Cosmos [MWH]
        [Route("models")]
        [HttpPost]
        public async Task<IActionResult> PostModels()
        {
            //Checks the keys passed in the HTTP request header [MWH]
            var vHeader = Request.Headers["X-Verification-Key"];
            //If the key in the header matches what we expect [MWH]
            if(vHeader == verficationString){
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = reader.ReadToEnd();
                    //Turns the passed data into Json [MWH]
                    var model = JsonConvert.DeserializeObject<Model>(body);
                    //If the model id is null create a new GUID [MWH]
                    if(model.Id == null){
                        //Creates a new GUID using the newGuid() method [MWH]
                        model.Id = Guid.NewGuid().ToString(); 
                    }
                    //If the job status == 1/Rejected then the below line of code adds (Rejected) to the JobType [MWH]
                    //if(model.JobStatus == JobStatus.Rejected){
                        //model.JobType += " (Rejected)";
                    //}
                    //Returns the created job with the GUID [MWH]
                    return new ObjectResult(await _cosmosDbService.AddItemASync(model));
                }
            }else{
                //Returns status code 403 (forbidden) [MWH]
                return StatusCode(403);
            }
        }

        //Handles the GET request. Can either get using the phone number or the phone number and ID [MWH]
        [Route("models/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetModels(string model,string id)
        {
            //Checks if the keys passed in the HTTP request matches what is expected [MWH]
            var vHeader = Request.Headers["X-Verification-Key"];
            if(vHeader == verficationString){
                //if the ID is null is only queries cosmos using the phone number. Else is uses the phone number and GUID [MWH]
                if(id != null){
                    return new ObjectResult((List<Model>)await _cosmosDbService.GetItemsAsync($"SELECT * FROM c WHERE c.id = '{id}'"));
               }else{
                    return new ObjectResult((List<Model>)await _cosmosDbService.GetItemsAsync($"SELECT * FROM c " ));
                }
            }else{
                return StatusCode(403);
            }

        }

        [Route("models")]
        [HttpGet]
        public async Task<IActionResult> GetAllModels(string model,string id)
        {
            //Checks if the keys passed in the HTTP request matches what is expected [MWH]
            var vHeader = Request.Headers["X-Verification-Key"];
            if(vHeader == verficationString){
                return new ObjectResult((List<Model>)await _cosmosDbService.GetItemsAsync($"SELECT * FROM c " ));
            }else{
                return StatusCode(403);
            }

        }

        [Route("wheels")]
        [HttpPost]
        public async Task<IActionResult> PostWheels()
        {
            //Checks the keys passed in the HTTP request header [MWH]
            var vHeader = Request.Headers["X-Verification-Key"];
            //If the key in the header matches what we expect [MWH]
            if(vHeader == verficationString){
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = reader.ReadToEnd();
                    //Turns the passed data into Json [MWH]
                    var data = JsonConvert.DeserializeObject<CarWheel>(body);
                    //If the wheel id is null create a new GUID [MWH]
                    if(data.Id == null){
                        //Creates a new GUID using the newGuid() method [MWH]
                        data.Id = Guid.NewGuid().ToString(); 
                    }
                    Console.Write(data.Model);
                    //Returns the created job with the GUID [MWH]
                    return new ObjectResult(await _cosmosDbService.AddWheelItemASync(data));
                }
            }else{
                //Returns status code 403 (forbidden) [MWH]
                return StatusCode(403);
            }
        }

         //Handles the GET request. Can either get using the phone number or the phone number and ID [MWH]
        [Route("wheels/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetWheels(string model,string id)
        {
            //Checks if the keys passed in the HTTP request matches what is expected [MWH]
            var vHeader = Request.Headers["X-Verification-Key"];
            if(vHeader == verficationString){
                //if the ID is null is only queries cosmos using the phone number. Else is uses the phone number and GUID [MWH]
                if(id != null){
                    return new ObjectResult((List<CarWheel>)await _cosmosDbService.GetWheelItemsAsync($"SELECT * FROM c WHERE c.id = '{id}'"));
               }else{
                    return new ObjectResult((List<CarWheel>)await _cosmosDbService.GetWheelItemsAsync($"SELECT * FROM c " ));
                }
            }else{
                return StatusCode(403);
            }

        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
