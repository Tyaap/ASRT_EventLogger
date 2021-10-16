using System.Collections.Generic;

namespace EventLogger
{
    public class Player
    {
        public ulong steamId;
        public int localIndex;
        public string name;
        public Dictionary<int, Result> results = new Dictionary<int, Result>();
        public int totalPoints;
        public decimal average;
        public decimal totalTime;

        public Player(ulong steamId, int localIndex, string name)
        {
            this.steamId = steamId;
            this.localIndex = localIndex;
            this.name = name;
        }

        public void AddResult(int index, Result result)
        {
            results.Add(index, result);
            totalPoints += result.points;
            average = totalPoints * 10m / results.Count;
            if (result.thisEvent.type != EventType.CaptureTheChao)
            {
                totalTime += (decimal)result.score.ToFloat();
            }
        }
    }
}
