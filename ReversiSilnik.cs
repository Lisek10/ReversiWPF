namespace Reversi {
    public class ReversiSilnik {
        protected int[,] plansza = new int[8, 8];
        public int NumerGraczaWykonującegoNastępnyRuch { get; protected set; }
        public int LiczbaPustychPól { get; private set; }

        public ReversiSilnik(int start) {
            NumerGraczaWykonującegoNastępnyRuch = start;
            plansza[3, 3] = plansza[4, 4] = 1;
            plansza[3, 4] = plansza[4, 3] = 2;
            LiczbaPustychPól = 60;
        }

        public int PobierzStanPola(int x, int y) => plansza[x, y];

        public int PołóżKamień(int x, int y, bool symulacja = false) {
            if (plansza[x, y] != 0) return -1;
            int przejęteRazem = 0;
            int przeciwnik = NumerGraczaWykonującegoNastępnyRuch == 1 ? 2 : 1;

            for (int dx = -1; dx <= 1; dx++) {
                for (int dy = -1; dy <= 1; dy++) {
                    if (dx == 0 && dy == 0) continue;
                    int i = x + dx, j = y + dy, licznik = 0;
                    while (i >= 0 && i < 8 && j >= 0 && j < 8 && plansza[i, j] == przeciwnik) {
                        i += dx; j += dy; licznik++;
                    }
                    if (licznik > 0 && i >= 0 && i < 8 && j >= 0 && j < 8 && plansza[i, j] == NumerGraczaWykonującegoNastępnyRuch) {
                        if (!symulacja) {
                            for (int k = 0; k <= licznik; k++) plansza[x + k * dx, y + k * dy] = NumerGraczaWykonującegoNastępnyRuch;
                        }
                        przejęteRazem += licznik;
                    }
                }
            }
            if (przejęteRazem > 0 && !symulacja) {
                NumerGraczaWykonującegoNastępnyRuch = przeciwnik;
                LiczbaPustychPól--;
            }
            return przejęteRazem;
        }

        public int LiczbaPól(int gracz) {
            int c = 0; foreach (int p in plansza) if (p == gracz) c++; return c;
        }

        public bool CzyGraZakończona() => LiczbaPustychPól == 0 || (!MożeWykonaćRuch(1) && !MożeWykonaćRuch(2));

        public bool MożeWykonaćRuch(int gracz) {
            int obecny = NumerGraczaWykonującegoNastępnyRuch;
            NumerGraczaWykonującegoNastępnyRuch = gracz;
            for (int i = 0; i < 8; i++)
                for (int j = 0; j < 8; j++)
                    if (PołóżKamień(i, j, true) > 0) {
                        NumerGraczaWykonującegoNastępnyRuch = obecny;
                        return true;
                    }
            NumerGraczaWykonującegoNastępnyRuch = obecny;
            return false;
        }

        public string NumerGraczaMającegoPrzewagę => LiczbaPól(1) > LiczbaPól(2) ? "Zielony" : "Brązowy";
    }
}