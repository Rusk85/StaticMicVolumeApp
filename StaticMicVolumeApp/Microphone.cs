using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Timers;

namespace StaticMicVolumeApp
{
    public class Microphone
    {
        private int _volume = 30;

        public int Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
            }
        }

        private UnsignedMixerControl _micVolumeControl = null;

        public Microphone(int volume)
        {
            _volume = volume;
        }

        public void SetVolume(object sender, ElapsedEventArgs e)
        {
#if Debug
            Debug.WriteLine("Setting mic volume...");
#endif
            setMicVolume();
        }

        private bool tryGetMic()
        {
            int? deviceNumber = null;
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var tmpDev = WaveIn.GetCapabilities(i);
                if (tmpDev.ProductName.ToLower().Contains("samson"))
                {
                    deviceNumber = i;
                    break;
                }
            }

            if (deviceNumber.HasValue)
            {
                var devNumber = deviceNumber.Value;
                var mixerLine = new MixerLine((IntPtr)devNumber, 0, MixerFlags.WaveIn);

                foreach (var control in mixerLine.Controls)
                {
                    if (control.ControlType == MixerControlType.Volume)
                    {
                        _micVolumeControl = control as UnsignedMixerControl;
                        break;
                    }
                }
            }
            return _micVolumeControl != null;
        }

        private void setMicVolume()
        {
            if (_micVolumeControl != null || tryGetMic())
            {
                _micVolumeControl.Percent = Volume;
            }
        }
    }
}