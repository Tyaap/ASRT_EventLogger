using System;

namespace EventLogger
{
    public class Result
    {
        public Result(Event thisEvent, Player player, Character character, Completion completion, int score, int position)
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
        public int score;
        public int position;
        public int points;

        public override string ToString()
        {
            return ToString();
        }

        public string ToString(bool hexTime = false)
        {
            string scoreString;
            
            if (thisEvent.type == EventType.NormalRace || thisEvent.type == EventType.BoostRace)
            {
                if (completion != Completion.DNF)
                {
                    scoreString = TruncatedTimeString(score.ToFloat(), 3);
                }
                else
                {
                    scoreString = TruncatedNumString(score.ToFloat(), 1) + "% (DNF)";
                }
            }
            else if (thisEvent.type == EventType.CaptureTheChao)
            {
                scoreString = score.ToString();
            }
            else
            {
                scoreString = TruncatedTimeString(score.ToFloat(), 3);
                if (completion == Completion.Finished)
                {
                    scoreString += " (finished)";
                }
            }

            string s = string.Format("{0}°\t{1}\t{2}\t{3}\t{4}",
                position,
                player.name,
                scoreString,
                character.GetDescription(),
                points
            );
            if (hexTime && thisEvent.type != EventType.CaptureTheChao && completion != Completion.DNF)
            {
                s += "\t" + score.ToString("X8");
            }
            return s;
        }


        public static string TruncatedTimeString(float time, int dp)
        {
            return ((int)time / 60) + ":" + ((int)time % 60).ToString("00") + TruncatedDecimalsString(time.ToString(), dp);
        }

        public static string TruncatedTimeString(decimal time, int dp)
        {
            return ((int)time / 60) + ":" + ((int)time % 60).ToString("00") + TruncatedDecimalsString(time.ToString(), dp);
        }

        public static string TruncatedDecimalsString(string s, int dp)
        {
            string tmp = TruncatedNumString(s, dp);
            int point = tmp.IndexOf('.');
            if (point == -1)
            {
                return "";
            }
            return tmp.Substring(point);
        }

        public static string TruncatedNumString(float num, int dp)
        {
            return TruncatedNumString(num.ToString(), dp);
        }

        public static string TruncatedNumString(decimal num, int dp)
        {
            return TruncatedNumString(num.ToString(), dp);
        }

        public static string TruncatedNumString(string s, int dp)
        {
            if (dp < 0)
            {
                return s;
            }
            int point = s.IndexOf('.');
            int numLen = point + dp + 1;
            if (point == -1 || numLen > s.Length)
            {
                return s;
            }
            else if (dp == 0)
            {
                return s.Substring(0, point);
            }
            else
            {
                return s.Substring(0, numLen).TrimEnd('0').TrimEnd('.');
            }
        }
    }
}
