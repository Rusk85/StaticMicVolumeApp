using NAudio.Mixer;
using NAudio.Wave;
using System;
using System.Collections.Generic;

namespace StaticMicVolumeApp
{
    internal class Microphones : List<Microphone>
    {
        public const string DisplayMember = "MicrophoneName";

        public const string ValueMember = "MicrophoneNumber";

        public Microphones()
        {
            getMics();
        }

        private void getMics()
        {
            var devices = new List<WaveInCapabilities>();
            Func<int, WaveInCapabilities> getMic = WaveIn.GetCapabilities;
            for (int i = 0; i < WaveIn.DeviceCount; i++)
            {
                var mic = getMic(i);
                var mixerLine = new MixerLine((IntPtr)i, 0, MixerFlags.WaveIn);
                foreach (var control in mixerLine.Controls)
                {
                    if (control.ControlType == MixerControlType.Volume)
                    {
                        var volumeControl = control as UnsignedMixerControl;
                        var aMic = new Microphone(volumeControl, mic.ProductName.Trim(), i);
                        this.Add(aMic);
                        break;
                    }
                }
            }
        }
    }
}