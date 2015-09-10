using CommandLine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SystemTimer = System.Timers.Timer;

namespace StaticMicVolumeApp
{
    public partial class Form1 : Form
    {

        private Microphone _mic = null;
        private SystemTimer _timer = null;
        private readonly int _defaultVolume;
        private readonly long _defaultInterval;


        public Form1()
        {
            InitializeComponent();
            var initiliazed = handleCmdLineArgs();
            if (!initiliazed && _mic == null)
            {
                _defaultVolume = Convert.ToInt32(tbVolume.Text);
                _defaultInterval = Convert.ToInt64(tbInterval.Text) * 1000;
                _mic = new Microphone(_defaultVolume);
                _timer = new SystemTimer(_defaultInterval);
                _timer.Elapsed += _mic.SetVolume;
            }
        }

        void _timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            _timer.Enabled = false;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (_timer.Enabled)
            {
                _timer.Enabled = false;
                this.button1.Text = "Start";
            }
            else
            {
                if (e.GetType() != typeof(CmdLineEventArgs))
                {
                    var volume = Convert.ToInt32(tbVolume.Text);
                    var interval = Convert.ToInt64(tbInterval.Text);
                    if (_defaultVolume != volume)
                    {
                        _mic.Volume = volume;
                    }
                    if (_defaultInterval != interval)
                    {
                        _timer.Interval = interval * 1000;
                    }
                }
                _timer.Enabled = true;
                this.button1.Text = "Running...";
            }
        }

        private void tbVolume_TextChanged(object sender, EventArgs e)
        {

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

        protected override void OnResize(EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                notifyIcon1.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                notifyIcon1.Visible = false;
            }
        }

        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private bool handleCmdLineArgs()
        {
            var args = Environment.GetCommandLineArgs();
            if (args.Length == 1)
            {
                return false;
            }

            args = args.Skip(1).ToArray();

            bool validArgs = true;

            var cmdLineOptions = Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(options => options, errors => { validArgs = false; return null; });

            if (!validArgs)
            {
                return false;
            }

            _mic = new Microphone(cmdLineOptions.Volume);
            _timer = new SystemTimer(cmdLineOptions.IntervalInMs);
            _timer.Elapsed += _mic.SetVolume;
            button1_Click(null, new CmdLineEventArgs());
            btnMinimize_Click(null, null);
            return true;
        }

    }
}
