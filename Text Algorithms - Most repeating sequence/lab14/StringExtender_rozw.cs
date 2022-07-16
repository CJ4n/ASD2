using System;
using System.Text;

namespace Lab15
{
    public static class stringExtender
    {
        /// <summary>
        /// Metoda zwraca okres słowa s, tzn. najmniejszą dodatnią liczbę p taką, że s[i]=s[i+p] dla każdego i od 0 do |s|-p-1.
        /// 
        /// Metoda musi działać w czasie O(|s|)
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static public int Period(this string s)
        {
            int[] P = makeP(s);
            return s.Length - P[s.Length];
        }

        /// <summary>
        /// Metoda wyznacza największą potęgę zawartą w słowie s.
        /// 
        /// Jeżeli x jest słowem, wówczas przez k-tą potęgę słowa x rozumiemy k-krotne powtórzenie słowa x
        /// (na przykład xyzxyzxyz to trzecia potęga słowa xyz).
        /// 
        /// Należy zwrócić największe k takie, że k-ta potęga jakiegoś słowa jest zawarta w s jako spójny podciąg.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="startIndex">Pierwszy indeks fragmentu zawierającego znalezioną potęgę</param>
        /// <param name="endIndex">Pierwszy indeks po fragmencie zawierającym znalezioną potęgę</param>
        /// <returns></returns>
        static public int MaxPower(this string s, out int startIndex, out int endIndex)
        {
            if (s == null || s.Length == 0)
            {
                startIndex = endIndex = 0;
                return 0;
            }
            string scpy = (string)s.Clone();
            startIndex = 0;
            endIndex = 1;
            int maxPow = 1;

            for(int i=0;i<s.Length;i++)
            {
                // scpy to podsłowo s[i..n-1], gdzie n = s.Length

                // wyznaczamy tablicę P dla słowa scpy
                int[] Pi = makeP(scpy);
                for (int j = 2; j <= scpy.Length; j++)
                {
                    // Ten warunek sprawdza, czy podsłowo s[i..i+j-1] jest potęgą większą niż maxPow
                    if(j%(j - Pi[j])==0 && j/(j-Pi[j])>maxPow)
                    {
                        maxPow = j / (j - Pi[j]);
                        startIndex = i;
                        endIndex = i + j;
                    }
                }
                scpy=scpy.Remove(0, 1);
            }

            return maxPow;
        }

        /// <summary>
        /// P[i] to długość najdłuższego prefikso-sufiksu słowa s[0..i-1]
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        static int[] makeP(string s)
        {
            int[] P = new int[s.Length + 1];
            int t = 0;

            for(int i=2;i<=s.Length;i++)
            {
                while (s[t] != s[i-1] && t > 0)
                    t = P[t];
                P[i] = t;
                if (s[i - 1] == s[t])
                    P[i] = ++t;
            }
            return P;
        }
    }
}
