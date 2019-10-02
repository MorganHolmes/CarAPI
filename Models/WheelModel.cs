using System;
using Newtonsoft.Json;

namespace starter_dotnet_core.Wheels{
    public class CarWheel{
        //GUID from the Cosmos database [MWH]
        //The Json object in Cosmos may have a lower case i at the start of Id [MWH]
        [JsonProperty("id")]
        public string Id {get;set;}

        public string Make {get;set;}

        public string Model {get;set;}

        public WheelDimensions WheelDimensions {get;set;}

        
    }
}

public class WheelDimensions{
            public int Diameter {get;set;}
            public int Width {get;set;}

            public int Offset {get;set;} 
        }