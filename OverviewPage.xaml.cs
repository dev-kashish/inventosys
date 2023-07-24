using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace InventoryMangementSystem
{
    /// <summary>
    /// Interaction logic for OverviewPage.xaml
    /// </summary>
    public partial class OverviewPage : Page
    {
        readonly string ConString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        string CmdString = string.Empty;
        public OverviewPage()
        {
            InitializeComponent();
            loadData();
        }

        private void loadData()
        {
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT * FROM ItemData WHERE Id LIKE '%" + idSearchTextBox.Text + "%' AND Name LIKE '%" + nameSearchTextBox.Text + "%' AND Category LIKE '%" + categorySearchTextBox.Text + "%' AND Assign LIKE '%" + assignSearchTextBox.Text + "%' AND Status LIKE '%" + statusSearchTextBox.Text + "%'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable("ItemData");
                sda.Fill(dt);
                itemDataGrid.ItemsSource = dt.DefaultView;
            }
            itemDataGrid.Items.SortDescriptions.Clear();
            itemDataGrid.Items.SortDescriptions.Add(new SortDescription("ID", ListSortDirection.Descending));
            itemDataGrid.Items.Refresh();
        }

        private void deleteData()
        {
            using (SqlConnection con = new SqlConnection(ConString))
            {
                DataRowView dr = (DataRowView)itemDataGrid.SelectedItem;
                con.Open();
                CmdString = "DELETE FROM ItemData WHERE Id=" + dr["Id"].ToString();
                SqlCommand cmd = new SqlCommand(CmdString, con);
                cmd.ExecuteNonQuery();
                con.Close();
                loadData();
            }
        }

        private void addItemBtn_Click(object sender, RoutedEventArgs e)
        {
            ItemAddPage addPage = new ItemAddPage();
            addPage.ShowDialog();
            loadData();
        }

        private void searchText_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadData();
        }

        private void editItemBtn_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dr = (DataRowView)itemDataGrid.SelectedItem;
            ItemEditPage editPage = new ItemEditPage();
            editPage.itemId = (int)dr["Id"];
            editPage.itemName = (string)dr["Name"];
            editPage.itemCategory = (string)dr["Category"];
            editPage.itemAssign = (string)dr["Assign"];
            editPage.itemStatus = (string)dr["Status"];
            editPage.ShowDialog();
            loadData();
        }

        private void deleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            delconfirmpopup.IsOpen = true;
        }

        private void deleteconfirmBtn_Click(object sender, RoutedEventArgs e)
        {
            deleteData();
            delconfirmpopup.IsOpen = false;
        }

        private void printBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "REPORT" + System.DateTime.Now.ToString().Replace(" ", "").Replace("-","").Replace(":", "") + ".pdf";
            saveFileDialog.DefaultExt= ".pdf";
            saveFileDialog.Filter = "PDF | *.pdf";
            saveFileDialog.Title = "Save PDF Report";
            saveFileDialog.ShowDialog();
            string filename = saveFileDialog.FileName;
            exportPdf(filename);
        }

        private void idSearchTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void exportPdf (string filename)
        {
            Document document = new Document();
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(filename, FileMode.Create));
            document.Open();
            iTextSharp.text.Font title = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12);
            iTextSharp.text.Font header = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            iTextSharp.text.Font content = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 7);
            iTextSharp.text.Font footer = iTextSharp.text.FontFactory.GetFont(FontFactory.HELVETICA, 6);

            PdfPTable table = new PdfPTable(5);
            float[] widths = new float[] { 2f, 6f, 4f, 4f, 3f };

            table.SetWidths(widths);

            table.WidthPercentage = 100;
            PdfPCell titleCell = new PdfPCell(new Phrase("Inventory List", title));
            titleCell.PaddingTop = 3;
            titleCell.PaddingBottom = 6;
            titleCell.Colspan = 5;
            titleCell.Rowspan = 3;
            titleCell.HorizontalAlignment = 1;
            table.AddCell(titleCell);


            table.AddCell(new Phrase("ID", header));
            table.AddCell(new Phrase("Name", header));
            table.AddCell(new Phrase("Category", header));
            table.AddCell(new Phrase("Assigned To", header));
            table.AddCell(new Phrase("Status", header));

            DataTable dt = new DataTable("ItemData");
            using (SqlConnection con = new SqlConnection(ConString))
            {
                CmdString = "SELECT * FROM ItemData WHERE Id LIKE '%" + idSearchTextBox.Text + "%' AND Name LIKE '%" + nameSearchTextBox.Text + "%' AND Category LIKE '%" + categorySearchTextBox.Text + "%' AND Assign LIKE '%" + assignSearchTextBox.Text + "%' AND Status LIKE '%" + statusSearchTextBox.Text + "%'";
                SqlCommand cmd = new SqlCommand(CmdString, con);
                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }


            foreach (DataRow dr in dt.Rows)
            {
                if (dr != null)
                {
                    table.AddCell(new Phrase(dr[0].ToString(), content));
                    table.AddCell(new Phrase(dr[1].ToString(), content));
                    table.AddCell(new Phrase(dr[2].ToString(), content));
                    table.AddCell(new Phrase(dr[3].ToString(), content));
                    table.AddCell(new Phrase(dr[4].ToString(), content));
                }
            }
            PdfPCell footerCell = new PdfPCell(new Phrase("Report generated by InventoSys on " + System.DateTime.Now, footer));
            footerCell.Colspan = 5;
            footerCell.VerticalAlignment = 1;
            footerCell.HorizontalAlignment = 1;
            table.AddCell(footerCell);
            document.Add(table);
            document.Close();
        }
    }
}
