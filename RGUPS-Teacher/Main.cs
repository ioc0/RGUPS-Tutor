using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace RGUPS_Teacher
{
    public partial class Main : Form
    {
        XmlDocument xDoc = new XmlDocument();
        string path;
        public Main()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var fm = new Form1();
            fm.Show();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        
        private void addTreeNode(XmlNode xmlNode, TreeNode treeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList xNodeList;
            if (xmlNode.HasChildNodes) //The current node has children
            {
                xNodeList = xmlNode.ChildNodes;
                for (var x = 0; x <= xNodeList.Count - 1; x++)
                //Loop through the child nodes
                {
                    xNode = xmlNode.ChildNodes[x];
                    treeNode.Nodes.Add(new TreeNode(xNode.Name));
                    tNode = treeNode.Nodes[x];
                    addTreeNode(xNode, tNode);
                }
            }
            else //No children, so add the outer xml (trimming off whitespace)
            {
                treeNode.Text = xmlNode.OuterXml.Trim();
            }
        }

        private void writeTreeNode()
        {
            try
            {
                string sStartupPath = Application.StartupPath;
                XmlTextWriter objXmlTextWriter =
                     new XmlTextWriter(path, null);
                objXmlTextWriter.Formatting = Formatting.Indented;
                objXmlTextWriter.WriteStartDocument();

                objXmlTextWriter.WriteStartElement("MySelectedValues");
                objXmlTextWriter.WriteStartElement("BookName");
                objXmlTextWriter.WriteString("new");
                objXmlTextWriter.WriteEndElement();
                
                objXmlTextWriter.WriteStartElement("ReleaseYear");
                objXmlTextWriter.WriteString("SOMEDATA");

                objXmlTextWriter.WriteEndElement();
                objXmlTextWriter.WriteStartElement("Publication");
                objXmlTextWriter.WriteString("Some publication");
                objXmlTextWriter.WriteEndElement();
                objXmlTextWriter.WriteEndElement();
                objXmlTextWriter.WriteEndDocument();
                objXmlTextWriter.Flush();
                objXmlTextWriter.Close();
                MessageBox.Show("The following file has been successfully created\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void tsmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Open XML Document";
            dlg.Filter = "XML Files (*.xml)|*.xml";
            
            dlg.FileName = Application.StartupPath + "\\..\\..\\example.xml";
            
            if (dlg.ShowDialog() == DialogResult.OK)
                path = dlg.FileName;
            try
                {
                    Cursor = Cursors.WaitCursor;

                    
                    xDoc.Load(dlg.FileName);

                    treeView1.Nodes.Clear();
                    treeView1.Nodes.Add(new
                        TreeNode(xDoc.DocumentElement.Name));
                    var tNode = new TreeNode();
                    tNode = treeView1.Nodes[0];
                    addTreeNode(xDoc.DocumentElement, tNode);

                    treeView1.ExpandAll();
                }
                catch (XmlException xExc)
                //Exception is thrown is there is an error in the Xml
                {
                    MessageBox.Show(xExc.Message);
                }
                catch (Exception ex) //General exception
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    Cursor = Cursors.Default; //Change the cursor back
                }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            writeTreeNode();
        }
    }
}
