namespace OsuTournamentBot.DataObjects
{
    public class MatchScore : Score
    {
        public int slot { get; set; }

        public int team { get; set; }

        public int pass { get; set; }
    }
}
