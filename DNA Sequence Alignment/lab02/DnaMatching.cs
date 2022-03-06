using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Lab2
{
    public class DnaMatching : MarshalByRefObject
    {

        /// <summary>
        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
        /// </summary>
        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
        {
            const int matchValue = 1;
            const int mismatchValue = -3;
            const int gapValue = -2;
            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
            maxValue[0, 0] = 0;
            int i, j;
            for (i = 1; i <= seq1.Length; i++)
            {
                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
            }
            for (i = 1; i <= seq2.Length; i++)
            {
                maxValue[0, i] = maxValue[0, i - 1] - 2;
            }

            for (i = 1; i <= seq1.Length; i++)
            {
                for (j = 1; j <= seq2.Length; j++)
                {
                    int a;
                    if (seq1[i - 1] == seq2[j - 1])
                        a = matchValue + maxValue[i - 1, j - 1];
                    else
                        a = mismatchValue + maxValue[i - 1, j - 1];
                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
                    maxValue[i, j] = max;
                }
            }

            string out1 = "";
            string out2 = "";
            i = seq1.Length;
            j = seq2.Length;

            while (i != 0 || j != 0)
            {
                int up, left;
                if (j - 1 < 0)
                    up = int.MinValue;
                else
                    up = maxValue[i, j - 1] + gapValue;
                if (i - 1 < 0)
                    left = int.MinValue;
                else
                    left = maxValue[i - 1, j] + gapValue;
                int a;
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (seq1[i - 1] == seq2[j - 1])
                        a = maxValue[i - 1, j - 1] + 1;
                    else
                        a = maxValue[i - 1, j - 1] - 3;
                }
                else a = int.MinValue;
                int max = Math.Max(Math.Max(up, left), a);

                if (max == a)
                {
                    out1 += seq1[i - 1];
                    out2 += seq2[j - 1];
                    j--;
                    i--;
                }
                else if (max == up)
                {
                    out1 += '-';
                    out2 += seq2[j - 1];
                    j--;
                }
                else
                {
                    out1 += seq1[i - 1];
                    out2 += '-';
                    i--;
                }
            }

            int val = maxValue[seq1.Length, seq2.Length];
            char[] c1 = out1.ToCharArray();
            char[] c2 = out2.ToCharArray();
            Array.Reverse(c1);
            Array.Reverse(c2);
            out1 = new string(c1);
            out2 = new string(c2);
            return (out1, out2, val);
        }


        /// <summary>
        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
        /// </summary>
        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
        {
            const int matchValue = 1;
            const int mismatchValue = -3;
            const int gapStartValue = -5;
            const int gapContinuationValue = -2;
            int[,] opt = new int[1 + seq1.Length, 1 + seq2.Length]; // ogólnie najlepsze rozwiązanie w [i,j]
            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length]; // najlpesze rozwiązanie  w [i,j] takie, że pierwszy wyrazm ma '-' na końcy, a drugi literke
            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length]; // najlpesze rozwiązanie  w [i,j] takie, że pierwszy wyrazm ma literke na końcy, a drugi '-'
            // informacja czy optymale rozwiązanie w [i,j] ma '-' w pierszym wyrazie
            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |-|
                                                                         // |C|
            // informacja czy optymale rozwiązanie w [i,j] ma '-' w drugim wyrazie
            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |C|
                                                                         // |-|
            opt[0, 0] = 0;
            w1[0, 0] = 0;
            w2[0, 0] = 0;
            int i, j;
            for (i = 1; i <= seq1.Length; i++)// wypełniamy tylko pierwszy górny wiersz tablicy
            {
                isGap2[i, 0] = true;
                if (i == 1)
                {
                    opt[i, 0] = gapStartValue;
                    w1[i, 0] = -(int.MaxValue - 10000);
                    w2[i, 0] = gapStartValue;
                }
                else
                {
                    opt[i, 0] = opt[i - 1, 0] + gapContinuationValue;
                    w1[i, 0] = -(int.MaxValue - 10000);
                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
                }
            }
            for (i = 1; i <= seq2.Length; i++) // wypełniamy tylko pierwszą lewą kolumne tablicy
            {
                isGap1[0, i] = true;
                if (i == 1)
                {
                    opt[0, i] = gapStartValue;
                    w1[0, i] = gapStartValue;
                    w2[0, i] = -(int.MaxValue - 10000);
                }
                else
                {
                    opt[0, i] = opt[0, i - 1] + gapContinuationValue;
                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
                    w2[0, i] = -(int.MaxValue - 10000);
                }
            }

            for (i = 1; i <= seq1.Length; i++)
                for (j = 1; j <= seq2.Length; j++)
                {
                    // znajdujemy najlepsze rozwiązanie idąc od lewej, i wybieramy najlepsze rozwiązanie stąd jako w2[i,j]
                    int left_w1, left_w2, left_opt, left_max;
                    if (isGap2[i - 1, j])
                        left_opt = opt[i - 1, j] + gapContinuationValue;
                    else
                        left_opt = opt[i - 1, j] + gapStartValue;
                    left_w1 = w1[i - 1, j] + gapStartValue;
                    left_w2 = w2[i - 1, j] + gapContinuationValue;
                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
                    w2[i, j] = left_max;

                    // znajdujemy najlepsze rozwiązanie idąc od góry, i wybieramy najlepsze rozwiązanie stąd jako w1[i,j]
                    int up_w1, up_w2, up_opt, up_max;
                    if (isGap1[i, j - 1])
                        up_opt = opt[i, j - 1] + gapContinuationValue;
                    else
                        up_opt = opt[i, j - 1] + gapStartValue;
                    up_w1 = w1[i, j - 1] + gapContinuationValue;
                    up_w2 = w2[i, j - 1] + gapStartValue;
                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
                    w1[i, j] = up_max;

                    // znajdujemy najlepsze rozwiązanie idąc na ukos
                    int accros_w1, accros_w2, accros_opt, accros_max;
                    if (seq1[i - 1] == seq2[j - 1])
                    {
                        accros_opt = opt[i - 1, j - 1] + matchValue;
                        accros_w1 = w1[i - 1, j - 1] + matchValue;
                        accros_w2 = w2[i - 1, j - 1] + matchValue;
                    }
                    else
                    {
                        accros_opt = opt[i - 1, j - 1] + mismatchValue;
                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
                    }
                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
                    // wybieramy ogołnie najlepsze rozwiązanie z tych 9 opcji i zapisujemy je jako opt[i,j]
                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
                    opt[i, j] = global_opt;
                    // uktualizujemy tablice isGap1/isGap2 w zależności, który(jeśli w ogóle) wyraz kończy się '-'
                    if (global_opt == left_max)
                        isGap2[i, j] = true;
                    else if (global_opt == up_max)
                        isGap1[i, j] = true;


                }
            string s1 = "", s2 = "";
            i = seq1.Length;
            j = seq2.Length;

            // rekonstrujemy drogę od tyłu w celu odtworzenia wyrazów
            // informacja, którą talice opt/w1/w2 wykorzystaliśmy do stworzenia wyrazu w poprzednim kroku
            int prev = 0;// 0 - opt, 1 - w1, 2 - w2
            while (i != 0 || j != 0)
            {
                if (prev == 1)// w1
                {
                    int uw1, uw2, uopt;
                    uw2 = w2[i, j - 1] + gapStartValue;
                    uw1 = w1[i, j - 1] + gapContinuationValue;
                    if (isGap1[i, j - 1])
                        uopt = opt[i, j - 1] + gapContinuationValue;
                    else
                        uopt = opt[i, j - 1] + gapStartValue;

                    int max = Math.Max(Math.Max(uw1, uw2), uopt);
                    if (max == uw1)
                        prev = 1;
                    else if (max == uw2)
                        prev = 2;
                    else
                        prev = 0;
                    s2 += seq2[j - 1];
                    j--;
                    s1 += "-";
                }
                else if (prev == 2) //w2
                {
                    int lw1, lw2, lopt;
                    lw1 = w1[i - 1, j] + gapStartValue;
                    lw2 = w2[i - 1, j] + gapContinuationValue;
                    if (isGap2[i - 1, j])
                        lopt = opt[i - 1, j] + gapContinuationValue;
                    else
                        lopt = opt[i - 1, j] + gapStartValue;

                    int max = Math.Max(Math.Max(lw1, lw2), lopt);
                    if (max == lw1)
                        prev = 1;
                    else if (max == lw2)
                        prev = 2;
                    else
                        prev = 0;
                    s1 += seq1[i - 1];
                    s2 += "-";
                    i--;
                }

                else //if (prev == 0) // opt
                {
                    int left_w1, left_w2, left_opt, left_max;
                    if (i - 1 >= 0)
                    {
                        if (isGap2[i - 1, j])
                            left_opt = opt[i - 1, j] + gapContinuationValue;
                        else
                            left_opt = opt[i - 1, j] + gapStartValue;
                        left_w1 = w1[i - 1, j] + gapStartValue;
                        left_w2 = w2[i - 1, j] + gapContinuationValue;
                        left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
                    }
                    else
                    {
                        left_opt = int.MinValue;
                        left_w1 = int.MinValue;
                        left_w2 = int.MinValue;
                        left_max = int.MinValue;
                    }
                    int up_w1, up_w2, up_opt, up_max;
                    if (j - 1 >= 0)
                    {
                        if (isGap1[i, j - 1])
                            up_opt = opt[i, j - 1] + gapContinuationValue;
                        else
                            up_opt = opt[i, j - 1] + gapStartValue;
                        up_w1 = w1[i, j - 1] + gapContinuationValue;
                        up_w2 = w2[i, j - 1] + gapStartValue;
                        up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
                    }
                    else
                    {
                        up_opt = int.MinValue;
                        up_w1 = int.MinValue;
                        up_w2 = int.MinValue;
                        up_max = int.MinValue;
                    }
                    int accros_w1, accros_w2, accros_opt, accros_max;
                    if (i - 1 >= 0 && j - 1 >= 0)
                    {
                        if (seq1[i - 1] == seq2[j - 1])
                        {
                            accros_opt = opt[i - 1, j - 1] + matchValue;
                            accros_w1 = w1[i - 1, j - 1] + matchValue;
                            accros_w2 = w2[i - 1, j - 1] + matchValue;
                        }
                        else
                        {
                            accros_opt = opt[i - 1, j - 1] + mismatchValue;
                            accros_w1 = w1[i - 1, j - 1] + mismatchValue;
                            accros_w2 = w2[i - 1, j - 1] + mismatchValue;
                        }
                        accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
                    }
                    else
                    {
                        accros_opt = int.MinValue;
                        accros_w1 = int.MinValue;
                        accros_w2 = int.MinValue;
                        accros_max = int.MinValue;
                    }
                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);

                    if (global_opt == left_max)
                    {
                        if (global_opt == left_w1)
                            prev = 1;
                        else if (global_opt == left_w2)
                            prev = 2;
                        else if (global_opt == left_opt)
                            prev = 0;

                        s1 += seq1[i - 1];
                        s2 += "-";
                        i--;

                    }
                    else if (global_opt == up_max)
                    {

                        if (global_opt == up_w1)
                            prev = 1;
                        else if (global_opt == up_w2)
                            prev = 2;
                        else if (global_opt == up_opt)
                            prev = 0;
                        s1 += "-";
                        s2 += seq2[j - 1];
                        j--;

                    }
                    else if (global_opt == accros_max)
                    {
                        if (global_opt == up_w1)
                            prev = 1;
                        else if (global_opt == up_w2)
                            prev = 2;
                        else if (global_opt == up_opt)
                            prev = 0;
                        s1 += seq1[i - 1];
                        s2 += seq2[j - 1];
                        i--;
                        j--;
                    }
                }
            }

            char[] c1 = s1.ToCharArray();
            char[] c2 = s2.ToCharArray();
            Array.Reverse(c1);
            Array.Reverse(c2);
            s1 = new string(c1);
            s2 = new string(c2);
            int val = opt[seq1.Length, seq2.Length];
            return (s1, s2, val);

        }
    }
}