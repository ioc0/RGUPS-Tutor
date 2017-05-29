using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGUPS_Teacher
{
    public partial class Form1 : Form
    {
        private string pathToBook1, pathToBook2, pathToBook3, link1, link2, link3;


        private void btnOpenBook2_Click(object sender, EventArgs e)
        {

        }

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            ofd.FileName = Application.StartupPath + "\\..\\..\\book.pdf";
            if (ofd.ShowDialog() == DialogResult.OK) {
                txtBoxPathToBook1.Text = ofd.FileName;
                pathToBook1 = ofd.FileName;
                

            }
        }
    }
}
