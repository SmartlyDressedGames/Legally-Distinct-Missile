using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Core.Extensions
{
    public class JsonExtension : JsonConverter
    {
        private readonly string _comment;
        public JsonExtension(string comment)
        {
            _comment = comment;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
            writer.WriteComment(_comment); // append comment
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
        JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
        public override bool CanConvert(Type objectType) => true;
        public override bool CanRead => false;
    }
}
