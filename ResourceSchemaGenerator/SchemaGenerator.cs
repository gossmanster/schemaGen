using Newtonsoft.Json;
using System;
using System.IO;
using System.Text;

namespace ResourceSchemaGenerator
{
    public class SchemaGenerator
    {
        public SchemaGenerator(SimpleSchema schema)
        {
            this.Schema = schema;

        }

        StringWriter stringWriter;
        JsonTextWriter writer;

        public SimpleSchema Schema { get; private set; }

        public string SchemaFileName
        {

            get
            {
                return @"http://schema.management.azure.com/schemas/" + this.Schema.ApiVersion + @"/" + Schema.Publisher + @".json";
            }
        }

        private void Write(string propertyName, string value)
        {
            this.writer.WritePropertyName(propertyName);
            this.writer.WriteValue(value);
        }

        private void WriteEnumProperty(string name, string value)
        {
            // { "name" : { "enum" : [ "value" ] } }
            this.writer.WritePropertyName(name);
            this.writer.WriteStartObject();
            this.writer.WritePropertyName("enum");
            this.writer.WriteStartArray();
            this.writer.WriteValue(value);
            this.writer.WriteEndArray();
            this.writer.WriteEndObject();
        }

        private void WritePropertiesSchema(ResourceDefinition def)
        {
            // { "properties" : 

            this.writer.WritePropertyName("properties");
            this.writer.WriteStartObject();
            this.Write("type", "object");
            this.writer.WritePropertyName("properties");
            this.writer.WriteToken(def.Properties.CreateReader());
  //        this.writer.WriteValue(def.Properties);
            this.writer.WriteEndObject();
        }
        private void WriteProperties(ResourceDefinition def)
        {
            // { "properties" : 
            this.writer.WriteStartObject();
            this.writer.WritePropertyName("properties");
            this.writer.WriteStartObject();
            this.WriteEnumProperty("type", this.Schema.Publisher + @"/" + def.Type);
            this.WriteEnumProperty("apiVersion", this.Schema.ApiVersion);
            this.WritePropertiesSchema(def);
            this.writer.WriteEndObject();
            this.writer.WriteEndObject();
        }
        private void WriteDefinition(ResourceDefinition def)
        {
            this.writer.WritePropertyName(def.Type);
            this.writer.WriteStartObject();
                this.Write("type", "object");
                this.writer.WritePropertyName("allOf");
                this.writer.WriteStartArray();
                    this.writer.WriteStartObject();
                    this.Write("$ref", @"#/definitions/resourceBase");
                    this.writer.WriteEndObject();
                    this.WriteProperties(def);
                this.writer.WriteEndArray();
                this.writer.WritePropertyName("required");
                this.writer.WriteStartArray();
                    this.writer.WriteValue("type");
                    this.writer.WriteValue("apiVersion");
                    this.writer.WriteValue("properties");
                this.writer.WriteEndArray();
                this.writer.WritePropertyName("additionalProperties");
                this.writer.WriteValue(false);
            this.writer.WriteEndObject();         
        }
        private void WriteDefinitions()
        {
            this.writer.WritePropertyName("definitions");
            this.writer.WriteStartObject();
            foreach(var d  in this.Schema.Definitions)
            {
                this.WriteDefinition(d);
            }
            this.writer.WriteEndObject();
        }
        public string WriteJson()
        {
            this.stringWriter = new StringWriter();
            this.writer = new JsonTextWriter(this.stringWriter);
            this.writer.Formatting = Formatting.Indented;
            this.writer.WriteStartObject();
            this.Write("id", this.SchemaFileName);
            this.Write("$schema", "http://json-schema.org/draft-04/schema#");
            this.Write("title", this.Schema.Publisher);
            this.Write("description", this.Schema.Description);

            this.WriteDefinitions();

            this.writer.WriteEndObject();

            return this.stringWriter.ToString();
        }
    }
}
