using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alt.Tests.MensagerBusLite45
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Inicio Testes");
            Exemple exemple1 = new Exemple("01");
            Exemple exemple2 = new Exemple("02");
            Exemple exemple3 = new Exemple("03");

            MenssagerBus.Instance.Subscrive(exemple1);
            MenssagerBus.Instance.Subscrive(exemple1);
            MenssagerBus.Instance.Subscrive(exemple1);

            MenssagerBus.Instance.Post(new MenssagerLogEvent("tst", "d1"));

            MenssagerBus.Instance.Subscrive(exemple2);
            MenssagerBus.Instance.Subscrive(exemple3);

            exemple3 = null;

            MenssagerBus.Instance.Post(new MenssagerLogEvent("tst", "d2"));

            MenssagerBus.Instance.Post(new MenssagerLogEvent("tst", "d3"));
   


            Console.WriteLine($"Fim Testes");

            Console.ReadKey();
        }
    }

    public class Exemple
    {
        public string Name { get; set; }

        public Exemple(string name)
        {
            this.Name = name;
        }

        [Subscribe]
        public void Reciver(MenssagerLogEvent log_messager)
        {
            Console.WriteLine($"{log_messager.Time.ToString("dd/MM/yyyy HH:mm:ss.fff")} - {Name} recebeu {log_messager.Log}");
        }
    }
}
