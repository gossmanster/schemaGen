using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ResourceSchemaGenerator;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace SchemaTests
{
    [TestClass]
    public class SimpleSchemaTests
    {
        public ResourceDefinition CreateMockDefinition(string typeName)
        {
            var definition = new ResourceDefinition() {
                Type = typeName
            };
            definition.Properties.Add("foo", (JToken)"bar");

            return definition;
        }
        [TestMethod]
        public void TestSchemaSerialization()
        {
            var schema = new SimpleSchema()
            {
                Publisher = "Microsoft.Mock",
                ApiVersion = "2014-06-26",
                Description = "Test Resource Schema"
            };

            schema.Definitions.Add(this.CreateMockDefinition("resource1"));
            schema.Definitions.Add(this.CreateMockDefinition("resource2"));

            string json = JsonConvert.SerializeObject(schema, Formatting.Indented);

            string expectedJson = @"
                                    {
                                      ""publisher"": ""Microsoft.Mock"",
                                      ""apiVersion"": ""2014-06-26"",
                                      ""description"": ""Test Resource Schema"",
                                      ""definitions"": [
                                        {
                                          ""type"": ""resource1"",
                                          ""properties"": {
                                            ""foo"": ""bar""
                                          }
                                        },
                                        {
                                          ""type"": ""resource2"",
                                          ""properties"": {
                                            ""foo"": ""bar""
                                          }
                                        }
                                      ]
                }";

            Assert.IsTrue(JToken.DeepEquals(JToken.Parse(expectedJson), JToken.Parse(json)));
        }

        [TestMethod]
        public void TestLoadSimpleSchema()
        {
            string json = Helpers.LoadTestFile("SourceSchemas", "websiteResources.json");

            Assert.IsNotNull(json);

            var schema = JsonConvert.DeserializeObject<SimpleSchema>(json);
            Assert.AreEqual("Microsoft.Web", schema.Publisher);
        }

        [TestMethod]
        public void TestGenSchema()
        {
            string loadedJson = Helpers.LoadTestFile("SourceSchemas", "websiteResources.json");

            Assert.IsNotNull(loadedJson);

            var schema = JsonConvert.DeserializeObject<SimpleSchema>(loadedJson);
            Assert.AreEqual("Microsoft.Web", schema.Publisher);

            var generator = new SchemaGenerator(schema);

            string generatedJson = generator.WriteJson();

            string expectedJson = Helpers.LoadTestFile("OutputSchemas", "Microsoft.Web.json");

            Assert.IsTrue(JToken.DeepEquals(JToken.Parse(expectedJson), JToken.Parse(generatedJson)));
        }
    }
}
