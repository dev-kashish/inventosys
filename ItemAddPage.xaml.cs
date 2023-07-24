using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace InventoryMangementSystem
{
    /// <summary>
    /// Interaction logic for ItemAddPage.xaml
    /// </summary>
    public partial class ItemAddPage : Window
    {
        public ItemAddPage()
        {
            InitializeComponent();
        }

        private void quantityText_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void addItem(int count)
        {
            string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string CmdString = "INSERT INTO ItemData (Name,Category,Assign,Status) VALUES (@name,@category,@assign,@status)";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@name", nameText.Text.Trim());
                cmd.Parameters.AddWithValue("@category", categoryText.Text.Trim());
                cmd.Parameters.AddWithValue("@assign", assignText.Text.Trim());
                cmd.Parameters.AddWithValue("@status", statusText.Text.Trim());
                con.Open();
                for (int i = 0; i < count; i++)
                {
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
        }

        private bool validateInput()
        {
            return nameText.Text.Trim() != string.Empty &&
                   categoryText.Text.Trim() != string.Empty &&
                   assignText.Text.Trim() != string.Empty &&
                   statusText.Text.Trim() != string.Empty;
        }
        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (validateInput())
            {
                addItem(Convert.ToInt16(quantityText.Text.Trim()));
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields are required");
            }
        }
    }
}
