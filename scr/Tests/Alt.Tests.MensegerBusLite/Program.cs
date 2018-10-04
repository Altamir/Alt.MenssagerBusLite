using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alt.Tests.MensegerBusLite
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            Console.WriteLine($"Inicio processo ");

            ReciverTest r1 = new ReciverTest("r-01");


            Console.WriteLine($"Programa chamou post  ");
            MenssagerBus.Instance.Post(new ExampleEvent2() { Dado = "Programa call 1", Time = DateTimeOffset.Now });

            ReciverTest r2 = new ReciverTest("r-02");
            MenssagerBus.Instance.Post(new ExampleEvent2() { Dado = "Programa call 2", Time = DateTimeOffset.Now });

            Console.WriteLine($"vai remover Removeu r-01  ");
            MenssagerBus.Instance.UnSubscriver(r1);
            Console.WriteLine($"terminou de Removeu r-01  ");

            Console.WriteLine($"Programa chamou post  ");
            MenssagerBus.Instance.Post(new ExampleEvent2() { Dado = "Programa call 3", Time = DateTimeOffset.Now });

            r1.DiaparaEvento1();

            List<ReciverTest> ricivers = new List<ReciverTest>();
            Console.WriteLine($"Inicio post ");
            Task.Factory.StartNew(() =>
            {
                for (int i = 1; i < 11; i++)
                {
                    ReciverTest r = new ReciverTest($"R0-{i.ToString()}");
                    ricivers.Add(r);
                    if (i > 3 && i % 2 == 1)
                    {
                        ReciverTest remover = ricivers[i - 3];
                        Console.WriteLine($"vai remover {remover.name} ");
                        MenssagerBus.Instance.UnSubscriver(remover);
                        Console.WriteLine($"terminou de Removeu {remover.name}  ");
                    }

                    if (i % 2 == 0)
                    {
                        r.DiaparaEvento1();
                    }
                    else
                    {
                        r.DiaparaEvento2();
                    }
                    if (i == 5)
                    {
                        Console.WriteLine($"Paradinha");
                    }
                }
            });
            Console.WriteLine($"Fim post ");

            Console.WriteLine($"Fim processo \n");

            Console.ReadKey();
        }
    }
}
