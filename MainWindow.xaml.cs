using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Reversi {
    public partial class MainWindow : Window {
        private ReversiSilnikAI silnik;
        private Button[,] przyciski = new Button[8, 8];
        private DispatcherTimer aiTimer = new DispatcherTimer();
        private bool graPrzeciwkoKomputerowi = true;

        public MainWindow() {
            InitializeComponent();
            StwórzPrzyciski();
            NowaGra(1);
            aiTimer.Interval = TimeSpan.FromMilliseconds(500);
            aiTimer.Tick += (s, e) => { aiTimer.Stop(); WykonajRuchAI(); };
        }

        private void StwórzPrzyciski() {
            for (int r = 0; r < 8; r++) {
                for (int c = 0; c < 8; c++) {
                    Button b = new Button { Tag = new Point(c, r), Background = Brushes.LightGray };
                    b.Click += Pole_Click;
                    planszaGrid.Children.Add(b);
                    przyciski[c, r] = b;
                }
            }
        }

        private void NowaGra(int ktoZaczyna) {
            silnik = new ReversiSilnikAI(ktoZaczyna);
            OdświeżPlanszę();
            if (graPrzeciwkoKomputerowi && ktoZaczyna == 2) aiTimer.Start();
        }

        private void Pole_Click(object sender, RoutedEventArgs e) {
            Point p = (Point)((Button)sender).Tag;
            if (silnik.PołóżKamień((int)p.X, (int)p.Y) > 0) {
                OdświeżPlanszę();
                SprawdźKoniecLubRuchAI();
            }
        }

        private void SprawdźKoniecLubRuchAI() {
            if (silnik.CzyGraZakończona()) {
                MessageBox.Show("Koniec gry! Zwycięzca: " + silnik.NumerGraczaMającegoPrzewagę);
                return;
            }
            if (graPrzeciwkoKomputerowi && silnik.NumerGraczaWykonującegoNastępnyRuch == 2) aiTimer.Start();
        }

        private void WykonajRuchAI() {
            if (silnik.ZnajdźNajlepszyRuch(out int x, out int y)) {
                silnik.PołóżKamień(x, y);
                OdświeżPlanszę();
                if (silnik.CzyGraZakończona()) MessageBox.Show("Koniec gry!");
            }
        }

        private void OdświeżPlanszę() {
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++) {
                    int stan = silnik.PobierzStanPola(i, j);
                    przyciski[i, j].Background = stan == 1 ? Brushes.Green : (stan == 2 ? Brushes.Sienna : Brushes.LightGray);
                }
            wynikZielony.Text = "Zielony: " + silnik.LiczbaPól(1);
            wynikBrazowy.Text = "Brązowy: " + silnik.LiczbaPól(2);
        }

        private void NowaGra_1Gracz_Ja_Click(object s, RoutedEventArgs e) { graPrzeciwkoKomputerowi = true; NowaGra(1); }
        private void NowaGra_1Gracz_AI_Click(object s, RoutedEventArgs e) { graPrzeciwkoKomputerowi = true; NowaGra(2); }
        private void NowaGra_2Graczy_Click(object s, RoutedEventArgs e) { graPrzeciwkoKomputerowi = false; NowaGra(1); }
        private void Zamknij_Click(object s, RoutedEventArgs e) => Close();
    }
}