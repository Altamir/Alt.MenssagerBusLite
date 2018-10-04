using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Attributes;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;

namespace Alt.Tests.MenssagerBusLiteDroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button Button;
        private EditText log;
        private EditText resul;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            this.Button = FindViewById<Button>(Resource.Id.bt_inverter);
            this.resul = FindViewById<EditText>(Resource.Id.ed_result);
            this.log = FindViewById<EditText>(Resource.Id.ed_log);

            Button.Click += Button_Click;
            log.Text = "";
            resul.Text = "";

            StartService(new Intent(this, typeof(InverterService)));
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            MenssagerBus.Instance.Post(new MenssagerLog("Clicou no bt"));

            var texto = resul.Text;
            resul.Text = "Aguarde....";

            MenssagerBus.Instance.Post(new ReverseRequiredMenssager(texto));
        }

        protected override void OnStart()
        {
            base.OnStart();
            MenssagerBus.Instance.Subscrive(this);
            MenssagerBus.Instance.Post(new MenssagerLog("Iniciouu"));
        }

        [Subscribe]
        public void Log(MenssagerLog menssagerLog)
        {
            RunOnUiThread(() =>
            {
                log.Text += $"{menssagerLog.Time.ToLocalTime().ToString("dd/M hh:mm-ss")}  {menssagerLog.Log} \n";
            });
        }

        [Subscribe]
        public void Reciver(ReverserResultMensseger resultMensseger)
        {
            MenssagerBus.Instance.Post(new MenssagerLog("Recebeu o inverter"));
            RunOnUiThread(() =>
            {
                resul.Text = $"{resultMensseger.Dado}";
            });
            MenssagerBus.Instance.Post(new MenssagerLog($"Inverteru para {resultMensseger.Dado}"));
        }

        protected override void OnStop()
        {
            MenssagerBus.Instance.UnSubscriver(this);
            base.OnStop();
        }
    }
}