using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Vasmegye
{
    public partial class Form1 : Form
    {
        List<SzemelyiSzam> szemelyiSzamok;
        readonly string eleres = "vas.txt";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            szemelyiSzamok = new List<SzemelyiSzam>();
            using (StreamReader sr = new StreamReader(eleres))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    szemelyiSzamok.Add(new SzemelyiSzam()
                    {
                        nem = Convert.ToInt32(line.Split('-')[0]),
                        szuletesiDatum = Convert.ToInt32(line.Split('-')[1]),
                        azonosito = Convert.ToInt32(line.Split('-')[2].Substring(0, 3)),
                        hiba = Convert.ToInt32(line.Split('-')[2].Substring(line.Split('-')[2].Length-1))
                    });
                }
            }
        }

        private bool CdvEll(SzemelyiSzam szam)
        {
            int ertek = 0;
            string szemelyiSzam = $"{szam.nem}{szam.szuletesiDatum}{szam.azonosito}";

            for (int i = 1; i <= 10; i++)
            {
                ertek += Convert.ToInt32(szemelyiSzam[i]) * i;
            }
            ertek /= 11;

            if (ertek == szam.hiba)
                return true;
            else
                return false;
        }
    }

    public struct SzemelyiSzam
    {
        public int nem;
        public int szuletesiDatum;
        public int azonosito;
        public int hiba;
    }
}
