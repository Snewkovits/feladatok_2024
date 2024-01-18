using System.Diagnostics.CodeAnalysis;

namespace Vasmegye_console
{
    class Program
    {
        static readonly string eleres = "vas.txt";

        public static void Main()
        {
            int counter = 0;

            List<SzemelyiSzam> szemelyiSzamok = new List<SzemelyiSzam>();
            List<SzemelyiSzam> torlendoSzamok = new List<SzemelyiSzam>();
            List<int> datumok = new List<int>();
            List<int> vizsgalt = new List<int>();
            int[] statisztika;

            Console.WriteLine("Nyomj egy gombot az adatok beolvasásához!");
            Console.ReadKey();
            Console.WriteLine("2. feladat: Adatok beolvasása, tárolása");w
            try
            {
                szemelyiSzamok = Beolvas();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Sikeres beolvasás");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Sikereteln beolvasás");
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("4. feladat: Ellenörzés ");

            foreach (SzemelyiSzam szam in szemelyiSzamok)
            {
                if (!CdvEll(szam))
                {
                    Console.WriteLine($"\tHibás a {szam} személyi azonosító!");
                    torlendoSzamok.Add(szam);
                }
            }
            foreach (SzemelyiSzam szam in torlendoSzamok)
                szemelyiSzamok.Remove(szam);

            Console.WriteLine($"5. feladat: Vas megyében a vizsgált évek alatt {szemelyiSzamok.Count} csecsemő született.");

            foreach (SzemelyiSzam szam in szemelyiSzamok)
                if (szam.nem == 1 || szam.nem == 3)
                    counter++;
            Console.WriteLine($"6. feladat: Fiúk száma: {counter}");

            foreach (SzemelyiSzam szam in szemelyiSzamok)
            {
                counter = int.Parse(szam.GetSzul().Substring(0, 2));
                if (!datumok.Contains(counter))
                    datumok.Add(counter);
            }

            foreach (int datum in datumok)
                if (datum > 9)
                    vizsgalt.Add(int.Parse($"19{datum}"));
                else
                    vizsgalt.Add(int.Parse($"200{datum}"));

            Console.WriteLine($"7. feladat: A vizsgált időszak: {vizsgalt.Min()} - {vizsgalt.Max()}");

            foreach (SzemelyiSzam szam in szemelyiSzamok)
            {
                if (szam.GetSzul().Substring(2, 4) == "0224")
                {
                    Console.WriteLine("8. feladat: Szökőnapon született baba!");
                    break;
                }
                else if (szemelyiSzamok[szemelyiSzamok.Count - 1].Equals(szam))
                    Console.WriteLine("8. feladat: Szökőnapon nem született baba!");
            }

            vizsgalt.Sort();
            statisztika = new int[vizsgalt.Count];
            foreach (SzemelyiSzam szam in szemelyiSzamok)
            {
                for (int i = 0; i < vizsgalt.Count; i++)
                {
                    if (int.Parse(szam.GetSzul().Substring(0, 2)) == int.Parse(vizsgalt[i].ToString().Substring(2, 2)))
                    {
                        statisztika[i]++;
                    }
                }
            }
            Console.WriteLine("9. feladat: Statisztika");
            for (int i = 0; i < vizsgalt.Count; i++)
            {
                Console.WriteLine($"\t{vizsgalt[i]} - {statisztika[i]} fő");
            }
        }

        static List<SzemelyiSzam> Beolvas()
        {
            List<SzemelyiSzam> result = new List<SzemelyiSzam>();
            using (StreamReader sr = new StreamReader(eleres))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine().Trim();
                    string szul = line.Split('-')[1];
                    if (int.Parse(szul.Substring(0, 2)) > 10)
                        szul = $"19{szul.Substring(0, 2)}-{szul.Substring(2, 2)}-{szul.Substring(4, 2)}";
                    else
                        szul = $"20{szul.Substring(0, 2)}-{szul.Substring(2, 2)}-{szul.Substring(4, 2)}";
                    result.Add(new SzemelyiSzam(Convert.ToInt32(line.Split('-')[0]), Convert.ToDateTime(szul), Convert.ToInt32(line.Split('-')[2].Substring(0, 3)), Convert.ToInt32(line.Split('-')[2].Substring(line.Split('-')[2].Length - 1))));
                }
            }

            return result;
        }

        static bool CdvEll(SzemelyiSzam szam)
        {
            int ertek = 0;
            string szemelyiSzam = szam.HibaNelkul();

            int index = 10;
            for (int i = 0; i < szemelyiSzam.Length; i++)
            {
                ertek += Convert.ToInt32(szemelyiSzam[i]) * index;
                index--;
            }

            ertek %= 11;

            if (ertek == szam.GetHiba())
                return true;
            else
                return false;
        }
    }

    public struct SzemelyiSzam
    {
        public int nem;
        public DateTime szuletesiDatum;
        public int azonosito;
        public int hiba;

        public SzemelyiSzam(int nem, DateTime szuletesiDatum, int azonosito, int hiba)
        {
            this.nem = nem;
            this.azonosito = azonosito;
            this.hiba = hiba;

            this.szuletesiDatum = szuletesiDatum;
        }

        public int GetHiba()
        {
            return hiba;
        }

        public string GetSzul()
        {
            string month;
            string day;
            if (szuletesiDatum.Month < 10)
                month = $"0{szuletesiDatum.Month}";
            else
                month = szuletesiDatum.Month.ToString();
            if (szuletesiDatum.Day < 10)
                day = $"0{szuletesiDatum.Day}";
            else
                day = szuletesiDatum.Day.ToString();
            return $"{szuletesiDatum.Year.ToString().Substring(2, 2)}{month}{day}";
        }

        public string HibaNelkul()
        {
            string month;
            string day;
            if (szuletesiDatum.Month < 10)
                month = $"0{szuletesiDatum.Month}";
            else
                month = szuletesiDatum.Month.ToString();
            if (szuletesiDatum.Day < 10)
                day = $"0{szuletesiDatum.Day}";
            else
                day = szuletesiDatum.Day.ToString();
            return $"{nem}{szuletesiDatum.Year.ToString().Substring(2, 2)}{month}{day}{azonosito}";
        }

        public string Egyben()
        {
            string month;
            string day;
            if (szuletesiDatum.Month < 10)
                month = $"0{szuletesiDatum.Month}";
            else
                month = szuletesiDatum.Month.ToString();
            if (szuletesiDatum.Day < 10)
                day = $"0{szuletesiDatum.Day}";
            else
                day = szuletesiDatum.Day.ToString();
            return $"{nem}{szuletesiDatum.Year.ToString().Substring(2, 2)}{month}{day}{azonosito}{hiba}";
        }

        public override string ToString()
        {
            string month;
            string day;
            if (szuletesiDatum.Month < 10)
                month = $"0{szuletesiDatum.Month}";
            else
                month = szuletesiDatum.Month.ToString();
            if (szuletesiDatum.Day < 10)
                day = $"0{szuletesiDatum.Day}";
            else
                day = szuletesiDatum.Day.ToString();
            return $"{nem}-{szuletesiDatum.Year.ToString().Substring(2, 2) + month + day}-{azonosito}{hiba}";
        }
    }
}
