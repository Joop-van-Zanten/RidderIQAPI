using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace RidderIQAPI
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			if (!Cryptography.Initialised)
			{
				string cryptFile = HttpContext.Current.Server.MapPath(".crypt");
				Dictionary<string, string> dic = null;
				if (!File.Exists(cryptFile))
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