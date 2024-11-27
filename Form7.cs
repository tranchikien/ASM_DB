using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM1__DB
{
    public partial class Form7 : Form
    {
        private SqlConnection connectionString = new SqlConnection (@"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True");

        public Form7()
        {
            InitializeComponent();
        }

        private void Form7_Load(object sender, EventArgs e)
        {
            
        }

        private void btnCreateReport_Click(object sender, EventArgs e)
        {
            // Kiểm tra Tab hiện tại và gọi hàm tương ứng
            if (tabControl1.SelectedTab.Text == "Sales Report")
            {
                LoadSalesReport();
            }
            else if (tabControl1.SelectedTab.Text == "Product Report")
            {
                LoadProductReport();
            }
            else if (tabControl1.SelectedTab.Text == "Employee Report")
            {
                LoadEmployeeReport();
            }
        }

        // Hàm tải dữ liệu cho Sales Report (tất cả giao dịch bán)
        private void LoadSalesReport()
        {
            try
            {
                string query = @"
                    SELECT 
                        S.SaleID AS [ID],
                        P.ProductName AS [Product Name],
                        S.QuantitySold AS [Sales Quantity],
                        CONVERT(VARCHAR, S.SaleDate, 103) AS [Sale Date],
                        (S.QuantitySold * P.SellingPrice) AS [Total Mony]
                    FROM 
                        Sales S
                    INNER JOIN 
                        Product P ON S.ProductID = P.ProductID";

                SqlCommand command = new SqlCommand(query, connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView1.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Sales Report: " + ex.Message);
            }
        }


        private void btnCreateReport2_Click(object sender, EventArgs e)
        {
            // Kiểm tra Tab hiện tại và gọi hàm tương ứng
            if (tabControl1.SelectedTab.Text == "Sales Report")
            {
                LoadSalesReport();
            }
            else if (tabControl1.SelectedTab.Text == "Product Report")
            {
                LoadProductReport();
            }
            else if (tabControl1.SelectedTab.Text == "Employee Report")
            {
                LoadEmployeeReport(); 
            }
        }

        // Hàm tải dữ liệu cho Product Report (doanh thu của các sản phẩm)
        private void LoadProductReport()
        {
            try
            {
                string query = @"
                       SELECT  
                          P.ProductID AS [ID], 
                          P.ProductName AS [Product Name], 
                          SUM(S.QuantitySold) AS [Sales Quantity], 
                          SUM(S.QuantitySold * P.SellingPrice) AS [Total Revenue] 
                       FROM Sales S
                       INNER JOIN Product P ON S.ProductID = P.ProductID 
                       GROUP BY P.ProductID, P.ProductName";

                SqlCommand command = new SqlCommand(query, connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView2.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Product Report: " + ex.Message);
            }
        }

        private void btnCreateReport3_Click(object sender, EventArgs e)
        {
            // Kiểm tra Tab hiện tại và gọi hàm tương ứng
            if (tabControl1.SelectedTab.Text == "Sales Report")
            {
                LoadSalesReport();
            }
            else if (tabControl1.SelectedTab.Text == "Product Report")
            {
                LoadProductReport();
            }
            else if (tabControl1.SelectedTab.Text == "Employee Report")
            {
                LoadEmployeeReport();
            }
        }

        // Hàm tải dữ liệu cho Employee Report (doanh thu và lợi nhuận theo nhân viên)
        private void LoadEmployeeReport()
        {
            try
            {
                string query = @"
                       SELECT 
                            E.EmployeeID AS [Employee ID], 
                            E.EmployeeName AS [Employee Name], 
                            COUNT(S.SaleID) AS [Số giao dịch],
                            SUM((P.SellingPrice - P.ProductCost) * S.QuantitySold) AS [Total Profit]
                       FROM Sales S
                       INNER JOIN Employee E ON S.EmployeeID = E.EmployeeID
                       INNER JOIN Product P ON S.ProductID = P.ProductID 
                       GROUP BY E.EmployeeID, E.EmployeeName";

                SqlCommand command = new SqlCommand(query, connectionString);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                dataGridView3.DataSource = table;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading Employee Report: " + ex.Message);
            }
        }
    }
}

