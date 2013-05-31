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
        
        public ViewModel()
        {
            timer.Interval = TickSpan;
            timer.Tick += TimerTick;
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
                return Time.Milliseconds.ToString("D2").Substring(0, 1);
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
            ReleasePropertiesChange();
        }
    }
}
