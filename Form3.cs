using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM1__DB
{
    public partial class Form3 : Form
    {
        string connecstring = @"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True;";
        SqlConnection conn;
        SqlCommand cmd;
        SqlDataAdapter adt;
        DataTable dt;

        public Form3()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {
           
            conn = new SqlConnection(@"Data Source=LAPTOP-8078I30L\SQLEXPRESS;Initial Catalog=QuanLyBanHang1;Integrated Security=True;TrustServerCertificate=True");
            conn = new SqlConnection(connecstring);
            // Thêm cột hình ảnh và cột ProductCost vào DataGridView  
            if (!dataGridView1.Columns.Contains("ImageColumn"))
            {
                DataGridViewImageColumn imgColumn = new DataGridViewImageColumn();
                imgColumn.Name = "ImageColumn";
                imgColumn.HeaderText = "Product Image";
                imgColumn.ImageLayout = DataGridViewImageCellLayout.Zoom;
                dataGridView1.Columns.Add(imgColumn);
            }

          
            LoadData();

        }

        private void LoadData()
        {
            try
            {
                dt = new DataTable();
                using (conn = new SqlConnection(connecstring))
                {
                    conn.Open();
                    cmd = new SqlCommand("SELECT * FROM Product", conn);
                    adt = new SqlDataAdapter(cmd);
                    adt.Fill(dt);

                    // Gán dữ liệu cho DataGridView  
                    dataGridView1.DataSource = dt;

                    // Hiển thị ảnh từ cột ProductImage trong DataGridView  
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string imgPath = row.Cells["ProductImage"].Value?.ToString();
                        if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                        {
                            row.Cells["ImageColumn"].Value = Image.FromFile(imgPath);
                        }
                        else
                        {
                            row.Cells["ImageColumn"].Value = null; // Hoặc để trống  
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào  
                if (string.IsNullOrEmpty(txtID.Text) || !int.TryParse(txtID.Text, out int productId))
                {
                    MessageBox.Show("Product ID must be a valid number.");
                    return;
                }
                if (string.IsNullOrEmpty(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int inventoryQuantity))
                {
                    MessageBox.Show("Inventory Quantity must be a valid number.");
                    return;
                }
                if (string.IsNullOrEmpty(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out decimal sellingPrice))
                {
                    MessageBox.Show("Selling Price must be a valid decimal.");
                    return;
                }
                if (string.IsNullOrEmpty(txtCost.Text) || !decimal.TryParse(txtCost.Text, out decimal productCost))
                {
                    MessageBox.Show("Product Cost must be a valid decimal.");
                    return;
                }
                if (!File.Exists(txtImage.Text))
                {
                    MessageBox.Show("Invalid image path.");
                    return;
                }

                // Thêm sản phẩm vào cơ sở dữ liệu  
                using (conn = new SqlConnection(connecstring))
                {
                    conn.Open();
                    cmd = new SqlCommand("INSERT INTO Product (ProductID, ProductName, InventoryQuantity, ProductImage, SellingPrice, ProductCost) VALUES (@ProductID, @ProductName, @InventoryQuantity, @ProductImage, @SellingPrice, @ProductCost)", conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@ProductName", txtName.Text);
                    cmd.Parameters.AddWithValue("@InventoryQuantity", inventoryQuantity);
                    cmd.Parameters.AddWithValue("@ProductImage", txtImage.Text);
                    cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                    cmd.Parameters.AddWithValue("@ProductCost", productCost); // Thêm tham số ProductCost  

                    cmd.ExecuteNonQuery();
                    LoadData();
                    MessageBox.Show("Product added successfully.");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e) 
        {
            try
            {
                // Kiểm tra và chuyển đổi dữ liệu  
                if (string.IsNullOrEmpty(txtID.Text) || !int.TryParse(txtID.Text, out int productId))
                {
                    MessageBox.Show("Product ID must be a valid number.");
                    return;
                }
                if (string.IsNullOrEmpty(txtQuantity.Text) || !int.TryParse(txtQuantity.Text, out int inventoryQuantity))
                {
                    MessageBox.Show("Inventory Quantity must be a valid number.");
                    return;
                }
                if (string.IsNullOrEmpty(txtPrice.Text) || !decimal.TryParse(txtPrice.Text, out decimal sellingPrice))
                {
                    MessageBox.Show("Selling Price must be a valid decimal.");
                    return;
                }
                if (string.IsNullOrEmpty(txtCost.Text) || !decimal.TryParse(txtCost.Text, out decimal productCost))
                {
                    MessageBox.Show("Product Cost must be a valid decimal.");
                    return;
                }

                using (conn = new SqlConnection(connecstring))
                {
                    conn.Open();
                    cmd = new SqlCommand("UPDATE Product SET ProductName = @ProductName, InventoryQuantity = @InventoryQuantity, ProductImage = @ProductImage, SellingPrice = @SellingPrice, ProductCost = @ProductCost WHERE ProductID = @ProductID", conn);
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.Parameters.AddWithValue("@ProductName", txtName.Text);
                    cmd.Parameters.AddWithValue("@InventoryQuantity", inventoryQuantity);
                    cmd.Parameters.AddWithValue("@ProductImage", txtImage.Text);
                    cmd.Parameters.AddWithValue("@SellingPrice", sellingPrice);
                    cmd.Parameters.AddWithValue("@ProductCost", productCost); // Thêm tham số ProductCost  

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product updated successfully.");
                        Clear();
                    }
                    else
                    {
                        MessageBox.Show("No product found with this ID.");
                    }
                }
                LoadData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {

            try
            {
                using (conn = new SqlConnection(connecstring))
                {
                    conn.Open();
                    cmd = new SqlCommand("DELETE FROM Product WHERE ProductID=@ID", conn);
                    cmd.Parameters.AddWithValue("@ID", txtID.Text);
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Product deleted successfully!");
                    }
                    else
                    {
                        MessageBox.Show("No products found with ID.");
                    }
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
      

        private void btnSeach_Click(object sender, EventArgs e)
        {
            try
            {
                string productId = txtID.Text.Trim();
                string productName = txtName.Text.Trim();
                string priceText = txtPrice.Text.Trim();

                string query = "SELECT * FROM Product WHERE 1=1";

                if (!string.IsNullOrEmpty(productId))
                {
                    query += " AND ProductID = @ProductID";
                }
                if (!string.IsNullOrEmpty(productName))
                {
                    query += " AND ProductName LIKE @ProductName";
                }
                if (!string.IsNullOrEmpty(priceText) && decimal.TryParse(priceText, out decimal sellingPrice))
                {
                    query += " AND SellingPrice = @SellingPrice";
                }

                using (conn = new SqlConnection(connecstring))
                {
                    conn.Open();
                    cmd = new SqlCommand(query, conn);

                    if (!string.IsNullOrEmpty(productId))
                    {
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                    }
                    if (!string.IsNullOrEmpty(productName))
                    {
                        cmd.Parameters.AddWithValue("@ProductName", "%" + productName + "%");
                    }
                    if (!string.IsNullOrEmpty(priceText) && decimal.TryParse(priceText, out decimal price))
                    {
                        cmd.Parameters.AddWithValue("@SellingPrice", price);
                    }

                    adt = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    adt.Fill(dt);

                    dataGridView1.DataSource = dt;

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        string imgPath = row.Cells["ProductImage"].Value?.ToString();
                        if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                        {
                            row.Cells["ImageColumn"].Value = Image.FromFile(imgPath);
                        }
                        else
                        {
                            row.Cells["ImageColumn"].Value = null;
                        }
                    }

                    if (dt.Rows.Count == 0)
                    {
                        MessageBox.Show("No matching products found.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Clear()
        {
            txtID.Text = "";
            txtName.Text = "";
            txtQuantity.Text = "";
            txtPrice.Text = "";
            txtCost.Text = "";
            txtImage.Text = "";
            pictureBox1.Image = null;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int i = e.RowIndex;

                // Lấy dữ liệu từ hàng được chọn  
                txtID.Text = dataGridView1.Rows[i].Cells["ProductID"].Value.ToString();
                txtName.Text = dataGridView1.Rows[i].Cells["ProductName"].Value.ToString();
                txtPrice.Text = dataGridView1.Rows[i].Cells["SellingPrice"].Value.ToString();
                txtQuantity.Text = dataGridView1.Rows[i].Cells["InventoryQuantity"].Value.ToString();
                txtImage.Text = dataGridView1.Rows[i].Cells["ProductImage"].Value.ToString();
                txtCost.Text = dataGridView1.Rows[i].Cells["ProductCost"].Value.ToString(); // Hiển thị ProductCost  

                // Hiển thị ảnh trong PictureBox  
                string imgPath = txtImage.Text;
                if (!string.IsNullOrEmpty(imgPath) && File.Exists(imgPath))
                {
                    pictureBox1.Image = Image.FromFile(imgPath);
                }
                else
                {
                    pictureBox1.Image = null;
                    MessageBox.Show("Image not found at the specified path.");
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void btnBrower_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Gán đường dẫn ảnh vào textbox và PictureBox  
                    txtImage.Text = ofd.FileName;
                    pictureBox1.Image = Image.FromFile(ofd.FileName);

                    // Cập nhật hình ảnh và đường dẫn trong DataGridView  
                    if (dataGridView1.SelectedRows.Count > 0)
                    {
                        DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                        selectedRow.Cells["ProductImage"].Value = txtImage.Text; // Đường dẫn  
                        selectedRow.Cells["ImageColumn"].Value = Image.FromFile(ofd.FileName); // Hình ảnh  
                    }
                    else
                    {
                        MessageBox.Show("Please select a row to update the image.");
                    }
                }
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Clear();
            LoadData();
        }
    }
}
