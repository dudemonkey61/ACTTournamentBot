using System;

namespace OsuTournamentBot.DataObjects
{
    public class Match
    {
        public int match_id { get; set; }

        public string name { get; set; }

        public DateTime start_time { get; set; }

        public DateTime? end_time { get; set; }
    }
}
