
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace XTMF2.Web.Data.Converters
{

    public class TypeConverter : JsonConverter<Type>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override Type Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Assembly.GetExecutingAssembly().GetType(reader.GetString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="options"></param>
        public override void Write(Utf8JsonWriter writer, Type value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.FullName);
        }
    }
}