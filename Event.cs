﻿using System;
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
        public Dictionary<int, Player> playerCache; // engine index -> player

        public Event(Session session, int index, EventType type, Map map)
        {
            startTime = DateTime.Now;
            this.session = session;
            this.index = index;
            this.type = type;
            this.map = map;
        }

        public Player CachePlayer(ulong steamId, int localIndex, string name, int engineIndex)
        {
            if (playerCache == null)
            {
                playerCache = new Dictionary<int, Player>();
            }
            if (!session.players.TryGetValue(new Tuple<ulong, int>(steamId, localIndex), out Player player))
            {
                player = session.AddPlayer(steamId, localIndex, name);
            }
            playerCache.Add(engineIndex, player);
            return player;
        }

        public Result AddResult(ulong steamId, int localIndex, string name, Character character, Completion completion, int score, int position)
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
            return ToString();
        }

        public string ToString(bool hexTimes = false)
        {
            string s = string.Format(
                "{0}\t{1}\t{2}",
                map.GetDescription(),
                type.GetDescription(),
                startTime.ToString("yy/MM/dd HH:mm"));

            if (results.Count > 0)
            {
                s += string.Format("\r\nPosition\tName\t{0}\tCharacter\tPoints", type != EventType.CaptureTheChao ? "Time" : "Chaos");
                if (type != EventType.CaptureTheChao && hexTimes)
                {
                    s += "\tHex Time";
                }
                for (int i = 0; i < results.Count; i++)
                {
                    s += "\r\n" + results[i].ToString(hexTimes);
                }
            }
            else
            {
                s += "\r\nNO RESULTS";
            }
            return s;
        }
    }
}
