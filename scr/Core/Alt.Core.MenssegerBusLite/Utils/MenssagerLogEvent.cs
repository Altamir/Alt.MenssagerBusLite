using System;

namespace Alt.Core.MenssegerBusLite.Utils
{
    public class MenssagerLogEvent
    {
        public int MenssagerLogEventId { get; protected set; }
        public string Tag { get; protected set; }
        public string Log { get; protected set; }
        public DateTimeOffset Time { get; protected set; }

        public MenssagerLogEvent(string tag, string log)
        {
            this.Tag = tag;
            this.Log = log;
            this.Time = DateTimeOffset.Now;
        }
    }
}
