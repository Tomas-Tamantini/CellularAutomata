using CellularAutomataModels;
using System.Collections.Generic;

namespace CellularAutomataDesktop
{
    class Samples
    {
        public static GameOfLife DieHard()
        {
            HashSet<(int, int)> livePositions = new() { (31, 22), (32, 21), (32, 22), (36, 21), (37, 21), (38, 21), (37, 23) };
            //Oscillator
            livePositions.UnionWith(Circuit((10, 5), (17, 7)));
            livePositions.UnionWith(Line((12, 6), 4, true));

            return new GameOfLife(livePositions);
        }

        public static WireWorld Diodes()
        {
            HashSet<(int, int)> wires = new();
            HashSet<(int, int)> electronHeads = new();
            HashSet<(int, int)> electronTails = new();

            //Circuit
            wires.UnionWith(Circuit((10, 24), (15, 29)));
            wires.UnionWith(Line((15, 29), 25, true));
            wires.UnionWith(Line((25, 13), 16, false));
            wires.UnionWith(Line((39, 13), 16, false));

            // Diodes
            wires.UnionWith(Circuit((24, 21), (26, 22)));
            wires.Remove((25, 22));

            wires.UnionWith(Circuit((38, 21), (40, 22)));
            wires.Remove((39, 21));

            //LEDs
            wires.UnionWith(Circuit((24, 12), (26, 14)));
            wires.UnionWith(Circuit((38, 12), (40, 14)));

            //Initial electron
            wires.Remove((11, 24));
            electronTails.Add((11, 24));
            wires.Remove((12, 24));
            electronHeads.Add((12, 24));

            return new WireWorld(wires, electronHeads, electronTails);
        }

        public static WireWorld CounterClockwiseDiodes()
        {
            HashSet<(int, int)> wires = new();
            HashSet<(int, int)> electronHeads = new();
            HashSet<(int, int)> electronTails = new();
            //Circuit
            wires.UnionWith(Circuit((25, 15), (39, 29)));

            // Diodes
            wires.UnionWith(Circuit((24, 21), (26, 22)));
            wires.Remove((25, 22));

            wires.UnionWith(Circuit((38, 21), (40, 22)));
            wires.Remove((39, 21));

            //Initial electron
            wires.Remove((31, 29));
            electronHeads.Add((31, 29));
            return new WireWorld(wires, electronHeads, electronTails);
        }

        private static HashSet<(int, int)> Line((int, int) lineStart, int length, bool isHorizontal)
        {
            HashSet<(int, int)> line = new();
            var (x0, y0) = lineStart;
            for (var i = 0; i < length; i++)
            {
                var newPos = isHorizontal ? (x0 + i, y0) : (x0, y0 + i);
                line.Add(newPos);
            }
            return line;
        }

        private static HashSet<(int, int)> Circuit((int, int) topLeft, (int, int) bottomRight)
        {
            var (xtl, ytl) = topLeft;
            var (xbr, ybr) = bottomRight;
            var height = ybr - ytl;
            var width = xbr - xtl;
            HashSet<(int, int)> circuit = new();
            circuit.UnionWith(Line(topLeft, width, true));
            circuit.UnionWith(Line(topLeft, height, false));
            circuit.UnionWith(Line((xtl, ybr), width, true));
            circuit.UnionWith(Line((xbr, ytl), height, false));
            circuit.Add(bottomRight);
            return circuit;
        }
    }
}
