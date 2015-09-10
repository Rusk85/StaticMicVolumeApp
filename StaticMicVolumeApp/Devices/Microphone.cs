using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Timers;

namespace StaticMicVolumeApp
{
    public class Microphone
    {
        private int _volume = 30;

        private string _micName = null;

        public string MicrophoneName { private set; get; }

        public int MicrophoneNumber { private set; get; }

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

        /// <summary>
        /// Constructor that tries to identify the mic by its name.
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="micName"></param>
        public Microphone(int volume, string micName = "samson")
        {
            _volume = volume;
            _micName = micName;
        }


        /// <summary>
        /// Constructor whos caller knows the ID of the mic.
        /// </summary>
        /// <param name="volume"></param>
        /// <param name="micNumber"></param>
        public Microphone(int volume, int micNumber)
            : this(volume)
        {
            initializeMic(micNumber);
        }


        /// <summary>
        /// Constructor whos caller knows the ID of the mic and already
        /// has its volume control.
        /// </summary>
        /// <param name="volumeControl"></param>
        /// <param name="micName"></param>
        /// <param name="micNumber"></param>
        public Microphone(UnsignedMixerControl volumeControl, 
            string micName, int micNumber)
        {
            _micVolumeControl = volumeControl;
            MicrophoneName = micName;
            MicrophoneNumber = micNumber;
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
                if (tmpDev.ProductName.ToLower().Contains(_micName.ToLower()))
                {
                    deviceNumber = i;
                    break;
                }
            }

            if (deviceNumber.HasValue)
            {
                initializeMic(deviceNumber.Value);
            }
            return _micVolumeControl != null;
        }


        private void initializeMic(int deviceNumber)
        {
            var mic = WaveIn.GetCapabilities(deviceNumber);
            var mixerLine = new MixerLine((IntPtr)deviceNumber, 0, MixerFlags.WaveIn);
            foreach (var control in mixerLine.Controls)
            {
                if (control.ControlType == MixerControlType.Volume)
                {
                    _micVolumeControl = control as UnsignedMixerControl;
                    break;
                }
            }
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