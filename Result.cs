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
            return ToString();
        }

        public string ToString(int dpTime = 3, int dpPercentage = 1)
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
                    scoreString = TruncatedTimeString(score, dpTime);
                }
                else
                {
                    scoreString = TruntateNumString(score.ToString(), dpPercentage) + "% (DNF)";
                }
            }
            else
            {
                scoreString = TruncatedTimeString(score, dpTime);
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

        public static string TruncatedTimeString(float time, int dp)
        {
            string s = TimeSpan.FromSeconds(time).ToString(@"m\:ss");
            if (dp == 0)
            {
                return s;
            }
            string tmp = TruntateNumString(time.ToString(), dp);
            int point = tmp.IndexOf('.');
            if (point == -1)
            {
                return s;
            }
            return s + tmp.Substring(point);
        }

        public static string TruntateNumString(string s, int dp)
        {
            if (dp < 0)
            {
                return s;
            }
            int point = s.IndexOf('.');
            if (point == -1 || point + dp + 1 > s.Length)
            {
                return s;
            }
            else if (dp == 0)
            {
                return s.Substring(0, point);
            }
            else
            {
                return s.Substring(0, point + dp + 1).TrimEnd('0').TrimEnd('.');
            }
        }
    }
}
