using System;
using System.Collections.Generic;
using System.Linq;

namespace ASD_lab14
{

    public class Substrings : MarshalByRefObject
    {
        /// <summary>
        /// Zadanie pierwsze, w którym musimy znaleźć najdłuższy fragment tekstu powtarzający się przynajmniej dwukrotnie
        /// </summary>
        /// <param name="text">Pierwszy string</param>
        /// <returns>
        /// length: długość najdłuższego fragmentu powtarzającego się przynajmniej 2 razy <br />
        /// longestCommonSubstring: najdłuższy fragment powtarzający się przynajmniej 2 razy
        /// </returns>
        public (int length, string longestSubstring) StageOne(string text)
        {
            int n = text.Length;
            int[,] dp = new int[n + 1, n + 1];
            int len = 0;
            int indexLast = 0;
            for (int i = 0; i < n; i++)
                for (int j = i; j < n; j++)
                {
                    if (text[i] == text[j] && dp[i, j] < j - i)
                    {
                        dp[i + 1, j + 1] = dp[i, j] + 1;
                        if (dp[i + 1, j + 1] > len)
                        {
                            len = dp[i + 1, j + 1];
                            indexLast = i + 1;
                        }
                    }
                }

            return (len, text.Substring(indexLast - len, len));
        }

        /// <summary>
        /// Zadanie drugie, w którym musimy znaleźć dwa najdłuższe powtarzające się fragmenty w dwóch stringach
        /// </summary>
        /// <param name="x">Pierwszy string</param>
        /// <param name="y">Drugi string</param>
        /// <returns>
        /// length: długość najdłuższego wspólnego fragmentu <br />
        /// longestCommonSubstring: najdłuższy wspólny fragment
        /// </returns>
        public (int length, string longestCommonSubstring) StageTwo(string x, string y)
        {
            int n = x.Length;
            int m = y.Length;
            int[,] dp = new int[n + 1, m + 1];
            int len = 0; // długość najdłuższego substringa
            int indexLast = 0; // index ostatniej pasującej litery w max substringu

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    if (x[i] == y[j])
                    {
                        dp[i + 1, j + 1] = dp[i, j] + 1;
                        if (dp[i + 1, j + 1] > len)
                        {
                            len = dp[i + 1, j + 1];
                            indexLast = i + 1;
                        }
                    }
                }
            }
            
            return (len, x.Substring(indexLast - len, len));
        }
    }
}
