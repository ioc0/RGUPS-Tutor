using System;
using System.IO;
using System.Text;
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
        private readonly string[] comboBoxArray = new string[6];
        private TreeNode mySelectedNode;
        private string path;
        private StreamWriter sr;
        private readonly XmlDocument xDoc = new XmlDocument();
        private XmlTextWriter xw;


        public Main()
        {
            comboBoxArray = File.ReadAllLines(Form1.path, Encoding.UTF8);


            InitializeComponent();
            foreach (var something in comboBoxArray)
                if (!something.Equals(""))
                    cmb1.Items.Add(something);
        }


        private void tsmExit_Click(object sender, EventArgs e)
        {
            Close();
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
                    mySelectedNode.BeginEdit();
            }
            else
            {
                MessageBox.Show("No tree node selected or selected node is a root node.\n" +
                                "Editing of root nodes is not allowed.", "Invalid selection");
            }
        }

        public void exportToXml3(TreeView tv, string filename)
        {
            sr = new StreamWriter(filename, false, Encoding.UTF8);
            //Write the header
            sr.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            //Write our root node
            sr.WriteLine("<" + treeView1.Nodes[0].Text + ">");
            foreach (TreeNode node in tv.Nodes)
                saveNode3(node.Nodes);
            //Close the root node
            sr.WriteLine("</" + treeView1.Nodes[0].Text + ">");
            //sr.WriteLine("</>");
            sr.Close();
        }

        private void saveNode3(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    sr.WriteLine("<" + node.Text + ">");
                    saveNode3(node.Nodes);
                    sr.WriteLine("</" + node.Text + ">");
                }
                else //No child nodes, so we just write the text
                {
                    sr.WriteLine(node.Text);
                }
        }

        private XmlTextWriter xr;

        public void exportToXml(TreeView tv, string filename)
        {
            xr = new XmlTextWriter(filename, System.Text.Encoding.UTF8);
            xr.WriteStartDocument();
            //Write our root node
            xr.WriteStartElement(treeView1.Nodes[0].Text);
            foreach (TreeNode node in tv.Nodes)
            {
                saveNode2(node.Nodes);
            }
            //Close the root node
            xr.WriteEndElement();
            xr.Close();
        }

        private void saveNode2(TreeNodeCollection tnc)
        {
            foreach (TreeNode node in tnc)
            {
                //If we have child nodes, we'll write 
                //a parent node, then iterrate through
                //the children
                if (node.Nodes.Count > 0)
                {
                    xr.WriteStartElement(node.Text);
                    saveNode2(node.Nodes);
                    xr.WriteEndElement();
                }
                else //No child nodes, so we just write the text
                {
                    xr.WriteString(node.Text);
                }
            }
        }

        private void tsmSave_Click(object sender, EventArgs e)
        {
           
           exportToXml(treeView1, path);
            
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
                if (treeView1.SelectedNode != null)
                {   
                    var tn = new TreeNode();
                    if (textBox1.Text.Length != 0) 
                    {
                        tn.Text = textBox1.Text;
                    }
                    else if (!cmbAdvanceChose.SelectedItem.Equals(""))
                    {
                        tn.Text = cmbAdvanceChose.SelectedItem.ToString();
                    }

                    //tn.Name = textBox1.Text;
                    
                    var selectedNode = treeView1.SelectedNode;
                    selectedNode.Nodes.Add(tn);
                    textBox1.Text = "";
                }
            treeView1.ExpandAll();
        }

        private void removeNode_Click(object sender, EventArgs e)
        {
            RemoveNodeMetod();
        }

        private void RemoveNodeMetod()
        {
            treeView1.SelectedNode.Remove();
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                var tn = new TreeNode();
                
                //tn.Name = textBox1.Text;
                tn.Text = cmb1.SelectedItem + "\t" + txtBoxPage.Text;
                var selectedNode = treeView1.SelectedNode;
                selectedNode.Nodes.Add(tn);
                textBox1.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());

            }
                    
                
        }

        private void настройкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var fm = new Form1();
            fm.Show();
        }

        private void tsmNew_Click(object sender, EventArgs e)
        {
            createNewExample();
        }

        private void createNewExample()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "New XML Document";
            sfd.Filter = "XML Files (*.xml)|*.xml";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = sfd.FileName;
                TreeNode tn1 = new TreeNode();
                tn1.Name = "main";
                tn1.Text = "РГУПС";
               
                treeView1.Nodes.Add(tn1);
                //var tNode = new TreeNode();
                tn1 = treeView1.Nodes[0];
                //addTreeNode(xDoc.DocumentElement, tNode);
                //exportToXml(treeView1,path);
                //OpenNewXml();
                treeView1.ExpandAll();
                
            }
            

             
        }

        private void OpenNewXml()
        {
            try
            {
                Cursor = Cursors.WaitCursor;


                xDoc.Load(path);

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
    }
}