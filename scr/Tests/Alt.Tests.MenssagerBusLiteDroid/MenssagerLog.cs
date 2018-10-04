using System;

namespace Alt.Tests.MenssagerBusLiteDroid
{
    public class MenssagerLog
    {
        public string Log { get; set; }
        public DateTimeOffset Time { get; set; }

        public MenssagerLog(string log)
        {
            this.Log = log;
            this.Time = DateTimeOffset.Now;
        }
    }
}