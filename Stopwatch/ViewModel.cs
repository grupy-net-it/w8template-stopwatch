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
        private TimeSpan time;
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
                return time;
            }

            set
            {
                time = value;

                OnPropertyChanged();
                OnPropertyChanged("Hours");
                OnPropertyChanged("Minutes");
                OnPropertyChanged("Seconds");
                OnPropertyChanged("Miliseconds");
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
                return Time.Milliseconds.ToString("D2").Substring(0, 2);
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
            IsWorking = true;
        }

        public void Pause()
        {
            timer.Stop();
            IsWorking = false;
        }

        public void Stop()
        {
            Pause();
            time = new TimeSpan(0, 0, 0);
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

        private void TimerTick(object sender, object e)
        {
            Time += TickSpan;
        }
    }
}
