using System;
using System.Windows.Forms;
using System.IO.Compression;
//using System.IO.Compression.FileSystem;
using System.Collections.Generic;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            this.Text = Application.ProductName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select Master Zip File";
            openFileDialog1.Filter = "Zip Files (*.zip)|*.zip";
            if (openFileDialog1.ShowDialog()==DialogResult.OK)
            {
                textBoxZipMaster.Text = openFileDialog1.FileName;
            }
            
        }

        private void buttonCompare_Click(object sender, EventArgs e)
        {
            if (textBoxZipMaster.Text.ToLower()==textBoxZipTest.Text.ToLower())
            {
                MessageBox.Show("You must select 2 DIFFERENT files!", "Select different files", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (string.IsNullOrEmpty(textBoxZipMaster.Text))
                return;

            if (string.IsNullOrEmpty(textBoxZipTest.Text))
                return;


            string zipMaster = textBoxZipMaster.Text;
            string zipTest = textBoxZipTest.Text;

            listView1.Items.Clear();

            List<ZipArchiveEntry> masterList = new List<ZipArchiveEntry>();

            
            using (ZipArchive archiveMaster = ZipFile.Open(zipMaster, ZipArchiveMode.Read))
            {
                Console.WriteLine("Master entry count:  {0}", archiveMaster.Entries.Count);
                foreach (ZipArchiveEntry entry in archiveMaster.Entries)
                {
                    masterList.Add(entry);
                }

            }

            using (ZipArchive archiveTest = ZipFile.Open(zipTest, ZipArchiveMode.Read))
            {
                Console.WriteLine("Test entry count:  {0}",archiveTest.Entries.Count);

                foreach(ZipArchiveEntry entry in masterList)
                {
                    var testEntry = archiveTest.GetEntry(entry.FullName);
                    if (testEntry != null)
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = entry.FullName;

                        String status = "";
                        if (testEntry.Length != entry.Length)
                        {
                            status += "Size,";
                            Console.WriteLine("Bad length:  " + testEntry.Length + " != " + entry.Length);
                        }

                        if (testEntry.LastWriteTime != entry.LastWriteTime)
                        {
                            status += "LastWriteTime,";
                            Console.WriteLine("Bad LastWriteTime:  " + testEntry.LastWriteTime + " != " + entry.LastWriteTime);
                        }

                        if (testEntry.CompressedLength != entry.CompressedLength)
                        {
                            status += "Compressed Size";
                            Console.WriteLine("Bad CompressedLength:  " + testEntry.CompressedLength + " != " + entry.CompressedLength);
                        }



                        if (string.IsNullOrEmpty(status))
                        {
                            status = "OK";
                        } else
                        {
                            status = "BAD:  " + status;
                            if (status.EndsWith(","))
                            {
                                status = status.Remove(status.Length-1);
                            }

                        }

                            

                        lvi.SubItems.Add(status);
                        listView1.Items.Add(lvi);
                    }
                    else
                    {
                        ListViewItem lvi = new ListViewItem();
                        lvi.Text = entry.FullName;
                        lvi.SubItems.Add("MISSING");
                        listView1.Items.Add(lvi);
                    }
                    
                }
                

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Select Test Zip File";
            openFileDialog1.Filter = "Zip Files (*.zip)|*.zip";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBoxZipTest.Text = openFileDialog1.FileName;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }
    }

 

    
}

