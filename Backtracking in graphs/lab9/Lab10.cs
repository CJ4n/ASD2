using System;
using System.Linq;
using System.Collections.Generic;
using ASD.Graphs;

namespace Lab10
{
    public class DeliveryPlanner : MarshalByRefObject
    {
        int truckCap;
        int[] eggDem;
        bool[] isRefuel;
        int TruckRange;
        Graph<int> g;
        Stack<int> s;
        bool[] visited;
        bool exists = false;
        int[] route;
        int minRouteFuel = int.MaxValue;
        bool any;
        int sumEggs;
        void SetAllVariables(Graph<int> railway,
            int[] eggDemand, int truckCapacity, int tankEngineRange, bool[] isRefuelStation, bool anySolution)
        {
            truckCap = truckCapacity;
            eggDem = eggDemand;
            isRefuel = isRefuelStation;
            TruckRange = tankEngineRange;
            g = railway;
            s = new Stack<int>();
            visited = new bool[railway.VertexCount];
            exists = false;
            route = null;
            minRouteFuel = int.MaxValue;
            any = anySolution;
            sumEggs = 0;
            foreach (var e in eggDemand)
                sumEggs += e;
        }


        /// <param name="railway">Graf reprezentujący sieć kolejową</param>
        /// <param name="eggDemand">Zapotrzebowanie na jajka na poszczególnyhc stacjach. Zerowy element tej tablicy zawsze jest 0</param>
        /// <param name="truckCapacity">Pojemność wagonu na jajka</param>
        /// <param name="tankEngineRange">Zasięg parowozu</param>
        /// <param name="isRefuelStation">na danym indeksie true, jeśli na danej stacji można uzupelnić węgiel i wodę</param>
        /// <param name="anySolution">Czy znaleźć jakiekolwiek rozwiązanie (true, etap 1), czy najkrótsze (false, etap 2)</param>
        /// <returns>Informację czy istnieje trasa oraz tablicę reprezentującą kolejne wierzchołki w trasie (pierwszy i ostatni element tablicy musi być 0). W przypadku, gdy zwracany jest false, wartość tego pola nie jest sprawdzana, może być null.</returns>
        public (bool routeExists, int[] route) PlanDelivery(Graph<int> railway,
            int[] eggDemand, int truckCapacity, int tankEngineRange, bool[] isRefuelStation, bool anySolution)
        {
            SetAllVariables(railway, eggDemand, truckCapacity, tankEngineRange, isRefuelStation, anySolution);

            s.Push(0);
            visited[0] = true;

            foreach (var e in railway.OutEdges(0))
            {
                if (exists && any) break;
                if (0 + g.GetEdgeWeight(0, e.To) > minRouteFuel) continue;
                if (TruckRange - g.GetEdgeWeight(0, e.To) < 0) continue;
                if (truckCapacity - eggDem[e.To] < 0) continue;

                visited[e.To] = true;
                Visit(e.To, truckCapacity - eggDem[e.To], TruckRange - g.GetEdgeWeight(0, e.To), sumEggs - eggDem[e.To], 0 + g.GetEdgeWeight(0, e.To));
                visited[e.To] = false;
            }

            return (exists, route);
        }

        void Visit(int station, int eggsInTruck, int curTruckRange, int eggsToDeliver, int curRouteFuel)
        {
            if (exists && any) return;

            s.Push(station);

            if (station == 0)
            {
                eggsInTruck = truckCap;
                if (eggsToDeliver == 0)
                {
                    IsThisBestRoute(curRouteFuel);
                    exists = true;
                    s.Pop();
                    return;
                }
            }

            if (isRefuel[station] == true)
                curTruckRange = TruckRange;

            foreach (var e in g.OutEdges(station))
            {
                if (exists && any) return;
                if (visited[e.To] == true && e.To != 0) continue;
                if (curRouteFuel + g.GetEdgeWeight(station, e.To) > minRouteFuel) continue;
                if (curTruckRange - g.GetEdgeWeight(station, e.To) < 0) continue;
                if (e.To != 0 && eggsInTruck - eggDem[e.To] < 0) continue;

                visited[e.To] = true;
                Visit(e.To, eggsInTruck - eggDem[e.To], curTruckRange - g.GetEdgeWeight(station, e.To), eggsToDeliver - eggDem[e.To], curRouteFuel + g.GetEdgeWeight(station, e.To));
                if (e.To != 0) visited[e.To] = false;
            }
            s.Pop();
        }

        void IsThisBestRoute(int curRouteFuel)
        {
            if (curRouteFuel < minRouteFuel)
            {
                minRouteFuel = curRouteFuel;
                route = s.ToArray();
            }
        }
    }
}
