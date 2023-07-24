using System.Configuration;
using System.Data.SqlClient;
using System.Windows;

namespace InventoryMangementSystem
{
    /// <summary>
    /// Interaction logic for ItemEditPage.xaml
    /// </summary>
    public partial class ItemEditPage : Window
    {
        public int itemId;
        public string? itemName, itemCategory, itemAssign, itemStatus;
        public ItemEditPage()
        {
            InitializeComponent();
        }

        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void confirmBtn_Click(object sender, RoutedEventArgs e)
        {
            if (validateData())
            {
                editData();
                this.Close();
            }
            else
            {
                MessageBox.Show("All fields are required.");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            idText.Text = itemId.ToString().Trim();
            nameText.Text = itemName.Trim();
            categoryText.Text = itemCategory.Trim();
            assignText.Text = itemAssign.Trim();
            statusText.Text = itemStatus.Trim();
        }

        private bool validateData()
        {
            return idText.Text.Trim() != string.Empty &&
                   categoryText.Text.Trim() != string.Empty &&
                   nameText.Text.Trim() != string.Empty &&
                   assignText.Text.Trim() != string.Empty &&
                   statusText.Text.Trim() != string.Empty;
        }

        private void editData()
        {
            string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string CmdString = "UPDATE ItemData SET Assign=@assign,Status=@status WHERE Id = @Id";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.Parameters.AddWithValue("@Id", idText.Text.Trim());
                cmd.Parameters.AddWithValue("@assign", assignText.Text.Trim());
                cmd.Parameters.AddWithValue("@status", statusText.Text.Trim());
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
