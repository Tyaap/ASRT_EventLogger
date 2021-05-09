using System;

namespace EventLogger
{
    public class Result
    {
        public Result(Event thisEvent, Player player, Character character, Completion completion, float score, int position)
        {
            this.thisEvent = thisEvent;
            this.player = player;
            this.character = character;
            this.score = score;
            this.position = position;
            this.points = 11 - position;
            this.completion = completion;
        }

        public Event thisEvent;
        public Player player;
        public Character character;
        public Completion completion;
        public float score;
        public int position;
        public int points;

        public override string ToString()
        {
            string scoreString;
            if (thisEvent.type == EventType.CaptureTheChao)
            {
                scoreString = score.ToString();
            }
            else if (thisEvent.type != EventType.BattleRace)
            {
                if (completion != Completion.DNF)
                {
                    scoreString = TimeSpan.FromSeconds(score).ToString(@"m\:ss\.fff");
                }
                else
                {
                    scoreString = TruncatedFloatString(score, 1) + "% (DNF)";
                }
            }
            else
            {
                scoreString = TimeSpan.FromSeconds(score).ToString(@"m\:ss\.fff");
                if (completion == Completion.Finished)
                {
                    scoreString += " (finished)";
                }
            }

            return string.Format("{0}°\t{1}\t{2}\t{3}\t{4}",
                position,
                player.name,
                scoreString,
                character.GetDescription(),
                points
            );; ;
        }

        public static string TruncatedFloatString(float n, int decimals)
        {
            string num = n.ToString();
            int dp = num.IndexOf('.');
            if (dp == -1)
            {
                return num;
            }
            else if (decimals == 0)
            {
                return num.Substring(0, dp);
            }
            else
            {
                return num.Substring(0, dp + decimals + 1);
            }
        }
    }
}
