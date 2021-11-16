using Ice.Lib.Reporting;
using Ice.Lib.ReportingServices.Rest.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CheckSSRS
{
    class Program
    {
        static void Main(string[] args)
        {

			try
			{
				Version systemVersion = SsrsReportingServiceCaller.CreateService(args[0], null, true).GetSystemVersion();
				Console.WriteLine(systemVersion);
				
			}
			catch(Exception ex)
            {
				Console.WriteLine(ex.ToString());
				
			}
			Console.WriteLine("Enter to exit()");
			Console.ReadLine();
		}

		public static IReportingService CreateService(string baseUrl, CultureInfo culture, bool useRest)
		{
			if (useRest)
			{
				Type t = typeof(ReportInformation).Assembly.GetType("Ice.Lib.Reporting.RestReportingService");

				IReportingService result = Activator.CreateInstance(t) as IReportingService;
				Configuration.DefaultApiClient.BasePath = SsrsUrlBuilder.BuildReportServiceUrl(baseUrl, useRest);
				Configuration.DefaultApiClient.Culture = culture;
				Configuration.DefaultApiClient.UseDefaultCredentials = true;
				Configuration.DefaultApiClient.RestClient.Timeout = (int)TimeSpan.FromMinutes(5.0).TotalMilliseconds;
				return result;
			}
			ReportingService reportingService = ((culture == null) ? new ReportingService() : new ReportingService(culture));
			reportingService.Url = SsrsUrlBuilder.BuildReportServiceUrl(baseUrl, useRest);
			reportingService.UseDefaultCredentials = true;
			reportingService.Timeout = (int)TimeSpan.FromMinutes(5.0).TotalMilliseconds;
			return reportingService;
		}


	}
}
