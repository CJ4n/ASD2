using System;
using System.Collections.Generic;
using System.Linq;
using ASD.Graphs;

namespace ASD_lab08
{
    [Serializable]
    public struct Cat
    {
        /// <summary>
        /// Zawiera identyfikatory osób, które kot zaakceptuje
        /// </summary>
        public int[] AcceptablePeople { get; }

        public Cat(int[] acceptablePeople)
        {
            AcceptablePeople = acceptablePeople;
        }
    }

    [Serializable]
    public struct Person
    {
        /// <summary>
        /// Maksymalna liczba kotów, którymi zajmie się opiekun
        /// </summary>
        public int MaxCats { get; }

        /// <summary>
        /// Kwoty, które osoba życzy sobie za opiekę nad kotami (catId -> int)
        /// </summary>
        public int[] Salaries { get; }

        public Person(int maxCats, int[] salaries)
        {
            MaxCats = maxCats;
            Salaries = salaries;
        }
    }

    public class Cats : MarshalByRefObject
    {
        /// <summary>
        /// Zadanie pierwsze, w którym nie bierzemy pod uwagę pieniędzy jakie nam przyjdzie zapłacić opiekunom
        /// </summary>
        /// <param name="cats">Tablica zawierające nasze koty</param>
        /// <param name="people">Tablica zawierająca dostępnych opiekunów</param>
        /// <returns>
        /// isPossible: wartość logiczna oznaczająca, czy przypisanie jest możliwe, 
        /// assignment: przypisanie kotów do opiekunów (personId -> [catId])
        /// </returns>
        public (bool isPossible, int[][] assignment) StageOne(Cat[] cats, Person[] people)
        {
            NetworkWithCosts<int, int> g = new NetworkWithCosts<int, int>(people.Length + cats.Length + 2);
            int s = people.Length + cats.Length;
            int t = people.Length + cats.Length + 1;
            int catsStart = people.Length;
            for (int p = 0; p < people.Length; p++)
            {
                g.AddEdge(s, p, people[p].MaxCats, 0);
            }
             
            for (int c = 0; c < cats.Length; c++)
            {
                g.AddEdge(catsStart + c, t, 1, 0);
                foreach (var p in cats[c].AcceptablePeople)
                {
                    g.AddEdge(p, catsStart + c, 1, 0);
                }
            }
            var flow = Flows.MinCostMaxFlow(g, s, t);

            if (flow.Item1 == cats.Length)
            {

                int[][] assignment = new int[people.Length][];
                for(int p = 0; p < people.Length; p++)
                {
                    assignment[p] = new int[flow.Item3.OutEdges(p).Count()];
                    int i = 0;
                    foreach (var c in flow.Item3.OutEdges(p))
                        assignment[p][i++]=c.To-catsStart;
                }
                return (true, assignment);
            }

            return (false, null);
        }

        /// <summary>
        /// Zadanie drugie, w którym bierzemy pod uwagę kwoty jakie nam przyjdzie zapłacić
        /// </summary>
        /// <param name="cats">Tablica zawierające nasze koty</param>
        /// <param name="people">Tablica zawierająca dostępnych opiekunów</param>
        /// <returns>
        /// isPossible: wartość logiczna oznaczająca, czy przypisanie jest możliwe,
        /// assignment: przypisanie kotów do opiekunów (personId -> [catId]),
        /// minCost: minimalna suma pieniędzy do zapłacenia opiekunom za opiekę nad wszystkimi kotami
        /// </returns>
        public (bool isPossible, int[][] assignment, int minCost) StageTwo(Cat[] cats, Person[] people)
        {

            NetworkWithCosts<int, int> g = new NetworkWithCosts<int, int>(people.Length + cats.Length + 2);
            int s = people.Length + cats.Length;
            int t = people.Length + cats.Length + 1;
            int catsStart = people.Length;
            for (int p = 0; p < people.Length; p++)
            {
                g.AddEdge(s, p, people[p].MaxCats, 0);
            }

            for (int c = 0; c < cats.Length; c++)
            {
                g.AddEdge(catsStart + c, t, 1, 0);
                foreach (var p in cats[c].AcceptablePeople)
                {
                    g.AddEdge(p, catsStart + c, 1, people[p].Salaries[c]);
                }
            }
            var flow = Flows.MinCostMaxFlow(g, s, t);

            if (flow.Item1 == cats.Length)
            {

                int[][] assignment = new int[people.Length][];
                for (int p = 0; p < people.Length; p++)
                {
                    assignment[p] = new int[flow.Item3.OutEdges(p).Count()];
                    int i = 0;
                    foreach (var c in flow.Item3.OutEdges(p))
                        assignment[p][i++] = c.To - catsStart;
                }
                return (true, assignment,flow.Item2);
            }

            return (false, null, int.MaxValue);
        }
    }
}
