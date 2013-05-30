using System;
using System.Collections.Generic;
using Stopwatch.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Stopwatch
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class MainPage : LayoutAwarePage
    {
        public MainPage()
        {
            InitializeComponent();

            DataContext = new ViewModel();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(object navigationParameter, Dictionary<string, object> pageState)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ViewModel;
            if (viewModel == null)
            {
                throw new NullReferenceException();
            }

            if (!viewModel.IsWorking)
            {
                viewModel.Start();
            }
            else
            {
                viewModel.Pause();
            }
        }
    }
}
