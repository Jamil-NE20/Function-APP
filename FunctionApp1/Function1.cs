using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace FunctionApp1
{
    public class Getdata
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("fname")]
            public string fastName { get; set; }

            [JsonProperty("lname")]
            public string lastName { get; set; }

            [JsonProperty("mname")]
            public string MiddleName { get; set; }

            [JsonProperty("phonenumber")]
            public string PhoneNumber { get; set; }
            
            [JsonProperty("specialty")]
            public string Specialty { get; set; }              
            [JsonProperty("PartitionKey")]
            public string PartitionKey { get; set; }
        }

    public static class Function1
    {      
        [FunctionName("GetItems")]
        
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)]
             HttpRequest req,
            [CosmosDB("virginia", "PCP", ConnectionStringSetting ="CosmosDB",
            SqlQuery = "SELECT * FROM c ORDER BY c._ts DESC")] 
            IEnumerable<Getdata> getdatas, 
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult(getdatas);
                  
        }  
    }
    public static class Function
    {
        [FunctionName("GetItembyId")]
        
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route ="GetItem/{id}")]
             HttpRequest req, string id,
            [CosmosDB("virginia", "PCP", ConnectionStringSetting ="CosmosDB",
            SqlQuery = "SELECT * FROM c WHERE c.id = {id} ORDER BY c._ts DESC")] 
            IEnumerable<Getdata> getdatas, 
            ILogger log)
        {
            log.LogInformation($"Function triggered");
           
          if(getdatas == null)
          {
              log.LogInformation($"item not found");
              return new NotFoundObjectResult("ID not found in collection");
          }
                      //log.LogInformation($"item not found{getdata.FirstName}");
              return new OkObjectResult(getdatas);        
        }    
    }        
        
} 