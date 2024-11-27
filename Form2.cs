using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ASM1__DB
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void productManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 productManagementToolStripMenuItem  = new Form3();
            productManagementToolStripMenuItem.ShowDialog();
        }

        private void personnelManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form4 personnelManagementToolStripMenuItem = new Form4();
            personnelManagementToolStripMenuItem.ShowDialog();
        }

        private void customerManagementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form5 customerManagementToolStripMenuItem = new Form5();
            customerManagementToolStripMenuItem.ShowDialog();
        }

        private void ssalesAndInventoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form6 salesAndInventoryToolStripMenuItem = new Form6();
            salesAndInventoryToolStripMenuItem.ShowDialog();
        }

        private void reportAnfStaticsticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form7 reportAnfStaticsticsToolStripMenuItem = new Form7();
            reportAnfStaticsticsToolStripMenuItem.ShowDialog();
        }
       
    }
}
