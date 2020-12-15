using MaterialSkin.Controls;
using System;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Diagnostics;
using NAudio.Wave;

namespace The_Glitchy_Parable
{
    public partial class Form1 : MaterialForm
    {
        Random r = new Random();
        string PathTSP = "";
        string Backup_Materials = "";
        string Backup_Sounds = "";
        string CustomFolder = "";
        public Form1()
        {
            InitializeComponent();
        }

        private static void ConvertMp3ToWav(string _inPath_, string _outPath_)
        {
            using (Mp3FileReader mp3 = new Mp3FileReader(_inPath_))
            {
                using (WaveStream pcm = WaveFormatConversionStream.CreatePcmStream(mp3))
                {
                    WaveFileWriter.CreateWaveFile(_outPath_, pcm);
                }
            }
        }

        private void DirectoryCopy(
        string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the source directory does not exist, throw an exception.
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            // If the destination directory does not exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }


            // Get the file contents of the directory to copy.
            FileInfo[] files = dir.GetFiles();

            foreach (FileInfo file in files)
            {
                // Create the path to the new copy of the file.
                string temppath = Path.Combine(destDirName, file.Name);

                // Copy the file.
                file.CopyTo(temppath, copySubDirs);
                progressBar1.Increment(1);
                richTextBox1.AppendText("Successfully Copied " + file.Name + "\n");
            }

            // If copySubDirs is true, copy the subdirectories.
            if (copySubDirs)
            {

                foreach (DirectoryInfo subdir in dirs)
                {
                    // Create the subdirectory.
                    string temppath = Path.Combine(destDirName, subdir.Name);

                    // Copy the subdirectories.
                    DirectoryCopy(subdir.FullName, temppath, copySubDirs);

                }
            }
        }
        

        private void materialRaisedButton1_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Stanley Parable\\thestanleyparable"))
            {
                PathTSP = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\The Stanley Parable";
                MessageBox.Show("Directory Found!", "Success!");
            }
            else
            {
                MessageBox.Show("Directory Not Found!", "Error.");
                return;
            }
            if (!System.IO.Directory.Exists(PathTSP + "\\material_backup"))
            {
                MessageBox.Show("Generating material_backup for recovery from corruption...", "Loading...");
                if (System.IO.Directory.Exists(PathTSP + "\\thestanleyparable\\materials"))
                {

                    DirectoryCopy(PathTSP + "\\thestanleyparable\\materials", PathTSP + "\\material_backup", true);

                    materialFlatButton1.Enabled = true;
                    materialFlatButton2.Enabled = true;
                    Backup_Materials = PathTSP + "\\material_backup";
                }
            }
            else
            {
                materialFlatButton1.Enabled = true;
                materialFlatButton2.Enabled = true;
                MessageBox.Show("Found Backup Materials!");
                Backup_Materials = PathTSP + "\\material_backup";
            }
            // SOUNDS!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            if (!System.IO.Directory.Exists(PathTSP + "\\sounds_backup"))
            {
                MessageBox.Show("Generating sounds_backup for recovery from corruption...", "Loading...");
                if (System.IO.Directory.Exists(PathTSP + "\\thestanleyparable\\sound"))
                {

                    DirectoryCopy(PathTSP + "\\thestanleyparable\\sound", PathTSP + "\\sounds_backup", true);
                    var models = Directory.GetFiles(PathTSP + "\\thestanleyparable\\materials", "*.vmt", System.IO.SearchOption.AllDirectories);
                    foreach (string s in models)
                    {
                        File.Delete(s);
                    }
                    materialFlatButton1.Enabled = true;
                    materialFlatButton2.Enabled = true;
                    Backup_Sounds = PathTSP + "\\sounds_backup";
                }
            }
            else
            {
                materialFlatButton1.Enabled = true;
                materialFlatButton2.Enabled = true;
                MessageBox.Show("Found Backup Sounds!!");
                Backup_Sounds = PathTSP + "\\sounds_backup";
            }
            // END!
            richTextBox1.AppendText(PathTSP + "\n");
            richTextBox1.AppendText(Backup_Materials + "\n");
            richTextBox1.AppendText(Backup_Sounds + "\n");
        }

        private void materialFlatButton2_Click(object sender, EventArgs e)
        {
            var files = Directory.GetFiles(PathTSP + "\\thestanleyparable\\materials", "*.*", System.IO.SearchOption.AllDirectories);
            var sounds = Directory.GetFiles(PathTSP + "\\thestanleyparable\\sound", "*.*", System.IO.SearchOption.AllDirectories);
            progressBar1.Maximum = files.Length + sounds.Length;
            if (System.IO.Directory.Exists(PathTSP + "\\material_backup"))
            {
                DirectoryCopy(PathTSP + "\\material_backup", PathTSP + "\\thestanleyparable\\materials", true);
            }
            else
            {
                MessageBox.Show("Didn't find backup materials :/");
            }
            if (System.IO.Directory.Exists(PathTSP + "\\sounds_backup"))
            {
                DirectoryCopy(PathTSP + "\\sounds_backup", PathTSP + "\\thestanleyparable\\sound", true);
            }
            else
            {
                MessageBox.Show("Didn't find backup sounds :/");
            }
            MessageBox.Show("Done!");
        }

        private void materialFlatButton1_Click(object sender, EventArgs e)
        {
            if (System.IO.Directory.Exists(PathTSP + "\\thestanleyparable\\materials"))
            {
                string[] targettextures = Directory.GetFiles(PathTSP + "\\thestanleyparable\\materials", "*.vtf", System.IO.SearchOption.AllDirectories);
                string[] targetsounds = Directory.GetFiles(PathTSP + "\\thestanleyparable\\sound", "*.wav", System.IO.SearchOption.AllDirectories);
                if (materialRadioButton5.Checked == true)
                {
                    targetsounds = Directory.GetFiles(PathTSP + "\\thestanleyparable\\sound\\narrator", "*.wav", System.IO.SearchOption.AllDirectories);
                }
                else if (materialRadioButton6.Checked == true)
                {
                    targetsounds = Directory.GetFiles(PathTSP + "\\thestanleyparable\\sound\\doors", "*.wav", System.IO.SearchOption.AllDirectories);
                }
                else if (materialRadioButton7.Checked == true)
                {
                    targetsounds = Directory.GetFiles(PathTSP + "\\thestanleyparable\\sound\\buttons", "*.wav", System.IO.SearchOption.AllDirectories);
                }
                else if (materialRadioButton9.Checked == true)
                {
                    targetsounds = Directory.GetFiles(PathTSP + "\\thestanleyparable\\sound\\player\\footsteps", "*.wav", System.IO.SearchOption.AllDirectories);
                }
                string[] textures = Directory.GetFiles(PathTSP + "\\material_backup", "*.vtf", System.IO.SearchOption.AllDirectories);
                string[] sounds = { PathTSP + "\\sounds_backup\\raphael1.wav", PathTSP + "\\sounds_backup\\raphael2.wav", PathTSP + "\\sounds_backup\\raphael3.wav", PathTSP + "\\sounds_backup\\raphael4.wav", PathTSP + "\\sounds_backup\\raphael5.wav", PathTSP + "\\sounds_backup\\raphael6.wav", PathTSP + "\\sounds_backup\\raphael7.wav", PathTSP + "\\sounds_backup\\raphael8.wav", PathTSP + "\\sounds_backup\\raphael9.wav", PathTSP + "\\sounds_backup\\raphael10.wav" };
                if (materialRadioButton2.Checked == true) //redundant code!
                {
                    sounds = Directory.GetFiles(PathTSP + "\\sounds_backup", "*.wav", System.IO.SearchOption.AllDirectories);
                }
                else
                {
                    if (materialRadioButton1.Checked == true)
                    {
                        sounds = Directory.GetFiles(PathTSP + "\\sounds_backup\\narrator", "*.wav", System.IO.SearchOption.AllDirectories);
                    }
                    else
                    {
                        if (materialRadioButton3.Checked == true)
                        {


                        }
                        else
                        {
                            if (materialRadioButton4.Checked == true)
                            {
                                if (CustomFolder != "")
                                {
                                    sounds = Directory.GetFiles(CustomFolder, "*.wav", System.IO.SearchOption.AllDirectories);
                                }
                                else
                                {
                                    MessageBox.Show("No Path Selected for custom sounds!");
                                    return;
                                }
                            }
                        }
                    }
                }
                Console.WriteLine(sounds.Length);
                progressBar1.Value = 0;
                // Copy the files and overwrite destination files if they already exist.




                if (materialCheckBox1.Checked == true)
                {
                    progressBar1.Maximum = textures.Length;
                    foreach (string s in targettextures)
                    {
                        progressBar1.Increment(1);
                        richTextBox1.AppendText("Successfully Copied " + Path.GetFileName(s) + "\n");
                        // Use static Path methods to extract only the file name from the path.

                        System.IO.File.Copy(Path.GetFullPath(textures[r.Next(textures.Length)]), s, true);


                    }
                }
                progressBar1.Value = 0;
                if (materialCheckBox2.Checked == true)
                {
                    progressBar1.Maximum = targetsounds.Length;
                    foreach (string s in targetsounds)
                    {

                        progressBar1.Increment(1);

                        richTextBox1.AppendText("Successfully Copied " + Path.GetFileName(s) + "\n");
                        // Use static Path methods to extract only the file name from the path.

                        System.IO.File.Copy(Path.GetFullPath(sounds[r.Next(sounds.Length)]), s, true);

                    }
                }






            }
            else
            {
                MessageBox.Show("not found materials");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        
        }

        private void materialRadioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (materialRadioButton4.Checked == true)
            {
                if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    CustomFolder = folderBrowserDialog1.SelectedPath;
                    string[] mp = Directory.GetFiles(CustomFolder, "*.mp3", System.IO.SearchOption.AllDirectories);
                    foreach (string s in mp)
                    {
                        ConvertMp3ToWav(s, s.Replace(".mp3", ".wav"));
                    }
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
        }

        private void materialSingleLineTextField1_TextChanged(object sender, EventArgs e)
        {
            r = new Random(materialSingleLineTextField1.Text.GetHashCode());
        }
    }
}
