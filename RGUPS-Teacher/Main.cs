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
using System.Xml;

/*Code taken from
 * https://www.codeproject.com/Articles/13099/Loading-and-Saving-a-TreeView-control-to-an-XML-fi
 * 
*/
namespace RGUPS_Teacher
{
    public partial class Main : Form
    {
        private StreamWriter sr;
        XmlDocument xDoc = new XmlDocument();
        string path;
        private TreeNode mySelectedNode;
        
        

        

        public Main()
        {
            
            InitializeComponent();
            
        }

       

        
    

       
    

        private void tsmExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tsmOpen_Click(object sender, EventArgs e)
        {
            OpenXmlNodes();
        }

        

       

        private void selectNode(object sender, TreeNodeMouseClickEventArgs e)
        {
            mySelectedNode = treeView1.GetNodeAt(e.X, e.Y);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditLabel();
        }




        //Working Methods
        private void OpenXmlNodes()
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
        private void EditLabel()
        {
            if (mySelectedNode != null && mySelectedNode.Parent != null)
            {
                treeView1.SelectedNode = mySelectedNode;
                treeView1.LabelEdit = true;
                if (!mySelectedNode.IsEditing)
                {
                    mySelectedNode.BeginEdit();
                }
            }
            else
            {
                MessageBox.Show("No tree node selected or selected node is a root node.\n" +
                   "Editing of root nodes is not allowed.", "Invalid selection");
            }
        }

        public void exportToXml(TreeView tv, string filename)
        {
            sr = new StreamWriter(filename, false, System.Text.Encoding.UTF8);
            //Write the header
            sr.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            //Write our root node
            sr.WriteLine("<" + treeView1.Nodes[0].Text + ">");
            foreach (TreeNode node in tv.Nodes)
            {
                saveNode(node.Nodes);
            }
            //Close the root node
            sr.WriteLine("</" + treeView1.Nodes[0].Text + ">");
            sr.Close();
        }

        private void saveNode(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    sr.WriteLine("<" + node.Text + ">");
                    saveNode(node.Nodes);
                    sr.WriteLine("</" + node.Text + ">");
                }
                else //No child nodes, so we just write the text
                    sr.WriteLine(node.Text);
            }
        }

        private void tsmSave_Click(object sender, EventArgs e)
        {
            try
            {
                exportToXml(this.treeView1, path);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddNodeMethod();

        }

        private void AddNodeMethod()
        {
            // If neither TreeNodeCollection is read-only, move the 
            // selected node from treeView1 to treeView2.
            if (!treeView1.Nodes.IsReadOnly)
            {
                if (treeView1.SelectedNode != null)
                {
                    TreeNode tn = new TreeNode();
                    //tn.Name = textBox1.Text;
                    tn.Text = textBox1.Text;
                    TreeNode selectedNode = this.treeView1.SelectedNode;
                    selectedNode.Nodes.Add(tn);
                    textBox1.Text = "";
                    
                    
                }
            }
        }

        private void removeNode_Click(object sender, EventArgs e)
        {
            RemoveNodeMetod();
        }

        private void RemoveNodeMetod()
        {
            this.treeView1.SelectedNode.Remove();
        }
        private void textAdd()
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new Form1();
            fm.Show();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                MessageBox.Show(Form1.allLinks[0]);
                foreach (string i in Form1.allLinks)
                {
                    if (!i.Equals(""))
                    {
                        cmb1.Items.Add(i);
                    }
                    
                }
            }
            catch (Exception)
            {

                
            }
            
        }
    }
}
