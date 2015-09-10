using CommandLine;

namespace StaticMicVolumeApp
{
    public class CommandLineOptions
    {
        [Option('v', "volume")]
        public int Volume { get; set; }

        private float _interval = 30000;

        [Option('i', "interval", Required = false)]
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
    }
}