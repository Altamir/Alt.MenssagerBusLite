using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Attributes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alt.Tests.MensegerBusLite
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine($"Inicio Registrar tipos ");

            Reciver1 r1 = new Reciver1("r1");

            Console.WriteLine($"Fim Registrar tipos \n");

            Console.WriteLine($"Programa chamou post  ");

            MenssagerBus.Instance.Post(new MenssagerData() { Dado = "Avoa!" });

            Reciver1 r2 = new Reciver1("r2");


            r1.DiaparaEvento();
            List<Reciver1> ricivers = new List<Reciver1>();

            Task.Factory.StartNew(() =>
            {
                for (int i = 0; i < 50; i++)
                {
                    ricivers.Add(new Reciver1($"R0-{i.ToString()}"));
                    r1.DiaparaEvento();
                }
                foreach (var item in ricivers)
                {
                    item.DiaparaEvento();
                }
            });

            Console.WriteLine($"Fim post ");

            Console.ReadKey();
        }
    }


    public class MenssagerData
    {
        public string Dado { get; set; }
    }

    public class EventLoco
    {
        public string Dado { get; set; }
    }

    public class Reciver1
    {
        private readonly string name;
        public Reciver1(string name)
        {
            this.name = name;
            MenssagerBus.Instance.Subscrive(this);
        }

        public void DiaparaEvento()
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} chamou post  ");
            MenssagerBus.Instance.Post(new EventLoco() { Dado = "Avoeiii!" });
        }

        [Subscribe]
        public void Reciver(MenssagerData menssegerData)
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} respodeu {nameof(MenssagerData)}  - {menssegerData.Dado}");
        }

        [Subscribe]
        public void Reciver(EventLoco menssegerData)
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} respodeu {nameof(EventLoco)} - {menssegerData.Dado}");
        }
    }
}
