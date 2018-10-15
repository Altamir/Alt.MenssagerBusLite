using Alt.Core.MenssegerBusLite;
using Alt.Core.MenssegerBusLite.Attributes;
using Alt.Core.MenssegerBusLite.Utils;
using Android.App;
using Android.OS;
using Android.Widget;
using System;

namespace Alt.Tests.MenssagerBusLiteDroid
{
    [Activity(Label = "SecondActivity")]
    public class SecondActivity : Activity
    {

        private Button voltar;
        private Button clear;
        private EditText log;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.Second);

            this.voltar = FindViewById<Button>(Resource.Id.bt_voltar);
            this.clear = FindViewById<Button>(Resource.Id.bt_clear);

            this.log = FindViewById<EditText>(Resource.Id.ed_log);
            this.log.Enabled = false;
            this.voltar.Click += VoltarButton_Click;
            this.clear.Click += Clear_Click;
            this.log.Text = "";

        }

        protected override void OnStart()
        {
            MenssagerBus.Instance.Subscrive(this);
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Second Iniciou"));
            base.OnStart();
        }

        protected override void OnStop()
        {
            MenssagerBus.Instance.Post(new MenssagerLogEvent("MenssagerBusLiteDroid", "Second stop"));
            MenssagerBus.Instance.UnSubscriver(this);
            base.OnStop();
        }

        private void VoltarButton_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private void Clear_Click(object sender, System.EventArgs e)
        {
            this.log.Text = "";
        }

        [Subscribe]
        public void Log(MenssagerLogEvent menssagerLog)
        {
            RunOnUiThread(() =>
            {
                this.log.Text += $"{menssagerLog.Time.ToLocalTime().ToString("dd/M hh:mm-ss")}  {menssagerLog.Log} \n";
            });
        }
    }
}