using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Label_Viewer
{
    public partial class PrintLabel : Form
    {
        string zplCode;
        public PrintLabel(string pZplCode)
        {
            InitializeComponent();
            zplCode = pZplCode;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            // Printer IP Address and communication port
            string ipAddress = printerIpText.Text;
            int port = 9100;

            try
            {
                // Open connection
                System.Net.Sockets.TcpClient client = new System.Net.Sockets.TcpClient();
                client.Connect(ipAddress, port);

                // Write ZPL String to connection
                System.IO.StreamWriter writer = new System.IO.StreamWriter(client.GetStream());
                writer.Write(zplCode);
                writer.Flush();

                // Close Connection
                writer.Close();
                client.Close();
                MessageBox.Show("Print Successful!", "Success");
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: {0}", ex);
                MessageBox.Show("No printer installed corresponds to the IP address given", "No response");
            }
        }

        private void printerIpText_TextChanged(object sender, EventArgs e)
        {
            printButton.Enabled = printerIpText.Text != "";
        }
    }
}
