using LiveCharts;
using LiveCharts.Wpf;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace InventoryMangementSystem
{
    public partial class DashboardPage : Page
    {
        string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        public DashboardPage()
        {
            InitializeComponent();
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            ItemAddPage itemAddPage = new ItemAddPage();
            itemAddPage.ShowDialog();
            loadAssignData();
            loadCategoryData("");
            loadStatusData("");
        }

        private void loadAssignData()
        {
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT * FROM AssignView WHERE Assign LIKE '%" + assignSearchTextBox.Text + "%' ORDER BY Quantity DESC";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                assignList.ItemsSource = dt.DefaultView;
            }
        }


        private void loadCategoryData(string selection)
        {
            SeriesCollection series = new SeriesCollection();
            string CmdString = string.Empty;
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT Category, COUNT(*) AS Quantity FROM ItemData WHERE Assign LIKE '%" + selection + "%' GROUP BY Category";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                categoryList.ItemsSource = dt.DefaultView;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PieSeries ps = new PieSeries
                    {
                        Title = rdr["Category"].ToString(),
                        Values = new ChartValues<int> { int.Parse(rdr["Quantity"].ToString()) },
                        DataLabels = true
                    };
                    series.Add(ps);
                }
                con.Close();
                categoryPie.Series = series;
            }
            categoryList.Items.SortDescriptions.Clear();
            categoryList.Items.SortDescriptions.Add(new SortDescription("Quantity", ListSortDirection.Descending));
            categoryList.Items.Refresh();
        }

        private void loadStatusData(string selection)
        {
            SeriesCollection series = new SeriesCollection();
            using (SqlConnection con = new SqlConnection(ConString))
            {
                string CmdString = "SELECT Status, COUNT(*) AS Quantity FROM ItemData WHERE Assign LIKE '%" + selection + "%' GROUP BY Status";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                statusList.ItemsSource = dt.DefaultView;
                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PieSeries ps = new PieSeries
                    {
                        Title = rdr["Status"].ToString(),
                        Values = new ChartValues<int> { int.Parse(rdr["Quantity"].ToString()) },
                        DataLabels = true
                    };
                    series.Add(ps);
                }
                con.Close();
                statusPie.Series = series;

            }
            statusList.Items.SortDescriptions.Clear();
            statusList.Items.SortDescriptions.Add(new SortDescription("Quantity", ListSortDirection.Descending));
            statusList.Items.Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            loadAssignData();
            loadCategoryData("");
            loadStatusData("");
        }

        private void assignSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadAssignData();
        }

        private void assignList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataRowView dr = (DataRowView)assignList.SelectedItem;
            if(dr != null)
            {
                string selection = dr[0].ToString();
                loadCategoryData(selection);
                loadStatusData(selection);
            }
            else
            {
                loadCategoryData("");
                loadStatusData("");
            }
        }

        private void showAllBtn_Click(object sender, RoutedEventArgs e)
        {
            loadCategoryData("");
            loadStatusData("");
        }
    }
}
