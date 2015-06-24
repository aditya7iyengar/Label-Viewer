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
using System.IO;

namespace Label_Viewer
{
    public partial class LabelSettings : Form
    {
        FileList form1;
        bool move = true;

        public LabelSettings(FileList form_instance)
        {
            InitializeComponent();
            form1 = form_instance;
            dpi.SelectedIndex = 2;
            if (form1.move_to_archive == true)
            {
                moveOnButton.Enabled = false;
                moveOffButton.Enabled = true;
            }
            else
            {
                moveOnButton.Enabled = true;
                moveOffButton.Enabled = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Closing the settings tab will now will render the label using default settings.\n"
                + "Do you still want to close the tab?", "Are you sure?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                this.Close();
            }
                    
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            if (waitTime.Value == 0)
            {
                MessageBox.Show("Please select a wait time greater than 0.", "Error in wait time");
            }
            else
            {
                form1.width = width.Value.ToString();
                form1.height = height.Value.ToString();
                if (dpi.SelectedIndex == 0){
                    form1.dpmm = "6";
                }
                else if (dpi.SelectedIndex == 1){
                    form1.dpmm = "8";
                }
                else if (dpi.SelectedIndex == 2){
                    form1.dpmm = "12";
                }
                else if (dpi.SelectedIndex == 3){
                    form1.dpmm = "24";
                }
                form1.wait_time = Decimal.ToInt32(waitTime.Value) * 1000;
                form1.move_to_archive = move;
                this.Close();
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "xml files (*.xml)|*.xml| All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openFileDialog1.OpenFile()) != null)
                    {
                        settingsText.Text = openFileDialog1.FileName;
                        using (myStream)
                        {
                            using (StreamReader str = new StreamReader(myStream, Encoding.UTF8))
                            {
                                string xml_contents = str.ReadToEnd();
                                using (XmlReader reader = XmlReader.Create(new StringReader(xml_contents)))
                                {
                                    Console.WriteLine("width");
                                    reader.ReadToFollowing("width");
                                    width.Value = System.Convert.ToDecimal(reader.ReadElementContentAsString());

                                    Console.WriteLine("height");
                                    reader.ReadToFollowing("height");
                                    height.Value = System.Convert.ToDecimal(reader.ReadElementContentAsString());

                                    
                                    reader.ReadToFollowing("dpi");
                                    string dp = reader.ReadElementContentAsString();
                                    if (dp == "153" ||dp == "150")
                                    {
                                        dpi.SelectedIndex = 0;
                                    }
                                    else if (dp == "203" || dp == "200")
                                    {
                                        dpi.SelectedIndex = 1;
                                    }
                                    else if (dp == "300" || dp == "302")
                                    {
                                        Console.WriteLine("dpi");
                                        dpi.SelectedIndex = 2;
                                    }
                                    else if (dp == "600" || dp == "604")
                                    {
                                        dpi.SelectedIndex = 3;
                                    }
                                    else
                                    {
                                        dpi.SelectedIndex = 2;
                                    }

                                    Console.WriteLine("wait_time");
                                    reader.ReadToFollowing("wait_time");
                                    waitTime.Value = System.Convert.ToDecimal(reader.ReadElementContentAsString());
                                }
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Error in reading the XML file. Please make sure that the xml is in proper format."
                        + "\nDefaulting to the following values: \nWidth: 4\nHeight: 6\nDpi: 300\nWait Time: 3", "ERROR!");
                    width.Text = "4";
                    height.Text = "6";
                    waitTime.Value = 3;
                    dpi.SelectedIndex = 2;
                }
            }
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Accepted XML Format:  \n<doc>"
                                                + "\n  <attributes>"
                                                + "\n     <width>4</width>"
                                                + "\n     <height>6</height>"
                                                + "\n     <dpi>300</dpi>"
                                                + "\n     <wait_time>1</wait_time>"
                                                + "\n  </attributes>"
                                                + "\n</doc>");
        }

        private void moveOffButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to turn off move to archive feature?"
                + "\n This will not move the previewed/printed files to the archive folder and they will stay in the source folder.", "Turn OFF Move?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                moveOnButton.Enabled = true;
                moveOffButton.Enabled = false;
                move = false;
            }
        }

        private void moveOnButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to turn on move to archive feature?"
                +"\n This will move all the previewed/printed files to the archive folder", "Turn ON Move?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                moveOffButton.Enabled = true;
                moveOnButton.Enabled = false;
                move = true;
            }
        }
    }
}
