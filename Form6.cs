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
    public partial class Form6 : Form
    {
        private string connectionString = @"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True";
        private SqlConnection connection;
        private SqlCommand command;
        private SqlDataAdapter adapter;
        private DataTable salesTable;
        private DataTable inventoryTable;


        public Form6()
        {
            InitializeComponent();
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            connection = new SqlConnection(@"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True");
            // Load dữ liệu ban đầu
            LoadSalesData();
            LoadInventoryData();
        }

        // *** TAB 1: SALES ***
        private void LoadSalesData()
        {
            try
            {
                string query = "SELECT * FROM Sales";
                adapter = new SqlDataAdapter(query, connection);
                salesTable = new DataTable();
                adapter.Fill(salesTable);
                dataGridView1.DataSource = salesTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading sales data: " + ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Sales (SaleID, ProductID, CustomerID, SaleDate, EmployeeID, QuantitySold) VALUES (@SaleID, @ProductID, @CustomerID, @SaleDate, @EmployeeID, @QuantitySold)";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@SaleID", txtSaleID.Text);
                command.Parameters.AddWithValue("@EmployeeID", txtEmployeeID.Text);
                command.Parameters.AddWithValue("@ProductID", txtProductID.Text);
                command.Parameters.AddWithValue("@CustomerID", txtCustomerID.Text);
                command.Parameters.AddWithValue("@SaleDate", dateTimePicker1.Value);
                command.Parameters.AddWithValue("@QuantitySold", txtQuantitySoID.Text);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Order created successfully!");
                LoadSalesData();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating order: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        // *** TAB 2: INVENTORY ***
        private void LoadInventoryData()
        {
            try
            {
                string query = "SELECT * FROM Inventory";
                adapter = new SqlDataAdapter(query, connection);
                inventoryTable = new DataTable();
                adapter.Fill(inventoryTable);
                dataGridView2.DataSource = inventoryTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading inventory data: " + ex.Message);
            }
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "INSERT INTO Inventory (InventoryID, ProductName, QuantityImported, ImportDate) VALUES (@InventoryID, @ProductName, @QuantityImported, @ImportDate)";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InventoryID", txtID.Text);
                command.Parameters.AddWithValue("@ProductName", txtProductname.Text);
                command.Parameters.AddWithValue("@QuantityImported", txtImported.Text);
                command.Parameters.AddWithValue("@ImportDate", dateTimePickerImportDate.Value);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Product added to inventory successfully!");
                LoadInventoryData();
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding product: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Please select a product to edit.");
                return;
            }

            try
            {
                string query = "UPDATE Inventory SET ProductName = @ProductName, QuantityImported = @QuantityImported, ImportDate = @ImportDate WHERE InventoryID = @InventoryID";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InventoryID", txtID.Text);
                command.Parameters.AddWithValue("@ProductName", txtProductname.Text);
                command.Parameters.AddWithValue("@QuantityImported", txtImported.Text);
                command.Parameters.AddWithValue("@ImportDate", dateTimePickerImportDate.Value);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Product updated successfully!");

                // Tải lại dữ liệu vào DataGridView
                LoadInventoryData();

                // Xóa dữ liệu khỏi TextBox
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating product: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtID.Text))
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            try
            {
                string query = "DELETE FROM Inventory WHERE InventoryID = @InventoryID";
                command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@InventoryID", txtID.Text);

                connection.Open();
                command.ExecuteNonQuery();
                connection.Close();

                MessageBox.Show("Product deleted successfully!");

                // Tải lại dữ liệu vào DataGridView
                LoadInventoryData();

                // Xóa dữ liệu khỏi TextBox
                ClearInputFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting product: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

        }


        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Lấy hàng hiện tại
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];

                // Hiển thị dữ liệu lên TextBox
                txtID.Text = row.Cells["InventoryID"].Value.ToString();
                txtProductname.Text = row.Cells["ProductName"].Value.ToString();
                txtImported.Text = row.Cells["QuantityImported"].Value.ToString();
                dateTimePickerImportDate.Value = Convert.ToDateTime(row.Cells["ImportDate"].Value);
            }
        }

        private void ClearInputFields()
        {
            txtID.Clear();
            txtProductname.Clear();
            txtImported.Clear();
            dateTimePickerImportDate.Value = DateTime.Now;
        }

    }
}
