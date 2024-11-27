using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Cryptography;



namespace ASM1__DB
{
    public partial class Form4 : Form
    {
        string connectionstring = @"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True;";
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adt;
        DataTable dt;

        public Form4()
        {
            InitializeComponent();
        }


        private void Form4_Load(object sender, EventArgs e)
        {
            conn = new SqlConnection(@"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True");
            LoadEmployeeData();
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;

        }

        public void LoadEmployeeData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string query = "SELECT EmployeeID, EmployeeName, Username, Position, Password FROM Employee";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt;

                        // Hiển thị tiêu đề cột mật khẩu
                        dataGridView1.Columns["Password"].HeaderText = "Password (Hashed)";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading employee data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        public string GenerateNewEmployeeID()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Employee";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        int count = (int)cmd.ExecuteScalar();
                        return "E" + (count + 1).ToString("D3"); // Tạo ID mới
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating new employee ID: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return "E001"; // Trả về ID mặc định nếu có lỗi
            }
        }

        private void SearchEmployee(string keyword)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    // Tìm kiếm theo tên hoặc mã nhân viên
                    string query = "SELECT EmployeeID, EmployeeName, Username, Position, Password FROM Employee " +
                                   "WHERE EmployeeName LIKE @Keyword OR EmployeeID LIKE @Keyword";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Keyword", "%" + keyword + "%");

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        da.Fill(dt);
                        dataGridView1.DataSource = dt; // Hiển thị kết quả tìm kiếm
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


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


        private void ClearTextBoxes()
        {
            txtName.Clear();
            txtUserName.Clear();
            txtPassword.Clear();
            txtPosition.Clear();
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                txtID.Text = row.Cells["EmployeeID"].Value.ToString(); // Không thêm tiền tố "E"
                txtName.Text = row.Cells["EmployeeName"].Value.ToString();
                txtUserName.Text = row.Cells["Username"].Value.ToString();
                txtPassword.Text = ""; // Không hiển thị mật khẩu đã hash
                txtPosition.Text = row.Cells["Position"].Value.ToString();
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtUserName.Text) ||
                    string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtPosition.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string newID = GenerateNewEmployeeID();

                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    conn.Open();
                    string query = "INSERT INTO Employee (EmployeeID, EmployeeName, Username, Password, Position) VALUES (@ID, @Name, @Username, @Password, @Position)";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", newID.Substring(1)); // Chỉ lưu số vào DB
                        cmd.Parameters.AddWithValue("@Name", txtName.Text);
                        cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                        cmd.Parameters.AddWithValue("@Password", HashPassword(txtPassword.Text)); // Mã hóa mật khẩu
                        cmd.Parameters.AddWithValue("@Position", txtPosition.Text);
                        cmd.ExecuteNonQuery();
                    }
                }

                LoadEmployeeData();

                MessageBox.Show($"Employee added successfully with ID: {newID}!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                ClearTextBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string employeeID = txtID.Text; // Lấy ID trực tiếp từ TextBox mà không xử lý thêm tiền tố

                if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtUserName.Text) ||
                    string.IsNullOrEmpty(txtPassword.Text) || string.IsNullOrEmpty(txtPosition.Text))
                {
                    MessageBox.Show("Please fill in all fields.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionstring))
                    {
                        conn.Open();
                        string query = "UPDATE Employee SET EmployeeName = @Name, Username = @Username, Password = @Password, Position = @Position WHERE EmployeeID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", employeeID); // Truyền ID đúng định dạng
                            cmd.Parameters.AddWithValue("@Name", txtName.Text);
                            cmd.Parameters.AddWithValue("@Username", txtUserName.Text);
                            cmd.Parameters.AddWithValue("@Password", HashPassword(txtPassword.Text)); // Mã hóa mật khẩu
                            cmd.Parameters.AddWithValue("@Position", txtPosition.Text);

                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                MessageBox.Show("No records were updated. Please check the Employee ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Employee updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadEmployeeData(); // Làm mới DataGridView sau khi cập nhật
                                ClearTextBoxes();   // Xóa nội dung TextBox
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to edit.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }



        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                string employeeID = txtID.Text; // Lấy ID trực tiếp từ TextBox

                try
                {
                    using (SqlConnection conn = new SqlConnection(connectionstring))
                    {
                        conn.Open();
                        string query = "DELETE FROM Employee WHERE EmployeeID = @ID";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            cmd.Parameters.AddWithValue("@ID", employeeID);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                MessageBox.Show("No records were deleted. Please check the Employee ID.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                MessageBox.Show("Employee deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadEmployeeData(); // Làm mới DataGridView sau khi xóa
                                ClearTextBoxes();   // Xóa nội dung TextBox
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting employee: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select an employee to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void dataGridView1_CellContentClick(object sender, EventArgs e)
        {
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string keyword = txtID.Text;
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a keyword to search.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SearchEmployee(keyword); // Gọi hàm tìm kiếm
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadEmployeeData(); // Tải lại toàn bộ dữ liệu nhân viên
            txtID.Clear(); // Xóa nội dung trong TextBox tìm kiếm
        }
    }    
}


