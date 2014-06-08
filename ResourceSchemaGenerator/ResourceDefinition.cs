using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSchemaGenerator
{
    public class ResourceDefinition
    {
        public ResourceDefinition()
        {
            this.Properties = new JObject();
        }
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public JObject Properties { get; set; }
    }
}
