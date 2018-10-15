using Alt.Core.MenssegerBusLite.Utils;

namespace Alt.Core.LogX.Repository
{
    public class LogXRepostory
    {
        private static readonly object block = new object();

        protected string Path { get; set; }
        protected string Filename { get; set; } = "LogX.db";
        public string CollectionName { get; protected set; }

        protected LogXRepostory(string file_name, string path, string collectionName = "MessagerLog")
        {
            if (!string.IsNullOrWhiteSpace(path))
            {
                this.Path = System.IO.Path.Combine(path, file_name);
            }
            this.CollectionName = collectionName;
        }

        public static LogXRepostory Instance { get; protected set; }

        public static LogXRepostory CreateIfNotExiste(string file_name, string path, string collectionName = "MessagerLog")
        {
            if (Instance == null)
            {
                lock (block)
                {
                    if (Instance == null)
                    {
                        Instance = new LogXRepostory(file_name, path, collectionName);
                    }
                }
            }

            return Instance;
        }


        public void Salva(MenssagerLogEvent logEvent)
        {
            using (LiteDB.LiteRepository repository = new LiteDB.LiteRepository(this.Path))
            {
                try
                {
                    lock (block)
                    {
                        repository.Upsert(entity: logEvent);
                    }
                }
                catch (System.Exception w)
                {

                    throw w;
                }
            }
        }
    }
}
