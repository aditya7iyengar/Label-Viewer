using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace Label_Viewer
{
    public partial class LabelViewer : Form
    {
        private string source_path;
        private string archive_path;
        private Timer global_timer;
        List<string> file_extensions = new List<string>();


        public LabelViewer()
        {

            InitializeComponent();

            source_path = "";
            archive_path = "";

            /* Setting up refresh timer to enable and disable the buttons. */
            global_timer = new Timer();
            global_timer.Interval = 500;
            global_timer.Tick += new EventHandler(refresh_label_viewer);
            global_timer.Start();

            /* Populating the file_extensions and adding it to the checklist */
            file_extensions.Add("Zebra Labels: .zpl");
            file_extensions.Add("Intermec Labels: .ipl");
            file_extensions.Add("Sato Labels: .lbl");
            file_extensions.Add("Datamax Labels: .dpl");

            int counter = 0;

            while (counter < file_extensions.Count)
            {
                extensionsList.Items.Add(file_extensions.ElementAt(counter));
                counter++;
            }

            extensionsList.CheckOnClick = false;

            helpProvider1 = new HelpProvider();

            helpProvider1.SetShowHelp(directoryText, true);
            helpProvider1.SetHelpString(directoryText, "Enter the path to the source directory.");

            helpProvider1.SetShowHelp(archiveText, true);
            helpProvider1.SetHelpString(archiveText, "Enter the path to the archive directory.");

            helpProvider1.SetShowHelp(revertButton, true);
            helpProvider1.SetHelpString(revertButton, "Moves ALL the files from the archive to source directory.");

            helpProvider1.SetShowHelp(searchButton, true);
            helpProvider1.SetHelpString(searchButton, "Starts the polling process on the source directory.");

            helpProvider1.SetShowHelp(InstructionsButton, true);
            helpProvider1.SetHelpString(InstructionsButton, "Follow these set of instructions.");

            helpProvider1.SetShowHelp(ExitButton, true);
            helpProvider1.SetHelpString(ExitButton, "Exit (Esc)");

            helpProvider1.HelpNamespace = "help.chm";

            helpProvider1.SetHelpNavigator(directoryText, HelpNavigator.TableOfContents);
        }


        private void refresh_label_viewer(object sender, EventArgs e)
        {

            source_path = directoryText.Text;
            archive_path = archiveText.Text;

            searchButton.Enabled = source_path != "" && archive_path != "" 
                && extensionsList.CheckedItems.Count > 0;

            revertButton.Enabled = source_path != "" && archive_path != "";

            extensionsList.Visible = source_path != "" && archive_path != "";

        }


        private void ExitButton_Click(object sender, EventArgs e)
        {

            file_extensions.Clear();

            extensionsList.Items.Clear();

            global_timer.Stop();

            this.Close();

        }


        private void InstructionsButton_Click(object sender, EventArgs e)
        {

            MessageBox.Show("Please follow the instructions below:\n\n"
                        + "- Provide the path to the source directory.\n"
                        + "- Provide the path to the archive.\n"
                        + "- Select the extensions of the labels you want to preview or print.\n"
                        + "- Click on revert to move all the labels from the archive to source directory.\n"
                        + "- Click on start polling to poll the source directory and get list of files with specified label extensions.\n"
                        + "- In the next form, Single or Multiple labels can be selected from the list of labels to be previewed or printed.\n"
                        + "- Settings can be changed accordingly using label settings button. \n"
                        + "- Press Escape to EXIT."
                        , "Instructions");

        }


        private void revertButton_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Do you want to move all the files from the archive to the source directory?"
                , "Are you sure?", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.Yes)
            {
                source_path = directoryText.Text;
                archive_path = archiveText.Text;

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
                else if (Directory.GetFiles(archive_path).Length == 0)
                {
                    MessageBox.Show("The specified archive folder is empty.", "No files to transfer.");
                }
                else
                {

                    bool did_transfer = false;
                    did_transfer = move_all_files(source_path, archive_path);

                    if (did_transfer)
                    {
                        MessageBox.Show("Moved all the files from archive to source directory."
                            , "Transfer Success");
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

                    if (extensionsList.CheckedItems.Contains("Zebra Labels: .zpl"))
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

                    if (extensionsList.CheckedItems.Contains("Intermec Labels: .ipl"))
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

                    if (extensionsList.CheckedItems.Contains("Sato Labels: .lbl"))
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

                    if (extensionsList.CheckedItems.Contains("Datamax Labels: .dpl"))
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

        private void browseButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                directoryText.Text = fbd.SelectedPath;
            }
        }

        private void browseButton2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                archiveText.Text = fbd.SelectedPath;
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {

            source_path = directoryText.Text;
            archive_path = archiveText.Text;

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

                List<string> file_ext = new List<string>();

                if (extensionsList.CheckedItems.Contains("Zebra Labels: .zpl"))
                {
                    file_ext.Add(".zpl");
                }

                if (extensionsList.CheckedItems.Contains("Intermec Labels: .ipl"))
                {
                    file_ext.Add(".ipl");
                }

                if (extensionsList.CheckedItems.Contains("Sato Labels: .lbl"))
                {
                    file_ext.Add(".lbl");
                }

                if (extensionsList.CheckedItems.Contains("Datamax Labels: .dpl"))
                {
                    file_ext.Add(".dpl");
                }

                FileList next_form = new FileList(source_path, archive_path, file_ext);
                
                next_form.ShowDialog();
                
            }

        }
    }
}
