using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace LABB3_Bordsbokning_Neliz
{
    public abstract class BokningAbstract : IBokning
    {
        public string Datum { get; set; }
        public string BordsTyp { get; set; }
        public string Tid { get; set; }

    }
    public class Bokning : BokningAbstract
    {
        public string Datum { get; set; }
        public string BordsTyp { get; set; }
        public string Tid { get; set; }
        public Bokning()
        {
            Datum = this.Datum;
            BordsTyp = this.BordsTyp;
            Tid = this.Tid;

        }
    }
    public class Bokare : Bokning
    {
        public async Task BokningAsync(string[] InputArray, string inputString)
        {
            //inparametrar är både inputvärdena separat (i vektor) och färdig 'sammansatt' string
            //tyckte det var mer sleek såhär haha
            Datum = InputArray[0];
            Tid = InputArray[1];
            BordsTyp = InputArray[2];
            Task<bool> jämförBord = Jämför();
            bool ärTillgänglig = await jämförBord;
            switch (ärTillgänglig)
            {
                case true:
                    SkrivTillFil(inputString);
                    MessageBox.Show("Tiden är bokad! För att se den i listan, tryck på knapp 'Visa Bokningar'!");
                    break;
                case false:
                    MessageBox.Show("Tiden är inte bokad. Gör ett nytt försök! \n:) ");
                    return;
            }
        }
        public async Task<bool> Jämför()
        {

            //först anropas metoden för att se om datum och tid är tillgänglig, och om
            //den är tillgänglig så anropas metod för att kolla om bord är tillgänglig. 
            //Om datumet inte är tillgängligt så anropas inte bord metoden.
            MessageBox.Show("Kollar om datum och tid är tillgänglig.\n(1/2)");
            await Task.Delay(1000);
            switch (JämförDatumTidMotFil(Datum, Tid))
            {
                case true:
                    MessageBox.Show("Önskad datum och tid är tillgänglig . . .!");
                    MessageBox.Show("Kollar om vald bord är tillgängligt . . .\n(2/2)");
                    await Task.Delay(1000);
                    bool jämförBord = JämförBordMotFil(Datum, Tid, BordsTyp);
                    switch (jämförBord)
                    {
                        case true:
                            MessageBox.Show("Bordet är tillgängligt!");
                            return true;
                        case false:
                            {
                                MessageBox.Show("Önskat bord är tyvärr inte tillgängligt..");
                                return false;
                            }
                    }
                case false:
                    MessageBox.Show("Önskad datum och tid är tyvärr inte tillgängligt..");
                    return false;
            }
        }
        public bool JämförDatumTidMotFil(string datum, string tid)
        {
            //fick äntligen in en query i koden här
            int räknare = 1;
            bool ärTillgänglig = true;
            using (StreamReader sr = new StreamReader("BokningarLog.txt", true))
            {
                try
                {
                    while (sr.ReadLine() != null)
                    {
                        var query = from search in sr.ReadToEnd().Split('|')
                                    group search by search.Contains(datum + " " + tid).ToString();
                        foreach (var item in query)
                        {
                            foreach (var count in item)
                            {
                                if (count.Contains(datum + " " + tid))
                                {
                                    räknare++;
                                }
                            }
                        }
                        if (räknare >= 5)
                        {
                            ärTillgänglig = false;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Det finns inga bokningar");
                }
            }
            return ärTillgänglig;
        }
        public bool JämförBordMotFil(string datum, string tid, string bord)
        {
            string tmp = "";
            bool ärTillgänglig = true;
            using (StreamReader sr = new StreamReader("BokningarLog.txt", true))
            {
                while ((tmp = sr.ReadLine()) != null)
                {
                    if (tmp.Contains(datum + " " + tid + " " + bord))
                    {
                        ärTillgänglig = false;
                    }
                }
            }
            return ärTillgänglig;
        }
        public void SkrivTillFil(string bokning)
        {

            using (StreamWriter sw = new StreamWriter("BokningarLog.txt", true))
            {
                sw.WriteLine(bokning);
            }
        }
        public void LäsFrånFil(ListBox listbox)
        {
            string tmp = "";
            using (StreamReader sr = new StreamReader("BokningarLog.txt", true))
            {
                while ((tmp = sr.ReadLine()) != null)
                {
                    listbox.Items.Add(tmp);
                }
            }
        }
        public void TaBortFrånFil(string input)
        {
            string tmp = "";
            List<string> bytUtLista = new List<string>();
            string bytUt = "";
            using (StreamReader sr = new StreamReader("BokningarLog.txt"))
            {
                while ((tmp = sr.ReadLine()) != null)
                {
                    bytUtLista.Add(tmp);
                }
                foreach (var item in bytUtLista)
                {
                    if (item != input)
                    {
                        bytUt += item + "\n";
                    }
                }
                sr.Close();
            }
            using (StreamWriter sw = new StreamWriter("BokningarLog.txt"))
            {
                sw.Write(bytUt);
                sw.Close();
            }
        }
        public void FixaTomFil()
        {
            //om txtfilen är tom, skriv nya bokningar hehe
            using (StreamReader sr = new StreamReader("BokningarLog.txt", true))
            {
                string text = sr.ReadToEnd();
                if (text == "")
                {
                    sr.Close();
                    string bokning1 = "|2022-10-04 20:30 Bord för  2 personer|Benedict Cumberbatch";
                    string bokning2 = "|2022-10-05 19:45 Bord för  6 personer|Dwayne Johnson";
                    string bokning3 = "|2022-10-05 19:45 Bord för  8 personer|Jason Statham";
                    SkrivTillFil(bokning1);
                    SkrivTillFil(bokning2);
                    SkrivTillFil(bokning3);
                }
            }
        }
    }

}
