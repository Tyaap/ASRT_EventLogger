using System;
using System.Collections.Generic;

namespace EventLogger
{
    public class Session
    {
        public DateTime startTime;
        public Dictionary<Tuple<ulong, int>, Player> players = new Dictionary<Tuple<ulong, int>, Player>();
        public List<Event> events = new List<Event>();
        public Event lastEvent;

        public Session()
        {
            startTime = DateTime.Now;
        }

        public Event AddEvent(EventType type, Map map)
        {
            lastEvent = new Event(this, events.Count, type, map);
            events.Add(lastEvent);
            return lastEvent;
        }

        public Player AddPlayer(ulong steamId, int localIndex, string name)
        {
            Player player = new Player(steamId, localIndex, name);
            players.Add(new Tuple<ulong, int>(steamId, localIndex), player);
            return player;
        }

        public override string ToString()
        {
            List<Player> sortedPlayers = new List<Player>(players.Values);
            sortedPlayers.Sort(ComparePlayers);
            string s = "\tRESULTS";
            foreach (Event e in events)
            {
                if (e.results.Count == 0)
                {
                    continue; // no results for this event
                }
                s += "\t" + (MapAcronym)e.map;
            }
            s += "\t\tPoints\tTracks\tAverage\tTime";

            for (int i = 0; i < sortedPlayers.Count; i++)
            {
                Player player = sortedPlayers[i];
                s += "\r\n" + (i + 1) + "°\t" + player.name;
                foreach (Event e in events)
                {
                    if (e.results.Count == 0)
                    {
                        continue; // no results for this event
                    }
                    s += "\t";
                    if (player.results.TryGetValue(e.index, out Result result))
                    {
                        s += result.points;
                    }
                }
                s += "\t\t" + player.totalPoints + "\t" + player.results.Count + "\t" + player.average + "\t" + Result.TruncatedTimeString(player.totalTime, 3);
                if (player.results.Count < events.Count)
                {
                    s += "*"; // asterisk on total times that don't include all session events
                }
            }
            return s;
        }

        public int ComparePlayers(Player x, Player y)
        {
            int comp = y.totalPoints.CompareTo(x.totalPoints);
            return comp != 0 ? comp : y.average.CompareTo(x.average);
        }
    }
}
