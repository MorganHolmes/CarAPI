using System;
using Newtonsoft.Json;

namespace starter_dotnet_core.Models
{
  //Model Used For The car models [MWH]
  public class Model
  {
  //GUID from the Cosmos database [MWH]
  //The Json object in Cosmos may have a lower case i at the start of Id [MWH]
  [JsonProperty("id")]
  public string Id {get;set;}

  //Manfacturer of the car[MWH]
   public Manufacturer Manufacture {get;set;}

  //Vehicle data [MWH]
   public string VehicleModel {get;set;}
  
    public string Registration {get;set;}
    public int HorsePower {get;set;}

    public string ConsoleColor {get;set;}

    public int NumOfDoors {get;set;}

    

  
    //Job status - enum stored in JobStatusEnum [MWH]
    //public  JobStatus JobStatus {get;set;}

  }

  //Manufcaturer class [MWH]
  public class Manufacturer{
     public string Company {get;set;}
     public string Origin {get;set;}

  }
}
