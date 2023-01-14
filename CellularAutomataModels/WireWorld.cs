namespace CellularAutomataModels
{
    public enum WireWorldStatus
    {
        empty = 0,
        wire = 1,
        electronHead = 2,
        electronTail = 3,
    }
    public class WireWorld : ICelullarAutomaton
    {
        private HashSet<(int, int)> wirePositions;
        private HashSet<(int, int)> electronHeads;
        private HashSet<(int, int)> electronTails;

        public WireWorld(HashSet<(int, int)> wirePositions, HashSet<(int, int)> electronHeads, HashSet<(int, int)> electronTails)
        {
            this.wirePositions = wirePositions;
            this.electronHeads = electronHeads;
            this.electronTails = electronTails;
            this.CheckOverlappingPositions();
        }

        private void CheckOverlappingPositions()
        {
            if (
                this.wirePositions.Intersect(this.electronHeads).Any() ||
                this.wirePositions.Intersect(this.electronTails).Any() ||
                this.electronTails.Intersect(this.electronHeads).Any())
                throw new ArgumentException("Cannot have overlapping positions");
        }

        public HashSet<Cell> Cells
        {
            get
            {
                var cells = new HashSet<Cell>();
                foreach (var pos in this.wirePositions) cells.Add(new Cell(pos, (int)WireWorldStatus.wire));
                foreach (var pos in this.electronHeads) cells.Add(new Cell(pos, (int)WireWorldStatus.electronHead));
                foreach (var pos in this.electronTails) cells.Add(new Cell(pos, (int)WireWorldStatus.electronTail));
                return cells;
            }
        }

        public void Step()
        {
            var newTailPositions = new HashSet<(int, int)>();
            var newHeadPositions = new HashSet<(int, int)>();
            var newWirePositions = new HashSet<(int, int)>();

            foreach (var pos in electronHeads) newTailPositions.Add(pos);
            foreach (var pos in electronTails) newWirePositions.Add(pos);
            foreach (var pos in wirePositions)
            {
                int numNeighboringHeads = NumNeighboringHeads(pos);
                if (numNeighboringHeads == 1 || numNeighboringHeads == 2) newHeadPositions.Add(pos);
                else newWirePositions.Add(pos);
            }
            electronTails = newTailPositions;
            electronHeads = newHeadPositions;
            wirePositions = newWirePositions;
        }

        private int NumNeighboringHeads((int, int) pos)
        {
            var numNeighboringHeads = 0;
            foreach (var neighboringPosition in Cell.NeighboringPositions(pos))
            {
                if (electronHeads.Contains(neighboringPosition)) numNeighboringHeads++;
            }
            return numNeighboringHeads;
        }

        public bool ChangeCellStatus(int xPos, int yPos, int newStatus)
        {
            if (newStatus == (int)WireWorldStatus.wire)
                return this.wirePositions.Add((xPos, yPos));
            if (newStatus == (int)WireWorldStatus.electronHead)
                return this.electronHeads.Add((xPos, yPos));
            if (newStatus == (int)WireWorldStatus.electronTail)
                return this.electronTails.Add((xPos, yPos));
            if (newStatus != (int)WireWorldStatus.empty) return false;
            if (wirePositions.Remove((xPos, yPos))) return true;
            if (electronHeads.Remove((xPos, yPos))) return true;
            if (electronTails.Remove((xPos, yPos))) return true;
            return false;
        }
    }
}
