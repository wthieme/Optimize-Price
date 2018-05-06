using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace optimize_price {
    class Program {
        static void Main (string[] args) {
            var eersteA = optimizePriceA (new [] { 10, 20, 30 }, new [] { 0, 0, 0 });
            var eersteB = optimizePriceB (new [] { 10, 20, 30 }, new [] { 0, 0, 0 });
            Console.WriteLine ($"Prijs = {eersteA}, {eersteB}");

            var tweedeA = optimizePriceA (new [] { 10, 20, 30 }, new [] { 5, 15, 6 });
            var tweedeB = optimizePriceB (new [] { 10, 20, 30 }, new [] { 5, 15, 6 });
            Console.WriteLine ($"Prijs = {tweedeA}, {tweedeB}");

            var derdeA = optimizePriceA (new [] { 20, 10, 20, 30 }, new [] { 15, 0, 15, 20 });
            var derdeB = optimizePriceB (new [] { 20, 10, 20, 30 }, new [] { 15, 0, 15, 20 });
            Console.WriteLine ($"Prijs = {derdeA}, {derdeB}");
            
            var vierdeA = optimizePriceA (new [] { 10, 10, 20, 30 }, new [] { 11, 21, 21, 36 });
            var vierdeB = optimizePriceB (new [] { 10, 10, 20, 30 }, new [] { 11, 21, 21, 36 });
            Console.WriteLine ($"Prijs = {vierdeA}, {vierdeB}");
        }

        public static int optimizePriceA (int[] price, int[] cost) {

            // De prijs die klanten willen betalen en de kosten in 1 lijst
            var klantEnKosten = price.Zip (cost, (p, c) => new { maxPrijs = p, kosten = c });

            Nullable<int> maxPrijs = null;
            int maxWinst = 0;

            // De door te rekenen prijzen op volgorde van laag naar hoog omdat bij gelijke winst 
            // de laagste prijs moet worden teruggegeven
            var prijzen = price.Distinct ().OrderBy (p => p);

            foreach (var prijs in prijzen) {
                var klantenDieKopen = klantEnKosten.Where (k => prijs <= k.maxPrijs);
                var winstgevendeKlanten = klantenDieKopen.Where (k => (prijs - k.kosten) > 0);
                var winst = winstgevendeKlanten.Sum (k => prijs - k.kosten);

                // Bewaar de beste prijs tot nu toe
                if (winst > maxWinst) {
                    maxPrijs = prijs;
                    maxWinst = winst;
                }
            }
            return maxPrijs.HasValue ? maxPrijs.Value : 0;
        }

        public static int optimizePriceB (int[] price, int[] cost) {

            // De door te rekenen prijzen
            var prijzen = price.Distinct ();

            // De prijs die klanten willen betalen en de kosten in 1 lijst
            var klanten = price.Zip (cost, (p, c) => new { maxPrijs = p, kosten = c });

            var prijsEnWinst = prijzen.Select (prijs => new {
                prijs = prijs, winst = klanten
                    .Where (k => prijs <= k.maxPrijs) // wil kopen
                    .Where (k => (prijs - k.kosten) > 0) // maakt winst
                    .Sum (k => prijs - k.kosten) // winst voor deze prijs
            });

            var bestePrijs = prijsEnWinst
                .Where (pw => pw.winst > 0)
                .OrderByDescending (pw => pw.winst)
                .ThenBy (pw => pw.prijs)
                .FirstOrDefault ();

            return bestePrijs == null ? 0 : bestePrijs.prijs;
        }
    }
}