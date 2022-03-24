using ASD.Graphs;
using System;

namespace ASD
{
    public class Lab04 : System.MarshalByRefObject
    {
        public (bool result, int[] route) Lab04_FindRoute(DiGraph<int> g, int start_v, int end_v, int day, int days_number)
        {
            // Algorytm to tak właściwei zmodyfikowany DFS.

            DiGraph<int> cpy = (DiGraph<int>)g.Clone();
            // Kopia grafu wejściowego, będę z niej usuwać krawędzie, żeby zaznaczyć którymi krawędziami już przeszedłem.
            // Nigdy nie chcemy przejść tą samą krawędzią więcej niż raz, bo wtedy lądujemy w tym samym mieście w tym samym dniu, co nam nic nie daje.
            // To było by chyba źle, jeżeli mielimyśmy takie trasy/krawędze, że pociąg odjeżdzą z nich co dziennie, wtedy można by tak chodzić w cyklu,
            // że byśmy mogli dosać dowlony dzień na nim, więc zejść z cyklu w dowolny dzień, z dowolnego miasta.
            // Kolejna kwestja, że w ten sposób zmniejszam ilość sąsiadów do przeiterowanie, co poprawia wydajność, a nie psuje algortmu,
            // jest to wydajniejsze niż np.zmienianie wagi krawędzie na -1 i na tej podstawie stwierdzanie czy możemy nią przejść

            Stack<int> path = new Stack<int>();
            // Na stosie trzymam te wierzchołki, po których już przeszedłem dochodząc do obecnego wierzchołka (tak aby zapamiętać/tworzyć trase jakbyśmy
            // używali rekurencji.

            int[,] deadEnd = new int[g.VertexCount, days_number];
            // Pamiętam, że byłem w danym wierzchołu w dany dzień i nic z tego nie wyszło, tj. nie udało się dojść do celu będąc w tym mieście w tym dniu,
            // więc nie chcemy odwiedzać tego wierzchołka również kiedy dochodzimy do niego jakąś jaką inną krawędzią, ale w tym samym dniu.

            int currentCity = start_v;
            int currentDay = day;
            path.Push(currentCity);
            while (path.Count != 0 && currentCity != end_v)
            {
                int city = path.Peek();
                int tmpDay = currentDay;
                // Jak już bylimsy w tym mieście w tym dniu i nic z tego nie wyszło, to się cofam (visited.pop()).
                if (deadEnd[city, currentDay] == 0)
                    // Przechodze po sąsiadach city ALE w grafie cpy NIE g!!! poniweaż wtedy foreach wykona się mniej razy niż jakbym oznaczał krawędzie,    
                    // które przeszedłem (np. zmieniał jej wage) bo tych krawędzi po prostu już nie będzie.
                    foreach (var e in cpy.OutEdges(city))
                    {
                        if (currentDay == e.weight)
                        {
                            currentCity = e.To;
                            currentDay = (currentDay + 1) % days_number;
                            break;
                        }
                    }
                if (tmpDay == currentDay)
                {
                    deadEnd[city, currentDay] = 1; // Zaznaczam, że byłem w city w currentDay i nic z tego nie wyszło.
                    currentDay--;
                    if (currentDay == -1)
                        currentDay = days_number - 1;
                    path.Pop();
                }
                else
                {
                    cpy.RemoveEdge(city, currentCity); // Zaznaczam, że już tą krawędzią przeszedłem, więc nie chce tego powtarzać.
                    path.Push(currentCity);
                }
            }
            bool ret;
            if (path.Count != 0)
                ret = path.Peek() == end_v;
            else ret = false;
            int[] pathArray;
            if (ret == true) // Przerabiam stos na tablice i ją odwracam.
            {
                pathArray = path.ToArray();
                Array.Reverse(pathArray);
            }
            else
                pathArray = null;
            return (ret, pathArray);
        }

        /// <summary>
        /// Etap 2 - szukanie trasy z jednego z miast z tablicy start_v do jednego z miast z tablicy end_v (startować można w dowolnym dniu)
        /// </summary>
        /// <param name="g">Ważony graf skierowany będący mapą</param>
        /// <param name="start_v">Tablica z indeksami wierzchołków startowych (trasę trzeba zacząć w jednym z nich)</param>
        /// <param name="end_v">Tablica z indeksami wierzchołków docelowych (trasę trzeba zakończyć w jednym z nich)</param>
        /// <param name="days_number">Liczba dni uwzględnionych w rozkładzie (tzn. wagi krawędzi są z przedziału [0, days_number-1])</param>
        /// <returns>(result, route) - result ma wartość true gdy podróż jest możliwa, wpp. false, 
        /// route to tablica z indeksami kolejno odwiedzanych miast (pierwszy indeks to indeks miasta startowego, ostatni to indeks miasta docelowego),
        /// jeżeli result == false to route ustawiamy na null</returns>
        public (bool result, int[] route) Lab04_FindRouteSets(DiGraph<int> g, int[] start_v, int[] end_v, int days_number)
        {
            // Alogytm jest prawie taki sam jak w etapie 1. Głowne zmiany do dotatkowe dwie pętle for (w odpowiedniej kolejności) oraz sprawdzanie, większej liczby miast końcowych.
            // 1. Iteruje po miastach początkowych i po wszytkich dniach., ALE pamiętając zdobyte informacje z wcześniejszych iteracji tych pętli (jak jesteśmy w jakimś miejści 
            // w danym dniu to maisto startowe jest bez znaczenia, podobnie nie ma znaczenia którego dnia wyruszyliśmy, jak jesteśmy w jakims mieście w jakimś dniu).
            // 3. Wywołuje algorytm z etapu 1 (z drobnymi zmianami) dla każdego miasta startowego i dla każdego dnia (ale każde kolejne wywyołanie jest wykonywame na mniejszym grafie
            // niż poprzednie bo mamy doświadczenie z poprzednich iteracji.
            // 4. Konwertujemy stos na tablice (jeżeli udało się znaleść ścieżke do miasta końcowego).
            // Ogólnie używanie tej samej kopi grafu przez cały algorytm sprawia, że nie sprawdzamy dwa razy tej samej złej drogi, więc efektywnie mamy taką samą maksymalną złożoność jak w etapie 1.

            DiGraph<int> cpy = (DiGraph<int>)g.Clone();
            // Kopia grafu wejściowego, będę z niej usuwać krawędzie, żeby zaznaczyć którymi krawędziami już przeszedłem.
            // Nigdy nie chcemy przejść tą samą krawędzią więcej niż raz, bo wtedy lądujemy w tym samym mieście w tym samym dniu, co nam nic nie daje.
            // To było by chyba źle, jeżeli mielimyśmy takie trasy/krawędze (ALE tylko kilka, w sensie nie wszytkie krawędzie mają taką własność,
            // jeśli wszystkie mają taką własność to zadanie sprowadza się do znalezienia dowolnej ściżeki między miastami), że pociąg odjeżdzą
            // z nich co dziennie, wtedy można by tak chodzić w cyklu, że byśmy mogli dosać dowlony dzień na nim, więc zejść z cyklu w dowolny dzień,
            // z dowolnego miasta. Kolejna kwestja, że w ten sposób zmniejszam ilość sąsiadów do przeiterowanie, co poprawia wydajność,
            // a nie psuje algortmu, jest to wydajniejsze niż np. zmienianie wagi krawędzie na -1 i na tej podstawie stwierdzanie czy możemy nią przejść

            Stack<int> path = new Stack<int>();
            // Na stosie trzymam te wierzchołki, po których już przeszedłem dochodząc do obecnego wierzchołka (tak aby zapamiętać/tworzyć trase jakbyśmy używali rekurencji.

            int[,] deadEnd = new int[g.VertexCount, days_number];
            // Pamiętam, że byłem w danym wierzchołu w dany dzień i nic z tego nie wyszło, tj. nie udało się dojść do celu będąc w tym mieście w tym dniu.

            // Iteruje po miastch startowych i dniach startu. Co ważne, zawsze używam tego samego grafu. To znaczy, jeżeli startujemy z pierwszego  miasta i nie znadjdujemy końca,
            // to i tak mamy już trochę inforamcji (tak samo dla każdego z dni startowych), tj. które krawędzie nic nie dają, i gdzie nie warto już chodzić. Zmiana miasta startowego 
            // wpływa tylko na rózne miejsca startu, ale jak już jesteśmy w jakimś mieście to nie ma znaczenia skąd startowaliśmy, więc informacje zebrane  w wcześniejszych iteracjach 
            //są nadal aktualne. Tak samo dzień startu wpływa tylko na dzień startu, jak jesteśmy już w trasie w jakimś dniu to jest to bez znaczenia jakiego dnia zaczeliśy naszą podróż.
            foreach (var start_city in start_v)
            {
                for (int startDay = 0; startDay < days_number; startDay++)
                {
                    int currentCity = start_city;
                    int currentDay = startDay;
                    path.Push(currentCity);
                    bool isEndCity = false;
                    while (path.Count != 0)
                    {
                        isEndCity = false;
                        foreach (var endCity in end_v) // Sprawdzam, czy przypadkiem nie jestem w jakimś mieście końcow
                        {
                            if (endCity == currentCity)
                            {
                                isEndCity = true;
                                break;
                            }
                        }
                        if (isEndCity) break;
                        int city = path.Peek();
                        int tmpDay = currentDay;
                        if (deadEnd[city, currentDay] == 0) // Czy byliśmy już w tym mieście w tym dniu?ym.
                        {
                            foreach (var e in cpy.OutEdges(city))
                            {
                                if (currentDay == e.weight)
                                {
                                    currentCity = e.To;
                                    currentDay = (currentDay + 1) % days_number;
                                    break;
                                }
                            }
                        }
                        if (tmpDay == currentDay)
                        {
                            deadEnd[city, currentDay] = 1; // Zaznaczam, że byłem w city w currentDay i nic z tego nie wyszło.
                            currentDay--;
                            if (currentDay == -1)
                                currentDay = days_number - 1;
                            path.Pop();
                        }
                        else
                        {
                            cpy.RemoveEdge(city, currentCity); // Zaznaczam, że już tą krawędzią przeszedłem, więc nie chce tego powtarzać
                            path.Push(currentCity);
                        }
                    }
                    if (isEndCity) // Stos -> tablice
                    {
                        int[] pathArray;
                        pathArray = path.ToArray();
                        Array.Reverse(pathArray);
                        return (true, pathArray);
                    }
                }
            }
            return (false, null);
        }
    }
}
