using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace RidderIQAPI
{
	/// <summary>
	/// IIS Web application
	/// </summary>
	public class WebApiApplication : HttpApplication
	{
		/// <summary>
		/// Start the application
		/// </summary>
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			// Add the Json Serializer to the HttpConfiguration object
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings = new JsonSerializerSettings
			{
				// Ignore the Self Reference looping
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
				// Do not Preserve the Reference Handling
				PreserveReferencesHandling = PreserveReferencesHandling.None,
			};

			// This line ensures Json for all clients, without this line it generates Json only for clients which request, for browsers default is XML
			GlobalConfiguration.Configuration.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

			if (!Cryptography.Initialised)
			{
				string cryptFile = HttpContext.Current.Server.MapPath(".crypt");
				Dictionary<string, string> dic = null;
				if (!File.Exists(cryptFile) || new FileInfo(cryptFile).Length == 0)
				{
					dic = new Dictionary<string, string>
					{
						{ "key", 32.RandomString() },
						{ "iv", 32.RandomString() }
					};

					using (var ms = new MemoryStream())
					{
						new BinaryFormatter().Serialize(ms, dic);
						File.WriteAllBytes(cryptFile, ms.ToArray());
					}
				}
				else
				{
					byte[] arrBytes = File.ReadAllBytes(cryptFile);
					using (var memStream = new MemoryStream())
					{
						memStream.Write(arrBytes, 0, arrBytes.Length);
						memStream.Seek(0, SeekOrigin.Begin);
						dic = new BinaryFormatter().Deserialize(memStream) as Dictionary<string, string>;
					}
				}
				Cryptography.Initialise(dic["key"], dic["iv"]);
			}
		}
	}
}