using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ResourceSchemaGenerator
{
    public class SimpleSchema
    {
        public SimpleSchema()
        {
            this.Definitions = new List<ResourceDefinition>();
        }
        [JsonProperty("publisher")]
        public string Publisher { get; set; }

        private string apiVersion;
        [JsonProperty("apiVersion")]
        public string ApiVersion
        {
            get
            {
                return this.apiVersion;
            }
            set
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(value, @"(^((\d\d\d\d-\d\d-\d\d)|([0-9]+(\.[0-9]+)?))(-[a-zA-Z][a-zA-Z0-9]*)?$)"))
                {
                    this.apiVersion = value;
                }
                else
                {
                    throw new Exception(String.Format("{0} is not a valid apiVersion", value));
                }
            }
        }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("definitions")]
        public List<ResourceDefinition> Definitions { get; private set; }
    }
}
