using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace InventoryMangementSystem
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);

        private void pnlControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void winCloseBtn_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void winMinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void winMaximizeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new PropertiesPage();
        }

        private void navDashboard_Checked(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new DashboardPage();
        }

        private void navOverview_Checked(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new OverviewPage();
        }

        private void navProperties_Checked(object sender, RoutedEventArgs e)
        {
            contentArea.Content = new PropertiesPage();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            navDashboard.IsChecked = true;
        }
    }
}
