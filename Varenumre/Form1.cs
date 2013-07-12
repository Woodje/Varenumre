using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Varenumre
{
    public partial class Form1 : Form
    {
        int Page = 1;
        string TextStringFile = "varenumre.txt", CSVstringFile = "test.csv", SaveTextStringToHere = "gemtfil.txt", TEXT;
        string[] Contentlist, Comparelist;
        string[][] array;
        List<string> FailedList = new List<string>();
        
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            label3.Text = "Før du fortsætter skal du have dine varenumre\ngemt i en komma-opdelt tekstfil.\n";
            label3.Text += "\nListen som indeholder de nye tal skal\nvære gemt i et CSV-format.\n";
            label3.Text += "\nHvis ikke det ovenstående kan opfyldes,\nkontakt da skaberen af dette program.";
            button2.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Page++;
            if (Page == 2)
            {
                label3.Text = "";
                label3.Visible = false;
                label1.Text = "Vælg den komma-opdelte tekstfil \nmed varenumre";
                button1.Enabled = false;
                button2.Visible = true;
                button2.Enabled = true;
            }
            if (Page == 3)
            {
                label1.Text = "Vælg csv-filen til sammenligning";
                button1.Enabled = false;
                button2.Enabled = true;
            }
            if (Page == 4)
            {
                label1.Text = "Vælg navn og placering på den nye fil";
                button1.Enabled = false;
                button2.Enabled = true;
                button2.Text = "Opret fil";
            }
            if (Page == 5)
            {
                label1.TextAlign = ContentAlignment.TopLeft;
                label1.Text = "Den nye fil vil blive gemt på din valgte placering med\nalle dine varenumre.\n";
                label1.Text += "\nVarenumre som ikke bliver fundet og opdateret\naf den ene eller anden grund, bliver gemt\n";
                label1.Text += "under dit valgte filnavn, men ender på \"-IKKE FUNDET\".";
                button1.Enabled = false;
                button2.Enabled = true;
                button2.Text = "Udfør";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            label1.Text = "Arbejder... Vent venligst!";
            if (Page == 2)
            {
                TextStringFile = Tools.findtextpath();
                if (TextStringFile != "error")
                {
                    label1.Text = "Tekstfil indlæst";
                    button2.Enabled = false;
                    button1.Enabled = true;
                }
                else
                {
                    button2.Enabled = true;
                    label1.Text = "Fejl, prøv igen!";
                }
            }
            if (Page == 3)
            {
                CSVstringFile = Tools.findcsvpath();
                if (CSVstringFile != "error")
                {
                    label1.Text = "Teksfil indlæst";
                    button2.Enabled = false;
                    button1.Enabled = true;
                }
                else
                {
                    button2.Enabled = true;
                    label1.Text = "Fejl, prøv igen!";
                }
            }
            if (Page == 4)
            {
                SaveTextStringToHere = Tools.savetextfilepath();
                if (SaveTextStringToHere != "error")
                {
                    label1.Text = "Stien til filen er indlæst";
                    button2.Enabled = false;
                    button1.Enabled = true;
                }
                else
                {
                    button2.Enabled = true;
                    label1.Text = "Fejl, prøv igen!";
                }
            }
            if (Page == 5)
            {
                label1.TextAlign = ContentAlignment.BottomCenter;
                Refresh();
                Invoke((MethodInvoker)delegate()
                {
                    Contentlist = Tools.Readtextfiletostring(TextStringFile);
                    progressBar1.Visible = true;
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = Contentlist.Length;
                    int count = 0;
                    foreach (string line in Contentlist)
                    {
                        progressBar1.Value = count++;
                    }
                    label2.Text = "Antal varenumre i tekstfilen : " + Contentlist.Length + "\n";
                });
                Refresh();
                Invoke((MethodInvoker)delegate()
                {
                    Comparelist = Tools.Readtextfiletostring(CSVstringFile);
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = Comparelist.Length;
                    int count = 0;
                    foreach (string line in Comparelist)
                    {
                        progressBar1.Value = count++;
                    }
                    label2.Text += "Antal varenumre i csv-filen  : " + Comparelist.Length + "\n";
                });
                Refresh();
                array = new string[Contentlist.Count()][];
                Invoke((MethodInvoker)delegate()
                {
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Maximum = Contentlist.Length;
                        // Declare the array of two elements: 

                    // Initialize the elements:
                    int i = 0;
                    foreach (string line in Contentlist)
                    {
                        array[i] = new string[35];
                        array[i] = Tools.Readcommaseparetedstring(Contentlist[i]);
                        progressBar1.Value = i++;
                    }
                    /*label2.Text = "";
                    for (int t = 0; t < 32; t++)
                        label2.Text += array[2590][t];
                    label2.Text += "\n";
                    label2.Text += Contentlist[2590];
                    label3.Visible = false;
                    progressBar1.Visible = false;*/
                });
                Refresh();
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            string[] FindPrices;
            string writerstring = "";
            int count = 0, countsuccess = 0, succesornot, o = 0;
            for (int j = 0; j < array.Count(); j++)
            {
                succesornot = 0;
                foreach (string search in Comparelist)
                {
                    o = 0;
                    var query = from val in search.Split(';')
                                select string.Copy(val);
                    foreach (string num in query)
                    {
                        if (o == 0)
                        {
                            if (array[j][0].Contains(num))
                            {
                                FindPrices = Tools.SearchForPricesInCompareString(search);
                                array[j][3] = FindPrices[0];
                                array[j][6] = FindPrices[1];
                                countsuccess++;
                                succesornot = 1;
                                break;
                            }
                        }
                        o++;
                    }
                }
                System.Threading.Thread.Sleep(1);
                worker.ReportProgress(count++);
                if (succesornot == 0)
                    FailedList.Add(Contentlist[count - 1]);
            }
            System.IO.StreamWriter addtofilesucces = new System.IO.StreamWriter(SaveTextStringToHere, true);
            for (int ol = 0; ol < array.Count(); ol++)
            {
                for (int k = 0; k < array[ol].Count(); k++)
                    writerstring += array[ol][k];
                addtofilesucces.WriteLine(writerstring);
                writerstring = "";
            }
            addtofilesucces.Close();
            StringBuilder PathOfFailed = new StringBuilder(SaveTextStringToHere);
            PathOfFailed.Replace(".txt", "-IKKE FUNDET.txt");
            System.IO.StreamWriter addtofilefailed = new System.IO.StreamWriter(PathOfFailed.ToString(), true);
            foreach (string line in FailedList)
            {
                addtofilefailed.WriteLine(line);
            }
            addtofilefailed.Close();
            TEXT += "Antal varenumre fundet og rettet     : " + countsuccess.ToString() + "\n";
            TEXT += "Antal varenumre som ikke blev fundet : " + FailedList.Count;
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                label1.Text = "Afbrudt!";
            }
            else if (e.Error != null)
            {
                label1.Text = "Fejl";
            }
            else
            {
                label2.Text += TEXT;
                label1.Text = "Fuldført!";
                button1.Visible = false;
                button2.Visible = false;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
            }
        }
    }
}