using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RGUPS_Teacher
{
    public partial class Form1 : Form
    {
        public static string path = @".\config.cfg";
        public static string[] allLinks;
        

        public Form1()
        {
            InitializeComponent();
            try
            {
                allLinks = File.ReadAllLines(path, Encoding.UTF8);
                txtBoxPathToBook1.Text = allLinks[0];
                txtBoxPathToBook2.Text = allLinks[1];
                txtBoxPathToBook3.Text = allLinks[2];
                txtBoxPathToAdv1.Text = allLinks[3];
                txtBoxPathToAdv2.Text = allLinks[4];
                txtBoxPathToAdv3.Text = allLinks[5];

            }
            catch (Exception)
            {

                throw;
            }
        }
        //Methods
        private void btnAdvMat1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxPathToAdv1.Text = ofd.FileName;
            }



        }
        private void btnSaveBook1_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxPathToBook1.Text = ofd.FileName;
            }

        }
        private void btnOpenSave3_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxPathToBook3.Text = ofd.FileName;
            }
        }
        private void btnOpenBook2_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxPathToBook2.Text = ofd.FileName;
            }
        }

        private void btnAdvMat2_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxPathToAdv2.Text = ofd.FileName;
            }
        }

        private void btnAdvMat3_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Title = "Open PDF Document";
            ofd.Filter = "PDF Files (*.pdf)|*.pdf";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtBoxPathToAdv3.Text = ofd.FileName;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            allLinks = new string[6] { txtBoxPathToBook1.Text, txtBoxPathToBook2.Text, txtBoxPathToBook3.Text, txtBoxPathToAdv1.Text, txtBoxPathToAdv2.Text, txtBoxPathToAdv3.Text };
            

            // This text is added only once to the file.
            if (!File.Exists(path))
            {
                // Create a file to write to.
                File.WriteAllLines(path, allLinks, Encoding.UTF8);
            }
            else
            {
                File.Delete(path);
                File.WriteAllLines(path, allLinks, Encoding.UTF8);

            }
            Close();

            // This text is always added, making the file longer over time
            // if it is not deleted.



            // Open the file to read from.
            //string[] readText = File.ReadAllLines(path, Encoding.UTF8);
            //foreach (string s in readText)
            //{
            //    Console.WriteLine(s);
            //}
        }
    }
}
