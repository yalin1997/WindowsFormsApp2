using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class testInfo_Form : Form
    {
        public testInfo_Form()
        {
            InitializeComponent();
        }
        private Boolean isNum(String str)
        {
            int num;
            if (Int32.TryParse(str,out num))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != ""&isNum(textBox2.Text))
            {
                Form1 f1 = new Form1(textBox1.Text,textBox2.Text);
                f1.Show();
                Close();
            }
            else
            {
                label4.Text = "年齡請輸入數字";
            }

        }
    }
}
