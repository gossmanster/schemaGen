using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceSchemaGenerator;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;


namespace SchemaTests
{
    [TestClass]
    public class ResourceDefinitionTests
    {
        [TestMethod]
        public void TestOutput()
        {
            var definition = new ResourceDefinition();

            definition.Type = "Microsoft.Mock/mockResource";
            definition.Properties = new JObject();
            definition.Properties.Add("foo", (JToken)"bar");

            string json = JsonConvert.SerializeObject(definition);

            Assert.AreEqual("{\"type\":\"Microsoft.Mock/mockResource\",\"properties\":{\"foo\":\"bar\"}}", json);


        }
    }
}
