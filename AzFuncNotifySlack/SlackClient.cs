using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Octopus
{
	public class SlackClient
	{
		private readonly Uri _uri;
		private readonly Encoding _encoding = new UTF8Encoding();
		
		public SlackClient(string slackUrlWithAccessToken)
		{
			_uri = new Uri(slackUrlWithAccessToken);
		}
		
		public string PostMessage(string text)
		{
			Payload payload = new Payload()
			{
				Text = text
			};
			
			return PostMessage(payload);
		}
		
		public string PostMessage(Payload payload)
		{
			string payloadJson = JsonConvert.SerializeObject(payload);
			
			using (WebClient client = new WebClient())
			{
				var data = new NameValueCollection();
				data["payload"] = payloadJson;
		
				var response = client.UploadValues(_uri, "POST", data);
				
				return _encoding.GetString(response);
			}
		}
	}
}