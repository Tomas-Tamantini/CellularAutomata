using CellularAutomataModels;

namespace CellularAutomataTests
{
    [TestClass]
    public class TestGameOfLife
    {
        private static GameOfLife GenerateGameAndStep((int, int)[] initialPositions)
        {
            var gameOfLife = new GameOfLife(initialPositions.ToHashSet());
            gameOfLife.Step();
            return gameOfLife;
        }

        [TestMethod]
        public void TestDeadWorldStaysDead()
        {
            var livePositions = Array.Empty<(int, int)>();
            GameOfLife gof = GenerateGameAndStep(livePositions);
            Assert.IsFalse(gof.Cells.Any());
        }



        [TestMethod]
        public void TestIsolatedCellsDie()
        {
            (int, int)[] livePositions = { (10, 10) };
            var gof = new GameOfLife(livePositions.ToHashSet());
            gof.Step();
            Assert.IsFalse(gof.Cells.Any());

        }

        [TestMethod]
        public void TestCellWithOneNeighborDies()
        {
            (int, int)[] livePositions = {
                (10, 10),
                (10, 11)
            };
            var gof = GenerateGameAndStep(livePositions);
            Assert.IsFalse(gof.Cells.Any());
        }
        [TestMethod]
        public void TestCellWithTwoNeighborsSurvives()
        {
            (int, int)[] livePositions = {
                (9, 9),
                (10, 10),
                (11, 11)
            };
            var gof = GenerateGameAndStep(livePositions);
            var survivingCell = new Cell((10, 10), (int)GameOfLifeStatus.alive);
            HashSet<Cell> liveCells = new() { survivingCell };
            Assert.IsTrue(gof.Cells.SetEquals(liveCells));
        }

        [TestMethod]
        public void TestCellWithThreeNeighborsIsBorn()
        {
            (int, int)[] livePositions = {
                (9, 9),
                (9, 11),
                (11, 11)
            };
            var gof = GenerateGameAndStep(livePositions);
            var newCell = new Cell((10, 10), (int)GameOfLifeStatus.alive);
            HashSet<Cell> liveCells = new() { newCell };
            Assert.IsTrue(gof.Cells.SetEquals(liveCells));
        }

        [TestMethod]
        public void TestGliderGlides()
        {
            (int, int)[] livePositions = {
                (0, 0), (1, 0), (1, 1), (2, 1), (0, 2)
            };
            var gof = GenerateGameAndStep(livePositions);
            (int, int)[] nextGenPositions = {
                (0, 0), (1, 0), (2, 0), (2, 1), (1, 2)
            };
            HashSet<Cell> liveCells = (from pos in nextGenPositions
                                       select new Cell(pos, (int)GameOfLifeStatus.alive)).ToHashSet();
            Assert.IsTrue(gof.Cells.SetEquals(liveCells));
        }

        [TestMethod]
        public void TestCanChangeStatusFromAliveToDead()
        {
            (int, int)[] livePositions = {(0, 0)};
            var gof  = new GameOfLife(livePositions.ToHashSet());
            Assert.IsTrue(gof.ChangeCellStatus(0, 0, (int)GameOfLifeStatus.dead));
            Assert.IsFalse(gof.Cells.Any());
        }

        [TestMethod]
        public void TestCanChangeStatusFromDeadToAlive()
        {
            var livePositions = Array.Empty<(int, int)>();
            var gof = new GameOfLife(livePositions.ToHashSet());
            Assert.IsTrue(gof.ChangeCellStatus(0, 0, (int)GameOfLifeStatus.alive));
            var newCell = new Cell((0, 0), (int)GameOfLifeStatus.alive);
            HashSet<Cell> liveCells = new() { newCell };
            Assert.IsTrue(gof.Cells.SetEquals(liveCells));
        }

        [TestMethod]
        public void TestChangingStatusReturnsFalseIfStatusWasNotChanged()
        {
            var livePositions = Array.Empty<(int, int)>();
            var gof = new GameOfLife(livePositions.ToHashSet());
            Assert.IsFalse(gof.ChangeCellStatus(0, 0, (int)GameOfLifeStatus.dead));
        }

        [TestMethod]
        public void TestChangingStatusReturnsFalseIfInvalidStatusIsUsed()
        {
            int badStatus = 123;
            var livePositions = Array.Empty<(int, int)>();
            var gof = new GameOfLife(livePositions.ToHashSet());
            Assert.IsFalse(gof.ChangeCellStatus(0, 0, badStatus));
        }
    }
}