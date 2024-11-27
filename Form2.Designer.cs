namespace ASM1__DB
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.productManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.personnelManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customerManagementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesAndInventoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StaticsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.MenuItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // productManagementToolStripMenuItem
            // 
            this.productManagementToolStripMenuItem.Name = "productManagementToolStripMenuItem";
            this.productManagementToolStripMenuItem.Size = new System.Drawing.Size(166, 24);
            this.productManagementToolStripMenuItem.Text = "Product Management";
            this.productManagementToolStripMenuItem.Click += new System.EventHandler(this.productManagementToolStripMenuItem_Click);
            // 
            // personnelManagementToolStripMenuItem
            // 
            this.personnelManagementToolStripMenuItem.Name = "personnelManagementToolStripMenuItem";
            this.personnelManagementToolStripMenuItem.Size = new System.Drawing.Size(181, 24);
            this.personnelManagementToolStripMenuItem.Text = "Employee Management";
            this.personnelManagementToolStripMenuItem.Click += new System.EventHandler(this.personnelManagementToolStripMenuItem_Click);
            // 
            // customerManagementToolStripMenuItem
            // 
            this.customerManagementToolStripMenuItem.Name = "customerManagementToolStripMenuItem";
            this.customerManagementToolStripMenuItem.Size = new System.Drawing.Size(178, 24);
            this.customerManagementToolStripMenuItem.Text = "Customer Management";
            this.customerManagementToolStripMenuItem.Click += new System.EventHandler(this.customerManagementToolStripMenuItem_Click);
            // 
            // salesAndInventoryToolStripMenuItem
            // 
            this.salesAndInventoryToolStripMenuItem.Name = "salesAndInventoryToolStripMenuItem";
            this.salesAndInventoryToolStripMenuItem.Size = new System.Drawing.Size(151, 24);
            this.salesAndInventoryToolStripMenuItem.Text = "Sales and Inventory";
            this.salesAndInventoryToolStripMenuItem.Click += new System.EventHandler(this.ssalesAndInventoryToolStripMenuItem_Click);
            // 
            // StaticsToolStripMenuItem
            // 
            this.StaticsToolStripMenuItem.Name = "StaticsToolStripMenuItem";
            this.StaticsToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
            this.StaticsToolStripMenuItem.Text = "Statics";
            this.StaticsToolStripMenuItem.Click += new System.EventHandler(this.reportAnfStaticsticsToolStripMenuItem_Click);
            // 
            // MenuItem
            // 
            this.MenuItem.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MenuItem.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.productManagementToolStripMenuItem,
            this.personnelManagementToolStripMenuItem,
            this.customerManagementToolStripMenuItem,
            this.salesAndInventoryToolStripMenuItem,
            this.StaticsToolStripMenuItem,
            this.toolStripMenuItem1});
            this.MenuItem.Location = new System.Drawing.Point(0, 0);
            this.MenuItem.Name = "MenuItem";
            this.MenuItem.Size = new System.Drawing.Size(808, 28);
            this.MenuItem.TabIndex = 0;
            this.MenuItem.Text = "MenuItem";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(156, 24);
            this.toolStripMenuItem1.Text = "toolStripMenuItem1";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(808, 298);
            this.Controls.Add(this.MenuItem);
            this.MainMenuStrip = this.MenuItem;
            this.Name = "Form2";
            this.Text = "Main";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.MenuItem.ResumeLayout(false);
            this.MenuItem.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem productManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem personnelManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem customerManagementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesAndInventoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem StaticsToolStripMenuItem;
        private System.Windows.Forms.MenuStrip MenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}