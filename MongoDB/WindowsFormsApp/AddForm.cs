using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp
{
    public partial class AddForm : Form
    {
        public AddForm()
        {
            InitializeComponent();            
        }
        Form1 pForm;
        public AddForm(Form1 pForm):this()
        {
            this.pForm = pForm;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pForm.add_Data(this.textBox1.Text);
            this.Close();
        }
    }
}
