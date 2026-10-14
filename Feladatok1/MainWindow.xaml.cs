using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace AlapfeladatokWPF
{
    public partial class MainWindow : Window
    {
        string[] nevek = {
            "Hello world!", "Köszönés", "Szám kétszerese", "Négy alapművelet",
            "Két szám nagyobbika", "Három szám legkisebbike", "Háromszög szerkeszthetősége",
            "Számtani és mértani közép", "Másodfokú egyenlet: van-e megoldás?",
            "Másodfokú egyenlet megoldásai", "Átfogó", "Téglatest", "Kör",
            "Körcikk és körív", "Számok szóközzel", "Számok egymás alatt",
            "Osztók", "Osztók összege", "Tökéletes szám", "Hatványozás",
            "Pozitív szám bekérése", "Tíznél kisebb számok összege", "Kettes tényezők",
            "Az alma szó bekérése", "Osztás hárommal", "Prímszámvizsgálat",
            "Prímszámok egy határig", "Prímosztók", "Prímtényezős felbontás",
            "Legnagyobb közös osztó", "Legkisebb közös többszörös"
        };

        // A 22. feladat több gombnyomáson át gyűjti az összeget.
        double osszeg = 0;
        bool befejezve = false;

        public MainWindow()
        {
            InitializeComponent();
            for (int i = 0; i < nevek.Length; i++)
                FeladatComboBox.Items.Add((i + 1) + ". " + nevek[i]);
            FeladatComboBox.SelectedIndex = 0;
        }

        private void FeladatComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Beallitas();
        }

        private void UjraButton_Click(object sender, RoutedEventArgs e)
        {
            Beallitas();
        }

        private void Beallitas()
        {
            int feladat = FeladatComboBox.SelectedIndex + 1;
            osszeg = 0;
            befejezve = false;
            FuttatButton.IsEnabled = true;
            ElsoTextBox.Clear();
            MasodikTextBox.Clear();
            HarmadikTextBox.Clear();
            EredmenyTextBox.Clear();

            ElsoPanel.Visibility = Visibility.Visible;
            MasodikPanel.Visibility = Visibility.Collapsed;
            HarmadikPanel.Visibility = Visibility.Collapsed;
            ElsoLabel.Text = "Szám:";
            MasodikLabel.Text = "Második szám:";
            HarmadikLabel.Text = "Harmadik szám:";
            LeirasTextBlock.Text = nevek[feladat - 1];

            if (feladat == 1) ElsoPanel.Visibility = Visibility.Collapsed;
            if (feladat == 2) ElsoLabel.Text = "Neved:";
            if (feladat == 24) ElsoLabel.Text = "Írd be az alma szót:";

            if (feladat == 4 || feladat == 5 || feladat == 8 || feladat == 11 ||
                feladat == 13 || feladat == 14 || feladat == 20 || feladat == 30 || feladat == 31)
                MasodikPanel.Visibility = Visibility.Visible;
            if (feladat == 13) MasodikPanel.Visibility = Visibility.Collapsed;

            if (feladat == 6 || feladat == 7 || feladat == 9 || feladat == 10 || feladat == 12)
            {
                MasodikPanel.Visibility = Visibility.Visible;
                HarmadikPanel.Visibility = Visibility.Visible;
            }
            if (feladat == 7 || feladat == 12)
            {
                ElsoLabel.Text = "a oldal / él:";
                MasodikLabel.Text = "b oldal / él:";
                HarmadikLabel.Text = "c oldal / él:";
            }
            if (feladat == 9 || feladat == 10)
            {
                ElsoLabel.Text = "a együttható:";
                MasodikLabel.Text = "b együttható:";
                HarmadikLabel.Text = "c együttható:";
                LeirasTextBlock.Text = "a*x² + b*x + c = 0; a valós megoldásokat vizsgáljuk.";
            }
            if (feladat == 11)
            {
                ElsoLabel.Text = "Első befogó:";
                MasodikLabel.Text = "Második befogó:";
            }
            if (feladat == 13) ElsoLabel.Text = "Átmérő:";
            if (feladat == 14)
            {
                ElsoLabel.Text = "Sugár:";
                MasodikLabel.Text = "Középponti szög (fok, 0–360):";
                LeirasTextBlock.Text = "A körcikk területét és a körív hosszát számoljuk.";
            }
            if (feladat == 20)
            {
                ElsoLabel.Text = "Hatványalap:";
                MasodikLabel.Text = "Egész kitevő:";
            }
            if (feladat == 21)
                LeirasTextBlock.Text = "Adj meg egy pozitív számot. Hibás adat után újra próbálkozhatsz.";
            if (feladat == 22)
                LeirasTextBlock.Text = "Egyesével adj meg számokat. A 10 vagy nagyobb szám lezárja a bekérést; ez már nem része az összegnek.";
            if (feladat == 24)
                LeirasTextBlock.Text = "Csak az alma szót fogadjuk el. Hibás szó után próbáld újra.";
        }

        private void FuttatButton_Click(object sender, RoutedEventArgs e)
        {
            int feladat = FeladatComboBox.SelectedIndex + 1;
            try
            {
                string eredmeny = Megoldas(feladat);
                EredmenyTextBox.Text = feladat + ". feladat:\n" + eredmeny;
            }
            catch (Exception ex)
            {
                EredmenyTextBox.Text = "Hiba: " + ex.Message;
            }
        }

        // Ponttal és vesszővel megadott tizedes számot is elfogad.
        private double Szam(TextBox mezo)
        {
            string szoveg = mezo.Text.Trim().Replace(',', '.');
            double ertek;
            if (!double.TryParse(szoveg, NumberStyles.Float, CultureInfo.InvariantCulture, out ertek)
                || double.IsNaN(ertek) || double.IsInfinity(ertek))
                throw new Exception("Érvényes számot adj meg!");
            return ertek;
        }

        private int Egesz(TextBox mezo)
        {
            int szam;
            if (!int.TryParse(mezo.Text.Trim(), out szam))
                throw new Exception("Egész számot adj meg!");
            // Az egyszerű ciklusok miatt korlátozzuk a bemenetet.
            if (szam < -1000000 || szam > 1000000)
                throw new Exception("Az egész szám -1 000 000 és 1 000 000 között legyen!");
            return szam;
        }

        private int PozitivEgesz(TextBox mezo)
        {
            int szam = Egesz(mezo);
            if (szam <= 0) throw new Exception("Pozitív egész számot adj meg!");
            return szam;
        }

        private double Pozitiv(TextBox mezo)
        {
            double szam = Szam(mezo);
            if (szam <= 0) throw new Exception("Pozitív számot adj meg!");
            return szam;
        }

        private bool Prim(int szam)
        {
            if (szam < 2) return false;
            for (int i = 2; i * i <= szam; i++)
                if (szam % i == 0) return false;
            return true;
        }

        private long Lnko(long a, long b)
        {
            a = Math.Abs(a);
            b = Math.Abs(b);
            while (b > 0)
            {
                long t = b;
                b = a % b;
                a = t;
            }
            return a;
        }

        private string Megoldas(int feladat)
        {
            switch (feladat)
            {
                case 1:
                    return "Hello world!";
                case 2:
                    if (ElsoTextBox.Text.Trim() == "") throw new Exception("Add meg a neved!");
                    return "Szia, " + ElsoTextBox.Text.Trim() + "!";
                case 3:
                    return "Kétszerese: " + (Szam(ElsoTextBox) * 2);
                case 4:
                    {
                        double a = Szam(ElsoTextBox), b = Szam(MasodikTextBox);
                        string szoveg = "Összeg: " + (a + b) + "\nKülönbség (a-b): " + (a - b)
                            + "\nSzorzat: " + (a * b);
                        if (b != 0) szoveg += "\nHányados (a/b): " + (a / b);
                        else szoveg += "\nNullával nem lehet osztani.";
                        return szoveg;
                    }
                case 5:
                    {
                        int a = Egesz(ElsoTextBox), b = Egesz(MasodikTextBox);
                        if (a > b) return "Nagyobbik: " + a;
                        if (b > a) return "Nagyobbik: " + b;
                        return "A két szám egyenlő: " + a;
                    }
                case 6:
                    {
                        int a = Egesz(ElsoTextBox), b = Egesz(MasodikTextBox), c = Egesz(HarmadikTextBox);
                        int legkisebb = a;
                        if (b < legkisebb) legkisebb = b;
                        if (c < legkisebb) legkisebb = c;
                        return "Legkisebb: " + legkisebb;
                    }
                case 7:
                    {
                        double a = Szam(ElsoTextBox), b = Szam(MasodikTextBox), c = Szam(HarmadikTextBox);
                        if (a > 0 && b > 0 && c > 0 && a + b > c && a + c > b && b + c > a)
                            return "A háromszög szerkeszthető.";
                        return "A háromszög nem szerkeszthető.";
                    }
                case 8:
                    {
                        int a = PozitivEgesz(ElsoTextBox), b = PozitivEgesz(MasodikTextBox);
                        return "Számtani közép: " + ((a + b) / 2.0)
                            + "\nMértani közép: " + Math.Sqrt((double)a * b);
                    }
                case 9:
                case 10:
                    {
                        double a = Szam(ElsoTextBox), b = Szam(MasodikTextBox), c = Szam(HarmadikTextBox);
                        if (a == 0)
                        {
                            if (b != 0) return "Ez elsőfokú egyenlet. Megoldása: x = " + (-c / b);
                            if (c == 0) return "Az egyenlet minden valós számra igaz.";
                            return "Nincs megoldás.";
                        }
                        double d = b * b - 4 * a * c;
                        if (double.IsNaN(d) || double.IsInfinity(d))
                            throw new Exception("Túl nagy együtthatók. Adj meg kisebb számokat!");
                        if (d < 0) return "Nincs valós megoldás.";
                        if (feladat == 9)
                            return d == 0 ? "Egy valós megoldás van." : "Két valós megoldás van.";
                        if (d == 0) return "x = " + (-b / (2 * a));
                        return "x1 = " + ((-b + Math.Sqrt(d)) / (2 * a))
                            + "\nx2 = " + ((-b - Math.Sqrt(d)) / (2 * a));
                    }
                case 11:
                    {
                        double a = Pozitiv(ElsoTextBox), b = Pozitiv(MasodikTextBox);
                        return "Átfogó: " + Math.Sqrt(a * a + b * b).ToString("F2");
                    }
                case 12:
                    {
                        double a = Pozitiv(ElsoTextBox), b = Pozitiv(MasodikTextBox), c = Pozitiv(HarmadikTextBox);
                        return "Felszín: " + (2 * (a * b + a * c + b * c)) + "\nTérfogat: " + (a * b * c);
                    }
                case 13:
                    {
                        double atmero = Pozitiv(ElsoTextBox), r = atmero / 2;
                        return "Kerület: " + (Math.PI * atmero) + "\nTerület: " + (Math.PI * r * r);
                    }
                case 14:
                    {
                        double r = Pozitiv(ElsoTextBox), szog = Szam(MasodikTextBox);
                        if (szog < 0 || szog > 360) throw new Exception("A szög 0 és 360 fok között legyen!");
                        return "Körcikk területe: " + (Math.PI * r * r * szog / 360)
                            + "\nKörív hossza: " + (2 * Math.PI * r * szog / 360);
                    }
                case 15:
                case 16:
                    {
                        int n = PozitivEgesz(ElsoTextBox);
                        if (n > 10000) throw new Exception("A kiíráshoz legfeljebb 10 000-et adj meg!");
                        List<string> szamok = new List<string>();
                        for (int i = 1; i <= n; i++) szamok.Add(i.ToString());
                        return string.Join(feladat == 15 ? " " : "\n", szamok);
                    }
                case 17:
                case 18:
                case 19:
                    {
                        int n = PozitivEgesz(ElsoTextBox);
                        long osszeg = 0;
                        List<string> osztok = new List<string>();
                        for (int i = 1; i <= n; i++)
                            if (n % i == 0) { osszeg += i; osztok.Add(i.ToString()); }
                        if (feladat == 17) return "Osztók: " + string.Join(" ", osztok);
                        if (feladat == 18) return "Osztók összege: " + osszeg;
                        return osszeg == 2L * n ? "Tökéletes szám." : "Nem tökéletes szám.";
                    }
                case 20:
                    {
                        double alap = Szam(ElsoTextBox);
                        int kitevo = Egesz(MasodikTextBox);
                        if (alap == 0 && kitevo <= 0)
                            throw new Exception("Nulla alaphoz pozitív kitevőt adj meg! A 0^0-t itt nem értelmezzük.");
                        double hatvany = 1;
                        for (int i = 0; i < Math.Abs(kitevo); i++) hatvany *= alap;
                        if (kitevo < 0) hatvany = 1 / hatvany;
                        if (double.IsInfinity(hatvany) || double.IsNaN(hatvany))
                            throw new Exception("Az eredmény nem ábrázolható. Adj meg kisebb értékeket!");
                        return "Hatványérték: " + hatvany;
                    }
                case 21:
                    {
                        double n = Szam(ElsoTextBox);
                        if (n <= 0) return "Nem pozitív. Adj meg másik számot, majd kattints újra!";
                        FuttatButton.IsEnabled = false;
                        return "Elfogadott pozitív szám: " + n;
                    }
                case 22:
                    {
                        if (befejezve) return "A bekérés véget ért. Összeg: " + osszeg;
                        double n = Szam(ElsoTextBox);
                        ElsoTextBox.Clear();
                        if (n < 10)
                        {
                            osszeg += n;
                            ElsoTextBox.Focus();
                            return "Eddigi összeg: " + osszeg + "\nAdj meg egy újabb számot!";
                        }
                        befejezve = true;
                        FuttatButton.IsEnabled = false;
                        return "Bekérés vége. A tíznél kisebb számok összege: " + osszeg;
                    }
                case 23:
                    {
                        int n = Egesz(ElsoTextBox), eredeti = n;
                        if (n == 0) return "A nulla esetén a kettővel osztás nem érne véget.";
                        string szoveg = eredeti + " = ";
                        while (n % 2 == 0) { szoveg += "2*"; n /= 2; }
                        return szoveg + n;
                    }
                case 24:
                    if (ElsoTextBox.Text != "alma") return "Nem megfelelő szó. Írd be pontosan: alma";
                    FuttatButton.IsEnabled = false;
                    return "Az alma gyümölcs!";
                case 25:
                    {
                        int n = Egesz(ElsoTextBox), eredeti = n, hanyados = 0;
                        while (n >= 3) { n -= 3; hanyados++; }
                        // Negatív bemenetnél is 0, 1 vagy 2 legyen a maradék.
                        while (n < 0) { n += 3; hanyados--; }
                        return eredeti + " = " + hanyados + "*3+" + n;
                    }
                case 26:
                    return Prim(Egesz(ElsoTextBox)) ? "Prímszám." : "Nem prímszám.";
                case 27:
                    {
                        int n = PozitivEgesz(ElsoTextBox);
                        if (n > 100000) throw new Exception("Ehhez legfeljebb 100 000-et adj meg!");
                        List<string> primszamok = new List<string>();
                        for (int i = 2; i <= n; i++) if (Prim(i)) primszamok.Add(i.ToString());
                        return primszamok.Count == 0 ? "Nincs prímszám eddig a határig." : string.Join(" ", primszamok);
                    }
                case 28:
                case 29:
                    {
                        int n = PozitivEgesz(ElsoTextBox), eredeti = n;
                        if (n == 1) return "Az 1-nek nincs prímosztója vagy prímtényezője.";
                        List<string> tenyezok = new List<string>();
                        for (int i = 2; i <= n; i++)
                        {
                            if (n % i == 0)
                            {
                                if (feladat == 28) tenyezok.Add(i.ToString());
                                while (n % i == 0)
                                {
                                    if (feladat == 29) tenyezok.Add(i.ToString());
                                    n /= i;
                                }
                            }
                        }
                        if (feladat == 28) return "Prímosztók: " + string.Join(" ", tenyezok);
                        return eredeti + " = " + string.Join("*", tenyezok);
                    }
                case 30:
                case 31:
                    {
                        long a = Egesz(ElsoTextBox), b = Egesz(MasodikTextBox);
                        if (a == 0 && b == 0) return "A két nulla LNKO-ját és LKKT-jét itt nem értelmezzük.";
                        long lnko = Lnko(a, b);
                        if (feladat == 30) return "LNKO: " + lnko;
                        if (a == 0 || b == 0) return "LKKT: 0";
                        return "LKKT: " + Math.Abs((a / lnko) * b);
                    }
            }
            return "Válassz egy feladatot!";
        }
    }
}