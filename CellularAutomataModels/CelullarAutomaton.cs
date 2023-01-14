namespace CellularAutomataModels
{
    public interface ICelullarAutomaton
    {
        public HashSet<Cell> Cells { get; }
        public void Step();
        public bool ChangeCellStatus(int xPos, int yPos, int newStatus);
    }
}
