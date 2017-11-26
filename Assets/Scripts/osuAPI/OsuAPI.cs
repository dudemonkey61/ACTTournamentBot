using System;
using System.Net;
using UnityEngine;
using OsuTournamentBot.DataObjects;

namespace OsuTournamentBot
{
    public class OsuAPI
    {
        private readonly WebClient client;
        private readonly string apiKey;

        private const string ApiUrl = "https://osu.ppy.sh/";
        private const string GetMatchURL = ApiUrl + "/api/get_match";

        public OsuAPI(string apiKey)
        {
            this.apiKey = apiKey;
            client = new WebClient();
        }

        public MultiMatch GetMatch(int matchId)
        {
            return GetResult<MultiMatch>(GetMatchURL + "?k=" + apiKey + "&mp=" + matchId);
        }

        private T GetResult<T>(string url)
        {
            T theResult = default(T);
            try
            {
                var jsonResponse = client.DownloadString(url);
                theResult = JsonUtility.FromJson<T>(jsonResponse);
            }

            catch(Exception e)
            {
                Console.WriteLine("The Api Key is not Valid");
            }
            
            return theResult;
        }
    }
}
