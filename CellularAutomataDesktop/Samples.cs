using CellularAutomataModels;
using System.Collections.Generic;

namespace CellularAutomataDesktop
{
    class Samples
    {
        public static GameOfLife DieHard()
        {
            HashSet<(int, int)> livePositions = new() { (31, 22), (32, 21), (32, 22), (36, 21), (37, 21), (38, 21), (37, 23) };
            for (var i = 10; i < 18; i++)
            {
                for (var j = 5; j < 8; j++)
                    livePositions.Add((i, j));
            }
            livePositions.Remove((11, 6));
            livePositions.Remove((16, 6));
            return new GameOfLife(livePositions);
        }

        public static WireWorld Diode()
        {
            HashSet<(int, int)> wires = new();
            HashSet<(int, int)> electronHeads = new();
            HashSet<(int, int)> electronTails = new();
            //Circuit
            for (var i = 0; i < 15; i++)
            {
                wires.Add((25 + i, 15));
                wires.Add((25 + i, 29));
                wires.Add((25, 15 + i));
                wires.Add((39, 15 + i));
            }
            // Diode 1
            wires.Add((24, 21));
            wires.Add((26, 21));
            wires.Add((24, 22));
            wires.Add((26, 22));
            wires.Remove((25, 22));


            //Diode 2
            wires.Add((40, 21));
            wires.Add((38, 21));
            wires.Add((40, 22));
            wires.Add((38, 22));
            wires.Remove((39, 21));

            //Initial electron
            wires.Remove((31, 29));
            electronHeads.Add((31, 29));
            return new WireWorld(wires, electronHeads, electronTails);
        }
    }
}
