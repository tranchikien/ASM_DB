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
    public partial class Form5 : Form
    {

        string connectionString = @"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True;";
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adt;
        DataTable dt;

        public Form5()
        {
            InitializeComponent();
        }

        private void Form5_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(@"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True");
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT CustomerID, CustomerName, PhoneNumber, Address FROM Customers";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading customer data: {ex.Message}");
            }
        }

        


        private void LoadTransactionHistory(int customerId)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = @"SELECT S.SaleID, P.ProductName, S.QuantitySold, S.SaleDate, E.EmployeeName FROM Sales SINNER JOIN Product P ON S.ProductID = P.ProductIDINNER JOIN Employee E ON S.EmployeeID = E.EmployeeIDWHERE S.CustomerID = @CustomerID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@CustomerID", customerId);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transaction history: {ex.Message}");
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    cmd = new SqlCommand( "INSERT INTO Customers (CustomerID, CustomerName, PhoneNumber, Address) VALUES (@ID, @Name, @Phone, @Address)", conn);
                    cmd.Parameters.AddWithValue("@ID", txtID.Text);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtNumber.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Add customer successfully!");
                    LoadCustomerData();
                    ClearCustomerForm();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding customer: {ex.Message}");
            }
        }
        private void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    cmd = new SqlCommand("UPDATE Customers SET CustomerName = @Name, PhoneNumber = @Phone, Address = @Address WHERE CustomerID = @ID", conn);
                    cmd.Parameters.AddWithValue("@ID", txtID.Text);
                    cmd.Parameters.AddWithValue("@Name", txtName.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtNumber.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer update successful!");
                        LoadCustomerData();
                        ClearCustomerForm();
                    }
                    else
                    {
                        MessageBox.Show("No customer found with this ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating customer: {ex.Message}");
            }
        }
        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string deleteSalesQuery = "DELETE FROM Sales WHERE CustomerID = @ID";
                    using (SqlCommand cmdDeleteSales = new SqlCommand(deleteSalesQuery, conn))
                    {
                        cmdDeleteSales.Parameters.AddWithValue("@ID", txtID.Text);
                        cmdDeleteSales.ExecuteNonQuery();
                    }
                    
                    string query = "DELETE FROM Customers WHERE CustomerID = @ID";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@ID", txtID.Text);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Customer deleted successfully!");
                        LoadCustomerData();
                        ClearCustomerForm();
                    }
                    else
                    {
                        MessageBox.Show("No customer found with this ID.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while deleting customer: {ex.Message}");
            }
        }

        private void btnSeach_Click(object sender, EventArgs e)
        {

            try
            {
                if (string.IsNullOrWhiteSpace(txtID.Text))
                {
                    MessageBox.Show("Please enter keywords to search!");
                    return;
                }

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Kiểm tra xem input có phải là số không
                    string searchQuery;
                    SqlCommand cmd = new SqlCommand();

                    if (int.TryParse(txtID.Text, out int customerId))
                    {
                        // Nếu nhập ID (số), tìm kiếm theo CustomerID
                        searchQuery = @"
                           SELECT CustomerID, CustomerName, PhoneNumber, Address 
                           FROM Customers 
                           WHERE CustomerID = @SearchID";
                        cmd = new SqlCommand(searchQuery, conn);
                        cmd.Parameters.AddWithValue("@SearchID", customerId);
                    }
                    else
                    {
                        // Nếu nhập chuỗi, tìm kiếm theo tên khách hàng
                        searchQuery = @"
                           SELECT CustomerID, CustomerName, PhoneNumber, Address 
                           FROM Customers 
                           WHERE CustomerName LIKE @SearchName";
                        cmd = new SqlCommand(searchQuery, conn);
                        cmd.Parameters.AddWithValue("@SearchName", "%" + txtID.Text + "%");
                    }

                    // Thực thi truy vấn
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {
                        dataGridView1.DataSource = dt;
                        MessageBox.Show("Search success! Customer" + dt.Rows.Count + " found.");
                    }
                    else
                    {
                        MessageBox.Show("No suitable customers found!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while searching....: {ex.Message}");
            }
        }


        private void dataGridViewCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtID.Text = dataGridView1.Rows[e.RowIndex].Cells["CustomerID"].Value.ToString();
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells["CustomerName"].Value.ToString();
                txtNumber.Text = dataGridView1.Rows[e.RowIndex].Cells["PhoneNumber"].Value.ToString();
                txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells["Address"].Value.ToString();

                int customerId = int.Parse(txtID.Text);
                LoadTransactionHistory(customerId);
            }
        }
        private void ClearCustomerForm()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtNumber.Text = "";
            txtAddress.Text = "";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadCustomerData(); // Tải lại toàn bộ dữ liệu nhân viên
            txtID.Clear(); 
        }
    }

}
