
using System;

namespace ASD
{

    class ChangeMaking
    {

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// bez ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości amount ( czyli rzędu o(amount) )
        /// </remarks>
        public int? NoLimitsDynamic(int amount, int[] coins, out int[] change)
        {
            // change = null;  // zmienić
            // K =amount
            // k - coins.length
            // n_1, n_2, ... ,n_k
            int[] T = new int[amount + 1];
            int[] P = new int[amount + 1]; // tu zapisujemy index coins
            T[0] = 0;
            for (int kk = 1; kk <= amount; kk++)
            {
                // T[kk] = int.MaxValue;
                for (int l = 0; l < coins.Length; l++)
                {
                    if (kk - coins[l] < 0) continue;
                    if (T[kk - coins[l]] == 0 && kk - coins[l] != 0) continue;
                    int c = 1 + T[kk - coins[l]]; // ile monet użyjemy w wariancie z tą monetą, kk - wartość którą teraz próbujemy rozmienić
                    if (T[kk] == 0 || c < T[kk])
                    {
                        P[kk] = l;
                        T[kk] = c;
                    }
                }
            }
            if (T[^1] == 0)
            {
                change = null;
                return null;
            }

            int current_amount = amount;
            change = new int[coins.Length];
            while (current_amount > 0)
            {
                change[P[current_amount]] += 1;
                current_amount -= coins[P[current_amount]];
            }
            return T[^1]; ;
        }

        /// <summary>
        /// Metoda wyznacza rozwiązanie problemu wydawania reszty przy pomocy minimalnej liczby monet
        /// z uwzględnieniem ograniczeń na liczbę monet danego rodzaju
        /// </summary>
        /// <param name="amount">Kwota reszty do wydania</param>
        /// <param name="coins">Dostępne nominały monet</param>
        /// <param name="limits">Liczba dostępnych monet danego nomimału</param>
        /// <param name="change">Liczby monet danego nominału użytych przy wydawaniu reszty</param>
        /// <returns>Minimalna liczba monet potrzebnych do wydania reszty</returns>
        /// <remarks>
        /// coins[i]  - nominał monety i-tego rodzaju
        /// limits[i] - dostepna liczba monet i-tego rodzaju (nominału)
        /// change[i] - liczba monet i-tego rodzaju (nominału) użyta w rozwiązaniu
        /// Jeśli dostepnymi monetami nie da się wydać danej kwoty to change = null,
        /// a metoda również zwraca null
        ///
        /// Wskazówka/wymaganie:
        /// Dodatkowa uzyta pamięć powinna (musi) być proporcjonalna do wartości iloczynu amount*(liczba rodzajów monet)
        /// ( czyli rzędu o(amount*(liczba rodzajów monet)) )
        /// </remarks>
        public int? Dynamic(int amount, int[] coins, int[] limits, out int[] change)
        {
            int[][] coinsUsed = new int[amount + 1][];
            for (int i = 0; i <= amount; ++i)
            {
                coinsUsed[i] = new int[coins.Length];
            }

            int[] minCoins = new int[amount + 1];
            for (int i = 1; i <= amount; ++i)
            {
                minCoins[i] = int.MaxValue - 1;
            }

            int[] limitsCopy = new int[limits.Length];
            limits.CopyTo(limitsCopy, 0);

            for (int i = 0; i < coins.Length; ++i)
            {
                while (limitsCopy[i] > 0)
                {
                    for (int j = amount; j >= 0; --j)
                    {
                        int currAmount = j + coins[i];
                        if (currAmount > amount) continue;
                        if (minCoins[currAmount] <= minCoins[j] + 1) continue;

                        minCoins[currAmount] = minCoins[j] + 1;
                        coinsUsed[j].CopyTo(coinsUsed[currAmount], 0);
                        coinsUsed[currAmount][i] += 1;
                    }

                    limitsCopy[i] -= 1;
                }
            }

            if (minCoins[amount] == int.MaxValue - 1)
            {
                change = null;
                return null;
            }

            change = coinsUsed[amount];
            return minCoins[amount];
        }

    }
}

//// K = amount
//// k - coins.length
//// n_1, n_2, ... ,n_k
//int[,] T = new int[amount + 1, coins.Length + 1]; // liczba monet to dla reszty K oraz jakich i ile monet używanmy
//int[] P = new int[amount + 1]; // tu zapisujemy index coins
//for (int i = 0; i < amount + 1; i++)
//    P[i] = -1;
//T[0, 0] = 0;
//for (int kk = 1; kk <= amount; kk++)
//{
//    // T[kk] = int.MaxValue;
//    for (int l = 0; l < coins.Length; l++)
//    {
//        if (kk < coins[l]) continue;
//        //if (T[kk, l + 1] == limits[l]) continue;
//        if (kk - coins[l] < 0) continue;
//        if (T[kk - coins[l], 0] == 0 && kk - coins[l] != 0) continue;
//        int c = 1 + T[kk - coins[l], 0]; // ile monet użyjemy w wariancie z tą monetą, kk - wartość którą teraz próbujemy rozmienić

//        if (T[kk - coins[l], l + 1] + 1 > limits[l]) continue;
//        if (T[kk, 0] == 0 || c < T[kk, 0])
//        {
//            for (int i = 1; i < coins.Length + 1; i++)
//                T[kk, i] = T[kk - coins[l], i];
//            P[kk] = l;

//            T[kk, l + 1]++;
//            T[kk, 0] = c;
//        }
//    }
//    //if (T[kk, 0] == 0 && kk != 0) T[kk, 0] = -1;

//}
//if (T[amount, 0] == 0)
//{
//    change = null;
//    return null;
//}

//int current_amount = amount;
//change = new int[coins.Length];
//while (current_amount > 0)
//{
//    change[P[current_amount]] += 1;
//    current_amount -= coins[P[current_amount]];
//}
//return T[amount, 0]; ;