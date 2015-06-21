using Newtonsoft.Json;
using System;
using Newtonsoft.Json.Linq;

namespace RuzikOdyssey.Infrastructure
{
	public abstract class JsonCreationConverter<T> : JsonConverter
	{
		protected abstract T Create(Type objectType, JObject jObject);
		public override bool CanConvert(Type objectType)
		{
			/* REMARK
			 * If .NET Framework 4.5
			 * return typeof(T).GetTypeInfo().IsAssignableFrom(objectType.GetTypeInfo());
			 */
			return typeof(T).IsAssignableFrom(objectType);
		}
		
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader.TokenType == JsonToken.Null) return null;
			
			// Load JObject from stream
			JObject jObject = JObject.Load(reader);
			
			// Create target object based on JObject
			T target = Create(objectType, jObject);
			
			//Create a new reader for this jObject, and set all properties to match the original reader.
			JsonReader jObjectReader = jObject.CreateReader();
			
			// Populate the object properties
			serializer.Populate(jObjectReader, target);
			
			return target;
		}
		
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			serializer.Serialize(writer, value);
		}
	}
}

