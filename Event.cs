using System;
using System.Collections.Generic;

namespace EventLogger
{
    public class Event
    {
        public Session session;
        public int index;
        public EventType type;
        public Map map;
        public List<Result> results = new List<Result>();
        public DateTime startTime;
        public DateTime resultsTime;

        public Event(Session session, int index, EventType type, Map map)
        {
            startTime = DateTime.Now;
            this.session = session;
            this.index = index;
            this.type = type;
            this.map = map;
        }

        public Result AddResult(ulong steamId, int localIndex, string name, Character character, Completion completion, float score, int position)
        {
            if (results.Count == 0)
            {
                resultsTime = DateTime.Now;
            }
            if (!session.players.TryGetValue(new Tuple<ulong, int>(steamId, localIndex), out Player player))
            {
                player = session.AddPlayer(steamId, localIndex, name);
            }
            Result result = new Result(this, player, character, completion, score, position);
            player.AddResult(index, result);
            results.Add(result);
            return result;
        }

        public override string ToString()
        {
            string s = string.Format(
                "{0}\t{1}\t{2}\r\n",
                map.GetDescription(),
                type.GetDescription(),
                startTime.ToString("yy/MM/dd HH:mm"));

            if (results.Count > 0)
            {
                s += string.Format("Position\tName\t{0}\tCharacter\tPoints\r\n{1}",
                type != EventType.CaptureTheChao ? "Time" : "Chaos",
                string.Join("\r\n", results));
            }
            else
            {
                s += "NO RESULTS";
            }

            return s;
        }
    }
}
