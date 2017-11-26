using System;
using OsuTournamentBot.Enums;

namespace OsuTournamentBot.DataObjects
{
    public class Game
    {
        public int game_id { get; set; }

        public DateTime start_time { get; set; }

        public DateTime? end_time { get; set; }

        public int beatmap_id { get; set; }

        public Mode play_mode { get; set; }

        public int match_type { get; set; }

        public ScoringType scoring_type { get; set; }

        public TeamType team_type { get; set; }

        public int mods { get; set; }

        public Score[] scores { get; set; }
    }
}
