using Alt.Core.LogX;
using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Utils;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Widget;
using Java.IO;

namespace Alt.Tests.MenssagerBusLiteDroid
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        private Button inverter;
        private Button go_second;
        private Button clear;
        private EditText log;
        private EditText resul;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            this.inverter = FindViewById<Button>(Resource.Id.bt_inverter);
            this.go_second = FindViewById<Button>(Resource.Id.bt_second);
            this.clear = FindViewById<Button>(Resource.Id.bt_clear);
            this.resul = FindViewById<EditText>(Resource.Id.ed_result);
            this.log = FindViewById<EditText>(Resource.Id.ed_log);

            this.go_second.Click += Go_second_Click;
            this.inverter.Click += Button_Click;
            this.clear.Click += Clear_Click;
            this.log.Text = "";
            this.log.Enabled = false;
            this.resul.Text = "";

            string path = CriaFolderDb();

            LogX.Instance
                .SetPath(path)
                .SetName("Test")
                .SetConsole(true)
                .SetDb(true)
                .Initialize();

            StartService(new Intent(this, typeof(InverterService)));
        }

        private void Go_second_Click(object sender, System.EventArgs e)
        {
            StartActivity(typeof(SecondActivity));
        }

        private void Clear_Click(object sender, System.EventArgs e)
        {
            this.log.Text = "";
            this.resul.Text = "";
        }

        private void Button_Click(object sender, System.EventArgs e)
        {
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Clicou no bt"));

            string texto = this.resul.Text;
            this.resul.Text = "Aguarde....";

            MenssagerBus.Instance.Post(new ReverseRequiredMenssager(texto));
        }

        protected override void OnStart()
        {
            base.OnStart();
            MenssagerBus.Instance.Subscrive(this);
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Main Iniciouu"));
        }

        [Subscribe]
        public void Log(MenssagerLogEvent menssagerLog)
        {
            RunOnUiThread(() =>
            {
                this.log.Text += $"{menssagerLog.Time.ToLocalTime().ToString("dd/M hh:mm-ss")}  {menssagerLog.Log} \n";
            });
        }

        [Subscribe]
        public void Reciver(ReverserResultMensseger resultMensseger)
        {
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Main Recebeu o inverter"));
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", $"Inverteu para {resultMensseger.Dado}"));
            RunOnUiThread(() =>
            {
                this.resul.Text = $"{resultMensseger.Dado}";
            });
        }

        protected override void OnStop()
        {
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Main Stop"));
            base.OnStop();
        }

        protected override void OnDestroy()
        {
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Main Destroy"));
            MenssagerBus.Instance.UnSubscriver(this);
            LogX.Instance.Finalize();
            base.OnDestroy();
        }


        public static string CriaFolderDb()
        {
            File file = new File(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + $"/LogDbs/");
            if (!file.Exists())
            {
                if (file.Mkdirs())
                {
                    return file.Path;
                }
            }
            else
            {
                return file.Path;
            }

            return string.Empty;
        }
    }
}