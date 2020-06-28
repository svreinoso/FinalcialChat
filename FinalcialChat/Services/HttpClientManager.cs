using FinalcialChat.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace FinalcialChat.Services
{
    public class HttpClientManager : IHttpClientManager
    {
        public string Get(string code)
        {
            try
            {
                var url = $"https://stooq.com/q/l/?s={code}&f=sd2t2ohlcv&h&e=csv";

                using (var wb = new WebClient())
                {
                    var response = wb.DownloadString(url);
                    return response;
                }

                //var response = client.(url).Wait();
                //response.EnsureSuccessStatusCode();
                //string responseBody = await response.Content.ReadAsStringAsync();
                //return responseBody;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine(e.Message);
                return "";
;           }
        }
    }
}