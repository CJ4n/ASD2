/////////////////////////dzialaaaaaaaaa!!!!!!!!!!!!!!!!!!

//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Linq;

//namespace Lab2
//{
//    public class DnaMatching : MarshalByRefObject
//    {

//        /// <summary>
//        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
//        /// </summary>
//        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

//        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
//        {
//            const int matchValue = 1;
//            const int mismatchValue = -3;
//            const int gapValue = -2;
//            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
//            maxValue[0, 0] = 0;
//            int i, j;
//            for (i = 1; i <= seq1.Length; i++)
//            {
//                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
//            }
//            for (i = 1; i <= seq2.Length; i++)
//            {
//                maxValue[0, i] = maxValue[0, i - 1] - 2;
//            }

//            for (i = 1; i <= seq1.Length; i++)
//            {
//                for (j = 1; j <= seq2.Length; j++)
//                {
//                    int a;
//                    if (seq1[i - 1] == seq2[j - 1])
//                        a = matchValue + maxValue[i - 1, j - 1];
//                    else
//                        a = mismatchValue + maxValue[i - 1, j - 1];
//                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
//                    maxValue[i, j] = max;
//                }
//            }

//            string out1 = "";
//            string out2 = "";
//            i = seq1.Length;
//            j = seq2.Length;

//            while (i != 0 || j != 0)
//            {
//                int up, left;
//                if (j - 1 < 0)
//                    up = -int.MaxValue;
//                else
//                    up = maxValue[i, j - 1] + gapValue;
//                if (i - 1 < 0)
//                    left = -int.MaxValue;
//                else
//                    left = maxValue[i - 1, j] + gapValue;
//                int a;
//                if (i - 1 >= 0 && j - 1 >= 0)
//                {
//                    if (seq1[i - 1] == seq2[j - 1])
//                        a = maxValue[i - 1, j - 1] + 1;
//                    else
//                        a = maxValue[i - 1, j - 1] - 3;
//                }
//                else a = -int.MaxValue;
//                int max = Math.Max(Math.Max(up, left), a);

//                if (max == a)
//                {
//                    out1 += seq1[i - 1];
//                    out2 += seq2[j - 1];
//                    j--;
//                    i--;
//                }
//                else if (max == up)
//                {
//                    out1 += '-';
//                    out2 += seq2[j - 1];
//                    j--;
//                }
//                else
//                {
//                    out1 += seq1[i - 1];
//                    out2 += '-';
//                    i--;
//                }
//            }

//            int val = maxValue[seq1.Length, seq2.Length];
//            char[] c1 = out1.ToCharArray();
//            char[] c2 = out2.ToCharArray();
//            Array.Reverse(c1);
//            Array.Reverse(c2);
//            out1 = new string(c1);
//            out2 = new string(c2);
//            return (out1, out2, val);
//        }


//        /// <summary>
//        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
//        /// </summary>
//        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
//        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
//        {
//            const int matchValue = 1;
//            const int mismatchValue = -3;
//            const int gapStartValue = -5;
//            const int gapContinuationValue = -2;
//            int[,] opt = new int[1 + seq1.Length, 1 + seq2.Length];
//            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length];
//            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length];
//            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |-|
//                                                                         // |C|

//            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |C|
//                                                                         // |-|
//            opt[0, 0] = 0;
//            w1[0, 0] = 0;
//            w2[0, 0] = 0;
//            int i, j;

//            //int[,] where = new int[seq1.Length + 1, seq2.Length + 1];
//            //where[0, 0] = 0;
//            for (i = 1; i <= seq1.Length; i++)
//            {
//                //where[i, 0] = 0;
//                isGap2[i, 0] = true;
//                if (i == 1)
//                {
//                    opt[i, 0] = gapStartValue;
//                    w1[i, 0] = -(int.MaxValue - 10000);
//                    w2[i, 0] = gapStartValue;
//                }
//                else
//                {
//                    opt[i, 0] = opt[i - 1, 0] + gapContinuationValue;
//                    w1[i, 0] = -(int.MaxValue - 10000);
//                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
//                }
//            }
//            for (i = 1; i <= seq2.Length; i++)
//            {
//                //where[0, i] = 0;
//                isGap1[0, i] = true;

//                if (i == 1)
//                {
//                    opt[0, i] = gapStartValue;
//                    w1[0, i] = gapStartValue;
//                    w2[0, i] = -(int.MaxValue - 10000);
//                }
//                else
//                {
//                    opt[0, i] = opt[0, i - 1] + gapContinuationValue;
//                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
//                    w2[0, i] = -(int.MaxValue - 10000);
//                }
//            }

//            for (i = 1; i <= seq1.Length; i++)
//                for (j = 1; j <= seq2.Length; j++)
//                {
//                    int left_w1, left_w2, left_opt, left_max;
//                    if (isGap2[i - 1, j])
//                        left_opt = opt[i - 1, j] + gapContinuationValue;
//                    else
//                        left_opt = opt[i - 1, j] + gapStartValue;
//                    left_w1 = w1[i - 1, j] + gapStartValue;
//                    left_w2 = w2[i - 1, j] + gapContinuationValue;
//                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//                    w2[i, j] = left_max;

//                    int up_w1, up_w2, up_opt, up_max;
//                    if (isGap1[i, j - 1])
//                        up_opt = opt[i, j - 1] + gapContinuationValue;
//                    else
//                        up_opt = opt[i, j - 1] + gapStartValue;
//                    up_w1 = w1[i, j - 1] + gapContinuationValue;
//                    up_w2 = w2[i, j - 1] + gapStartValue;
//                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//                    w1[i, j] = up_max;

//                    int accros_w1, accros_w2, accros_opt, accros_max;

//                    if (seq1[i - 1] == seq2[j - 1])
//                    {
//                        accros_opt = opt[i - 1, j - 1] + matchValue;
//                        accros_w1 = w1[i - 1, j - 1] + matchValue;
//                        accros_w2 = w2[i - 1, j - 1] + matchValue;
//                    }
//                    else
//                    {
//                        accros_opt = opt[i - 1, j - 1] + mismatchValue;
//                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//                    }
//                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

//                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
//                    opt[i, j] = global_opt;
//                    if (global_opt == left_max)
//                    {
//                        //if (global_opt == left_w1)
//                        //    where[i, j] = 1;
//                        //else if (global_opt == left_w2)
//                        //    where[i, j] = 2;
//                        //else if (global_opt == left_opt)
//                        //    where[i, j] = 0;

//                        isGap2[i, j] = true;
//                    }
//                    else if (global_opt == up_max)
//                    {
//                        isGap1[i, j] = true;
//                        //if (global_opt == up_w1)
//                        //    where[i, j] = 1;
//                        //else if (global_opt == up_w2)
//                        //    where[i, j] = 2;
//                        //else if (global_opt == up_opt)
//                        //    where[i, j] = 0;
//                    }
//                    //else if (global_opt == accros_max)
//                    //{
//                    //    //if (global_opt == up_w1)
//                    //    //    where[i, j] = 1;
//                    //    //else if (global_opt == up_w2)
//                    //    //    where[i, j] = 2;
//                    //    //else if (global_opt == up_opt)
//                    //    //    where[i, j] = 0;
//                    //}
//                }
//            string s1 = "", s2 = "";
//            i = seq1.Length;
//            j = seq2.Length;

//            int prev = 0;// 0 - opt, 1 - w1, 2 - w2
//            while (i != 0 || j != 0)
//            {
//                if (prev == 1)
//                {
//                    int uw1, uw2, uopt;
//                    uw2 = w2[i, j - 1] + gapStartValue;
//                    uw1 = w1[i, j - 1] + gapContinuationValue;
//                    if (isGap1[i, j - 1])
//                        uopt = opt[i, j - 1] + gapContinuationValue;
//                    else
//                        uopt = opt[i, j - 1] + gapStartValue;

//                    int max = Math.Max(Math.Max(uw1, uw2), uopt);
//                    if (max == uw1)
//                        prev = 1;
//                    else if (max == uw2)
//                        prev = 2;
//                    else
//                        prev = 0;
//                    s2 += seq2[j - 1];
//                    j--;
//                    s1 += "-";
//                }
//                else if (prev == 2)
//                {
//                    int lw1, lw2, lopt;
//                    lw1 = w1[i - 1, j] + gapStartValue;
//                    lw2 = w2[i - 1, j] + gapContinuationValue;
//                    if (isGap2[i - 1, j])
//                        lopt = opt[i - 1, j] + gapContinuationValue;
//                    else
//                        lopt = opt[i - 1, j] + gapStartValue;

//                    int max = Math.Max(Math.Max(lw1, lw2), lopt);
//                    if (max == lw1)
//                        prev = 1;
//                    else if (max == lw2)
//                        prev = 2;
//                    else
//                        prev = 0;
//                    s1 += seq1[i - 1];
//                    s2 += "-";
//                    i--;
//                }

//                else if (prev == 0)
//                {
//                    int left_w1, left_w2, left_opt, left_max;
//                    if (i - 1 >= 0)
//                    {
//                        if (isGap2[i - 1, j])
//                            left_opt = opt[i - 1, j] + gapContinuationValue;
//                        else
//                            left_opt = opt[i - 1, j] + gapStartValue;
//                        left_w1 = w1[i - 1, j] + gapStartValue;
//                        left_w2 = w2[i - 1, j] + gapContinuationValue;
//                        left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//                    }
//                    else
//                    {
//                        left_opt = int.MinValue;
//                        left_w1 = int.MinValue;
//                        left_w2 = int.MinValue;
//                        left_max = int.MinValue;
//                    }
//                    int up_w1, up_w2, up_opt, up_max;
//                    if (j - 1 >= 0)
//                    {
//                        if (isGap1[i, j - 1])
//                            up_opt = opt[i, j - 1] + gapContinuationValue;
//                        else
//                            up_opt = opt[i, j - 1] + gapStartValue;
//                        up_w1 = w1[i, j - 1] + gapContinuationValue;
//                        up_w2 = w2[i, j - 1] + gapStartValue;
//                        up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//                    }
//                    else
//                    {
//                        up_opt = int.MinValue;
//                        up_w1 = int.MinValue;
//                        up_w2 = int.MinValue;
//                        up_max = int.MinValue;
//                    }
//                    int accros_w1, accros_w2, accros_opt, accros_max;
//                    if (i - 1 >= 0 && j - 1 >= 0)
//                    {
//                        if (seq1[i - 1] == seq2[j - 1])
//                        {
//                            accros_opt = opt[i - 1, j - 1] + matchValue;
//                            accros_w1 = w1[i - 1, j - 1] + matchValue;
//                            accros_w2 = w2[i - 1, j - 1] + matchValue;
//                        }
//                        else
//                        {
//                            accros_opt = opt[i - 1, j - 1] + mismatchValue;
//                            accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//                            accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//                        }
//                        accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
//                    }
//                    else
//                    {
//                        accros_opt = int.MinValue;
//                        accros_w1 = int.MinValue;
//                        accros_w2 = int.MinValue;
//                        accros_max = int.MinValue;
//                    }
//                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);

//                    if (global_opt == left_max)
//                    {
//                        if (global_opt == left_w1)
//                            prev = 1;
//                        else if (global_opt == left_w2)
//                            prev = 2;
//                        else if (global_opt == left_opt)
//                            prev = 0;

//                        s1 += seq1[i - 1];
//                        s2 += "-";
//                        i--;

//                    }
//                    else if (global_opt == up_max)
//                    {

//                        if (global_opt == up_w1)
//                            prev = 1;
//                        else if (global_opt == up_w2)
//                            prev = 2;
//                        else if (global_opt == up_opt)
//                            prev = 0;
//                        s1 += "-";
//                        s2 += seq2[j - 1];
//                        j--;

//                    }
//                    else if (global_opt == accros_max)
//                    {
//                        if (global_opt == up_w1)
//                            prev = 1;
//                        else if (global_opt == up_w2)
//                            prev = 2;
//                        else if (global_opt == up_opt)
//                            prev = 0;
//                        s1 += seq1[i - 1];
//                        s2 += seq2[j - 1];
//                        i--;
//                        j--;
//                    }
//                }
//            }

//            char[] c1 = s1.ToCharArray();
//            char[] c2 = s2.ToCharArray();
//            Array.Reverse(c1);
//            Array.Reverse(c2);
//            s1 = new string(c1);
//            s2 = new string(c2);
//            int val = opt[seq1.Length, seq2.Length];
//            return (s1, s2, val);

//        }
//    }
//}


/////////////////////////dzialaaaaaaaaa!!!!!!!!!!!!!!!!!!



/////////////////////////////fonale 3
///////
////using System;
////using System.Collections.Generic;
////using System.Text;
////using System.Linq;

////namespace Lab2
////{
////    public class DnaMatching : MarshalByRefObject
////    {

////        /// <summary>
////        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
////        /// </summary>
////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
////        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
////        {
////            const int matchValue = 1;
////            const int mismatchValue = -3;
////            const int gapValue = -2;
////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
////            maxValue[0, 0] = 0;
////            int i, j;
////            for (i = 1; i <= seq1.Length; i++)
////            {
////                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
////            }
////            for (i = 1; i <= seq2.Length; i++)
////            {
////                maxValue[0, i] = maxValue[0, i - 1] - 2;
////            }

////            for (i = 1; i <= seq1.Length; i++)
////            {
////                for (j = 1; j <= seq2.Length; j++)
////                {
////                    int a;
////                    if (seq1[i - 1] == seq2[j - 1])
////                        a = matchValue + maxValue[i - 1, j - 1];
////                    else
////                        a = mismatchValue + maxValue[i - 1, j - 1];
////                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
////                    maxValue[i, j] = max;
////                }
////            }

////            string out1 = "";
////            string out2 = "";
////            i = seq1.Length;
////            j = seq2.Length;

////            while (i != 0 || j != 0)
////            {
////                int up, left;
////                if (j - 1 < 0)
////                    up = -int.MaxValue;
////                else
////                    up = maxValue[i, j - 1] + gapValue;
////                if (i - 1 < 0)
////                    left = -int.MaxValue;
////                else
////                    left = maxValue[i - 1, j] + gapValue;
////                int a;
////                if (i - 1 >= 0 && j - 1 >= 0)
////                {
////                    if (seq1[i - 1] == seq2[j - 1])
////                        a = maxValue[i - 1, j - 1] + 1;
////                    else
////                        a = maxValue[i - 1, j - 1] - 3;
////                }
////                else a = -int.MaxValue;
////                int max = Math.Max(Math.Max(up, left), a);

////                if (max == a)
////                {
////                    out1 += seq1[i - 1];
////                    out2 += seq2[j - 1];
////                    j--;
////                    i--;
////                }
////                else if (max == up)
////                {
////                    out1 += '-';
////                    out2 += seq2[j - 1];
////                    j--;
////                }
////                else
////                {
////                    out1 += seq1[i - 1];
////                    out2 += '-';
////                    i--;
////                }
////            }

////            int val = maxValue[seq1.Length, seq2.Length];
////            char[] c1 = out1.ToCharArray();
////            char[] c2 = out2.ToCharArray();
////            Array.Reverse(c1);
////            Array.Reverse(c2);
////            out1 = new string(c1);
////            out2 = new string(c2);
////            return (out1, out2, val);
////        }


////        /// <summary>
////        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
////        /// </summary>
////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
////        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
////        {
////            const int matchValue = 1;
////            const int mismatchValue = -3;
////            const int gapStartValue = -5;
////            const int gapContinuationValue = -2;
////            int[,] opt = new int[1 + seq1.Length, 1 + seq2.Length];
////            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length];
////            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length];
////            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |-|
////                                                                         // |C|

////            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |C|
////                                                                         // |-|
////            opt[0, 0] = 0;
////            w1[0, 0] = 0;
////            w2[0, 0] = 0;
////            int i, j;

////            int[,] where = new int[seq1.Length + 1, seq2.Length + 1];
////            //0 - accros, 1 - left, 2 - up :w1
////            //3 - accros, 4 - left, 5 - up :w2
////            //6 - accros, 7 - left, 9 - up :w1
////            where[0, 0] = 0;
////            //string[,] str1 = new string[seq1.Length + 1, seq2.Length + 1];
////            //string[,] str2 = new string[seq1.Length + 1, seq2.Length + 1];
////            for (i = 1; i <= seq1.Length; i++)
////            {
////                where[i, 0] = 0;
////                //str1[i, 0] = str1[i-1, 0]+seq1[i-1];
////                //str2[i, 0] = str2[i - 1, 0] + "-";
////                isGap2[i, 0] = true;
////                if (i == 1)
////                {
////                    opt[i, 0] = gapStartValue;
////                    w1[i, 0] = -(int.MaxValue - 10000);
////                    w2[i, 0] = gapStartValue;
////                }
////                else
////                {
////                    opt[i, 0] = opt[i - 1, 0] + gapContinuationValue;
////                    w1[i, 0] = -(int.MaxValue - 10000);
////                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
////                }
////            }
////            for (i = 1; i <= seq2.Length; i++)
////            {
////                where[0, i] = 0;
////                //str1[0, i] = str1[0, i-1] + "-";
////                //str2[0, i] = str2[0, i-1] + seq2[i - 1];
////                isGap1[0, i] = true;

////                if (i == 1)
////                {
////                    opt[0, i] = gapStartValue;
////                    w1[0, i] = gapStartValue;
////                    w2[0, i] = -(int.MaxValue - 10000);
////                }
////                else
////                {
////                    opt[0, i] = opt[0, i - 1] + gapContinuationValue;
////                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
////                    w2[0, i] = -(int.MaxValue - 10000);
////                }
////            }

////            for (i = 1; i <= seq1.Length; i++)
////                for (j = 1; j <= seq2.Length; j++)
////                {
////                    int left_w1, left_w2, left_opt, left_max;
////                    if (isGap2[i - 1, j])
////                        left_opt = opt[i - 1, j] + gapContinuationValue;
////                    else
////                        left_opt = opt[i - 1, j] + gapStartValue;
////                    left_w1 = w1[i - 1, j] + gapStartValue;
////                    left_w2 = w2[i - 1, j] + gapContinuationValue;
////                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
////                    w2[i, j] = left_max;

////                    int up_w1, up_w2, up_opt, up_max;
////                    if (isGap1[i, j - 1])
////                        up_opt = opt[i, j - 1] + gapContinuationValue;
////                    else
////                        up_opt = opt[i, j - 1] + gapStartValue;
////                    up_w1 = w1[i, j - 1] + gapContinuationValue;
////                    up_w2 = w2[i, j - 1] + gapStartValue;
////                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
////                    w1[i, j] = up_max;

////                    int accros_w1, accros_w2, accros_opt, accros_max;

////                    if (seq1[i - 1] == seq2[j - 1])
////                    {
////                        accros_opt = opt[i - 1, j - 1] + matchValue;
////                        accros_w1 = w1[i - 1, j - 1] + matchValue;
////                        accros_w2 = w2[i - 1, j - 1] + matchValue;
////                    }
////                    else
////                    {
////                        accros_opt = opt[i - 1, j - 1] + mismatchValue;
////                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
////                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
////                    }
////                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

////                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
////                    opt[i, j] = global_opt;
////                    //0 - accros, 1 - left, 2 - up :w1
////                    //3 - accros, 4 - left, 5 - up :w2
////                    //6 - accros, 7 - left, 8 - up :opt
////                    if (global_opt == left_max)
////                    {
////                        if (global_opt == left_w1)
////                            where[i, j] = 1;
////                        else if (global_opt == left_w2)
////                            where[i, j] = 2;
////                        else if (global_opt == left_opt)
////                            where[i, j] = 0;

////                        isGap2[i, j] = true;
////                        //where[i, j] = -1;
////                        //str1[i, j] = str1[i - 1, j] + seq1[i - 1];
////                        //str2[i, j] = str2[i - 1, j] + "-";
////                    }
////                    else if (global_opt == up_max)
////                    {
////                        isGap1[i, j] = true;
////                        if (global_opt == up_w1)
////                            where[i, j] = 1;
////                        else if (global_opt == up_w2)
////                            where[i, j] = 2;
////                        else if (global_opt == up_opt)
////                            where[i, j] = 0;
////                        //str1[i, j] = str1[i, j - 1] + "-";
////                        //str2[i, j] = str2[i, j - 1] + seq2[j - 1];
////                    }
////                    else if (global_opt == accros_max)
////                    {
////                        if (global_opt == up_w1)
////                            where[i, j] = 1;
////                        else if (global_opt == up_w2)
////                            where[i, j] = 2;
////                        else if (global_opt == up_opt)
////                            where[i, j] = 0;
////                        //str1[i, j] = str1[i-1, j - 1] + seq1[i - 1];
////                        //str2[i, j] = str2[i-1, j - 1] + seq2[j - 1];
////                    }
////                    else Console.WriteLine("fdsffffffffffffffffffffffffff");
////                }
////            string s1 = "", s2 = "";
////            i = seq1.Length;
////            j = seq2.Length;
////            //0 - accros, 1 - left, 2 - up :w1
////            //3 - accros, 4 - left, 5 - up :w2
////            //6 - accros, 7 - left, 8 - up :opt
////            int which = 0;//0 -opt 1- w1 2 -w2 

////            while (i != 0 || j != 0)
////            {
////                which = where[i, j];
////                //Console.WriteLine(which);

////                if (which == 1)//w1
////                {
////                    int accros, left, up;
////                    if (i - 1 >= 0 && j - 1 >= 0)
////                    {
////                        if (seq1[i - 1] == seq2[j - 1])
////                            accros = w1[i - 1, j - 1] + matchValue;
////                        else
////                            accros = w1[i - 1, j - 1] + mismatchValue;
////                    }
////                    else
////                        accros = -(int.MaxValue - 10000);
////                    if (i - 1 >= 0)
////                    {
////                        left = w1[i - 1, j] + gapStartValue;
////                    }
////                    else
////                        left = -(int.MaxValue - 10000);
////                    if (j - 1 >= 0)
////                    {
////                        up = w1[i, j - 1] + gapContinuationValue;
////                    }
////                    else
////                        up = -(int.MaxValue - 10000);
////                    int max = Math.Max(Math.Max(left, up), accros);
////                    if (max == -(int.MaxValue - 10000)) break;

////                    if (max == left)
////                    {
////                        s1 += seq1[i - 1];
////                        s2 += "-";
////                        i--;

////                    }
////                    else if (max == up)
////                    {
////                        s1 += "-";
////                        s2 += seq2[j - 1];
////                        j--;
////                    }
////                    else
////                    {
////                        s1 += seq1[i - 1];
////                        s2 += seq2[j - 1];
////                        i--;
////                        j--;
////                    }
////                }
////                else if (which == 2)//w2
////                {
////                    int accros, left, up;
////                    if (i - 1 >= 0 && j - 1 >= 0)
////                    {
////                        if (seq1[i - 1] == seq2[j - 1])
////                            accros = w2[i - 1, j - 1] + matchValue;
////                        else
////                            accros = w2[i - 1, j - 1] + mismatchValue;
////                    }
////                    else
////                        accros = -(int.MaxValue - 10000);
////                    if (i - 1 >= 0)
////                    {

////                        left = w2[i - 1, j] + gapContinuationValue;
////                    }
////                    else
////                        left = -(int.MaxValue - 10000);
////                    if (j - 1 >= 0)
////                    {
////                        up = w2[i, j - 1] + gapStartValue;

////                    }
////                    else
////                        up = -(int.MaxValue - 10000);
////                    int max = Math.Max(Math.Max(left, up), accros);
////                    if (max == -(int.MaxValue - 10000)) break;
////                    if (max == left)
////                    {
////                        s1 += seq1[i - 1];
////                        s2 += "-";
////                        i--;

////                    }
////                    else if (max == up)
////                    {
////                        s1 += "-";
////                        s2 += seq2[j - 1];
////                        j--;
////                    }
////                    else
////                    {
////                        s1 += seq1[i - 1];
////                        s2 += seq2[j - 1];
////                        i--;
////                        j--;
////                    }
////                }
////                else if (which == 0)//opt
////                {
////                    int accros, left, up;
////                    if (i - 1 >= 0 && j - 1 >= 0)
////                    {
////                        if (seq1[i - 1] == seq2[j - 1])
////                            accros = opt[i - 1, j - 1] + matchValue;
////                        else
////                            accros = opt[i - 1, j - 1] + mismatchValue;
////                    }
////                    else
////                        accros = -(int.MaxValue - 10000);
////                    if (i - 1 >= 0)
////                    {
////                        if (isGap2[i - 1, j])
////                            left = opt[i - 1, j] + gapContinuationValue;
////                        else
////                            left = opt[i - 1, j] + gapStartValue;
////                    }
////                    else
////                        left = -(int.MaxValue - 10000);
////                    if (j - 1 >= 0)
////                    {
////                        if (isGap1[i, j - 1])
////                            up = opt[i, j - 1] + gapContinuationValue;
////                        else
////                            up = opt[i, j - 1] + gapStartValue;
////                    }
////                    else
////                        up = -(int.MaxValue - 10000);
////                    int max = Math.Max(Math.Max(left, up), accros);
////                    if (max == -(int.MaxValue - 10000))
////                    {
////                        break;
////                    }
////                    if (max == left)
////                    {
////                        s1 += seq1[i - 1];
////                        s2 += "-";
////                        i--;

////                    }
////                    else if (max == up)
////                    {
////                        s1 += "-";
////                        s2 += seq2[j - 1];
////                        j--;
////                    }
////                    else
////                    {
////                        s1 += seq1[i - 1];
////                        s2 += seq2[j - 1];
////                        i--;
////                        j--;
////                    }
////                }

////            }
////            //while (i != 0 || j != 0)
////            //{
////            //    int left_w1, left_w2, left_opt, left_max;
////            //    if (isGap2[i - 1, j])
////            //        left_opt = opt[i - 1, j] + gapContinuationValue;
////            //    else
////            //        left_opt = opt[i - 1, j] + gapStartValue;
////            //    left_w1 = w1[i - 1, j] + gapStartValue;
////            //    left_w2 = w2[i - 1, j] + gapContinuationValue;
////            //    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
////            //    //w2[i, j] = left_max;

////            //    int up_w1, up_w2, up_opt, up_max;
////            //    if (isGap1[i, j - 1])
////            //        up_opt = opt[i, j - 1] + gapContinuationValue;
////            //    else
////            //        up_opt = opt[i, j - 1] + gapStartValue;
////            //    up_w1 = w1[i, j - 1] + gapContinuationValue;
////            //    up_w2 = w2[i, j - 1] + gapStartValue;
////            //    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
////            //    //w1[i, j] = up_max;

////            //    int accros_w1, accros_w2, accros_opt, accros_max;

////            //    if (seq1[i - 1] == seq2[j - 1])
////            //    {
////            //        accros_opt = opt[i - 1, j - 1] + matchValue;
////            //        accros_w1 = w1[i - 1, j - 1] + matchValue;
////            //        accros_w2 = w2[i - 1, j - 1] + matchValue;
////            //    }
////            //    else
////            //    {
////            //        accros_opt = opt[i - 1, j - 1] + mismatchValue;
////            //        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
////            //        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
////            //    }
////            //    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

////            //    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
////            //    //opt[i, j] = global_opt;
////            //    //0 - accros, 1 - left, 2 - up :w1
////            //    //3 - accros, 4 - left, 5 - up :w2
////            //    //6 - accros, 7 - left, 8 - up :opt
////            //    if (global_opt == left_max)
////            //    {
////            //        if (global_opt == left_w1)
////            //            s1 += seq1[i];
////            //        else if (global_opt == left_w2)
////            //            where[i, j] = 4;
////            //        else if (global_opt == left_opt)
////            //            where[i, j] = 7;

////            //        //isGap2[i, j] = true;
////            //        //where[i, j] = -1;
////            //        //str1[i, j] = str1[i - 1, j] + seq1[i - 1];
////            //        //str2[i, j] = str2[i - 1, j] + "-";
////            //    }
////            //    else if (global_opt == up_max)
////            //    {
////            //        //isGap1[i, j] = true;
////            //        if (global_opt == up_w1)
////            //            where[i, j] = 2;
////            //        else if (global_opt == up_w2)
////            //            where[i, j] = 5;
////            //        else if (global_opt == up_opt)
////            //            where[i, j] = 8;
////            //        //str1[i, j] = str1[i, j - 1] + "-";
////            //        //str2[i, j] = str2[i, j - 1] + seq2[j - 1];
////            //    }
////            //    else if (global_opt == accros_max)
////            //    {
////            //        if (global_opt == up_w1)
////            //            where[i, j] = 0;
////            //        else if (global_opt == up_w2)
////            //            where[i, j] = 3;
////            //        else if (global_opt == up_opt)
////            //            where[i, j] = 6;
////            //        //str1[i, j] = str1[i-1, j - 1] + seq1[i - 1];
////            //        //str2[i, j] = str2[i-1, j - 1] + seq2[j - 1];
////            //    }
////            //}
////            char[] c1 = s1.ToCharArray();
////            char[] c2 = s2.ToCharArray();
////            Array.Reverse(c1);
////            Array.Reverse(c2);
////            s1 = new string(c1);
////            s2 = new string(c2);

////            int val = opt[seq1.Length, seq2.Length];

////            //Console.WriteLine();
////            //for (j = 0; j <= seq2.Length; j++)
////            //{
////            //    for (i = 0; i <= seq1.Length; i++)
////            //    {


////            //        Console.Write(string.Format("{0,2:d}", where[i, j]));
////            //        //console.write(string.format("{0,6:d}", isgap2[i, j]));
////            //    }
////            //    Console.WriteLine();
////            //}
////            return (s1, s2, val);
////            //Console.WriteLine();
////            //string news1 = "";
////            //string news2 = "";

////            //for (i = 0; i < s1.Length; i++)
////            //{
////            //    if (s1[i] != '-')
////            //        news1 += s1[i];
////            //    if (s2[i] != '-')
////            //        news2 += s2[i];

////            ////}
////            //if (string.Compare(str1[seq1.Length, seq2.Length], s1) == 0) Console.WriteLine("okkkkkkkkkkkk s1");
////            //if (string.Compare(str2[seq1.Length, seq2.Length], s2) == 0) Console.WriteLine("okkkkkkkkkkkk s2");
////            //Console.WriteLine("sum= " + sum + " val= " + val);
////            //Console.WriteLine("s1: "+s1+" \ns2: "+s2 );

////            //Console.WriteLine();
////            //for (j = 0; j <= seq2.Length; j++)
////            //{
////            //    for (i = 0; i <= seq1.Length; i++)
////            //    {];


////            //        Console.Write(String.Format("{0,2:D}", where[i, j]));
////            //        //Console.Write(String.Format("{0,6:D}", isGap2[i, j]));
////            //    }
////            //    Console.WriteLine(); ;
////            //}
////            //return (str1[seq1.Length, seq2.Length], str2[seq1.Length, seq2.Length], val);
////            //i = seq1.Length;
////            //j = seq2.Length;
////            //string s1 = "", s2 = "";
////            //while (i != 0 || j != 0)
////            //{
////            //    int left_w1, left_w2, left_opt, left_max;
////            //    if (i - 1 >= 0)
////            //    {
////            //        if (isGap2[i - 1, j])
////            //            left_opt = opt[i - 1, j] + gapContinuationValue;
////            //        else
////            //            left_opt = opt[i - 1, j] + gapStartValue;
////            //        //}
////            //        //else
////            //        //    left_opt = -int.MaxValue;
////            //        //if (i - 1 >= 0)
////            //        //{
////            //        left_w1 = w1[i - 1, j] + gapStartValue;
////            //        left_w2 = w2[i - 1, j] + gapContinuationValue;
////            //        //}
////            //        //else
////            //        //{
////            //        //left_w1 = left_w2 = -int.MaxValue;
////            //        //}
////            //        left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
////            //    }
////            //    else
////            //        left_max = -int.MaxValue;
////            //    int up_w1, up_w2, up_opt, up_max;
////            //    if (j - 1 >= 0)
////            //    {
////            //        if (isGap1[i, j - 1])
////            //            up_opt = opt[i, j - 1] + gapContinuationValue;
////            //        else
////            //            up_opt = opt[i, j - 1] + gapStartValue;

////            //        up_w1 = w1[i, j - 1] + gapContinuationValue;
////            //        up_w2 = w2[i, j - 1] + gapStartValue;
////            //        up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
////            //        //w1[i, j] = up_max;
////            //    }
////            //    else
////            //        up_max = -int.MaxValue;

////            //    int accros_w1, accros_w2, accros_opt, accros_max;
////            //    if (i - 1 >= 0 && j - 1 >= 0)
////            //    {
////            //        if (seq1[i - 1] == seq2[j - 1])
////            //        {
////            //            accros_opt = opt[i - 1, j - 1] + matchValue;
////            //            accros_w1 = w1[i - 1, j - 1] + matchValue;
////            //            accros_w2 = w2[i - 1, j - 1] + matchValue;
////            //        }
////            //        else
////            //        {
////            //            accros_opt = opt[i - 1, j - 1] + mismatchValue;
////            //            accros_w1 = w1[i - 1, j - 1] + mismatchValue;
////            //            accros_w2 = w2[i - 1, j - 1] + mismatchValue;
////            //        }
////            //        accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
////            //    }
////            //    else
////            //        accros_max = -int.MaxValue;

////            //    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
////            //    //opt[i, j] = global_opt;
////            //    if (global_opt == -int.MaxValue)
////            //        break;
////            //    if (global_opt == left_max)
////            //    {
////            //        s1 += seq1[i - 1];
////            //        s2 += "-";
////            //        i--;
////            //    }
////            //    else if (global_opt == up_max)
////            //    {
////            //        s1 += "-";
////            //        s2 += seq2[j - 1];
////            //        j--;
////            //    }
////            //    else
////            //    {
////            //        s1 += seq1[i - 1];
////            //        s2 += seq2[j - 1];
////            //        i--;
////            //        j--;
////            //    }


////            //}





////            //Console.WriteLine();
////            //for ( j = 0; j <= seq2.Length; j++)
////            //{
////            //    for (i = 0; i <= seq1.Length; i++)
////            //    {
////            //        Console.Write(String.Format("{0,4:D}", opt[i, j]));
////            //    }
////            //    Console.WriteLine(); ;
////            //}

////        }
////    }
////}

//////////////////////////////////finale 3

///////////////////////////////finale 2
/////////
///////// using System;
//////using System.Collections.Generic;
//////using System.Text;
//////using System.Linq;

//////namespace Lab2
//////{
//////    public class DnaMatching : MarshalByRefObject
//////    {

//////        /// <summary>
//////        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
//////        /// </summary>
//////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//////        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

//////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
//////        {
//////            const int matchValue = 1;
//////            const int mismatchValue = -3;
//////            const int gapValue = -2;
//////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
//////            maxValue[0, 0] = 0;
//////            int i, j;
//////            for (i = 1; i <= seq1.Length; i++)
//////            {
//////                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
//////            }
//////            for (i = 1; i <= seq2.Length; i++)
//////            {
//////                maxValue[0, i] = maxValue[0, i - 1] - 2;
//////            }

//////            for (i = 1; i <= seq1.Length; i++)
//////            {
//////                for (j = 1; j <= seq2.Length; j++)
//////                {
//////                    int a;
//////                    if (seq1[i - 1] == seq2[j - 1])
//////                        a = matchValue + maxValue[i - 1, j - 1];
//////                    else
//////                        a = mismatchValue + maxValue[i - 1, j - 1];
//////                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
//////                    maxValue[i, j] = max;
//////                }
//////            }

//////            string out1 = "";
//////            string out2 = "";
//////            i = seq1.Length;
//////            j = seq2.Length;

//////            while (i != 0 || j != 0)
//////            {
//////                int up, left;
//////                if (j - 1 < 0)
//////                    up = -int.MaxValue;
//////                else
//////                    up = maxValue[i, j - 1] + gapValue;
//////                if (i - 1 < 0)
//////                    left = -int.MaxValue;
//////                else
//////                    left = maxValue[i - 1, j] + gapValue;
//////                int a;
//////                if (i - 1 >= 0 && j - 1 >= 0)
//////                {
//////                    if (seq1[i - 1] == seq2[j - 1])
//////                        a = maxValue[i - 1, j - 1] + 1;
//////                    else
//////                        a = maxValue[i - 1, j - 1] - 3;
//////                }
//////                else a = -int.MaxValue;
//////                int max = Math.Max(Math.Max(up, left), a);

//////                if (max == a)
//////                {
//////                    out1 += seq1[i - 1];
//////                    out2 += seq2[j - 1];
//////                    j--;
//////                    i--;
//////                }
//////                else if (max == up)
//////                {
//////                    out1 += '-';
//////                    out2 += seq2[j - 1];
//////                    j--;
//////                }
//////                else
//////                {
//////                    out1 += seq1[i - 1];
//////                    out2 += '-';
//////                    i--;
//////                }
//////            }

//////            int val = maxValue[seq1.Length, seq2.Length];
//////            char[] c1 = out1.ToCharArray();
//////            char[] c2 = out2.ToCharArray();
//////            Array.Reverse(c1);
//////            Array.Reverse(c2);
//////            out1 = new string(c1);
//////            out2 = new string(c2);
//////            return (out1, out2, val);
//////        }


//////        /// <summary>
//////        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
//////        /// </summary>
//////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//////        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
//////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
//////        {
//////            const int matchValue = 1;
//////            const int mismatchValue = -3;
//////            const int gapStartValue = -5;
//////            const int gapContinuationValue = -2;
//////            int[,] opt = new int[1 + seq1.Length, 1 + seq2.Length];
//////            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length];
//////            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length];
//////            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |-|
//////                                                                         // |C|

//////            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |C|
//////                                                                         // |-|
//////            opt[0, 0] = 0;
//////            w1[0, 0] = 0;
//////            w2[0, 0] = 0;
//////            int i, j;

//////            int[,] where = new int[seq1.Length + 1, seq2.Length + 1];
//////            //0 - accros, 1 - left, 2 - up :w1
//////            //3 - accros, 4 - left, 5 - up :w2
//////            //6 - accros, 7 - left, 9 - up :w1
//////            where[0, 0] = -2;
//////            //string[,] str1 = new string[seq1.Length + 1, seq2.Length + 1];
//////            //string[,] str2 = new string[seq1.Length + 1, seq2.Length + 1];
//////            for (i = 1; i <= seq1.Length; i++)
//////            {
//////                where[i, 0] = -1;
//////                //str1[i, 0] = str1[i-1, 0]+seq1[i-1];
//////                //str2[i, 0] = str2[i - 1, 0] + "-";
//////                isGap2[i, 0] = true;
//////                if (i == 1)
//////                {
//////                    opt[i, 0] = gapStartValue;
//////                    w1[i, 0] = -(int.MaxValue - 10000);
//////                    w2[i, 0] = gapStartValue;
//////                }
//////                else
//////                {
//////                    opt[i, 0] = opt[i - 1, 0] + gapContinuationValue;
//////                    w1[i, 0] = -(int.MaxValue - 10000);
//////                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
//////                }
//////            }
//////            for (i = 1; i <= seq2.Length; i++)
//////            {
//////                where[0, i] = 1;
//////                //str1[0, i] = str1[0, i-1] + "-";
//////                //str2[0, i] = str2[0, i-1] + seq2[i - 1];
//////                isGap1[0, i] = true;

//////                if (i == 1)
//////                {
//////                    opt[0, i] = gapStartValue;
//////                    w1[0, i] = gapStartValue;
//////                    w2[0, i] = -(int.MaxValue - 10000);
//////                }
//////                else
//////                {
//////                    opt[0, i] = opt[0, i - 1] + gapContinuationValue;
//////                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
//////                    w2[0, i] = -(int.MaxValue - 10000);
//////                }
//////            }

//////            for (i = 1; i <= seq1.Length; i++)
//////                for (j = 1; j <= seq2.Length; j++)
//////                {
//////                    int left_w1, left_w2, left_opt, left_max;
//////                    if (isGap2[i - 1, j])
//////                        left_opt = opt[i - 1, j] + gapContinuationValue;
//////                    else
//////                        left_opt = opt[i - 1, j] + gapStartValue;
//////                    left_w1 = w1[i - 1, j] + gapStartValue;
//////                    left_w2 = w2[i - 1, j] + gapContinuationValue;
//////                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//////                    w2[i, j] = left_max;

//////                    int up_w1, up_w2, up_opt, up_max;
//////                    if (isGap1[i, j - 1])
//////                        up_opt = opt[i, j - 1] + gapContinuationValue;
//////                    else
//////                        up_opt = opt[i, j - 1] + gapStartValue;
//////                    up_w1 = w1[i, j - 1] + gapContinuationValue;
//////                    up_w2 = w2[i, j - 1] + gapStartValue;
//////                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//////                    w1[i, j] = up_max;

//////                    int accros_w1, accros_w2, accros_opt, accros_max;

//////                    if (seq1[i - 1] == seq2[j - 1])
//////                    {
//////                        accros_opt = opt[i - 1, j - 1] + matchValue;
//////                        accros_w1 = w1[i - 1, j - 1] + matchValue;
//////                        accros_w2 = w2[i - 1, j - 1] + matchValue;
//////                    }
//////                    else
//////                    {
//////                        accros_opt = opt[i - 1, j - 1] + mismatchValue;
//////                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//////                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//////                    }
//////                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

//////                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
//////                    opt[i, j] = global_opt;
//////                    //0 - accros, 1 - left, 2 - up :w1
//////                    //3 - accros, 4 - left, 5 - up :w2
//////                    //6 - accros, 7 - left, 8 - up :opt
//////                    if (global_opt == left_max)
//////                    {
//////                        if (global_opt == left_w1)
//////                            where[i, j] = 1;
//////                        else if (global_opt == left_w2)
//////                            where[i, j] = 4;
//////                        else if (global_opt == left_opt)
//////                            where[i, j] = 7;

//////                        isGap2[i, j] = true;
//////                        //where[i, j] = -1;
//////                        //str1[i, j] = str1[i - 1, j] + seq1[i - 1];
//////                        //str2[i, j] = str2[i - 1, j] + "-";
//////                    }
//////                    else if (global_opt == up_max)
//////                    {
//////                        isGap1[i, j] = true;
//////                        if (global_opt == up_w1)
//////                            where[i, j] = 2;
//////                        else if (global_opt == up_w2)
//////                            where[i, j] = 5;
//////                        else if (global_opt == up_opt)
//////                            where[i, j] = 8;
//////                        //str1[i, j] = str1[i, j - 1] + "-";
//////                        //str2[i, j] = str2[i, j - 1] + seq2[j - 1];
//////                    }
//////                    else if (global_opt == accros_max)
//////                    {
//////                        if (global_opt == up_w1)
//////                            where[i, j] = 0;
//////                        else if (global_opt == up_w2)
//////                            where[i, j] = 3;
//////                        else if (global_opt == up_opt)
//////                            where[i, j] = 6;
//////                        //str1[i, j] = str1[i-1, j - 1] + seq1[i - 1];
//////                        //str2[i, j] = str2[i-1, j - 1] + seq2[j - 1];
//////                    }
//////                    else Console.WriteLine("fdsffffffffffffffffffffffffff");
//////                }
//////            string s1 = "", s2 = "";
//////            i = seq1.Length;
//////            j = seq2.Length;
//////            //0 - accros, 1 - left, 2 - up :w1
//////            //3 - accros, 4 - left, 5 - up :w2
//////            //6 - accros, 7 - left, 8 - up :opt
//////            //while (i != 0 || j != 0)
//////            //{
//////            //    if (where[i, j] == 0)
//////            //    {
//////            //        s1 += seq1[i - 1];
//////            //        s2 += seq2[j - 1];
//////            //        i--;
//////            //        j--;
//////            //    }
//////            //    else if (where[i, j] == -1)
//////            //    {
//////            //        s1 += seq1[i - 1];
//////            //        s2 += "-";
//////            //        i--;
//////            //    }
//////            //    else if (where[i, j] == 1)
//////            //    {
//////            //        s1 += "-";
//////            //        s2 += seq2[j - 1];
//////            //        j--;
//////            //    }
//////            //    else
//////            //    {
//////            //        //Console.WriteLine("sth is really worng!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
//////            //    }
//////            //}
//////            while (i != 0 || j != 0)
//////            {
//////                int left_w1, left_w2, left_opt, left_max;
//////                if (isGap2[i - 1, j])
//////                    left_opt = opt[i - 1, j] + gapContinuationValue;
//////                else
//////                    left_opt = opt[i - 1, j] + gapStartValue;
//////                left_w1 = w1[i - 1, j] + gapStartValue;
//////                left_w2 = w2[i - 1, j] + gapContinuationValue;
//////                left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//////                w2[i, j] = left_max;

//////                int up_w1, up_w2, up_opt, up_max;
//////                if (isGap1[i, j - 1])
//////                    up_opt = opt[i, j - 1] + gapContinuationValue;
//////                else
//////                    up_opt = opt[i, j - 1] + gapStartValue;
//////                up_w1 = w1[i, j - 1] + gapContinuationValue;
//////                up_w2 = w2[i, j - 1] + gapStartValue;
//////                up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//////                w1[i, j] = up_max;

//////                int accros_w1, accros_w2, accros_opt, accros_max;

//////                if (seq1[i - 1] == seq2[j - 1])
//////                {
//////                    accros_opt = opt[i - 1, j - 1] + matchValue;
//////                    accros_w1 = w1[i - 1, j - 1] + matchValue;
//////                    accros_w2 = w2[i - 1, j - 1] + matchValue;
//////                }
//////                else
//////                {
//////                    accros_opt = opt[i - 1, j - 1] + mismatchValue;
//////                    accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//////                    accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//////                }
//////                accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

//////                int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
//////                opt[i, j] = global_opt;
//////                //0 - accros, 1 - left, 2 - up :w1
//////                //3 - accros, 4 - left, 5 - up :w2
//////                //6 - accros, 7 - left, 8 - up :opt
//////                if (global_opt == left_max)
//////                {
//////                    if (global_opt == left_w1)
//////                        s1 += seq1[i];
//////                    else if (global_opt == left_w2)
//////                        where[i, j] = 4;
//////                    else if (global_opt == left_opt)
//////                        where[i, j] = 7;

//////                    isGap2[i, j] = true;
//////                    //where[i, j] = -1;
//////                    //str1[i, j] = str1[i - 1, j] + seq1[i - 1];
//////                    //str2[i, j] = str2[i - 1, j] + "-";
//////                }
//////                else if (global_opt == up_max)
//////                {
//////                    isGap1[i, j] = true;
//////                    if (global_opt == up_w1)
//////                        where[i, j] = 2;
//////                    else if (global_opt == up_w2)
//////                        where[i, j] = 5;
//////                    else if (global_opt == up_opt)
//////                        where[i, j] = 8;
//////                    //str1[i, j] = str1[i, j - 1] + "-";
//////                    //str2[i, j] = str2[i, j - 1] + seq2[j - 1];
//////                }
//////                else if (global_opt == accros_max)
//////                {
//////                    if (global_opt == up_w1)
//////                        where[i, j] = 0;
//////                    else if (global_opt == up_w2)
//////                        where[i, j] = 3;
//////                    else if (global_opt == up_opt)
//////                        where[i, j] = 6;
//////                    //str1[i, j] = str1[i-1, j - 1] + seq1[i - 1];
//////                    //str2[i, j] = str2[i-1, j - 1] + seq2[j - 1];
//////                }
//////            }
//////            char[] c1 = s1.ToCharArray();
//////            char[] c2 = s2.ToCharArray();
//////            Array.Reverse(c1);
//////            Array.Reverse(c2);
//////            s1 = new string(c1);
//////            s2 = new string(c2);

//////            int val = opt[seq1.Length, seq2.Length];
//////            return (s1, s2, val);
//////            //Console.WriteLine();
//////            //string news1 = "";
//////            //string news2 = "";

//////            //for (i = 0; i < s1.Length; i++)
//////            //{
//////            //    if (s1[i] != '-')
//////            //        news1 += s1[i];
//////            //    if (s2[i] != '-')
//////            //        news2 += s2[i];

//////            ////}
//////            //if (string.Compare(str1[seq1.Length, seq2.Length], s1) == 0) Console.WriteLine("okkkkkkkkkkkk s1");
//////            //if (string.Compare(str2[seq1.Length, seq2.Length], s2) == 0) Console.WriteLine("okkkkkkkkkkkk s2");
//////            //Console.WriteLine("sum= " + sum + " val= " + val);
//////            //Console.WriteLine("s1: "+s1+" \ns2: "+s2 );

//////            //Console.WriteLine();
//////            //for (j = 0; j <= seq2.Length; j++)
//////            //{
//////            //    for (i = 0; i <= seq1.Length; i++)
//////            //    {];


//////            //        Console.Write(String.Format("{0,2:D}", where[i, j]));
//////            //        //Console.Write(String.Format("{0,6:D}", isGap2[i, j]));
//////            //    }
//////            //    Console.WriteLine(); ;
//////            //}
//////            //return (str1[seq1.Length, seq2.Length], str2[seq1.Length, seq2.Length], val);
//////            //i = seq1.Length;
//////            //j = seq2.Length;
//////            //string s1 = "", s2 = "";
//////            //while (i != 0 || j != 0)
//////            //{
//////            //    int left_w1, left_w2, left_opt, left_max;
//////            //    if (i - 1 >= 0)
//////            //    {
//////            //        if (isGap2[i - 1, j])
//////            //            left_opt = opt[i - 1, j] + gapContinuationValue;
//////            //        else
//////            //            left_opt = opt[i - 1, j] + gapStartValue;
//////            //        //}
//////            //        //else
//////            //        //    left_opt = -int.MaxValue;
//////            //        //if (i - 1 >= 0)
//////            //        //{
//////            //        left_w1 = w1[i - 1, j] + gapStartValue;
//////            //        left_w2 = w2[i - 1, j] + gapContinuationValue;
//////            //        //}
//////            //        //else
//////            //        //{
//////            //        //left_w1 = left_w2 = -int.MaxValue;
//////            //        //}
//////            //        left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//////            //    }
//////            //    else
//////            //        left_max = -int.MaxValue;
//////            //    int up_w1, up_w2, up_opt, up_max;
//////            //    if (j - 1 >= 0)
//////            //    {
//////            //        if (isGap1[i, j - 1])
//////            //            up_opt = opt[i, j - 1] + gapContinuationValue;
//////            //        else
//////            //            up_opt = opt[i, j - 1] + gapStartValue;

//////            //        up_w1 = w1[i, j - 1] + gapContinuationValue;
//////            //        up_w2 = w2[i, j - 1] + gapStartValue;
//////            //        up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//////            //        //w1[i, j] = up_max;
//////            //    }
//////            //    else
//////            //        up_max = -int.MaxValue;

//////            //    int accros_w1, accros_w2, accros_opt, accros_max;
//////            //    if (i - 1 >= 0 && j - 1 >= 0)
//////            //    {
//////            //        if (seq1[i - 1] == seq2[j - 1])
//////            //        {
//////            //            accros_opt = opt[i - 1, j - 1] + matchValue;
//////            //            accros_w1 = w1[i - 1, j - 1] + matchValue;
//////            //            accros_w2 = w2[i - 1, j - 1] + matchValue;
//////            //        }
//////            //        else
//////            //        {
//////            //            accros_opt = opt[i - 1, j - 1] + mismatchValue;
//////            //            accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//////            //            accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//////            //        }
//////            //        accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
//////            //    }
//////            //    else
//////            //        accros_max = -int.MaxValue;

//////            //    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
//////            //    //opt[i, j] = global_opt;
//////            //    if (global_opt == -int.MaxValue)
//////            //        break;
//////            //    if (global_opt == left_max)
//////            //    {
//////            //        s1 += seq1[i - 1];
//////            //        s2 += "-";
//////            //        i--;
//////            //    }
//////            //    else if (global_opt == up_max)
//////            //    {
//////            //        s1 += "-";
//////            //        s2 += seq2[j - 1];
//////            //        j--;
//////            //    }
//////            //    else
//////            //    {
//////            //        s1 += seq1[i - 1];
//////            //        s2 += seq2[j - 1];
//////            //        i--;
//////            //        j--;
//////            //    }


//////            //}





//////            //Console.WriteLine();
//////            //for ( j = 0; j <= seq2.Length; j++)
//////            //{
//////            //    for (i = 0; i <= seq1.Length; i++)
//////            //    {
//////            //        Console.Write(String.Format("{0,4:D}", opt[i, j]));
//////            //    }
//////            //    Console.WriteLine(); ;
//////            //}

//////        }
//////    }
//////}
//////////////////////////////////final 2
/////////////////////////////////////// new final1
////////using System;
////////using System.Collections.Generic;
////////using System.Text;
////////using System.Linq;

////////namespace Lab2
////////{
////////    public class DnaMatching : MarshalByRefObject
////////    {

////////        /// <summary>
////////        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
////////        /// </summary>
////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
////////        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
////////        {
////////            const int matchValue = 1;
////////            const int mismatchValue = -3;
////////            const int gapValue = -2;
////////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
////////            maxValue[0, 0] = 0;
////////            int i, j;
////////            for (i = 1; i <= seq1.Length; i++)
////////            {
////////                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
////////            }
////////            for (i = 1; i <= seq2.Length; i++)
////////            {
////////                maxValue[0, i] = maxValue[0, i - 1] - 2;
////////            }

////////            for (i = 1; i <= seq1.Length; i++)
////////            {
////////                for (j = 1; j <= seq2.Length; j++)
////////                {
////////                    int a;
////////                    if (seq1[i - 1] == seq2[j - 1])
////////                        a = matchValue + maxValue[i - 1, j - 1];
////////                    else
////////                        a = mismatchValue + maxValue[i - 1, j - 1];
////////                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
////////                    maxValue[i, j] = max;
////////                }
////////            }

////////            string out1 = "";
////////            string out2 = "";
////////            i = seq1.Length;
////////            j = seq2.Length;

////////            while (i != 0 || j != 0)
////////            {
////////                int up, left;
////////                if (j - 1 < 0)
////////                    up = -int.MaxValue;
////////                else
////////                    up = maxValue[i, j - 1] + gapValue;
////////                if (i - 1 < 0)
////////                    left = -int.MaxValue;
////////                else
////////                    left = maxValue[i - 1, j] + gapValue;
////////                int a;
////////                if (i - 1 >= 0 && j - 1 >= 0)
////////                {
////////                    if (seq1[i - 1] == seq2[j - 1])
////////                        a = maxValue[i - 1, j - 1] + 1;
////////                    else
////////                        a = maxValue[i - 1, j - 1] - 3;
////////                }
////////                else a = -int.MaxValue;
////////                int max = Math.Max(Math.Max(up, left), a);

////////                if (max == a)
////////                {
////////                    out1 += seq1[i - 1];
////////                    out2 += seq2[j - 1];
////////                    j--;
////////                    i--;
////////                }
////////                else if (max == up)
////////                {
////////                    out1 += '-';
////////                    out2 += seq2[j - 1];
////////                    j--;
////////                }
////////                else
////////                {
////////                    out1 += seq1[i - 1];
////////                    out2 += '-';
////////                    i--;
////////                }
////////            }

////////            int val = maxValue[seq1.Length, seq2.Length];
////////            char[] c1 = out1.ToCharArray();
////////            char[] c2 = out2.ToCharArray();
////////            Array.Reverse(c1);
////////            Array.Reverse(c2);
////////            out1 = new string(c1);
////////            out2 = new string(c2);
////////            return (out1, out2, val);
////////        }


////////        /// <summary>
////////        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
////////        /// </summary>
////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
////////        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
////////        {
////////            const int matchValue = 1;
////////            const int mismatchValue = -3;
////////            const int gapStartValue = -5;
////////            const int gapContinuationValue = -2;
////////            int[,] opt = new int[1 + seq1.Length, 1 + seq2.Length];
////////            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length];
////////            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length];
////////            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |-|
////////                                                                         // |C|

////////            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length]; // |C|
////////                                                                         // |-|
////////            opt[0, 0] = 0;
////////            int i, j;

////////            int[,] where = new int[seq1.Length + 1, seq2.Length + 1]; //0 - accros, -1 - left, 1 - up

////////            for (i = 1; i <= seq1.Length; i++)
////////            {
////////                where[i, 0] = -1;

////////                if (i == 1)
////////                {
////////                    opt[i, 0] = gapStartValue;
////////                    w1[i, 0] = gapStartValue;
////////                    w2[i, 0] = gapStartValue;
////////                }
////////                else
////////                {
////////                    opt[i, 0] = opt[i - 1, 0] + gapContinuationValue;
////////                    w1[i, 0] = w1[i - 1, 0] + gapContinuationValue;
////////                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
////////                }
////////            }
////////            for (i = 1; i <= seq2.Length; i++)
////////            {
////////                where[0, i] = 1;
////////                if (i == 1)
////////                {
////////                    opt[0, i] = gapStartValue;
////////                    w1[0, i] = gapStartValue;
////////                    w2[0, i] = gapStartValue;
////////                }
////////                else
////////                {
////////                    opt[0, i] = opt[0, i - 1] + gapContinuationValue;
////////                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
////////                    w2[0, i] = w2[0, i - 1] + gapContinuationValue;
////////                }
////////            }


////////            for (i = 1; i <= seq1.Length; i++)
////////                for (j = 1; j <= seq2.Length; j++)
////////                {
////////                    int left_w1, left_w2, left_opt, left_max;
////////                    if (isGap2[i - 1, j])
////////                        left_opt = opt[i - 1, j] + gapContinuationValue;
////////                    else
////////                        left_opt = opt[i - 1, j] + gapStartValue;
////////                    left_w1 = w1[i - 1, j] + gapStartValue;
////////                    left_w2 = w2[i - 1, j] + gapContinuationValue;
////////                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
////////                    w2[i, j] = left_max;

////////                    int up_w1, up_w2, up_opt, up_max;
////////                    if (isGap1[i, j - 1])
////////                        up_opt = opt[i, j - 1] + gapContinuationValue;
////////                    else
////////                        up_opt = opt[i, j - 1] + gapStartValue;
////////                    up_w1 = w1[i, j - 1] + gapContinuationValue;
////////                    up_w2 = w2[i, j - 1] + gapStartValue;
////////                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
////////                    w1[i, j] = up_max;

////////                    int accros_w1, accros_w2, accros_opt, accros_max;

////////                    if (seq1[i - 1] == seq2[j - 1])
////////                    {
////////                        accros_opt = opt[i - 1, j - 1] + matchValue;
////////                        accros_w1 = w1[i - 1, j - 1] + matchValue;
////////                        accros_w2 = w2[i - 1, j - 1] + matchValue;
////////                    }
////////                    else
////////                    {
////////                        accros_opt = opt[i - 1, j - 1] + mismatchValue;
////////                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
////////                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
////////                    }
////////                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

////////                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
////////                    opt[i, j] = global_opt;

////////                    if (global_opt == left_max)
////////                    {
////////                        isGap2[i, j] = true;
////////                        where[i, j] = -1;
////////                    }
////////                    else if (global_opt == up_max)
////////                    {
////////                        isGap1[i, j] = true;
////////                        where[i, j] = 1;
////////                    }
////////                    else
////////                        where[i, j] = 0;


////////                }
////////            string s1 = "", s2 = "";
////////            i = seq1.Length;
////////            j = seq2.Length;
////////            while (i != 0 || j != 0)
////////            {
////////                if (where[i, j] == 0)
////////                {
////////                    s1 += seq1[i - 1];
////////                    s2 += seq2[j - 1];
////////                    i--;
////////                    j--;
////////                }
////////                else if (where[i, j] == -1)
////////                {
////////                    s1 += seq1[i - 1];
////////                    s2 += "-";
////////                    i--;
////////                }
////////                else if (where[i, j] == 1)
////////                {
////////                    s1 += "-";
////////                    s2 += seq2[j - 1];
////////                    j--;
////////                }
////////                else
////////                {
////////                    Console.WriteLine("sth is really worng!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
////////                }
////////            }
////////            char[] c1 = s1.ToCharArray();
////////            char[] c2 = s2.ToCharArray();
////////            Array.Reverse(c1);
////////            Array.Reverse(c2);
////////            s1 = new string(c1);
////////            s2 = new string(c2);
////////            Console.WriteLine();
////////            //string news1 = "";
////////            //string news2 = "";

////////            //for (i = 0; i < s1.Length; i++)
////////            //{
////////            //    if (s1[i] != '-')
////////            //        news1 += s1[i];
////////            //    if (s2[i] != '-')
////////            //        news2 += s2[i];

////////            //}
////////            //if (string.Compare(news1, seq1) == 0) Console.WriteLine("okkkkkkkkkkkk s1");
////////            //if (string.Compare(news2, seq2) == 0) Console.WriteLine("okkkkkkkkkkkk s2");
////////            int val = opt[seq1.Length, seq2.Length];
////////            //Console.WriteLine("sum= " + sum + " val= " + val);
////////            Console.WriteLine("s1: " + s1 + " \ns2: " + s2);
////////            return (s1, s2, val);

////////            //i = seq1.Length;
////////            //j = seq2.Length;
////////            //string s1 = "", s2 = "";
////////            //while (i != 0 || j != 0)
////////            //{
////////            //    int left_w1, left_w2, left_opt, left_max;
////////            //    if (i - 1 >= 0)
////////            //    {
////////            //        if (isGap2[i - 1, j])
////////            //            left_opt = opt[i - 1, j] + gapContinuationValue;
////////            //        else
////////            //            left_opt = opt[i - 1, j] + gapStartValue;
////////            //        //}
////////            //        //else
////////            //        //    left_opt = -int.MaxValue;
////////            //        //if (i - 1 >= 0)
////////            //        //{
////////            //        left_w1 = w1[i - 1, j] + gapStartValue;
////////            //        left_w2 = w2[i - 1, j] + gapContinuationValue;
////////            //        //}
////////            //        //else
////////            //        //{
////////            //        //left_w1 = left_w2 = -int.MaxValue;
////////            //        //}
////////            //        left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
////////            //    }
////////            //    else
////////            //        left_max = -int.MaxValue;
////////            //    int up_w1, up_w2, up_opt, up_max;
////////            //    if (j - 1 >= 0)
////////            //    {
////////            //        if (isGap1[i, j - 1])
////////            //            up_opt = opt[i, j - 1] + gapContinuationValue;
////////            //        else
////////            //            up_opt = opt[i, j - 1] + gapStartValue;

////////            //        up_w1 = w1[i, j - 1] + gapContinuationValue;
////////            //        up_w2 = w2[i, j - 1] + gapStartValue;
////////            //        up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
////////            //        //w1[i, j] = up_max;
////////            //    }
////////            //    else
////////            //        up_max = -int.MaxValue;

////////            //    int accros_w1, accros_w2, accros_opt, accros_max;
////////            //    if (i - 1 >= 0 && j - 1 >= 0)
////////            //    {
////////            //        if (seq1[i - 1] == seq2[j - 1])
////////            //        {
////////            //            accros_opt = opt[i - 1, j - 1] + matchValue;
////////            //            accros_w1 = w1[i - 1, j - 1] + matchValue;
////////            //            accros_w2 = w2[i - 1, j - 1] + matchValue;
////////            //        }
////////            //        else
////////            //        {
////////            //            accros_opt = opt[i - 1, j - 1] + mismatchValue;
////////            //            accros_w1 = w1[i - 1, j - 1] + mismatchValue;
////////            //            accros_w2 = w2[i - 1, j - 1] + mismatchValue;
////////            //        }
////////            //        accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
////////            //    }
////////            //    else
////////            //        accros_max = -int.MaxValue;

////////            //    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
////////            //    //opt[i, j] = global_opt;
////////            //    if (global_opt == -int.MaxValue)
////////            //        break;
////////            //    if (global_opt == left_max)
////////            //    {
////////            //        s1 += seq1[i - 1];
////////            //        s2 += "-";
////////            //        i--;
////////            //    }
////////            //    else if (global_opt == up_max)
////////            //    {
////////            //        s1 += "-";
////////            //        s2 += seq2[j - 1];
////////            //        j--;
////////            //    }
////////            //    else
////////            //    {
////////            //        s1 += seq1[i - 1];
////////            //        s2 += seq2[j - 1];
////////            //        i--;
////////            //        j--;
////////            //    }


////////            //}


////////            //char[] c1 = s1.ToCharArray();
////////            //char[] c2 = s2.ToCharArray();
////////            //Array.Reverse(c1);
////////            //Array.Reverse(c2);
////////            //s1 = new string(c1);
////////            //s2 = new string(c2);
////////            //Console.WriteLine();
////////            //Console.WriteLine("s1:" + s1);
////////            //Console.WriteLine("s2:" + s2);

////////            //int sum = 0;
////////            //bool gap1 = false;
////////            //bool gap2 = false;
////////            //for (i = 0; i < s1.Length; i++)
////////            //{

////////            //    if (s1[i] == s2[i])
////////            //    {
////////            //        sum += 1;
////////            //        gap1 = false;
////////            //        gap2 = false;
////////            //    }
////////            //    else
////////            //    if (s1[i] == '-' && gap1 == true)
////////            //    {
////////            //        sum -= 2;
////////            //        gap1 = true;
////////            //        gap2 = false;
////////            //    }
////////            //    else if (s1[i] == '-' && gap1 == false)
////////            //    {
////////            //        sum -= 5;
////////            //        gap1 = true;
////////            //        gap2 = false;
////////            //    }
////////            //    else if (s2[i] == '-' && gap2 == true)
////////            //    {
////////            //        sum -= 2;
////////            //        gap2 = true;
////////            //        gap1 = false;
////////            //    }
////////            //    else if (s2[i] == '-' && gap2 == false)
////////            //    {
////////            //        sum -= 5;
////////            //        gap2 = true;
////////            //        gap1 = false;
////////            //    }
////////            //    else
////////            //    {
////////            //        sum -= 3;
////////            //        gap2 = false;
////////            //        gap1 = false;
////////            //    }

////////            //}

////////            //for (i = 0; i < s1.Length; i++)
////////            //{
////////            //    Console.WriteLine(s1[i].ToString()+s2[i].ToString());
////////            //}
////////            //Console.WriteLine();
////////            //for ( j = 0; j <= seq2.Length; j++)
////////            //{
////////            //    for (i = 0; i <= seq1.Length; i++)
////////            //    {
////////            //        Console.Write(String.Format("{0,4:D}", opt[i, j]));
////////            //    }
////////            //    Console.WriteLine(); ;
////////            //}
////////            //string s1 = "", s2 = "";
////////            //i = seq1.Length;
////////            //j = seq2.Length;
////////            //while (i != 0 || j != 0)
////////            //{
////////            //    if (where[i, j] == 0)
////////            //    {
////////            //        s1 += seq1[i - 1];
////////            //        s2 += seq2[j - 1];
////////            //        i--;
////////            //        j--;
////////            //    }
////////            //    else if (where[i, j] == -1)
////////            //    {
////////            //        s1 += seq1[i - 1];
////////            //        s2 += "-";
////////            //        i--;
////////            //    }
////////            //    else if (where[i, j] == 1)
////////            //    {
////////            //        s1 += "-";
////////            //        s2 += seq2[j - 1];
////////            //        j--;
////////            //    }
////////            //    else
////////            //    {
////////            //        Console.WriteLine("sth is really worng!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
////////            //    }
////////            //}
////////            //char[] c1 = s1.ToCharArray();
////////            //char[] c2 = s2.ToCharArray();
////////            //Array.Reverse(c1);
////////            //Array.Reverse(c2);
////////            //s1 = new string(c1);
////////            //s2 = new string(c2);
////////            //Console.WriteLine();
////////            //string news1 = "";
////////            //string news2 = "";

////////            //for (i = 0; i < s1.Length; i++)
////////            //{
////////            //    if (s1[i] != '-')
////////            //        news1 += s1[i];
////////            //    if (s2[i] != '-')
////////            //        news2 += s2[i];

////////            //}
////////            //if (string.Compare(news1, seq1) == 0) Console.WriteLine("okkkkkkkkkkkk s1");
////////            //if (string.Compare(news2, seq2) == 0) Console.WriteLine("okkkkkkkkkkkk s2");
////////            //int val = opt[seq1.Length, seq2.Length];
////////            ////Console.WriteLine("sum= " + sum + " val= " + val);

////////            //return (s1, s2, val);
////////        }
////////    }
////////}



/////////// new final1









/////////////////////////////////////finall

//////////using System;
//////////using System.Collections.Generic;
//////////using System.Text;
//////////using System.Linq;

//////////namespace Lab2
//////////{
//////////    public class DnaMatching : MarshalByRefObject
//////////    {

//////////        /// <summary>
//////////        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
//////////        /// </summary>
//////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//////////        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

//////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
//////////        {
//////////            const int matchValue = 1;
//////////            const int mismatchValue = -3;
//////////            const int gapValue = -2;
//////////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
//////////            maxValue[0, 0] = 0;
//////////            int i, j;
//////////            for (i = 1; i <= seq1.Length; i++)
//////////            {
//////////                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
//////////            }
//////////            for (i = 1; i <= seq2.Length; i++)
//////////            {
//////////                maxValue[0, i] = maxValue[0, i - 1] - 2;
//////////            }

//////////            for (i = 1; i <= seq1.Length; i++)
//////////            {
//////////                for (j = 1; j <= seq2.Length; j++)
//////////                {
//////////                    int a;
//////////                    if (seq1[i - 1] == seq2[j - 1])
//////////                        a = matchValue + maxValue[i - 1, j - 1];
//////////                    else
//////////                        a = mismatchValue + maxValue[i - 1, j - 1];
//////////                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
//////////                    maxValue[i, j] = max;
//////////                }
//////////            }

//////////            string out1 = "";
//////////            string out2 = "";
//////////            i = seq1.Length;
//////////            j = seq2.Length;

//////////            while (i != 0 || j != 0)
//////////            {
//////////                int up, left;
//////////                if (j - 1 < 0)
//////////                    up = -int.MaxValue;
//////////                else
//////////                    up = maxValue[i, j - 1] + gapValue;
//////////                if (i - 1 < 0)
//////////                    left = -int.MaxValue;
//////////                else
//////////                    left = maxValue[i - 1, j] + gapValue;
//////////                int a;
//////////                if (i - 1 >= 0 && j - 1 >= 0)
//////////                {
//////////                    if (seq1[i - 1] == seq2[j - 1])
//////////                        a = maxValue[i - 1, j - 1] + 1;
//////////                    else
//////////                        a = maxValue[i - 1, j - 1] - 3;
//////////                }
//////////                else a = -int.MaxValue;
//////////                int max = Math.Max(Math.Max(up, left), a);

//////////                if (max == a)
//////////                {
//////////                    out1 += seq1[i - 1];
//////////                    out2 += seq2[j - 1];
//////////                    j--;
//////////                    i--;
//////////                }
//////////                else if (max == up)
//////////                {
//////////                    out1 += '-';
//////////                    out2 += seq2[j - 1];
//////////                    j--;
//////////                }
//////////                else
//////////                {
//////////                    out1 += seq1[i - 1];
//////////                    out2 += '-';
//////////                    i--;
//////////                }
//////////            }

//////////            int val = maxValue[seq1.Length, seq2.Length];
//////////            char[] c1 = out1.ToCharArray();
//////////            char[] c2 = out2.ToCharArray();
//////////            Array.Reverse(c1);
//////////            Array.Reverse(c2);
//////////            out1 = new string(c1);
//////////            out2 = new string(c2);
//////////            return (out1, out2, val);
//////////        }


//////////        /// <summary>
//////////        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
//////////        /// </summary>
//////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//////////        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
//////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
//////////        {
//////////            const int matchValue = 1;
//////////            const int mismatchValue = -3;
//////////            const int gapStartValue = -5;
//////////            const int gapContinuationValue = -2;
//////////            int[,] opt = new int[1 + seq1.Length, 1 + seq2.Length];
//////////            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length];
//////////            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length];
//////////            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length];// |-|
//////////                                                                        // |C|

//////////            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length];// |C|
//////////                                                                        // |-|
//////////            opt[0, 0] = 0;
//////////            int i, j;
//////////            for (i = 1; i <= seq1.Length; i++)
//////////            {
//////////                if (i == 1)
//////////                {
//////////                    opt[i, 0] = gapStartValue;
//////////                    w1[i, 0] = gapStartValue;
//////////                    w2[i, 0] = gapStartValue;
//////////                }
//////////                else
//////////                {
//////////                    opt[i, 0] = opt[i - 1, 0] + gapContinuationValue;
//////////                    w1[i, 0] = w1[i - 1, 0] + gapContinuationValue;
//////////                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
//////////                }
//////////            }
//////////            for (i = 1; i <= seq2.Length; i++)
//////////            {
//////////                if (i == 1)
//////////                {
//////////                    opt[0, i] = gapStartValue;
//////////                    w1[0, i] = gapStartValue;
//////////                    w2[0, i] = gapStartValue;
//////////                }
//////////                else
//////////                {
//////////                    opt[0, i] = opt[0, i - 1] + gapContinuationValue;
//////////                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
//////////                    w2[0, i] = w2[0, i - 1] + gapContinuationValue;
//////////                }
//////////            }


//////////            for (i = 1; i <= seq1.Length; i++)
//////////                for (j = 1; j <= seq2.Length; j++)
//////////                {
//////////                    int left_w1, left_w2, left_opt, left_max;
//////////                    if (isGap2[i - 1, j])
//////////                        left_opt = opt[i - 1, j] + gapContinuationValue;
//////////                    else
//////////                        left_opt = opt[i - 1, j] + gapStartValue;
//////////                    left_w1 = w1[i - 1, j] + gapStartValue;
//////////                    left_w2 = w2[i - 1, j] + gapContinuationValue;
//////////                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//////////                    w2[i, j] = left_max;

//////////                    int up_w1, up_w2, up_opt, up_max;
//////////                    if (isGap1[i, j - 1])
//////////                        up_opt = opt[i, j - 1] + gapContinuationValue;
//////////                    else
//////////                        up_opt = opt[i, j - 1] + gapStartValue;
//////////                    up_w1 = w1[i, j - 1] + gapContinuationValue;
//////////                    up_w2 = w2[i, j - 1] + gapStartValue;
//////////                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//////////                    w1[i, j] = up_max;

//////////                    int accros_w1, accros_w2, accros_opt, accros_max;

//////////                    if (seq1[i - 1] == seq2[j - 1])
//////////                    {
//////////                        accros_opt = opt[i - 1, j - 1] + matchValue;
//////////                        accros_w1 = w1[i - 1, j - 1] + matchValue;
//////////                        accros_w2 = w2[i - 1, j - 1] + matchValue;
//////////                    }
//////////                    else
//////////                    {
//////////                        accros_opt = opt[i - 1, j - 1] + mismatchValue;
//////////                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//////////                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//////////                    }
//////////                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

//////////                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
//////////                    opt[i, j] = global_opt;

//////////                    if (global_opt == left_max)
//////////                        isGap2[i, j] = true;
//////////                    else if (global_opt == up_max)
//////////                        isGap1[i, j] = true;


//////////                }


//////////            //i = seq1.Length;
//////////            //j = seq2.Length;
//////////            //string s1 = "", s2 = "";
//////////            //while (i != 0 || j != 0)
//////////            //{
//////////            //    int left_w1, left_w2, left_opt, left_max;
//////////            //    if (i - 1 >= 0)
//////////            //    {
//////////            //        if (isGap2[i - 1, j])
//////////            //            left_opt = opt[i - 1, j] + gapContinuationValue;
//////////            //        else
//////////            //            left_opt = opt[i - 1, j] + gapStartValue;
//////////            //        //}
//////////            //        //else
//////////            //        //    left_opt = -int.MaxValue;
//////////            //        //if (i - 1 >= 0)
//////////            //        //{
//////////            //        left_w1 = w1[i - 1, j] + gapStartValue;
//////////            //        left_w2 = w2[i - 1, j] + gapContinuationValue;
//////////            //        //}
//////////            //        //else
//////////            //        //{
//////////            //        //left_w1 = left_w2 = -int.MaxValue;
//////////            //        //}
//////////            //        left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
//////////            //    }
//////////            //    else
//////////            //        left_max = -int.MaxValue;
//////////            //    int up_w1, up_w2, up_opt, up_max;
//////////            //    if (j - 1 >= 0)
//////////            //    {
//////////            //        if (isGap1[i, j - 1])
//////////            //            up_opt = opt[i, j - 1] + gapContinuationValue;
//////////            //        else
//////////            //            up_opt = opt[i, j - 1] + gapStartValue;

//////////            //        up_w1 = w1[i, j - 1] + gapContinuationValue;
//////////            //        up_w2 = w2[i, j - 1] + gapStartValue;
//////////            //        up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
//////////            //        //w1[i, j] = up_max;
//////////            //    }
//////////            //    else
//////////            //        up_max = -int.MaxValue;

//////////            //    int accros_w1, accros_w2, accros_opt, accros_max;
//////////            //    if (i - 1 >= 0 && j - 1 >= 0)
//////////            //    {
//////////            //        if (seq1[i - 1] == seq2[j - 1])
//////////            //        {
//////////            //            accros_opt = opt[i - 1, j - 1] + matchValue;
//////////            //            accros_w1 = w1[i - 1, j - 1] + matchValue;
//////////            //            accros_w2 = w2[i - 1, j - 1] + matchValue;
//////////            //        }
//////////            //        else
//////////            //        {
//////////            //            accros_opt = opt[i - 1, j - 1] + mismatchValue;
//////////            //            accros_w1 = w1[i - 1, j - 1] + mismatchValue;
//////////            //            accros_w2 = w2[i - 1, j - 1] + mismatchValue;
//////////            //        }
//////////            //        accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);
//////////            //    }
//////////            //    else
//////////            //        accros_max = -int.MaxValue;

//////////            //    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
//////////            //    //opt[i, j] = global_opt;
//////////            //    if (global_opt == -int.MaxValue)
//////////            //        break;
//////////            //    if (global_opt == left_max)
//////////            //    {
//////////            //        s1 += seq1[i - 1];
//////////            //        s2 += "-";
//////////            //        i--;
//////////            //    }
//////////            //    else if (global_opt == up_max)
//////////            //    {
//////////            //        s1 += "-";
//////////            //        s2 += seq2[j - 1];
//////////            //        j--;
//////////            //    }
//////////            //    else
//////////            //    {
//////////            //        s1 += seq1[i - 1];
//////////            //        s2 += seq2[j - 1];
//////////            //        i--;
//////////            //        j--;
//////////            //    }


//////////            //}


//////////            char[] c1 = s1.ToCharArray();
//////////            char[] c2 = s2.ToCharArray();
//////////            Array.Reverse(c1);
//////////            Array.Reverse(c2);
//////////            s1 = new string(c1);
//////////            s2 = new string(c2);
//////////            //Console.WriteLine();
//////////            //Console.WriteLine("s1:" + s1);
//////////            //Console.WriteLine("s2:" + s2);

//////////            //int sum = 0;
//////////            //bool gap1 = false;
//////////            //bool gap2 = false;
//////////            //for (i = 0; i < s1.Length; i++)
//////////            //{

//////////            //    if (s1[i] == s2[i])
//////////            //    {
//////////            //        sum += 1;
//////////            //        gap1 = false;
//////////            //        gap2 = false;
//////////            //    }
//////////            //    else
//////////            //    if (s1[i] == '-' && gap1 == true)
//////////            //    {
//////////            //        sum -= 2;
//////////            //        gap1 = true;
//////////            //        gap2 = false;
//////////            //    }
//////////            //    else if (s1[i] == '-' && gap1 == false)
//////////            //    {
//////////            //        sum -= 5;
//////////            //        gap1 = true;
//////////            //        gap2 = false;
//////////            //    }
//////////            //    else if (s2[i] == '-' && gap2 == true)
//////////            //    {
//////////            //        sum -= 2;
//////////            //        gap2 = true;
//////////            //        gap1 = false;
//////////            //    }
//////////            //    else if (s2[i] == '-' && gap2 == false)
//////////            //    {
//////////            //        sum -= 5;
//////////            //        gap2 = true;
//////////            //        gap1 = false;
//////////            //    }
//////////            //    else
//////////            //    {
//////////            //        sum -= 3;
//////////            //        gap2 = false;
//////////            //        gap1 = false;
//////////            //    }

//////////            //}

//////////            //for (i = 0; i < s1.Length; i++)
//////////            //{
//////////            //    Console.WriteLine(s1[i].ToString()+s2[i].ToString());
//////////            //}
//////////            //Console.WriteLine();
//////////            //for ( j = 0; j <= seq2.Length; j++)
//////////            //{
//////////            //    for (i = 0; i <= seq1.Length; i++)
//////////            //    {
//////////            //        Console.Write(String.Format("{0,4:D}", opt[i, j]));
//////////            //    }
//////////            //    Console.WriteLine(); ;
//////////            //}

//////////            int val = opt[seq1.Length, seq2.Length];
//////////            //Console.WriteLine("sum= " + sum + " val= " + val);

//////////            return (s1, s2, val);
//////////        }
//////////    }
//////////}

/////////////////////////////////////////////////////////////////////////finall

////////////dobry trzeci etap

////////////using System;
////////////using System.Collections.Generic;
////////////using System.Text;
////////////using System.Linq;

////////////namespace Lab2
////////////{
////////////    public class DnaMatching : MarshalByRefObject
////////////    {

////////////        /// <summary>
////////////        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
////////////        /// </summary>
////////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
////////////        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

////////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
////////////        {
////////////            const int matchValue = 1;
////////////            const int mismatchValue = -3;
////////////            const int gapValue = -2;
////////////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
////////////            maxValue[0, 0] = 0;
////////////            int i, j;
////////////            for (i = 1; i <= seq1.Length; i++)
////////////            {
////////////                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
////////////            }
////////////            for (i = 1; i <= seq2.Length; i++)
////////////            {
////////////                maxValue[0, i] = maxValue[0, i - 1] - 2;
////////////            }

////////////            for (i = 1; i <= seq1.Length; i++)
////////////            {
////////////                for (j = 1; j <= seq2.Length; j++)
////////////                {
////////////                    int a;
////////////                    if (seq1[i - 1] == seq2[j - 1])
////////////                        a = matchValue + maxValue[i - 1, j - 1];
////////////                    else
////////////                        a = mismatchValue + maxValue[i - 1, j - 1];
////////////                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
////////////                    maxValue[i, j] = max;
////////////                }
////////////            }

////////////            string out1 = "";
////////////            string out2 = "";
////////////            i = seq1.Length;
////////////            j = seq2.Length;

////////////            while (i != 0 || j != 0)
////////////            {
////////////                int up, left;
////////////                if (j - 1 < 0)
////////////                    up = -int.MaxValue;
////////////                else
////////////                    up = maxValue[i, j - 1] + gapValue;
////////////                if (i - 1 < 0)
////////////                    left = -int.MaxValue;
////////////                else
////////////                    left = maxValue[i - 1, j] + gapValue;
////////////                int a;
////////////                if (i - 1 >= 0 && j - 1 >= 0)
////////////                {
////////////                    if (seq1[i - 1] == seq2[j - 1])
////////////                        a = maxValue[i - 1, j - 1] + 1;
////////////                    else
////////////                        a = maxValue[i - 1, j - 1] - 3;
////////////                }
////////////                else a = -int.MaxValue;
////////////                int max = Math.Max(Math.Max(up, left), a);

////////////                if (max == a)
////////////                {
////////////                    out1 += seq1[i - 1];
////////////                    out2 += seq2[j - 1];
////////////                    j--;
////////////                    i--;
////////////                }
////////////                else if (max == up)
////////////                {
////////////                    out1 += '-';
////////////                    out2 += seq2[j - 1];
////////////                    j--;
////////////                }
////////////                else
////////////                {
////////////                    out1 += seq1[i - 1];
////////////                    out2 += '-';
////////////                    i--;
////////////                }
////////////            }

////////////            int val = maxValue[seq1.Length, seq2.Length];
////////////            char[] c1 = out1.ToCharArray();
////////////            char[] c2 = out2.ToCharArray();
////////////            Array.Reverse(c1);
////////////            Array.Reverse(c2);
////////////            out1 = new string(c1);
////////////            out2 = new string(c2);
////////////            return (out1, out2, val);
////////////        }


////////////        /// <summary>
////////////        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
////////////        /// </summary>
////////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
////////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
////////////        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
////////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
////////////        {
////////////            const int matchValue = 1;
////////////            const int mismatchValue = -3;
////////////            const int gapStartValue = -5;
////////////            const int gapContinuationValue = -2;
////////////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
////////////            int[,] w1 = new int[1 + seq1.Length, 1 + seq2.Length];
////////////            int[,] w2 = new int[1 + seq1.Length, 1 + seq2.Length];
////////////            bool[,] isGap1 = new bool[1 + seq1.Length, 1 + seq2.Length];// |-|
////////////                                                                        // |C|

////////////            bool[,] isGap2 = new bool[1 + seq1.Length, 1 + seq2.Length];// |C|
////////////                                                                        // |-|
////////////            maxValue[0, 0] = 0;
////////////            //string[,] s1 = new string[1 + seq1.Length, 1 + seq2.Length];
////////////            //string[,] s2 = new string[1 + seq1.Length, 1 + seq2.Length];
////////////            //s1[0, 0] = "";
////////////            //s2[0, 0] = "";
////////////            for (int i = 1; i <= seq1.Length; i++)
////////////            {
////////////                if (i == 1)
////////////                {
////////////                    maxValue[i, 0] = gapStartValue;
////////////                    w1[i, 0] = gapStartValue;
////////////                    w2[i, 0] = gapStartValue;
////////////                }
////////////                else
////////////                {
////////////                    maxValue[i, 0] = maxValue[i - 1, 0] + gapContinuationValue;
////////////                    w1[i, 0] = w1[i - 1, 0] + gapContinuationValue;
////////////                    w2[i, 0] = w2[i - 1, 0] + gapContinuationValue;
////////////                }

////////////                //if (s2[i - 1, 0].EndsWith('-'))
////////////                //    maxValue[i, 0] = maxValue[i - 1, 0] + gapContinuationValue;
////////////                //else
////////////                //    maxValue[i, 0] = maxValue[i - 1, 0] + gapStartValue;


////////////                //s1[i, 0] = s1[i - 1, 0] + seq1[i - 1].ToString();
////////////                //s2[i, 0] = s2[i - 1, 0] + "-";
////////////            }
////////////            for (int i = 1; i <= seq2.Length; i++)
////////////            {

////////////                if (i == 1)
////////////                {
////////////                    maxValue[0, i] = gapStartValue;
////////////                    w1[0, i] = gapStartValue;
////////////                    w2[0, i] = gapStartValue;
////////////                }
////////////                else
////////////                {
////////////                    maxValue[0, i] = maxValue[0, i - 1] + gapContinuationValue;
////////////                    w1[0, i] = w1[0, i - 1] + gapContinuationValue;
////////////                    w2[0, i] = w2[0, i - 1] + gapContinuationValue;
////////////                }
////////////                //if (s1[0, i - 1].EndsWith('-'))
////////////                //    maxValue[0, i] = maxValue[0, i - 1] + gapContinuationValue;
////////////                //else
////////////                //    maxValue[0, i] = maxValue[0, i - 1] + gapStartValue;


////////////                //s1[0, i] = s1[0, i - 1] + "-";
////////////                //s2[0, i] = s2[0, i - 1] + seq2[i - 1].ToString();
////////////            }

////////////            for (int i = 1; i <= seq1.Length; i++)
////////////                for (int j = 1; j <= seq2.Length; j++)
////////////                {

////////////                    int left_w1, left_w2, left_opt, left_max;
////////////                    if (isGap2[i - 1, j])
////////////                        left_opt = maxValue[i - 1, j] + gapContinuationValue;
////////////                    else
////////////                        left_opt = maxValue[i - 1, j] + gapStartValue;
////////////                    left_w1 = w1[i - 1, j] + gapStartValue;
////////////                    left_w2 = w2[i - 1, j] + gapContinuationValue;
////////////                    left_max = Math.Max(Math.Max(left_w1, left_w2), left_opt);
////////////                    w2[i, j] = left_max;

////////////                    int up_w1, up_w2, up_opt, up_max;
////////////                    if (isGap1[i, j - 1])
////////////                        up_opt = maxValue[i, j - 1] + gapContinuationValue;
////////////                    else
////////////                        up_opt = maxValue[i, j - 1] + gapStartValue;
////////////                    up_w1 = w1[i, j - 1] + gapContinuationValue;
////////////                    up_w2 = w2[i, j - 1] + gapStartValue;
////////////                    up_max = Math.Max(Math.Max(up_w1, up_w2), up_opt);
////////////                    w1[i, j] = up_max;

////////////                    int accros_w1, accros_w2, accros_opt, accros_max;

////////////                    if (seq1[i - 1] == seq2[j - 1])
////////////                    {
////////////                        accros_opt = maxValue[i - 1, j - 1] + matchValue;
////////////                        accros_w1 = w1[i - 1, j - 1] + matchValue;
////////////                        accros_w2 = w2[i - 1, j - 1] + matchValue;
////////////                    }
////////////                    else
////////////                    {
////////////                        accros_opt = maxValue[i - 1, j - 1] + mismatchValue;
////////////                        accros_w1 = w1[i - 1, j - 1] + mismatchValue;
////////////                        accros_w2 = w2[i - 1, j - 1] + mismatchValue;
////////////                    }
////////////                    accros_max = Math.Max(Math.Max(accros_w1, accros_w2), accros_opt);

////////////                    int global_opt = Math.Max(Math.Max(left_max, up_max), accros_max);
////////////                    maxValue[i, j] = global_opt;

////////////                    if (global_opt == left_max)
////////////                    {
////////////                        isGap2[i, j] = true;
////////////                    }
////////////                    else if (global_opt == up_max)
////////////                    {
////////////                        isGap1[i, j] = true;
////////////                    }






////////////                    ///////////////////
////////////                    //int a;
////////////                    //if (seq1[i - 1] == seq2[j - 1])
////////////                    //    a = matchValue + maxValue[i - 1, j - 1];
////////////                    //else
////////////                    //    a = mismatchValue + maxValue[i - 1, j - 1];

////////////                    //int a2;
////////////                    //if (isGap1[i, j - 1])
////////////                    //    a2 = maxValue[i, j - 1] + gapContinuationValue;
////////////                    //else
////////////                    //    a2 = maxValue[i, j - 1] + gapStartValue;

////////////                    //int a3;
////////////                    //if (isGap2[i, j - 1])
////////////                    //    a3 = maxValue[i - 1, j] + gapContinuationValue;
////////////                    //else
////////////                    //    a3 = maxValue[i - 1, j] + gapStartValue;

////////////                    //int up, left, accros1, accros2;
////////////                    //left = t1[i, j - 1] + gapContinuationValue;
////////////                    //up = t2[i - 1, j] + gapContinuationValue;

////////////                    //if (seq1[i - 1] == seq2[j - 1])
////////////                    //    accros1 = t1[i - 1, j - 1] + matchValue;
////////////                    //else
////////////                    //    accros1 = t1[i - 1, j - 1] + mismatchValue;

////////////                    //if (seq1[i - 1] == seq2[j - 1])
////////////                    //    accros2 = t2[i - 1, j - 1] + matchValue;
////////////                    //else
////////////                    //    accros2 = t2[i - 1, j - 1] + mismatchValue;


////////////                    //int accros = Math.Max(accros1, accros2);
////////////                    //int max = Math.Max(Math.Max(a, a2), a3);
////////////                    //int up_left = Math.Max(up, left);
////////////                    //int final = Math.Max(Math.Max(max, accros), up_left);
////////////                    //t1[i, j] = Math.Max(a2,left);
////////////                    //t2[i, j] = Math.Max(a3,up);
////////////                    //if (final == t1[i, j] || final == maxValue[i-1,j] + gapContinuationValue)
////////////                    //{
////////////                    //    isGap2[i, j] = true;
////////////                    //}
////////////                    //else if (final == t2[i, j] || final == maxValue[i, j - 1] + gapContinuationValue)
////////////                    //{
////////////                    //    isGap1[i, j] = true;
////////////                    //}

////////////                    //maxValue[i, j] = final;

////////////                    ///////////////////
////////////                    //if (s1[i, j - 1].Last() == '-')
////////////                    //    up = maxValue[i, j - 1] + gapContinuationValue;
////////////                    //else
////////////                    //    up = maxValue[i, j - 1] + gapStartValue;

////////////                    //int left;
////////////                    //if (s2[i - 1, j].Last() == '-')
////////////                    //    left = maxValue[i - 1, j] + gapContinuationValue;
////////////                    //else
////////////                    //    left = maxValue[i - 1, j] + gapStartValue;


////////////                    //int max = Math.Max(Math.Max(up, left), a);
////////////                    //if (max == left)
////////////                    //{
////////////                    //    s1[i, j] = (s1[i - 1, j] + seq1[i - 1].ToString());
////////////                    //    s2[i, j] = (s2[i - 1, j] + "-");
////////////                    //}
////////////                    //else if (max == up)
////////////                    //{
////////////                    //    s1[i, j] = (s1[i, j - 1] + "-");
////////////                    //    s2[i, j] = (s2[i, j - 1] + seq2[j - 1].ToString());
////////////                    //}
////////////                    //else if (max == a)
////////////                    //{

////////////                    //    s1[i, j] = (s1[i - 1, j - 1] + seq1[i - 1].ToString());
////////////                    //    s2[i, j] = (s2[i - 1, j - 1] + seq2[j - 1].ToString());
////////////                    //}
////////////                    //Console.WriteLine("s1: "+s1[i, j]);
////////////                    //Console.WriteLine("s2: "+s2[i, j]);
////////////                    //maxValue[i, j] = max;
////////////                }
////////////            //Console.WriteLine(); ;
////////////            //for (int i = 0; i <= seq2.Length; i++)
////////////            //{
////////////            //    for (int j = 0; j <= seq1.Length; j++)
////////////            //    { Console.Write(maxValue[j, i] + "  "); }
////////////            //    Console.WriteLine(); ;
////////////            //}
////////////            //{
////////////            //Console.WriteLine();
////////////            //for (int j = 0; j <= seq2.Length; j++)
////////////            //{
////////////            //    for (int i = 0; i <= seq1.Length; i++)
////////////            //    {
////////////            //        Console.Write(String.Format("{0,4:D}", maxValue[i, j]));

////////////            //    }
////////////            //    Console.WriteLine(); ;
////////////            //}

////////////            int val = maxValue[seq1.Length, seq2.Length];
////////////            //Console.WriteLine(val);
////////////            //int aa = Math.Max(t1[seq1.Length, seq2.Length], t2[seq1.Length, seq2.Length]);
////////////            //val = Math.Max(aa, val);
////////////            //string sout1 = s1[seq1.Length, seq2.Length].ToString();
////////////            //string sout2 = s2[seq1.Length, seq2.Length].ToString();
////////////            return (null, null, val);
////////////        }
////////////    }
////////////}








//////////////////////////////////////////////////////////



//////////////using System;
//////////////using System.Collections.Generic;
//////////////using System.Text;
//////////////using System.Linq;

//////////////namespace Lab2
//////////////{
//////////////    public class DnaMatching
//////////////    {

//////////////        /// <summary>
//////////////        ///   Wariant I z prostym systemem oceny jakości dopasowania dwóch sekwencji DNA
//////////////        /// </summary>
//////////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//////////////        ///  w pierwszym etapie można zwracać nulle zamiast ciągów dopasowania </returns>

//////////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV1(string seq1, string seq2)
//////////////        {
//////////////            const int matchValue = 1;
//////////////            const int mismatchValue = -3;
//////////////            const int gapValue = -2;
//////////////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
//////////////            maxValue[0, 0] = 0;
//////////////            //StringBuilder[,,] s1 = new StringBuilder[1 + seq1.Length, 1 + seq2.Length, 2];
//////////////            //StringBuilder[,,] s2 = new StringBuilder[1 + seq1.Length, 1 + seq2.Length,2];
//////////////            //string[,] s1 = new string[1 + seq1.Length, 1 + seq2.Length];
//////////////            //string[,] s2 = new string[1 + seq1.Length, 1 + seq2.Length];
//////////////            int i, j;
//////////////            for (i = 1; i <= seq1.Length; i++)
//////////////            {
//////////////                maxValue[i, 0] = maxValue[i - 1, 0] - 2;
//////////////                //s1[i, 0, 0] = new StringBuilder();
//////////////                //s1[i, 0, 1] = new StringBuilder();
//////////////                //s2[i, 0] = new StringBuilder();
//////////////                //s1[i, 0].Append(s1[i - 1, 0]);s1[i, 0].Append(seq1[i - 1].ToString());
//////////////                //s2[i, 0].Append(s2[i - 1, 0]); s2[i, 0].Append("-");
//////////////                //s1[i, 0, 0].Append(s1[i - 1, 0, 0]); s1[i, 0, 0].Append(seq1[i - 1].ToString());
//////////////                //s1[i, 0, 1].Append(s1[i - 1, 0, 1]); s1[i, 0, 1].Append("-");
//////////////                //s1[i, 0].Append(s1[i - 1, 0]).Append(seq1[i - 1].ToString());
//////////////                //s2[i, 0].Append(s2[i - 1, 0]).Append("-"); 
//////////////                //s1[i, 0].Append(s1[i - 1, 0]+seq1[i - 1].ToString());
//////////////                //s2[i, 0].Append(s2[i - 1, 0]+"-");
//////////////                //s1[i, 0] = s1[i - 1, 0] + seq1[i - 1].ToString();
//////////////                //s2[i, 0] = s2[i - 1, 0] + "-";
//////////////            }
//////////////            for (i = 1; i <= seq2.Length; i++)
//////////////            {
//////////////                maxValue[0, i] = maxValue[0, i - 1] - 2;
//////////////                //s1[0, i, 0] = new StringBuilder();
//////////////                //s1[0, i, 1] = new StringBuilder();
//////////////                //s1[0, i].Append(s1[0, i - 1]).Append("-");
//////////////                //s2[0, i].Append(s2[0, i - 1]).Append(seq2[i - 1].ToString());
//////////////                //s1[0, i, 0].Append(s1[0, i - 1, 0]); s1[0, i, 0].Append("-");
//////////////                //s1[0, i, 1].Append(s1[0, i - 1, 1]); s1[0, i, 1].Append(seq2[i - 1].ToString());
//////////////                //s2[0, i].Append(s2[0, i - 1] + seq2[i - 1].ToString());
//////////////                //s1[0, i].Append(s1[0, i - 1] + "-");
//////////////                //s2[0, i] = s2[0, i - 1] + seq2[i - 1].ToString();
//////////////                //s1[0, i] = s1[0, i - 1] + "-";
//////////////            }

//////////////            for (i = 1; i <= seq1.Length; i++)
//////////////            {
//////////////                for (j = 1; j <= seq2.Length; j++)
//////////////                {
//////////////                    int a;
//////////////                    if (seq1[i - 1] == seq2[j - 1])
//////////////                        a = matchValue + maxValue[i - 1, j - 1];
//////////////                    else
//////////////                        a = mismatchValue + maxValue[i - 1, j - 1];

//////////////                    int max = Math.Max(Math.Max(maxValue[i, j - 1] + gapValue, maxValue[i - 1, j] + gapValue), a);
//////////////                    //s1[i, j, 0] = new StringBuilder();
//////////////                    //s1[i, j, 1] = new StringBuilder();
//////////////                    if (max == a)
//////////////                    {
//////////////                        //s1[i, j].Append(s1[i - 1, j - 1]); s1[i, j].Append(seq1[i - 1].ToString());
//////////////                        //s2[i, j].Append(s2[i - 1, j - 1]); s2[i, j].Append(seq2[j - 1].ToString());

//////////////                        //s1[i, j, 0].Append(s1[i - 1, j - 1, 0]); s1[i, j, 0].Append(seq1[i - 1].ToString());
//////////////                        //s1[i, j, 1].Append(s1[i - 1, j - 1, 1]); s1[i, j, 1].Append(seq2[j - 1].ToString());
//////////////                        //s1[i, j].Append(s1[i - 1, j - 1]).Append(seq1[i - 1].ToString());
//////////////                        ////StringBuilder sb= new StringBuilder()
//////////////                        //s2[i, j].Append(s2[i - 1, j - 1]).Append(seq2[j - 1].ToString());
//////////////                        //s1[i, j].Append(s1[i - 1, j - 1]+seq1[i - 1].ToString());
//////////////                        ////StringBuilder sb= new StringBuilder()
//////////////                        //s2[i, j].Append(s2[i - 1, j - 1]+seq2[j - 1].ToString());
//////////////                        //s1[i, j] = (s1[i - 1, j - 1] + seq1[i - 1].ToString());
//////////////                        //s2[i, j] = (s2[i - 1, j - 1] + seq2[j - 1].ToString());
//////////////                    }
//////////////                    else if (max == maxValue[i, j - 1] + gapValue)
//////////////                    {
//////////////                        //    s1[i, j, 0].Append(s1[i, j - 1, 0]); s1[i, j, 0].Append("-");
//////////////                        //    s1[i, j, 1].Append(s1[i, j - 1, 1]); s1[i, j, 1].Append(seq2[j - 1]);

//////////////                        //s1[i, j].Append(s1[i, j - 1]).Append("-");
//////////////                        //s2[i, j].Append(s2[i, j - 1]).Append(seq2[j - 1]);
//////////////                        //s1[i, j].Append(s1[i, j - 1]); s1[i, j].Append("-");
//////////////                        //s2[i, j].Append(s2[i, j - 1]); s2[i, j].Append(seq2[j - 1]);
//////////////                        //s1[i, j].Append(s1[i, j - 1]+"-");
//////////////                        //s2[i, j].Append(s2[i, j - 1]+seq2[j - 1].ToString());
//////////////                        //s1[i, j] = (s1[i, j - 1] + "-");
//////////////                        //s2[i, j] = (s2[i, j - 1] + seq2[j - 1].ToString());
//////////////                    }
//////////////                    else
//////////////                    {
//////////////                        //s1[i, j].Append(s1[i - 1, j]); s1[i, j].Append(seq1[i - 1]);
//////////////                        //s2[i, j].Append(s2[i - 1, j]);s2[i, j].Append("-");
//////////////                        //s1[i, j, 0].Append(s1[i - 1, j, 0]); s1[i, j, 0].Append(seq1[i - 1]);
//////////////                        //s1[i, j, 1].Append(s1[i - 1, j, 1]); s1[i, j, 1].Append("-");

//////////////                        //s1[i, j].Append(s1[i - 1, j]).Append(seq1[i - 1]);
//////////////                        //s2[i, j].Append(s2[i - 1, j]).Append("-");
//////////////                        //s1[i, j].Append(s1[i - 1, j]+seq1[i - 1].ToString());
//////////////                        //s2[i, j].Append(s2[i - 1, j]+"-");
//////////////                        //s1[i, j] = (s1[i - 1, j] + seq1[i - 1].ToString());
//////////////                        //s2[i, j] = (s2[i - 1, j] + "-");
//////////////                    }
//////////////                    maxValue[i, j] = max;
//////////////                    //Console.WriteLine("(" + i + " " + j + ")");
//////////////                }
//////////////            }
//////////////            Console.WriteLine(); ;
//////////////            //for (i = 0; i <= seq2.Length; i++)
//////////////            //{
//////////////            //    for (j = 0; j <= seq1.Length; j++)
//////////////            //    { Console.Write(maxValue[j, i] + "  "); }
//////////////            //    Console.WriteLine(); ;

//////////////            //}
//////////////            string out1 = "";
//////////////            string out2 = "";


//////////////            int sum = maxValue[seq1.Length, seq2.Length];
//////////////            i = seq1.Length; j = seq2.Length;

//////////////            while (i != 0 || j != 0)
//////////////            {
//////////////                int up, left;
//////////////                if (j - 1 < 0)
//////////////                    up = -int.MaxValue;
//////////////                else
//////////////                    up = maxValue[i, j - 1] + gapValue;
//////////////                if (i - 1 < 0)
//////////////                    left = -int.MaxValue;
//////////////                else
//////////////                    left = maxValue[i - 1, j] + gapValue;
//////////////                int a;
//////////////                if (i - 1 >= 0 && j - 1 >= 0)
//////////////                {
//////////////                    if (seq1[i - 1] == seq2[j - 1])
//////////////                        a = maxValue[i - 1, j - 1] + 1;
//////////////                    else
//////////////                        a = maxValue[i - 1, j - 1] - 3;
//////////////                }
//////////////                else a = -int.MaxValue;
//////////////                int max = Math.Max(Math.Max(up, left), a);

//////////////                //Console.WriteLine("--------------------");
//////////////                if (max == a)
//////////////                {
//////////////                    out1 += (seq1[i - 1]);
//////////////                    out2 += (seq2[j - 1]);
//////////////                    if (seq1[i - 1] == seq2[j - 1])
//////////////                        sum += 3;
//////////////                    else
//////////////                        sum -= 1; ;
//////////////                    j--;
//////////////                    i--;
//////////////                }
//////////////                else if (max == up)
//////////////                {
//////////////                    out1 += '-';
//////////////                    out2 += seq2[j - 1];
//////////////                    sum += 2;
//////////////                    j--;
//////////////                }
//////////////                else
//////////////                {
//////////////                    sum += 2;
//////////////                    out1 += seq1[i - 1];
//////////////                    out2 += '-';
//////////////                    i--;
//////////////                }
//////////////            }


//////////////            int val = maxValue[seq1.Length, seq2.Length];
//////////////            //string sout1 = s1[seq1.Length, seq2.Length].ToString();
//////////////            //string sout2 = s2[seq1.Length, seq2.Length].ToString();
//////////////            char[] c1 = out1.ToCharArray();
//////////////            char[] c2 = out2.ToCharArray();
//////////////            Array.Reverse(c1);
//////////////            Array.Reverse(c2);
//////////////            out1 = new string(c1);
//////////////            out2 = new string(c2);
//////////////            //out2=out2.Trim();
//////////////            //Console.WriteLine("s1:" + seq1 + "\ns2:" + seq2);
//////////////            //Console.WriteLine("s1:" + sout1 + "\ns2:" + sout2);
//////////////            //Console.WriteLine("s1:" + out1 + "\ns2:" + out2);
//////////////            return (out1, out2, val);
//////////////        }


//////////////        /// <summary>
//////////////        ///   Wariant II z zaawansowanym systemem oceny jakości dopasowania dwóch sekwencji DNA
//////////////        /// </summary>
//////////////        /// <param name="seq1"> pierwsza niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////////        /// <param name="seq2"> druga niepusta sekwencja DNA złożona ze znaków 'A', 'C', 'G', 'T'</param>
//////////////        /// <returns>(dopasowanie [ciąg 1], dopasowanie [ciąg 2], wartość całego dopasowania). 
//////////////        ///  w trzecim etapie można zwracać nulle zamiast ciągów dopasowania </returns>
//////////////        public (string matchingSeq1, string matchingSeq2, int bestMatchingValue) FindMatchingV2(string seq1, string seq2)
//////////////        {
//////////////            const int matchValue = 1;
//////////////            const int mismatchValue = -3;
//////////////            const int gapStartValue = -5;
//////////////            const int gapContinuationValue = -2;
//////////////            int[,] maxValue = new int[1 + seq1.Length, 1 + seq2.Length];
//////////////            maxValue[0, 0] = 0;
//////////////            string[,] s1 = new string[1 + seq1.Length, 1 + seq2.Length];
//////////////            string[,] s2 = new string[1 + seq1.Length, 1 + seq2.Length];
//////////////            s1[0, 0] = "";
//////////////            s2[0, 0] = "";
//////////////            for (int i = 1; i <= seq1.Length; i++)
//////////////            {
//////////////                if (s2[i - 1, 0].EndsWith('-'))
//////////////                    maxValue[i, 0] = maxValue[i - 1, 0] + gapContinuationValue;
//////////////                else
//////////////                    maxValue[i, 0] = maxValue[i - 1, 0] + gapStartValue;


//////////////                s1[i, 0] = s1[i - 1, 0] + seq1[i - 1].ToString();
//////////////                s2[i, 0] = s2[i - 1, 0] + "-";
//////////////            }
//////////////            for (int i = 1; i <= seq2.Length; i++)
//////////////            {
//////////////                if (s1[0, i - 1].EndsWith('-'))
//////////////                    maxValue[0, i] = maxValue[0, i - 1] + gapContinuationValue;
//////////////                else
//////////////                    maxValue[0, i] = maxValue[0, i - 1] + gapStartValue;


//////////////                s1[0, i] = s1[0, i - 1] + "-";
//////////////                s2[0, i] = s2[0, i - 1] + seq2[i - 1].ToString();
//////////////            }

//////////////            for (int i = 1; i <= seq1.Length; i++)
//////////////                for (int j = 1; j <= seq2.Length; j++)
//////////////                {
//////////////                    int a;
//////////////                    if (seq1[i - 1] == seq2[j - 1])
//////////////                        a = matchValue + maxValue[i - 1, j - 1];
//////////////                    else
//////////////                        a = mismatchValue + maxValue[i - 1, j - 1];

//////////////                    int up;
//////////////                    if (s1[i, j - 1].Last() == '-')
//////////////                        up = maxValue[i, j - 1] + gapContinuationValue;
//////////////                    else
//////////////                        up = maxValue[i, j - 1] + gapStartValue;

//////////////                    int left;
//////////////                    if (s2[i - 1, j].Last() == '-')
//////////////                        left = maxValue[i - 1, j] + gapContinuationValue;
//////////////                    else
//////////////                        left = maxValue[i - 1, j] + gapStartValue;


//////////////                    int max = Math.Max(Math.Max(up, left), a);
//////////////                    if (max == left)
//////////////                    {
//////////////                        s1[i, j] = (s1[i - 1, j] + seq1[i - 1].ToString());
//////////////                        s2[i, j] = (s2[i - 1, j] + "-");
//////////////                    }
//////////////                    else if (max == up)
//////////////                    {
//////////////                        s1[i, j] = (s1[i, j - 1] + "-");
//////////////                        s2[i, j] = (s2[i, j - 1] + seq2[j - 1].ToString());
//////////////                    }
//////////////                    else if (max == a)
//////////////                    {

//////////////                        s1[i, j] = (s1[i - 1, j - 1] + seq1[i - 1].ToString());
//////////////                        s2[i, j] = (s2[i - 1, j - 1] + seq2[j - 1].ToString());
//////////////                    }
//////////////                    //Console.WriteLine("s1: "+s1[i, j]);
//////////////                    //Console.WriteLine("s2: "+s2[i, j]);
//////////////                    maxValue[i, j] = max;
//////////////                }
//////////////            //Console.WriteLine(); ;
//////////////            //for (int i = 0; i <= seq2.Length; i++)
//////////////            //{
//////////////            //    for (int j = 0; j <= seq1.Length; j++)
//////////////            //    { Console.Write(maxValue[j, i] + "  "); }
//////////////            //    Console.WriteLine(); ;
//////////////            //}
//////////////            //{
//////////////            //for (int j = 0; j <= seq2.Length; j++)
//////////////            //{
//////////////            //    for (int i = 0; i <= seq1.Length; i++)
//////////////            //    { Console.Write(maxValue[i, j] + "  "); }
//////////////            //    Console.WriteLine(); ;
//////////////            //}
//////////////            int val = maxValue[seq1.Length, seq2.Length];
//////////////            string sout1 = s1[seq1.Length, seq2.Length].ToString();
//////////////            string sout2 = s2[seq1.Length, seq2.Length].ToString();
//////////////            return (sout1, sout2, val);
//////////////        }
//////////////    }
//////////////}