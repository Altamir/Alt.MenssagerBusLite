using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using System.Linq;
using System.Threading;

namespace Alt.Tests.MenssagerBusLiteDroid
{
    [Service]
    public class InverterService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        [return: GeneratedEnum]
        public override StartCommandResult OnStartCommand(Intent intent, [GeneratedEnum] StartCommandFlags flags, int startId)
        {
            return StartCommandResult.RedeliverIntent;
        }

        public override void OnCreate()
        {
            MenssagerBus.Instance.Subscrive(this);
            base.OnCreate();
        }

        public override void OnDestroy()
        {
            MenssagerBus.Instance.UnSubscriver(this);
            base.OnDestroy();
        }

        [Subscribe]
        public void Inverter(ReverseRequiredMenssager menssager)
        {

            new Thread(new ThreadStart(() =>
            {
                MenssagerBus.Instance.Post(new MenssagerLogEvent("InverterService", $"iniciou inverter"));

                Thread.Sleep(3000);               
                string result = "";
                for (int i = menssager.Dado.Length - 1; i >= 0; i--)
                {
                    result += menssager.Dado[i];
                }

                MenssagerBus.Instance.Post(new ReverserResultMensseger(result));

            })).Start();

        }
    }
}