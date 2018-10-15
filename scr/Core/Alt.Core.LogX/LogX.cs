using Alt.Core.LogX.Repository;
using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Utils;
using LiteDB;
using System;


namespace Alt.Core.LogX
{
    public class LogX
    {
        public string Path { get; protected set; }
        public string Name { get; protected set; }

        protected static LogX _instance { get; set; }

        private bool InConsole { get; set; }
        private bool InDB { get; set; }


        protected LogX(string path)
        {
            this.Path = path;
        }

        public static LogX Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LogX("");
                }

                return _instance;
            }
        }

        public LogX Initialize()
        {
            LogX instance = Instance;

            if (this.InDB)
            {
                BsonMapper mapper = BsonMapper.Global;

                mapper.Entity<MenssagerLogEvent>()
                      .Id(x => x.MenssagerLogEventId);

                LogXRepostory.CreateIfNotExiste(file_name: this.Name, path: this.Path);
            }

            MenssegerBusLite.MenssagerBus.Instance.Subscrive(instance);
            return instance;
        }

        public LogX SetPath(string path)
        {
            LogX instance = Instance;
            instance.Path = path;
            return instance;
        }

        public LogX SetName(string name)
        {
            LogX instance = Instance;
            instance.Name = $"{name}.db";
            return instance;
        }

        public LogX SetConsole(bool active)
        {
            Instance.InConsole = active;
            return Instance;
        }

        public LogX SetDb(bool active)
        {
            Instance.InDB = active;           

            return Instance;
        }

        public LogX Finalize()
        {
            LogX instance = Instance;
            MenssegerBusLite.MenssagerBus.Instance.UnSubscriver(instance);
            return instance;
        }


        [Subscribe]
        public void Log(MenssagerLogEvent log)
        {
            if (this.InConsole)
            {
                Console.WriteLine($"{log.Time.ToLocalTime().ToString("dd/M hh:mm-ss")} : {log.Tag} - {log.Log}");
            }
            if (this.InDB)
            {
                LogXRepostory.Instance.Salva(log);
            }
        }
    }
}
