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
using System.Drawing.Printing;

namespace Label_Viewer
{
    public partial class FileList : Form
    {
        List<string> to_print;
        private string source_path;
        private string archive_path;
        private Timer global_timer;
        List<string> label_extensions;
        public bool move_to_archive = true;
        public int wait_time = 3000;
        private bool polling;
        public string width = "4";
        public string height = "6";
        public string dpmm = "12";
        List<string> FileNames;

        public FileList(string source, string archive, List<string> extensions)
        {

            InitializeComponent();
            source_path = source;
            archive_path = archive;
            label_extensions = extensions;
            polling = false;
            FileNames = new List<string>();
            SearchSourceDirectory();
            global_timer = new Timer();
            global_timer.Interval = wait_time;
            global_timer.Tick += new EventHandler(refresh_file_list);
            global_timer.Start();

            
        }

        private void refresh_file_list(object sender, EventArgs e)
        {
            if (!polling)
            {
                labelList.Visible = true;
                SearchSourceDirectory();
            }
            else
            {
                labelList.Visible = false;
            }
            settingsButton.Enabled = ContainsZPL();
        }

        private void SearchSourceDirectory()
        {
            List<string> remove_nonexistants = new List<string>();
            foreach (string fName in labelList.Items)
            {
                if (!File.Exists(source_path + "\\" + fName))
                {
                    remove_nonexistants.Add(fName);
                }
                
            }
            foreach (string fname in remove_nonexistants)
            {
                labelList.Items.Remove(fname);
            }
            foreach (string FileName in Directory.GetFiles(source_path))
            {

                FileInfo fi = new FileInfo(FileName);
                if (fi.Extension.Equals(".zpl") && label_extensions.Contains(".zpl"))
                {
                    if (!labelList.Items.Contains(fi.Name))
                    {
                        labelList.Items.Add(fi.Name);
                        FileNames.Add(FileName);
                    }
                }
                else if (fi.Extension.Equals(".ipl") && label_extensions.Contains(".ipl"))
                {
                    if (!labelList.Items.Contains(fi.Name))
                    {
                        labelList.Items.Add(fi.Name);
                        FileNames.Add(FileName);
                    }
                }
                else if (fi.Extension.Equals(".lbl") && label_extensions.Contains(".lbl"))
                {
                    if (!labelList.Items.Contains(fi.Name))
                    {
                        labelList.Items.Add(fi.Name);
                        FileNames.Add(FileName);
                    }
                }
                else if (fi.Extension.Equals(".dpl") && label_extensions.Contains(".dpl"))
                {
                    if (!labelList.Items.Contains(fi.Name))
                    {
                        labelList.Items.Add(fi.Name);
                        FileNames.Add(FileName);
                    }
                }

            }

        }

        private bool ContainsZPL()
        {
            foreach (string FileName in Directory.GetFiles(source_path))
            {

                FileInfo fi = new FileInfo(FileName);
                if (fi.Extension.Equals(".zpl") && label_extensions.Contains(".zpl"))
                {
                    return true;
                }
            }

            return false;
        }

        
        private void settingsButton_Click(object sender, EventArgs e)
        {
            var settings_tab = new LabelSettings(this);
            settings_tab.ShowDialog();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectAllButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < labelList.Items.Count; i++)
            {
                labelList.SelectedIndex = i;
            }
        }

        private void startPollingButton_Click(object sender, EventArgs e)
        {
            if (labelList.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Please select one or more label files to preview.", "No labels selected");
            }
            else
            {
                bool prompted = false;
                List<string> temp = new List<string>();
                temp = labelList.SelectedItems.Cast<string>().ToList();
                foreach (string fName in temp)
                {
                    Console.WriteLine("FILE:"+ fName);
                    string FileName = source_path + "\\" + fName;
                    FileInfo fi = new FileInfo(FileName);
                    if (!fi.Extension.Equals(".zpl") && !prompted)
                    {
                        prompted = true;
                        MessageBox.Show("Currently we do not have resources to preview labels of formats other than Zebra.");

                    }
                    else if (fi.Extension.Equals(".zpl"))
                    {
                        try
                        {
                            StreamReader sr = new StreamReader(FileName);
                            string zplstr = sr.ReadToEnd();
                            sr.Close();

                            var preview_window = new PreviewWindow(zplstr, width, height, dpmm, FileName);
                            preview_window.Show();

                            if (move_to_archive)
                            {
                                if (!File.Exists(archive_path + "\\" + fi.Name))
                                {
                                    File.Move(FileName, archive_path + "\\" + fi.Name);
                                }
                                else
                                {
                                    File.Delete(archive_path + "\\" + fi.Name);
                                    File.Move(FileName, archive_path + "\\" + fi.Name);
                                }
                            }

                        }
                        catch
                        {
                            Console.WriteLine("File Not Found");
                        }
                    }
                }
                List<string> to_remove_two = new List<string>();
                foreach (string itm in labelList.SelectedItems)
                {
                    to_remove_two.Add(itm);
                }
                labelList.SelectedItems.Clear();
                if (move_to_archive)
                {
                    foreach (string i in to_remove_two)
                    {
                        if (labelList.Items.Contains(i))
                        {
                            labelList.Items.Remove(i);
                        }
                    }
                }
                SearchSourceDirectory();
            }
        }

        private void printAllButton_Click(object sender, EventArgs e)
        {
            if (labelList.SelectedItems.Count <= 0)
            {
                MessageBox.Show("Please select one or more label files to print.", "No labels selected");
            }
            else
            {
                to_print = new List<string>();
                foreach (string filename in labelList.SelectedItems)
                {
                    to_print.Add(filename);
                }
                PrintDocument pd = new PrintDocument();
                pd.PrintPage += new PrintPageEventHandler(this.pd_PrintPage);
                printDialog1 = new PrintDialog();
                if (printDialog1.ShowDialog() == DialogResult.OK)
                {
                    printDialog1.ShowHelp = true;
                    pd.Print();
                }
            }
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs printE)
        {
            int rows = 0;
            int cols = 0;
            List<string> to_remove = new List<string>();
            foreach (string fName in to_print)
            {
                
                string FileName = source_path + "\\" + fName;
                FileInfo fi = new FileInfo(FileName);
                if (fi.Extension.Equals(".zpl"))
                {
                    StreamReader sr = new StreamReader(FileName);
                    string zplstr = sr.ReadToEnd();
                    sr.Close();
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
                        Image image;
                        image = Image.FromStream(responseStream);

                        int int_width = Int32.Parse(width);
                        int int_height = Int32.Parse(height);
                        //Console.WriteLine("------IMAGE WIDTH = " + image.Width + "\n--------IMAGE HEIGHT = " + image.Height
                        //    +"------THIS WIDTH = " + this.Width);
                        int iWidth = 405;
                        int iHeight = 470;
                        Bitmap b = new Bitmap(image);
                        if (image.Width / iWidth > image.Height / iHeight)
                        {
                            b = ResizeImage(image, iWidth, iWidth * image.Height / image.Width);
                        }
                        else
                        {
                            b = ResizeImage(image, iHeight * image.Width / image.Height, iHeight);
                        }
                        var g = Graphics.FromImage(b);
                        //Bitmap to_draw = ResizeImage(image, (previewBox.Height) * int_width / int_height, previewBox.Height); 
                        if (rows > 1)
                        {
                            foreach (string rem in to_remove)
                            {
                                if (to_print.Contains(rem))
                                {
                                    to_print.Remove(rem);
                                }
                            }
                            printE.HasMorePages = true;
                            return;
                        }
                        if (cols == 0)
                        {
                            printE.Graphics.DrawImageUnscaled(b, new Point(cols * (2 * iWidth - b.Width), rows * iHeight));
                            cols++;
                        }
                        else
                        {
                            printE.Graphics.DrawImageUnscaled(b, new Point(cols * (2 * iWidth - b.Width), rows * iHeight));
                            rows++;
                            cols--;
                        }
                        to_remove.Add(fName);
                        if (move_to_archive)
                        {
                            if (!File.Exists(archive_path + "\\" + fi.Name))
                            {
                                File.Move(FileName, archive_path + "\\" + fi.Name);
                            }
                            else
                            {
                                File.Delete(archive_path + "\\" + fi.Name);
                                File.Move(FileName, archive_path + "\\" + fi.Name);
                            }
                        }

                    }
                    catch
                    {
                        MessageBox.Show("Unexpected error occured while printing. Please check your printer settings and the ZPL files.",
                            "Unexpected Error");
                    }

                }

            }


            foreach (string rem in to_remove)
            {
                to_print.Remove(rem);
            }

            printE.HasMorePages = false;
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

        private void RevertButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to move all the files from the archive to the source directory?"
                , "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {

                if (!Directory.Exists(source_path))
                {
                    MessageBox.Show(source_path + " is not a path to a valid directory." +
                    " Please provide a valid path to the source directory", "Invalid Source Path");
                }

                else if (!Directory.Exists(archive_path))
                {
                    MessageBox.Show(archive_path + " is not a path to a valid directory." +
                    " Please provide a valid path to the archive directory", "Invalid Archive Path");
                }

                else
                {

                    bool did_transfer = false;
                    did_transfer = move_all_files(source_path, archive_path);

                    if (did_transfer)
                    {
                        MessageBox.Show("Moved all the files from archive to source directory."
                            , "Transfer Success");
                        SearchSourceDirectory();
                    }

                    else
                    {
                        MessageBox.Show("Unexpected error occured while transfering."
                            , "Transfer Failed");
                    }

                }
            }
        }

        private bool move_all_files(string source, string archive)
        {

            bool success = true;

            foreach (string FileName in Directory.GetFiles(archive))
            {
                FileInfo fi = new FileInfo(FileName);

                if (fi.Extension.Equals(".zpl"))
                {

                    if (label_extensions.Contains(".zpl"))
                    {

                        try
                        {

                            if (!File.Exists(source + "\\" + fi.Name))
                            {
                                File.Move(FileName, source + "\\" + fi.Name);
                            }

                            else
                            {
                                File.Delete(FileName);
                            }

                        }

                        catch
                        {
                            success = false;
                            return success;
                        }
                    }

                }

                else if (fi.Extension.Equals(".ipl"))
                {

                    if (label_extensions.Contains("ipl"))
                    {

                        try
                        {

                            if (!File.Exists(source + "\\" + fi.Name))
                            {
                                File.Move(FileName, source + "\\" + fi.Name);
                            }

                            else
                            {
                                File.Delete(FileName);
                            }

                        }

                        catch
                        {
                            success = false;
                            return success;
                        }
                    }

                }

                else if (fi.Extension.Equals(".lbl"))
                {

                    if (label_extensions.Contains(".lbl"))
                    {

                        try
                        {

                            if (!File.Exists(source + "\\" + fi.Name))
                            {
                                File.Move(FileName, source + "\\" + fi.Name);
                            }

                            else
                            {
                                File.Delete(FileName);
                            }

                        }

                        catch
                        {
                            success = false;
                            return success;
                        }
                    }

                }

                else if (fi.Extension.Equals(".dpl"))
                {

                    if (label_extensions.Contains(".dpl"))
                    {

                        try
                        {

                            if (!File.Exists(source + "\\" + fi.Name))
                            {
                                File.Move(FileName, source + "\\" + fi.Name);
                            }

                            else
                            {
                                File.Delete(FileName);
                            }

                        }

                        catch
                        {
                            success = false;
                            return success;
                        }
                    }

                }

            }

            return success;
        }
    }
}
