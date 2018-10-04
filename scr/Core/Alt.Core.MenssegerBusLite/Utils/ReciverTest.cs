using Alt.Core.MenssegerBusLite.Attributes;
using System;

namespace Alt.Core.MenssegerBusLite.Utils
{
    public class ReciverTest
    {
        public readonly string name;
        public ReciverTest(string name)
        {
            this.name = name;
            MenssagerBus.Instance.Subscrive(this);
        }

        public void DiaparaEvento1()
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} chamou post  ");
            MenssagerBus.Instance.Post(new ExampleEvent1() { Dado = $"{name}" });
        }

        public void DiaparaEvento2()
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} chamou post  ");
            MenssagerBus.Instance.Post(new ExampleEvent2() { Dado = $"{name}" , Time = DateTimeOffset.Now});
        }

        [Subscribe]
        public void Reciver(ExampleEvent1 menssegerData)
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} respodeu {nameof(ExampleEvent1)}  - {menssegerData.Dado}");
        }

        [Subscribe]
        public void Reciver(ExampleEvent2 menssegerData)
        {
            Console.WriteLine($"{DateTime.Now} : {this.name} respodeu {nameof(ExampleEvent2)} - {menssegerData.Dado} in {menssegerData.Time}");
        }
    }
}
