using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Security.Cryptography;

namespace ASM1__DB
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
           
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string filePath = "users.txt";

            // Kiểm tra nếu file không tồn tại, tạo file và thêm tài khoản Admin
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    // Hash mật khẩu Admin
                    string hashedAdminPassword = HashPassword("1000");
                    sw.WriteLine($"Admin,{hashedAdminPassword},Admin");
                }
            }
            else
            {
                // Đảm bảo Admin đã có trong file
                var users = File.ReadAllLines(filePath);
                bool adminExists = users.Any(line => line.StartsWith("Admin,"));

                if (!adminExists)
                {
                    using (StreamWriter sw = File.AppendText(filePath))
                    {
                        string hashedAdminPassword = HashPassword("1000");
                        sw.WriteLine($"Admin,{hashedAdminPassword},Admin");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string pass = txtPass.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Please enter your name", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrEmpty(pass))
            {
                MessageBox.Show("Please enter your password", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string filePath = "users.txt"; // Đường dẫn đến file lưu thông tin người dùng

                if (File.Exists(filePath))
                {
                    var users = File.ReadAllLines(filePath);
                    foreach (var line in users)
                    {
                        var data = line.Split(',');
                        string storedUsername = data[0];
                        string storedPassword = data[1];
                        string storedRole = data[2];

                        // Kiểm tra tên đăng nhập và mật khẩu (so sánh mật khẩu đã hash)
                        if (storedUsername == name && storedPassword == HashPassword(pass))
                        {
                            // Kiểm tra nếu là admin
                            if (storedUsername == "Admin" && storedPassword == HashPassword("1000"))
                            {
                                Form2 form2 = new Form2(); // Form Menu của Admin
                                form2.Show();
                                this.Hide(); // Ẩn form đăng nhập
                                return;
                            }
                            // Chuyển đến form tương ứng theo role
                            else if (storedRole == "Customer")
                            {
                                Form5 form5 = new Form5(); // Form khách hàng
                                form5.Show();
                            }
                            else if (storedRole == "Employee")
                            {
                                Form4 form4 = new Form4(); // Form nhân viên
                                form4.Show();
                            }

                            this.Hide(); // Ẩn form đăng nhập sau khi đăng nhập thành công
                            return;
                        }
                    }
                }

                MessageBox.Show("Invalid username or password", "Notice",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult check_Exit = MessageBox.Show("Do you want to exit?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (check_Exit == DialogResult.Yes)
            {
                this.Close();
            }
        }

        

        private void SignUp_Click(object sender, EventArgs e)
        {
            Form8 form8 = new Form8(new Form4());
            form8.Show();
            this.Hide();
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

        private void RegisterAdmin()
        {
            string adminPassword = "1000";
            string hashedPassword = HashPassword(adminPassword);

            // Giả sử bạn đang thêm admin vào file
            string adminData = "Admin," + hashedPassword + ",Admin";
            File.AppendAllText("users.txt", adminData + Environment.NewLine);
        }
    }
}
