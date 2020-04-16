using System;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

class JsonPathConverter : JsonConverter
{
	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		var jObj = JObject.Load(reader);
		var targetObj = Activator.CreateInstance(objectType);
		foreach (PropertyInfo propInfo in objectType.GetProperties().Where(p => p.CanRead && p.CanWrite))
		{
            var attribute = propInfo.GetCustomAttributes(true).OfType<JsonPropertyAttribute>().FirstOrDefault();
			var jsonPath = (attribute != null ? attribute.PropertyName : propInfo.Name);
			var token = jObj.SelectToken(jsonPath);
			if (token != null && token.Type != JTokenType.Null)
			{
				var value = token.ToObject(propInfo.PropertyType, serializer);
				propInfo.SetValue(targetObj, value, null);
			}
		}
		return targetObj;
	}
	
	public override bool CanConvert(Type objectType)
	{
		return false;
	}
	
	public override bool CanWrite
	{
		get { return false; }
	}
	
	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		throw new NotImplementedException();
	}
}