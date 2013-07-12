using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace Varenumre
{
    class Tools
    {
        public static string[] Readtextfiletostring(string filename)
        {
            List<string> filecontent = new List<string>();
            using (StreamReader file = File.OpenText(filename))
            {
                while (!file.EndOfStream)
                {
                    string line = file.ReadLine();
                    filecontent.Add(line);
                }
            }
            return filecontent.ToArray();
        }

        public static string[] Searchcontentlist(string[] list)
        {
            List<string> Modifiedlist = new List<string>();
            foreach (string line in list)
            {
                StringBuilder linemodifier = new StringBuilder(line);
                linemodifier.Remove((Regex.Match(line, ",").Index), (line.Length - Regex.Match(line, ",").Index));
                linemodifier.Remove(0, 1);
                linemodifier.Remove(linemodifier.Length - 1, 1);
                Modifiedlist.Add(linemodifier.ToString());
            }
            return Modifiedlist.ToArray();
        }

        public static string[] CompareToList(string[] list, string[] list2)
        {
            List<string> Modifiedlist = new List<string>();
            foreach (string line in list)
            {
                foreach (string search in list2)
                {
                    if (search.Contains(line))
                    {
                        Modifiedlist.Add(line);
                        break;
                    }
                }
            }
            return Modifiedlist.ToArray();
        }

        public static string[] SearchCompareString(string searchcomparestring)
        {
            List<string> PricesFound = new List<string>();

            string CompareText;

            StringBuilder linemodifier = new StringBuilder(searchcomparestring);

            CompareText = linemodifier.Remove(0, (Regex.Match(searchcomparestring, ";").Index) + 1).ToString();
            
            StringBuilder linemodifier2 = new StringBuilder(CompareText);
            PricesFound.Add(linemodifier2.Remove((Regex.Match(CompareText, ";").Index), (CompareText.Length - Regex.Match(CompareText, ";").Index)).ToString());

            CompareText = linemodifier.Remove(0, (Regex.Match(CompareText, ";").Index) + 1).ToString();

            CompareText = linemodifier.Remove(0, (Regex.Match(CompareText, ";").Index) + 1).ToString();

            PricesFound.Add(linemodifier.Remove((Regex.Match(CompareText, ";").Index), (CompareText.Length - Regex.Match(CompareText, ";").Index)).ToString());

            CompareText = PricesFound[0] + "  --  " + PricesFound[1];

            return PricesFound.ToArray();
        }

        public static string SearchString(string searchstring, string replacefirstprice, string replacesecondprice)
        {
            string Text, ThirdText, NewText;

            StringBuilder linemodifier = new StringBuilder(searchstring);
            StringBuilder linemodifier2 = new StringBuilder(searchstring);

            linemodifier2.Remove((Regex.Match(searchstring, ",").Index),(searchstring.Length - Regex.Match(searchstring, ",").Index));
            
            NewText = linemodifier2.ToString();

            linemodifier.Remove(0, (Regex.Match(searchstring, ",").Index) + 1).ToString();
            
            ThirdText = linemodifier.ToString();
            Text = linemodifier.ToString();

            StringBuilder linemodifier3 = new StringBuilder(Text);
            linemodifier3.Remove((Regex.Match(ThirdText, ",").Index), (ThirdText.Length - Regex.Match(ThirdText, ",").Index));
            NewText += "," + linemodifier3.ToString();

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();
            ThirdText = linemodifier.ToString();

            StringBuilder linemodifier4 = new StringBuilder(Text);
            linemodifier4.Remove((Regex.Match(ThirdText, ",").Index), (ThirdText.Length - Regex.Match(ThirdText, ",").Index));
            NewText += "," + linemodifier4.ToString();
            
            StringBuilder linemodifier5 = new StringBuilder(NewText);
            linemodifier5.Remove(1, linemodifier5.Length - 1);
            
            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            //This is where it happens. First price.
            NewText = NewText + "," + linemodifier5.ToString() + replacefirstprice + linemodifier5.ToString();

            StringBuilder linemodifier6 = new StringBuilder(Text);
            linemodifier6.Remove((Regex.Match(Text, ",").Index), (Text.Length - Regex.Match(Text, ",").Index));

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            NewText = NewText + "," + linemodifier6.ToString() + ",";

            StringBuilder linemodifier7 = new StringBuilder(Text);
            linemodifier7.Remove((Regex.Match(Text, ",").Index), (Text.Length - Regex.Match(Text, ",").Index));

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            Text = linemodifier.Remove(0, (Regex.Match(Text, ",").Index) + 1).ToString();

            NewText = NewText + linemodifier7.ToString() + "," + linemodifier5.ToString() + replacesecondprice + linemodifier5.ToString() + "," + Text;

            return NewText;
        }

        public static string findtextpath()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "Tekst Fil | *.txt";
            DialogResult UserClicksOK = openFile.ShowDialog();
            string filepath = openFile.FileName.ToString();
            if (UserClicksOK != DialogResult.OK)
            {
                filepath = "error";
            }
            return filepath.ToString();
        }

        public static string[] Readcommaseparetedstring(string commaseparetedstring)
        {
            string[][] NewArray = new string[1][];
            NewArray[0] = new string[35];

            int i = 0, k = 0, l = 0;
            string test = "";
            var query = from val in commaseparetedstring.Split(',')
                        select string.Copy(val);
            foreach (string num in query)
            {
                if (!num.StartsWith("\"") && num.EndsWith("\""))
                    l = 0;
                if (l == 1)
                {
                    test += "," + num;
                }
                if (num.StartsWith("\"") && !num.EndsWith("\""))
                {
                    test = num;
                    l = 1;
                }
                else if (!num.StartsWith("\"") && num.EndsWith("\""))
                {
                    i--;
                    if (k != 0)
                    {
                        NewArray[0][i] = "," + test + "," + num;
                    }
                    else
                        NewArray[0][i] = test + "," + num;
                    k++;
                    test = "\"";
                }
                else
                {
                    if (num == "\"")
                    {
                        if (test == "\"")
                        {
                            NewArray[0][i] = "," + num;
                        }
                        else
                        {
                            i--;
                            NewArray[0][i] = "," + test;
                            test = "\"";
                        }
                    }
                    else
                    {
                        if (k != 0)
                        {
                            NewArray[0][i] = "," + num;
                        }
                        else
                            NewArray[0][i] = num;
                        k++;
                    }
                }
                i++;
            }
            return NewArray[0];
        }

        public static string[] SearchForPricesInCompareString(string searchcomparestring)
        {
            List<string> PricesFound = new List<string>();
            int i = 0;
            var query = from val in searchcomparestring.Split(';')
                        select string.Copy(val);
            foreach (string num in query)
            {
                if (i == 1 || i == 3)
                {
                    if (num.Contains("."))
                    {
                        PricesFound.Add(",\"" + num.Replace(".",",") + "\"");
                    }
                    else
                    PricesFound.Add(",\"" + num + "\"");
                }
                i++;
            }

            return PricesFound.ToArray();
        }
        
        public static string findcsvpath()
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Filter = "CSV Fil | *.csv";
            DialogResult UserClicksOK = openFile.ShowDialog();
            string filepath = openFile.FileName.ToString();
            if (UserClicksOK != DialogResult.OK)
            {
                filepath = "error";
            }
            return filepath.ToString();
        }
        
        public static string savetextfilepath()
        {
            SaveFileDialog savefile = new SaveFileDialog();
            savefile.Title = "Gem tekst fil";
            savefile.Filter = "Text files (*.txt)|*.txt";
            savefile.RestoreDirectory = true;
            if (savefile.ShowDialog() != DialogResult.OK)
            {
                return "error";
            }
            return savefile.FileName;
        }
    }
}
