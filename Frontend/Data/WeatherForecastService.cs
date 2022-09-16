using LeagueVisualiser.Controllers;
using LeagueVisualiser.Models;

namespace Frontend.Data
{
    public class WeatherForecastService
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        public async Task<List<RiotSummoner>> GetSummonersAsync()
        {
            List<RiotSummoner> riotSummoners = new();
            RiotClient client = new();
            List<string> summonerNames = new List<string>
            {
                "Trussen", "Ahadoo","dumbooo1998", "Skruen","Specht", "KongOller","runeklonki", "Kolomito", "TSchmidt"
            };
            foreach (var s in summonerNames)
            {
                RiotSummoner summoner = await client.getSummoner(s);
                List<LeagueStats> ls = await client.getMatchList(summoner.id);
                int soloq = 0;
                int flex = 0;
                for (int i = 0; i < ls.Count; i++)
                {
                    if (ls[i].queueType == "RANKED_SOLO_5x5")
                        soloq = i;
                    if (ls[i].queueType == "RANKED_FLEX_SR")
                        flex = i;
                }
                var soloqwinrate = ((float)ls[soloq].wins / ((float)ls[soloq].wins + (float)ls[soloq].losses));
                var flexwinrate = ((float)ls[flex].wins / ((float)ls[flex].wins + (float)ls[flex].losses));
                summoner.soloqwinrate = soloqwinrate * 100;
                summoner.flexwinrate = flexwinrate * 100;
                summoner.games = ls[soloq].wins + ls[soloq].losses;
                summoner.soloqrank = ls[soloq].rank;
                summoner.soloqtier = ls[soloq].tier;
                summoner.flexqrank = ls[flex].rank;
                summoner.flextier = ls[flex].tier;
                riotSummoners.Add(summoner);
            }
            return riotSummoners;

        }

    }
}