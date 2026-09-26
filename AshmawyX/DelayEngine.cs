using System;
using System.Threading;

namespace AshmawyX
{
    public class DelayEngine
    {
        private double delayFrom = 0.0;
        private double delayTo = 0.0;
        private readonly Random rnd = new Random();

        private string mode = "Random";
        // Modes supported: "None", "Fixed", "Random"

        public void SetMode(string delayMode)
        {
            mode = delayMode;
        }

        public void SetFixed(double seconds)
        {
            delayFrom = seconds;
            delayTo = seconds;
        }

        public void SetRandom(double from, double to)
        {
            delayFrom = from;
            delayTo = to;
        }

        public void Apply()
        {
            if (mode == "None")
                return;

            if (mode == "Fixed")
            {
                Thread.Sleep((int)(delayFrom * 1000));
                return;
            }

            if (mode == "Random")
            {
                double min = Math.Min(delayFrom, delayTo);
                double max = Math.Max(delayFrom, delayTo);

                double randomDelay = min + rnd.NextDouble() * (max - min);
                Thread.Sleep((int)(randomDelay * 1000));
                return;
            }
        }
    }
}
