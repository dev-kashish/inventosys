using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace InventoryMangementSystem.UserControls
{
    /// <summary>
    /// Interaction logic for AutoCompleteBox.xaml
    /// </summary>
    public partial class AutoCompleteBox : UserControl
    {
        public string PlaceholderText
        {
            get { return (string)GetValue(PlaceholderTextProperty); }
            set { SetValue(PlaceholderTextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PlaceholderText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderTextProperty =
            DependencyProperty.Register("PlaceholderText", typeof(string), typeof(AutoCompleteBox), new PropertyMetadata(""));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(AutoCompleteBox), new PropertyMetadata(""));


        public AutoCompleteBox()
        {
            InitializeComponent();
            PART_ListBox.AddHandler(ListBoxItem.MouseDownEvent, new RoutedEventHandler(PART_ListBox_Click), true);
        }

        private void loadData()
        {
            string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            string CmdString = string.Empty;
            string? Tag = this.Tag.ToString();
            List<string> args = new List<string>();
            using (SqlConnection con = new SqlConnection(ConString))
            {

                CmdString = "SELECT * FROM " + Tag + "View WHERE " + Tag + " LIKE '%" + PART_TextBox.Text + "%'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    args.Add(reader.GetString(0));
                }
                con.Close();
                PART_ListBox.ItemsSource = args;
            }
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            PART_TextBox.Tag = PlaceholderText.ToString();
            PART_TextBox.Text = this.Text;
        }

        private void PART_TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            PART_ListBox.Visibility = Visibility.Hidden;
        }

        private void PART_TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            loadData();
            PART_ListBox.Visibility = Visibility.Visible;
        }

        private void PART_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadData();
            this.Text = PART_TextBox.Text;
        }

        private void PART_ListBox_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {

        }

        private void PART_ListBox_Click(object sender, RoutedEventArgs e)
        {
            PART_TextBox.Text = PART_ListBox.SelectedValue.ToString();
            PART_ListBox.UnselectAll();
            PART_ListBox.Visibility = Visibility.Hidden;
        }
    }
}
