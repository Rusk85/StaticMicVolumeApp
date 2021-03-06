﻿using CommandLine;

namespace StaticMicVolumeApp
{
    public class CommandLineOptions
    {
        [Option('v', "volume", HelpText="Volume in Percent.")]
        public int Volume { get; set; }

        private float _interval = 30000;
        [Option('i', "interval", Required = false, HelpText="Interval in Seconds.")]
        public int Interval
        {
            set
            {
                _interval = value * 1000;
            }
        }

        public float IntervalInMs
        {
            get
            {
                return _interval;
            }
        }


        [Option('n', "name", Required=false)]
        public string MicName { get; set; }
    }
}