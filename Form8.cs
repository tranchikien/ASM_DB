using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace ASM1__DB
{
    public partial class Form8 : Form
    {
        //// Kết nối với Form4
        private Form4 form4;
        private string connectionString = @"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True;";

        public Form8(Form4 form4)
        {
            InitializeComponent();
            this.form4 = form4;
        }


        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtusername.Text.Trim();
            string password = txtpassword.Text.Trim();
            string role = cmbRole.SelectedItem?.ToString();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Please fill in all fields.", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string hashedPassword = HashPassword(password); // Hash mật khẩu

            try
            {
                string filePath = "users.txt"; // Đường dẫn đến file lưu thông tin người dùng

                using (StreamWriter writer = new StreamWriter(filePath, true)) // 'true' để append vào cuối file
                {
                    // Lưu thông tin người dùng vào file
                    writer.WriteLine($"{username},{hashedPassword},{role}");
                }

                MessageBox.Show("User registered successfully!", "Success",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Chuyển form tương ứng dựa trên role
                if (role == "Employee")
                {
                    Form4 form4 = new Form4();
                    form4.Show();
                }
                else if (role == "Customer")
                {
                    Form5 form5 = new Form5(); // Form khách hàng
                    form5.Show();
                }

                this.Hide(); // Đóng Form8
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error registering user: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
             Form1 form1 = new Form1();
            form1.Show();
            this.Close();
        }

        // Hàm hash mật khẩu
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                foreach (byte b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }

        //// Tạo ID mới cho khách hàng
        //private string GenerateNewCustomerID(SqlConnection conn)
        //{
        //    string query = "SELECT ISNULL(MAX(CustomerID), 0) + 1 AS NewID FROM Customers";
        //    using (SqlCommand cmd = new SqlCommand(query, conn))
        //    {
        //        int newID = (int)cmd.ExecuteScalar();
        //        return newID.ToString();
        //    }
        //}


    }
}
