using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace optimize_price {
    class Program {
        static void Main (string[] args) {
            var eerste = optimizePrice (new [] { 10, 20, 30 }, new [] { 0, 0, 0 });
            Console.WriteLine ($"Prijs = {eerste}");
            var tweede = optimizePrice (new [] { 10, 20, 30 }, new [] { 5, 15, 6 });
            Console.WriteLine ($"Prijs = {tweede}");
            var derde = optimizePrice (new [] { 20, 10, 20, 30 }, new [] { 5, 6, 15, 6 });
            Console.WriteLine ($"Prijs = {derde}");
            var vierde = optimizePrice (new [] { 10, 10, 20, 30 }, new [] { 11, 21, 21, 36 });
            Console.WriteLine ($"Prijs = {vierde}");
        }

        public static int optimizePrice (int[] price, int[] cost) {

            var klanten = price.Zip (cost, (p, c) => new { maxPrijs = p, kosten = c });

            Nullable<int> maxPrijs = null;
            int maxWinst = 0;

            var prijzen = price.Distinct ().OrderBy (p => p);
            foreach (var prijs in prijzen) {
                var klantenDieKopen = klanten.Where (k => prijs <= k.maxPrijs);
                var winstgevendeKlanten = klantenDieKopen.Where (k => (prijs - k.kosten) > 0);
                var winst = winstgevendeKlanten.Sum (k => prijs - k.kosten);
                if (winst > maxWinst) {
                    maxPrijs = prijs;
                    maxWinst = winst;
                }
            }
            return maxPrijs.HasValue ? maxPrijs.Value : 0;
        }
    }
}