using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Riot_API_Test
{
    class RiotAPI
    {
        private const string APIEndpoint = "http://euw.api.pvp.net/";
        public string Region = "euw";

        public HttpWebResponse MakeRequest(string url)
        {
            var req = (HttpWebRequest) WebRequest.Create(url);
            var resp = (HttpWebResponse) req.GetResponse();

            return resp;
        }

        private string[] GetStreamString(Stream stream)
        {
            StreamReader objReader = new StreamReader(stream);
            List<string> lines = new List<string>();

            string sLine = "";
            
            while (sLine != null)
            {
                sLine = objReader.ReadLine();
                if (sLine != null)
                    lines.Add(sLine);
            }

            return lines.ToArray();
        }

        public int GetSummonerID(string summonerName)
        {
            var resp = MakeRequest(APIEndpoint + "api/lol/" + Region + "/v1.4/summoner/by-name/" + summonerName);
            var lines = GetStreamString(resp.GetResponseStream());

            foreach (var item in lines)
            {
                if (lines.Contains("id"))
                {
                    return 
                }
            }
        }
    }
}
