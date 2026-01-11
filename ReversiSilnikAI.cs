using System;
namespace Reversi {
    public class ReversiSilnikAI : ReversiSilnik {
        public ReversiSilnikAI(int s) : base(s) { }

        private static readonly int[,] Wagi = {
            { 100, -20, 10, 5, 5, 10, -20, 100 },
            { -20, -50, -2, -2, -2, -2, -50, -20 },
            { 10, -2, 5, 1, 1, 5, -2, 10 },
            { 5, -2, 1, 0, 0, 1, -2, 5 },
            { 5, -2, 1, 0, 0, 1, -2, 5 },
            { 10, -2, 5, 1, 1, 5, -2, 10 },
            { -20, -50, -2, -2, -2, -2, -50, -20 },
            { 100, -20, 10, 5, 5, 10, -20, 100 }
        };

        public bool ZnajdźNajlepszyRuch(out int bestX, out int bestY) {
            bestX = bestY = -1; int maxWartość = int.MinValue;
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    int wynik = PołóżKamień(x, y, true);
                    if (wynik > 0) {
                        int wartość = wynik + Wagi[x, y];
                        if (wartość > maxWartość) { maxWartość = wartość; bestX = x; bestY = y; }
                    }
                }
            }
            return bestX != -1;
        }
    }
}