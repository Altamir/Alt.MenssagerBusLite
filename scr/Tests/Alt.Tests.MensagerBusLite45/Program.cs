using Alt.Core.MenssegerBusLite;
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
            Console.WriteLine($"Inicio Registrar tipos ");

            ReciverTest r1 = new ReciverTest("r-01");

            Console.WriteLine($"Fim Registrar tipos \n");

            Console.WriteLine($"Programa chamou post  ");

            MenssagerBus.Instance.Post(new ExampleEvent1() { Dado = "Programa call" });

            ReciverTest r2 = new ReciverTest("r-02");

            Console.WriteLine($"vai remover Removeu r-1  ");
            MenssagerBus.Instance.UnSubscriver(r1);
            Console.WriteLine($"terminou de Removeu r-1  ");

            MenssagerBus.Instance.Post(new ExampleEvent2() { Dado = "Programa call" });

            r1.DiaparaEvento1();

            List<ReciverTest> ricivers = new List<ReciverTest>();

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

            Console.ReadKey();
        }
    }
}
