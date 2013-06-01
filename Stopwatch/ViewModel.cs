using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Stopwatch.Annotations;
using Windows.UI.Xaml;

namespace Stopwatch
{
    public sealed class ViewModel : INotifyPropertyChanged
    {
        private static readonly TimeSpan TickSpan = new TimeSpan(0, 0, 0, 0, 10);
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private DateTime startTime;
        private TimeSpan timeElapsed;
        private bool isWorking;
        private int visualizedDeciseconds = 0;
        private int visualizedSeconds = 0;
        private int visualizedMinutes = 0;
        private int visualizedHours = 0;
        private double[] squaresDeciseconds = new double[10];
        private double[] squaresSeconds = new double[60];
        private double[] squaresMinutes = new double[60];
        private double[] squaresHours = new double[24];

        private const double ActiveOpacity = 1.0;
        private const double InactiveOpacity = 0.2;

        public ViewModel()
        {
            timer.Interval = TickSpan;
            timer.Tick += TimerTick;
            startTime = DateTime.Now;
            TimeElapsed = new TimeSpan(0);

            for (int i = 0; i < this.squaresDeciseconds.Length; i++) this.squaresDeciseconds[i] = InactiveOpacity;
            for (int i = 0; i < this.squaresSeconds.Length; i++) this.squaresSeconds[i] = InactiveOpacity;
            for (int i = 0; i < this.squaresMinutes.Length; i++) this.squaresMinutes[i] = InactiveOpacity;
            for (int i = 0; i < this.squaresHours.Length; i++) this.squaresHours[i] = InactiveOpacity;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public TimeSpan Time
        {
            get
            {
                return DateTime.Now - startTime + timeElapsed;
            }
        }

        public TimeSpan TimeElapsed
        {
            get
            {
                return timeElapsed;
            }

            set
            {
                timeElapsed = value;

                ReleasePropertiesChange();
            }
        }

        public string Hours
        {
            get
            {
                return Time.Hours.ToString("D2");
            }
        }

        public string Minutes
        {
            get
            {
                return Time.Minutes.ToString("D2");
            }
        }

        public string Seconds
        {
            get
            {
                return Time.Seconds.ToString("D2");
            }
        }

        public string Miliseconds
        {
            get
            {
                return ((Time.Milliseconds) / 100).ToString();
            }
        }

        public bool IsWorking
        {
            get
            {
                return isWorking;
            }

            set
            {
                isWorking = value;
                OnPropertyChanged();
            }
        }

        public void Start()
        {
            timer.Start();
            startTime = DateTime.Now;
            IsWorking = true;
        }

        public void Pause()
        {
            timer.Stop();
            timeElapsed += DateTime.Now - startTime;
            IsWorking = false;
        }

        public void Stop()
        {
            Pause();
            timeElapsed = new TimeSpan(0, 0, 0);
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void ReleasePropertiesChange()
        {
            OnPropertyChanged("TimeElapsed");
            OnPropertyChanged("Hours");
            OnPropertyChanged("Minutes");
            OnPropertyChanged("Seconds");
            OnPropertyChanged("Miliseconds");
        }

        private void TimerTick(object sender, object e)
        {
            RecomputeSquares(ref this.visualizedDeciseconds, 10, this.Time.Milliseconds / 100, this.squaresDeciseconds, "SquaresDeciseconds");
            RecomputeSquares(ref this.visualizedSeconds, 60, this.Time.Seconds, this.squaresSeconds, "SquaresSeconds");
            RecomputeSquares(ref this.visualizedMinutes, 60, this.Time.Minutes, this.squaresMinutes, "SquaresMinutes");
            RecomputeSquares(ref this.visualizedHours, 24, this.Time.Hours, this.squaresHours, "SquaresHours");

            ReleasePropertiesChange();
        }

        private void RecomputeSquares(ref int visualized, int max, int currentTime, double[] squaresArray, string propertyName)
        {
            var changed = false;
            if (visualized != currentTime)
            {
                changed = true;
            }

            int i = visualized;
            var toRedraw = visualized = (currentTime % max);
            if (currentTime==0 )
            {
                i = 0;
                toRedraw = max - 1;
            }

            for (; i <= toRedraw; i++)
            {
                if (i < visualized)
                    squaresArray[i] = ActiveOpacity;
                else
                    squaresArray[i] = InactiveOpacity;
            }

            if (changed) OnPropertyChanged(propertyName);
        }

        public double[] SquaresDeciseconds
        {
            get { return this.squaresDeciseconds; }
        }

        public double[] SquaresSeconds
        {
            get { return this.squaresSeconds; }
        }

        public double[] SquaresMinutes
        {
            get { return this.squaresMinutes; }
        }

        public double[] SquaresHours
        {
            get { return this.squaresHours; }
        }
    }
}
