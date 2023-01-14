namespace CellularAutomataModels
{
    public enum GameOfLifeStatus
    {
        dead = 0,
        alive = 1
    }
    public class GameOfLife : ICelullarAutomaton
    {
        private HashSet<(int, int)> livePositions;
        public GameOfLife(HashSet<(int, int)> livePositions)
        {
            this.livePositions = livePositions;
        }

        public HashSet<Cell> Cells
        {
            get
            {
                return (from pos in livePositions
                        select new Cell(pos, (int)GameOfLifeStatus.alive)).ToHashSet();
            }
        }

        public bool ChangeCellStatus(int xPos, int yPos, int newStatus)
        {
            if (newStatus == (int)GameOfLifeStatus.dead)
                return this.livePositions.Remove((xPos, yPos));
            if (newStatus == (int)GameOfLifeStatus.alive)
                return this.livePositions.Add((xPos, yPos));
            return false;
            
        }
       
        public void Step()
        {
            Dictionary<(int, int), int> numLiveNeighbors = NumLiveNeighborsPerPosition();
            var newLivePositions = new HashSet<(int, int)>();
            foreach (var kvp in numLiveNeighbors)
            {
                var pos = kvp.Key;
                var numNeighbors = kvp.Value;
                bool isAlive = this.livePositions.Contains(pos);
                if (numNeighbors == 3 || (isAlive && numNeighbors == 2)) newLivePositions.Add(pos);
            }
            this.livePositions = newLivePositions;
        }

        private Dictionary<(int, int), int> NumLiveNeighborsPerPosition()
        {
            var numLiveNeighbors = new Dictionary<(int, int), int>();
            foreach (var pos in this.livePositions)
            {
                foreach (var neighboringPosition in Cell.NeighboringPositions(pos))
                {
                    if (numLiveNeighbors.ContainsKey(neighboringPosition))
                        numLiveNeighbors[neighboringPosition] += 1;
                    else
                        numLiveNeighbors[neighboringPosition] = 1;
                }
            }
            return numLiveNeighbors;
        }

        
    }
}
