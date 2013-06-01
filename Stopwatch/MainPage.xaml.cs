using System;
using System.Collections.Generic;
using Stopwatch.Common;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;
using Windows.UI;

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

            this.CreateSquares();
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

        private void CreateSquares()
        {
            this.CreateSquaresOfType(2, 5, this.SquaresDeciseconds, 24, 6, "Deciseconds");
            this.CreateSquaresOfType(6, 10, this.SquaresSeconds, 19, 6, "Seconds");
            this.CreateSquaresOfType(6, 10, this.SquaresMinutes, 19, 6, "Minutes");
            this.CreateSquaresOfType(4, 6, this.SquaresHours, 24, 6, "Hours");
        }

        private void CreateSquaresOfType(int rows, int cols, Grid grid, double width, double margin, string name)
        {
            for (int row = 0; row < rows; row++)
            {
                grid.RowDefinitions.Add(new RowDefinition());
                for (int col = 0; col < cols; col++)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    var rect = new Rectangle() { Width = width, Height = width, Margin = new Thickness(0, 0, margin, margin), Fill = new SolidColorBrush(Colors.White)/*, Opacity = 0.2*/ };
                    grid.Children.Add(rect);
                    rect.SetValue(Grid.RowProperty, row);
                    rect.SetValue(Grid.ColumnProperty, col);
                    var binding = new Binding();
                    binding.Path = new PropertyPath(string.Format("Squares{1}[{0}]", row * cols + col, name));
                    rect.SetBinding(OpacityProperty, binding);
                }
            }
        }

        private void Unsnap_Click(object sender, RoutedEventArgs e)
        {
            Windows.UI.ViewManagement.ApplicationView.TryUnsnap();
        }
    }
}
