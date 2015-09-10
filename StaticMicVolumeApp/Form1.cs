using CommandLine;
using MoreLinq;
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
        private Microphones _microphones = null;
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
            fill_cbMics();
            cbMics.SelectedIndexChanged += selectMicrophone;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
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
                cbMics.Enabled = true;
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
                cbMics.Enabled = false;
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

            if (String.IsNullOrEmpty(cmdLineOptions.MicName))
            {
                _mic = new Microphone(cmdLineOptions.Volume);
            }
            else
            {
                _mic = new Microphone(cmdLineOptions.Volume, cmdLineOptions.MicName);
            }
            _timer = new SystemTimer(cmdLineOptions.IntervalInMs);
            _timer.Elapsed += _mic.SetVolume;
            button1_Click(null, new CmdLineEventArgs());
            btnMinimize_Click(null, null);
            return true;
        }

        private void fill_cbMics()
        {
            var mics = new Microphones();
            if (mics.Any())
            {
                var width = TextRenderer.MeasureText(mics.MaxBy(m => 
                    m.MicrophoneName.Length).MicrophoneName, cbMics.Font).Width;
                _microphones = mics;
                cbMics.Width = width + 10;
                cbMics.DataSource = mics;
                cbMics.DisplayMember = Microphones.DisplayMember;
                cbMics.ValueMember = Microphones.ValueMember;
            }
        }

        private void selectMicrophone(object sender, EventArgs e)
        {
            var selectedMicNumber = Convert.ToInt32(cbMics.SelectedValue);
            var selectedMic = _microphones.FirstOrDefault(m => 
                m.MicrophoneNumber == selectedMicNumber);
            if (selectedMic != null)
            {
                initializeSelectedMicMonitoring(selectedMic);
            }

        }


        private void initializeSelectedMicMonitoring(Microphone mic)
        {
            _mic = mic;
            _timer = new SystemTimer(Convert.ToInt64(tbInterval.Text) * 1000);
            _timer.Elapsed += mic.SetVolume;
            _mic.Volume = Convert.ToInt32(tbVolume.Text);
        }



    }
}
