using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Drawing.Printing;

namespace Label_Viewer
{
    public partial class PreviewWindow : Form
    {
        string zplstr = "";
        string width, height, dpmm;
        string filePath;
        Image image;
        string nameOfFile;
        public PreviewWindow(string zpl_str, string pWidth, 
            string pHeight, string pDpmm, string pFilePath)
        {
            InitializeComponent();
            zplstr = zpl_str;
            filePath = pFilePath;
            char[] delimiterChar = { '\\', '.' };
            string[] words = filePath.Split(delimiterChar);
            nameOfFile = words[words.Length - 2];
            this.Text = nameOfFile + "." + words[words.Length - 1];
            if (zplstr != "")
            {
                width = pWidth;
                height = pHeight;
                dpmm = pDpmm;

                byte[] zpl = Encoding.UTF8.GetBytes(zplstr);

                var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/" + dpmm + "dpmm/labels/" + width + "x" + height + "/0/");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = zpl.Length;
                try
                {
                    var requestStream = request.GetRequestStream();
                    requestStream.Write(zpl, 0, zpl.Length);
                    requestStream.Close();
                    var response = (HttpWebResponse)request.GetResponse();
                    var responseStream = response.GetResponseStream();
                    image = Image.FromStream(responseStream);
                    previewBox.Image = null;
                    previewBox.Height = this.Height - 100;
                    previewBox.Width = previewBox.Height - 100;
                    int int_width = Int32.Parse(width);
                    int int_height = Int32.Parse(height);
                    Bitmap to_draw = ResizeImage(image, (previewBox.Height) * int_width / int_height, previewBox.Height);
                    previewBox.Image = to_draw;
                    trackBar1.Value = 3;
                }
                catch
                {
                    image = previewBox.Image;
                    previewBox.Image = ResizeImage(image, previewBox.Width, previewBox.Height);
                    MessageBox.Show("Cannot generate preview of the label."
                        + " Please check the zpl code for the file at path: \n\n"
                        + filePath, "Error: ZPL format.");
                    printButton.Enabled = false;
                    saveButton.Enabled = false;
                    emailButton.Enabled = false;
                    printPDFButton.Enabled = false;
                    trackBar1.Value = 3;
                    trackBar1.Enabled = false;
                }
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void printButton_Click(object sender, EventArgs e)
        {
            var opened_form = new PrintLabel(zplstr);
            opened_form.Show();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "PNG Image|*.png|JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save the Image";
            saveFileDialog1.ShowDialog();

            // If the file name is not an empty string open it for saving.
            if (saveFileDialog1.FileName != "")
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        image.Save(fs,
                        System.Drawing.Imaging.ImageFormat.Png);
                        MessageBox.Show("Saved image as " + saveFileDialog1.FileName + ".", "Saved Image file");
                        break;
                    case 2:
                        image.Save(fs,
                        System.Drawing.Imaging.ImageFormat.Jpeg);
                        MessageBox.Show("Saved image as " + saveFileDialog1.FileName + ".", "Saved Image file");
                        break;

                    case 3:
                        image.Save(fs,
                        System.Drawing.Imaging.ImageFormat.Bmp);
                        MessageBox.Show("Saved image as " + saveFileDialog1.FileName + ".", "Saved Image file");
                        break;

                    case 4:
                        image.Save(fs,
                        System.Drawing.Imaging.ImageFormat.Gif);
                        MessageBox.Show("Saved image as " + saveFileDialog1.FileName + ".", "Saved Image file");
                        break;
                }
                fs.Close();
            }
        }

        private void emailButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sorry for the inconvenience. The email feature of the application is currently under progress."
            + "\n\nSuggestion: You can save the image and email it as an attatchment.", "Under Progress");
            //var email_form = new SendEmail(zplstr, filePath, width, height, dpmm);
            //email_form.Show();
        }

        private void LabelViewer_ResizeBegin(object sender, EventArgs e)
        {
            int zoom_adder = trackBar1.Value - 3;
            previewBox.Image = null;
            previewBox.Height = (this.Height - 100) + (this.Height - 100) * zoom_adder / 5;
            previewBox.Width = (previewBox.Height - 100);
            int int_width = Int32.Parse(width);
            int int_height = Int32.Parse(height);
            Bitmap to_draw = ResizeImage(image, (previewBox.Height) * int_width / int_height, previewBox.Height);
            previewBox.Image = to_draw;
        }

        private void LabelViewer_Resize(object sender, EventArgs e)
        {
            int zoom_adder = trackBar1.Value - 3;
            previewBox.Image = null;
            previewBox.Height = (this.Height - 100) + (this.Height - 100) * zoom_adder / 5;
            previewBox.Width = (previewBox.Height - 100);
            int int_width = Int32.Parse(width);
            int int_height = Int32.Parse(height);
            Bitmap to_draw = ResizeImage(image, (previewBox.Height) * int_width / int_height, previewBox.Height);
            previewBox.Image = to_draw;
        }

        private void LabelViewer_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            saveButton_Click(sender, e);
        }

        private void LabelViewer_DoubleClick(object sender, EventArgs e)
        {
            saveButton_Click(sender, e);
        }

        private void LabelViewer_DragLeave(object sender, EventArgs e)
        {
            saveButton_Click(sender, e);
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int zoom_adder = trackBar1.Value - 3;
            previewBox.Image = null;
            previewBox.Height = (this.Height - 100) + (this.Height - 100) * zoom_adder / 5;
            previewBox.Width = (previewBox.Height - 100);
            int int_width = Int32.Parse(width);
            int int_height = Int32.Parse(height);
            Bitmap to_draw = ResizeImage(image, (previewBox.Height) * int_width / int_height, previewBox.Height);
            previewBox.Image = to_draw;
        }

        private void printPDFButton_Click(object sender, EventArgs e)
        {
            Bitmap b = ResizeImage(image, this.Height * image.Width / image.Height, this.Height);
            var g = Graphics.FromImage(b);

            PrintDocument pd = new PrintDocument();

            pd.PrintPage += (object printSender, PrintPageEventArgs printE) =>
            {
                printE.Graphics.DrawImageUnscaled(b, new Point(0, 0));
            };
            printDialog1 = new PrintDialog();
            if (printDialog1.ShowDialog() == DialogResult.OK)
            {
                printDialog1.ShowHelp = true;
                pd.Print();
            }

        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
