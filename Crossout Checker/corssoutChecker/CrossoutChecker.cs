using System;


namespace ASD
{
    class CrossoutChecker
    {
        /// <summary>
        /// Sprawdza, czy podana lista wzorców zawiera wzorzec x
        /// </summary>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="x">Jedyny znak szukanego wzorca</param>
        /// <returns></returns>
        bool comparePattern(char[][] patterns, char x)
        {

            foreach (char[] pat in patterns)
            {
                if (pat.Length == 1 && pat[0] == x)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Sprawdza, czy podana lista wzorców zawiera wzorzec xy
        /// </summary>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="x">Pierwszy znak szukanego wzorca</param>
        /// <param name="y">Drugi znak szukanego wzorca</param>
        /// <returns></returns>
        bool comparePattern(char[][] patterns, char x, char y)
        {
            foreach (char[] pat in patterns)
            {
                if (pat.GetLength(0) == 2 && pat[0] == x && pat[1] == y)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Metoda sprawdza, czy podany ciąg znaków można sprowadzić do ciągu pustego przez skreślanie zadanych wzorców.
        /// Zakładamy, że każdy wzorzec składa się z jednego lub dwóch znaków!
        /// </summary>
        /// <param name="sequence">Ciąg znaków</param>
        /// <param name="patterns">Lista wzorców</param>
        /// <param name="crossoutsNumber">Minimalna liczba skreśleń gwarantująca sukces lub int.MaxValue, jeżeli się nie da</param>
        /// <returns></returns>
        public bool Erasable(char[] sequence, char[][] patterns, out int crossoutsNumber)
        {
            int[,] canBe = new int[sequence.Length, sequence.Length];
            for (int i = 0; i < sequence.Length; i++)
                for (int o = 0; o < sequence.Length; o++)
                    canBe[i, o] = -1;
            int j = 0;
            while (j < sequence.Length)
            {
                for (int i = 0; i < sequence.Length; i += 1)
                {
                    if (j == 0)
                    {
                        if (comparePattern(patterns, sequence[i]))
                        {
                            canBe[i, i] = 1;
                        }
                    }
                    else if (j == 1)
                    {
                        if (i + 1 < sequence.Length && comparePattern(patterns, sequence[i], sequence[i + 1]))
                        {
                            canBe[i, i + 1] = 1;
                        }
                        else if (i + 1 < sequence.Length && comparePattern(patterns, sequence[i + 1]))
                        {
                            if (canBe[i, i] != -1)
                                canBe[i, i + 1] = 2;
                            if (canBe[i + 1, i] != -1)
                                canBe[i, i] = 2;
                        }
                    }
                    else
                    {
                        int cur = int.MaxValue;
                        if (sequence.Length <= i + j) break;
                        if (i + j - 2 >= 0 && canBe[i, i + j - 2] != -1)
                        {
                            if (comparePattern(patterns, sequence[i + j - 1], sequence[i + j]))
                                if (canBe[i, i + j - 2] + 1 < cur)
                                    cur = canBe[i, i + j - 2] + 1;
                        }
                        if (i + j - 1 >= 0 && canBe[i, i + j - 1] != -1)
                        {
                            if (comparePattern(patterns, sequence[i + j]))
                                if (canBe[i, i + j - 1] + 1 < cur)
                                    cur = canBe[i, i + j - 1] + 1;
                        }
                        if (i + j - 1 >= 0 && i + 1 < sequence.Length && canBe[i + 1, i + j - 1] != -1)
                        {
                            if (comparePattern(patterns, sequence[i], sequence[i + j]))
                                if (canBe[i + 1, i + j - 1] + 1 < cur)
                                    cur = canBe[i + 1, i + j - 1] + 1;
                        }
                        if (i + 1 < sequence.Length && canBe[i + 1, i + j] != -1)
                        {
                            if (comparePattern(patterns, sequence[i]))
                                if (canBe[i + 1, i + j] + 1 < cur)
                                    cur = canBe[i + 1, i + j] + 1;
                        }
                        if (i + 2 < sequence.Length && canBe[i + 2, i + j] != -1)
                        {
                            if (comparePattern(patterns, sequence[i], sequence[i + 1]))
                                if (canBe[i + 2, i + j] + 1 < cur)
                                    cur = canBe[i + 2, i + j] + 1;
                        }

                        if (cur != int.MaxValue || canBe[i, j + i] > cur)
                        {
                            canBe[i, j + i] = cur;
                            for (int k = i + j + 1; k < sequence.Length; k++)
                            {
                                if (canBe[j + i + 1, k] != -1)
                                {
                                    if (canBe[i, k] == -1 || canBe[i, k] > canBe[j + i + 1, k] + canBe[i, j + i])
                                        canBe[i, k] = canBe[j + i + 1, k] + canBe[i, j + i];
                                }
                            }
                            for (int k = i + j - 1; k >= 0; k--)
                            {
                                if (canBe[j + i - 1, k] != -1)
                                {
                                    if (canBe[i, k] == -1 || canBe[i, k] > canBe[j + i - 1, k] + canBe[i, j + i])
                                        canBe[i, k] = canBe[j + i - 1, k] + canBe[i, j + i];
                                }
                            }
                        }
                    }
                }
                j++;
            }

            if (canBe[0, sequence.Length - 1] == -1)
            {
                crossoutsNumber = int.MaxValue;
                return false;
            }
            else
                crossoutsNumber = canBe[0, sequence.Length - 1];

            return true;
        }

        /// <summary>
        /// Metoda sprawdza, jaka jest minimalna długość ciągu, który można uzyskać z podanego poprzez skreślanie zadanych wzorców.
        /// Zakładamy, że każdy wzorzec składa się z jednego lub dwóch znaków!
        /// </summary>
        /// <param name="sequence">Ciąg znaków</param>
        /// <param name="patterns">Lista wzorców</param>
        /// <returns></returns>
        public int MinimumRemainder(char[] sequence, char[][] patterns)
        {
            int[,] canBe = new int[sequence.Length, sequence.Length];
            for (int i = 0; i < sequence.Length; i++)
                for (int o = 0; o < sequence.Length; o++)
                    canBe[i, o] = -1;
            int j = 0;
            while (j < sequence.Length)
            {
                for (int i = 0; i < sequence.Length; i += 1)
                {
                    if (j == 0)
                    {
                        if (comparePattern(patterns, sequence[i]))
                        {
                            canBe[i, i] = 1;
                        }
                    }
                    else if (j == 1)
                    {
                        if (i + 1 < sequence.Length && comparePattern(patterns, sequence[i], sequence[i + 1]))
                        {
                            canBe[i, i + 1] = 1;
                        }
                        else if (i + 1 < sequence.Length && comparePattern(patterns, sequence[i + 1]))
                        {
                            if (canBe[i, i] != -1)
                                canBe[i, i + 1] = 2;
                            if (canBe[i + 1, i] != -1)
                                canBe[i, i] = 2;
                        }
                    }
                    else
                    {
                        int cur = int.MaxValue;
                        if (sequence.Length <= i + j) break;
                        if (i + j - 2 >= 0 && canBe[i, i + j - 2] != -1)
                        {
                            if (comparePattern(patterns, sequence[i + j - 1], sequence[i + j]))
                                if (canBe[i, i + j - 2] + 1 < cur)
                                    cur = canBe[i, i + j - 2] + 1;
                        }
                        if (i + j - 1 >= 0 && canBe[i, i + j - 1] != -1)
                        {
                            if (comparePattern(patterns, sequence[i + j]))
                                if (canBe[i, i + j - 1] + 1 < cur)
                                    cur = canBe[i, i + j - 1] + 1;
                        }
                        if (i + j - 1 >= 0 && i + 1 < sequence.Length && canBe[i + 1, i + j - 1] != -1)
                        {
                            if (comparePattern(patterns, sequence[i], sequence[i + j]))
                                if (canBe[i + 1, i + j - 1] + 1 < cur)
                                    cur = canBe[i + 1, i + j - 1] + 1;
                        }
                        if (i + 1 < sequence.Length && canBe[i + 1, i + j] != -1)
                        {
                            if (comparePattern(patterns, sequence[i]))
                                if (canBe[i + 1, i + j] + 1 < cur)
                                    cur = canBe[i + 1, i + j] + 1;
                        }
                        if (i + 2 < sequence.Length && canBe[i + 2, i + j] != -1)
                        {
                            if (comparePattern(patterns, sequence[i], sequence[i + 1]))
                                if (canBe[i + 2, i + j] + 1 < cur)
                                    cur = canBe[i + 2, i + j] + 1;
                        }

                        if (cur != int.MaxValue || canBe[i, j + i] > cur)
                        {
                            canBe[i, j + i] = cur;
                            for (int k = i + j + 1; k < sequence.Length; k++)
                            {
                                if (canBe[j + i + 1, k] != -1)
                                {
                                    if (canBe[i, k] == -1 || canBe[i, k] > canBe[j + i + 1, k] + canBe[i, j + i])
                                        canBe[i, k] = canBe[j + i + 1, k] + canBe[i, j + i];
                                }
                            }
                            for (int k = i + j - 1; k >= 0; k--)
                            {
                                if (canBe[j + i - 1, k] != -1)
                                {
                                    if (canBe[i, k] == -1 || canBe[i, k] > canBe[j + i - 1, k] + canBe[i, j + i])
                                        canBe[i, k] = canBe[j + i - 1, k] + canBe[i, j + i];
                                }
                            }
                        }
                    }
                }
                j++;
            }

            // printing whole matrix
            //for (int i = -1; i < sequence.Length; i++)
            //{
            //    if (i == -1)
            //    {
            //        Console.Write("   ");
            //        for (int k = 0; k < sequence.Length; k++)
            //            Console.Write("" + String.Format("{0,3}", k) + "");
            //    }
            //    else
            //        for (int o = -1; o < sequence.Length; o++)
            //        {
            //            if (o == -1)
            //                Console.Write(String.Format("{0,3}", i) + "");
            //            else
            //                Console.Write(String.Format("{0,3}", canBe[i, o]) + "");
            //        }
            //    Console.WriteLine();
            //}

            if (canBe[0, sequence.Length - 1] != -1) return 0;

            int howManyCovered = 0;
            for (int i = 0; i < sequence.Length; i++)
            {
                for (j = sequence.Length-1; j >=0;)
                {
                    if (i >= sequence.Length) break;
                    if (canBe[i, j] != -1)
                    {
                        howManyCovered += j - i + 1;
                        i = j + 1;
                        j = sequence.Length-1;
                    }
                    else
                        j--;
                }
            }
            return sequence.Length - howManyCovered;
        }
    }
}
