using System.Collections.Generic;
using System.IO;
using static MemoryHelper;

namespace EventLogger
{
    public class Logger
    {
        public string logFolder;
        public bool cacheEventPlayers;
        public Session currentSession;
        public List<Session> sessions = new List<Session>();
        public Dictionary<Session, int> logFileCurrentEventCount = new Dictionary<Session, int>();
        public int dpTime;
        public int dpPercentage;

        public Logger(string logFolder = null, bool cacheEventPlayers = false, int dpTime = 3, int dpPercentage = 1)
        {
            this.logFolder = string.IsNullOrEmpty(logFolder) ? "" : logFolder + "\\";
            this.cacheEventPlayers = cacheEventPlayers;
            this.dpTime = dpTime;
            this.dpPercentage = dpPercentage;
        }

        public Session NewSession()
        {
            currentSession = new Session();
            sessions.Add(currentSession);
            logFileCurrentEventCount.Add(currentSession, 0);
            return currentSession;
        }

        //  Log the details of the current event in this session 
        public Event NewEvent(Session session = null)
        {
            if (session == null)
            {
                session = currentSession;
            }
            EventType type = (EventType)ReadUInt(ReadInt(0xBC7430));
            Map map = (Map)ReadUInt(ReadInt(0xBC7434));
            Event newEvent = session.AddEvent(type, map);
            if (cacheEventPlayers)
            {
                CacheEventPlayers(newEvent);
            }
            return newEvent;
        }

        public void CacheEventPlayers(Event thisEvent = null)
        {
            if (thisEvent == null)
            {
                thisEvent = currentSession.lastEvent;
            }
            int count = ReadInt(0xBCE934);
            for (int i = 0; i < count; i++)
            {
                if (!GetOnlineIndex(i, out int onlineIndex, out int localIndex))
                {
                    continue; // this is either an AI or a player who left before we could cache them
                }
                ulong steamId = ReadULong(ReadInt(ReadInt(0xEC1A88) + 0x528 + onlineIndex * 4) + 0x2BF0);
                string name = ReadString(ReadInt(ReadInt(0xEC1A88) + 0x528 + onlineIndex * 4) + 0x25D8 + 0x180 * localIndex);
                thisEvent.CachePlayer(steamId, localIndex, name, i);
            }
        }

        // Logs the results for an event
        public void LogEventResults(Event thisEvent = null)
        {
            if (thisEvent == null)
            {
                thisEvent = currentSession.lastEvent;
            }

            int count = ReadInt(0xBCE934);
            for (int i = 0; i < count; i++)
            {
                int engineIndex = ReadInt(0xC4FA94 + i * 4); // only works for custom game
                ulong steamId;
                int localIndex;
                string name;
                if (thisEvent.playerCache != null && thisEvent.playerCache.TryGetValue(engineIndex, out Player player))
                {
                    steamId = player.steamId;
                    localIndex = player.localIndex;
                    name = player.name;
                }
                else if (GetOnlineIndex(engineIndex, out int onlineIndex, out localIndex))
                {
                    steamId = ReadULong(ReadInt(ReadInt(0xEC1A88) + 0x528 + onlineIndex * 4) + 0x2BF0);
                    name = ReadString(ReadInt(ReadInt(0xEC1A88) + 0x528 + onlineIndex * 4) + 0x25D8 + 0x180 * localIndex);
                }
                else
                {
                    continue; // this is either an AI or an un-cached player who left the lobby mid-race
                }
                Character character = (Character)(ReadByte(ReadInt(0xEC1A88) + 0x101D50 + engineIndex) / 2);
                Completion completion = GetPlayerCompletion(engineIndex, currentSession.lastEvent.type);
                float score = GetPlayerScore(engineIndex, thisEvent.type, completion);

                thisEvent.AddResult(steamId, localIndex, name, character, completion, score, i + 1);
            }
        }

        public bool GetOnlineIndex(int engineIndex, out int onlineIndex, out int localIndex)
        {
            int playerId = ReadUShort(ReadInt(0xEC1A88) + 0x101D3C + engineIndex * 2);
            if (playerId != 0)
            {
                int onlineId = playerId & 0xFFF8;
                int count = ReadByte(ReadInt(0xEC1A88) + 0x525);
                for (int i = 0; i < count; i++)
                {
                    int testId = ReadUShort(ReadInt(ReadInt(0xEC1A88) + 0x528 + i * 4) + 0x6A);
                    if (testId == onlineId)
                    {
                        onlineIndex = i;
                        localIndex = playerId & 7;
                        return true;
                    }
                }
            }
            onlineIndex = -1;
            localIndex = -1;
            return false;
        }

        public Completion GetPlayerCompletion(int engineIndex, EventType type)
        {
            int count = ReadInt(0xBCE904);
            for (int i = 0; i < count; i++)
            {
                if (ReadInt(0xBCE8B0 + i * 8) == engineIndex)
                {
                    Completion completion = (Completion)ReadInt(0xBCE8B0 + i * 8 + 4);
                    if (type == EventType.BattleRace && completion == Completion.Finished && GetProgressPercentage(engineIndex) < 99.5)
                    {
                        completion = Completion.DNF;
                    }
                    return completion;
                }
            }
            return Completion.NA;
        }

        public float GetPlayerScore(int engineIndex, EventType type, Completion completion)
        {
            if (type == EventType.BoostRace || type == EventType.NormalRace)
            {
                if (completion == Completion.Finished)
                {
                    return ReadFloat(ReadInt(ReadInt(ReadInt(0xBCE920) + engineIndex * 4) + 0xC1B8) + 0x28); // race time
                }
                else
                {
                    return GetProgressPercentage(engineIndex); // progress percentage
                }
            }
            else if (type == EventType.BattleRace)
            {
                return ReadFloat(ReadInt(0xBCE90C) + 0x120 + engineIndex * 4); // survival/race time
            }
            else if (type == EventType.BattleArena)
            {
                return ReadFloat(ReadInt(0xBCE910) + 0x120 + engineIndex * 4); // survival time
            }
            else
            {
                return ReadByte(ReadInt(ReadInt(ReadInt(0xBCE920) + engineIndex * 4) + 0xC1B8) + 0xC0); // chaos
            }
        }

        public float GetProgressPercentage(int engineIndex)
        {
            return ReadFloat(ReadInt(ReadInt(ReadInt(0xBCE920) + engineIndex * 4) + 0xC1B8) + 0x30) / ReadFloat(ReadInt(0xE9A9D8) + 0x70) / ReadByte(0xBCE908) * 100;
        }

        public void WriteLogFiles(Session session = null)
        {
            if (session == null)
            {
                session = currentSession;
            }
            if (logFileCurrentEventCount.TryGetValue(session, out int eventCount))
            {
                logFileCurrentEventCount[session] = session.events.Count;
            }
            else
            {
                eventCount = 0;
            }
            if (eventCount == session.events.Count)
            {
                return;
            }

            using (StreamWriter sw = File.AppendText(logFolder + "SessionEvents_" + currentSession.startTime.ToString("yyyy-MM-dd--HH-mm") + ".txt"))
            {
                for (int i = eventCount; i < session.events.Count; i++)
                {
                    sw.Write((i > 0 ? "\r\n\r\n" : "") + session.events[i].ToString(dpTime, 1));
                }
            }
            File.WriteAllText(
                logFolder + "SessionSummary_" + currentSession.startTime.ToString("yyyy-MM-dd--HH-mm") + ".txt",
                currentSession.ToString()
            );

            using (StreamWriter sw = File.CreateText(logFolder + "Players_" + currentSession.startTime.ToString("yyyy-MM-dd--HH-mm") + ".txt"))
            {
                sw.WriteLine("Name\tSteam ID");
                foreach (Player player in session.players.Values)
                {
                    sw.WriteLine(player.name + "\t" + player.steamId);
                }
            }
        }
    }
}
