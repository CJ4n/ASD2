//using System;
//using System.Linq;
//using System.Collections.Generic;

//namespace ASD
//{
//    public class SheepHeard : MarshalByRefObject
//    {
//        public int Cross((double, double) o, (double, double) a, (double, double) b)
//        {
//            double value = (a.Item1 - o.Item1) * (b.Item2 - o.Item2) - (a.Item2 - o.Item2) * (b.Item1 - o.Item1);
//            return Math.Abs(value) < 1e-10 ? 0 : value < 0 ? -1 : 1;
//        }
//        public (double, double)[] ConvexHull((double, double)[] points)
//        {
//            var asList = new List<(double, double)>(new HashSet<(double, double)>(points));
//            asList.Sort();

//            if (asList.Count <= 3)
//                return null;

//            var lowerList = new List<(double, double)>();
//            foreach ((double, double) p in asList)
//            {
//                while (lowerList.Count >= 2 && Cross(lowerList[lowerList.Count - 2], lowerList[lowerList.Count - 1], p) <= 0)
//                {
//                    lowerList.RemoveAt(lowerList.Count - 1);
//                }
//                lowerList.Add(p);
//            }

//            asList.Reverse();
//            var upperList = new List<(double, double)>();
//            foreach ((double, double) p in asList)
//            {
//                while (upperList.Count >= 2 && Cross(upperList[upperList.Count - 2], upperList[upperList.Count - 1], p) <= 0)
//                {
//                    upperList.RemoveAt(upperList.Count - 1);
//                }
//                upperList.Add(p);
//            }

//            lowerList.RemoveAt(lowerList.Count - 1);
//            upperList.RemoveAt(upperList.Count - 1);

//            lowerList.AddRange(upperList);
//            return lowerList.ToArray();
//        }

//        public (double x, double y) OrthogonalProjection((float A, float B, float C) shed, (double x, double y) p)
//        {
//            if (shed.B == 0)
//                return (-(shed.C / shed.A), p.y);
//            if (shed.A == 0)
//                return (p.x, -shed.C / shed.B);
//            double x = (-shed.A * shed.B * p.y + shed.B * shed.B * p.x - shed.C * shed.A) / (shed.B * shed.B + shed.A * shed.A);
//            double y = (-shed.A * x - shed.C) / shed.B;

//            return (x, y);
//        }
//        /// <param name="dogs">Tablica opisującapozycje psow</param>
//        /// <param name="shed">Pozycja sciany budynku gospodarczego</param>
//        /// <returns>Wierzchołki wielokatku w którym owce są bezpieczne</returns>
//        public (double x, double y)[] SafePolygon((double x, double y)[] dogs, (float A, float B, float C) shed)
//        {
//            if (dogs.Length < 1) return null;

//            (double, double) minX = (double.MaxValue, double.MaxValue);
//            (double, double) minY = (double.MaxValue, double.MaxValue);
//            (double, double) maxX = (double.MinValue, double.MinValue);
//            (double, double) maxY = (double.MinValue, double.MinValue);
//            foreach (var d in dogs) // n
//            {
//                (double x, double y) p = OrthogonalProjection(shed, d);
//                if (minX.Item1 > p.x)
//                    minX = p;
//                if (minY.Item2 > p.y)
//                    minY = p;
//                if (maxX.Item1 < p.x)
//                    maxX = p;
//                if (maxY.Item2 < p.y)
//                    maxY = p;
//            }
//            var points = new (double, double)[dogs.Length + 4];
//            dogs.CopyTo(points, 0);
//            points[dogs.Length] = minX;
//            points[dogs.Length + 1] = minY;
//            points[dogs.Length + 2] = maxX;
//            points[dogs.Length + 3] = maxY;

//            return ConvexHull(points); // O(nlogn)
//        }

//        //public bool IsPointInPolygon((double x, double y)[] polygon, (double x, double y) testPoint)
//        //{
//        //    if (polygon.Length == 3)// czy na pewno zawzxe tak bedze??? wychodzo ze chyba tak
//        //    {
//        //        // polygon.Length==3 ZAWSZE -> stała złożonoś
//        //        //bool result = false;
//        //        int j = polygon.Length - 1;
//        //        for (int i = 0; i < polygon.Length; i++)
//        //        {
//        //            //if (polygon[i].y < testPoint.y && polygon[j].y >= testPoint.y || polygon[j].y < testPoint.y && polygon[i].y >= testPoint.y)
//        //            //{
//        //            //    if (polygon[i].x + (testPoint.y - polygon[i].y) / (polygon[j].y - polygon[i].y) * (polygon[j].x - polygon[i].x) < testPoint.x)
//        //            //    {
//        //            //        result = !result;
//        //            //    }
//        //            //}
//        //            var crossRet = Cross(testPoint, polygon[j], polygon[i]);
//        //            if (crossRet < 0)
//        //                return false;
//        //            else if (crossRet == 0) // sprawdzam czy punkt lezy na odcinku
//        //            {
//        //                (double x, double y) = polygon[j];
//        //                (double x, double y) tmp = polygon[i];
//        //                double AB = Math.Sqrt((tmp.x - x) * (tmp.x - x) + (tmp.y - y) * (tmp.y - y));
//        //                double AP = Math.Sqrt((testPoint.x - x) * (testPoint.x - x) + (testPoint.y - y) * (testPoint.y - y));
//        //                double PB = Math.Sqrt((tmp.x - testPoint.x) * (tmp.x - testPoint.x) + (tmp.y - testPoint.y) * (tmp.y - testPoint.y));
//        //                if (Math.Abs(AB - AP - PB) < 1e-10)
//        //                    return true;
//        //                return false;// ??
//        //            }

//        //            j = i;
//        //        }
//        //        return true;
//        //        //if (result)
//        //        //    return result;
//        //        //(double x, double y) p = polygon[polygon.Length - 1];
//        //        //for (int i = 0; i < polygon.Length; i++)
//        //        //{
//        //        //    (double x, double y) tmp = polygon[i];
//        //        //    double AB = Math.Sqrt((tmp.x - p.x) * (tmp.x - p.x) + (tmp.y - p.y) * (tmp.y - p.y));
//        //        //    double AP = Math.Sqrt((testPoint.x - p.x) * (testPoint.x - p.x) + (testPoint.y - p.y) * (testPoint.y - p.y));
//        //        //    double PB = Math.Sqrt((tmp.x - testPoint.x) * (tmp.x - testPoint.x) + (tmp.y - testPoint.y) * (tmp.y - testPoint.y));
//        //        //    if (AB == AP + PB)
//        //        //        return true;
//        //        //    p = tmp;
//        //        //}
//        //        //return false;
//        //    }

//        //    var ret = Cross(testPoint, polygon[0], polygon[polygon.Length / 2]);
//        //    if (ret < 0)
//        //    {
//        //        var t = polygon.Take((polygon.Length / 2 + 1)).ToArray();
//        //        return IsPointInPolygon(t, testPoint);
//        //        //return IsPointInPolygon(polygon[0..(polygon.Length / 2 + 1)], testPoint);
//        //    }
//        //    else if (ret > 0)
//        //    {
//        //        var t = polygon.Skip((polygon.Length / 2)).Take(polygon.Length - (polygon.Length / 2)).ToArray();
//        //        var r = new (double, double)[1 + t.Length];
//        //        t.CopyTo(r, 0);
//        //        r[t.Length] = polygon[0];
//        //        //var res = polygon[0..1].Concat(polygon[(polygon.Length / 2)..polygo   n.Length]).ToArray();
//        //        return IsPointInPolygon(r, testPoint);
//        //    }
//        //    else
//        //    {
//        //        (double x, double y) = polygon[polygon.Length / 2];
//        //        (double x, double y) tmp = polygon[0];
//        //        double AB = Math.Sqrt((tmp.x - x) * (tmp.x - x) + (tmp.y - y) * (tmp.y - y));
//        //        double AP = Math.Sqrt((testPoint.x - x) * (testPoint.x - x) + (testPoint.y - y) * (testPoint.y - y));
//        //        double PB = Math.Sqrt((tmp.x - testPoint.x) * (tmp.x - testPoint.x) + (tmp.y - testPoint.y) * (tmp.y - testPoint.y));
//        //        if (Math.Abs(AB - AP - PB) < 1e-10)// trzeba pamietac ze to sa bledy numeryczne co moze psuc program z dziwnego powodu
//        //            return true;
//        //        return false;
//        //    }
//        //}



//        // ALgorytm sprawdzania czy punkt nadleży do wielokąta, działający w O(logn). Działam na indeksach elementów w polygon, a nie
//        // na polygon bo jest to znacznie wydajniejsze. Dziele przedział [s,..,e] na pół i biorę ten podprzedział, który zawiera testPoint
//        // ostatecznie dostaje trójkąt składający się z indeksów: 0,s,e. Gdzei s,e sąsiadują z sobą
//        public bool IsPointInPolygon((double x, double y)[] polygon, (double x, double y) testPoint)
//        {
//            int s = 0, e = polygon.Length - 1;// s - początek przedzedział, który rozpatruje, e - koniec przedziału, który rozpatruje
//            while (true)
//            {
//                if (Math.Abs(s - e) == 1) // stała złożoność bo algorytm działa zawsze na 3 punktach/odcinkach
//                {
//                    int[] indexs = { 0, s, e };
//                    int j = e;
//                    foreach (var i in indexs)
//                    {
//                        var crossRet = Cross(testPoint, polygon[j], polygon[i]);
//                        if (crossRet < 0) // jeżeli punkt leży na prawo od odcinka to leży poza trójkątem
//                            return false;
//                        else if (crossRet == 0) // sprawdzam czy punkt lezy na odcinku
//                        {
//                            (double x, double y) p2 = polygon[j];
//                            (double x, double y) tmp = polygon[i];
//                            if (p2.x < tmp.x)
//                            {
//                                var tmp2 = tmp;
//                                tmp = p2;
//                                p2 = tmp2;
//                            }
//                            if (p2.x >= testPoint.x && testPoint.x >= tmp.x)
//                            {
//                                if (p2.y < tmp.y)
//                                {
//                                    var tmp2 = tmp;
//                                    tmp = p2;
//                                    p2 = tmp2;
//                                }
//                                if (p2.y >= testPoint.y && testPoint.y >= tmp.y)
//                                    return true;
//                            }
//                            return false; // jezeli crossRet == 0 i punkt nie leży na odcinku to leże gdzieś na przedłużeniu go, czyli poza wielokątem
//                        }
//                        j = i;
//                    } // jak zawsze leży na lewo od odcinka to leży wewnątrz wielokąta
//                    return true;
//                }
//                // dziele dostępny przedział na pół i biorę ten który zawiera testPoint
//                var ret = Cross(testPoint, polygon[0], polygon[(e + s + 1) / 2]);// dziele przedział [s,e] na dwie części prostą 0_(e+s+1)/2, czyli od początku polygon do środka przedziału [s,..,e]
//                if (ret < 0) // przedział [s,.., (e+s+1)/2], więcj poprawiam koniec przedziału
//                    e = ((e + s + 1) / 2);
//                else if (ret > 0) // przedział [(e+s+1)/2, .. , e], więcj poprawiam początek przedziału
//                    s = ((e + s + 1) / 2);
//                else // jeżli punkt leży na prostej przechodzącej przez 0, (e+s+1)/2, czyli potencialnie wewnątrze wielokątu, to sprawdzam czy testPoint zawiera się w tej prostej
//                {
//                    (double x, double y) p2 = polygon[(e + s + 1) / 2];
//                    (double x, double y) p1 = polygon[0];
//                    if (p2.x < p1.x)
//                    {
//                        var tmp2 = p1;
//                        p1 = (p2.x, p2.y);
//                        (p2.x, p2.y) = tmp2;
//                    }
//                    if (p2.x >= testPoint.x && testPoint.x >= p1.x)
//                    {
//                        if (p2.y < p1.y)
//                        {
//                            var tmp2 = p1;
//                            p1 = p2;
//                            p2 = tmp2;
//                        }
//                        if (p2.y >= testPoint.y && testPoint.y >= p1.y)
//                            return true;
//                    }
//                    return false;
//                }
//            }
//        }

//        /// <param name="sheeps">Tablica opisującapozycje owiec</param>
//        /// <param name="dogs">Tablica opisującapozycje psow</param>
//        /// <param name="shed">Pozycja sciany budynku gospodarczego</param>
//        /// <returns>Liczba bezpiecznych owiec</returns>
//        public int CheckCoverage((double x, double y)[] sheeps, (double x, double y)[] dogs, (float A, float B, float C) shed) // O(nlogn + mlogn)
//        {
//            var safaArea = SafePolygon(dogs, shed); // O(nlogn)
//            if (safaArea == null) return 0;
//            int sum = 0;
//            foreach (var s in sheeps)// m
//                if (IsPointInPolygon(safaArea, s)) // O(logn)
//                    sum++;

//            return sum;
//        }
//    }
//}