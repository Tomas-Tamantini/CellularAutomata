namespace CellularAutomataTests
{
    [TestClass]
    public class TestWireWorld
    {
        [TestMethod]
        public void TestCannotHavePositionOverlap()
        {
            HashSet<(int, int)> wires = new() { (1, 1), (2, 2) };
            HashSet<(int, int)> electronHeads = new() { (2, 2), (3, 3) };
            HashSet<(int, int)> electronTails = new() { (3, 3), (4, 4) };
            Assert.ThrowsException<ArgumentException>(() => _ = new WireWorld(wires, electronHeads, electronTails));
        }

        [TestMethod]
        public void TestDeadWorldStaysDead()
        {
            var wireWorld = EmptyWorld();
            wireWorld.Step();
            Assert.IsFalse(wireWorld.Cells.Any());
        }

        [TestMethod]
        public void TestElectronHeadBecomesTail()
        {
            HashSet<(int, int)> wires = new();
            HashSet<(int, int)> electronHeads = new() { (1, 1)};
            HashSet<(int, int)> electronTails = new();
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            wireWorld.Step();
            var newCell = new Cell((1, 1), (int)WireWorldStatus.electronTail);
            HashSet<Cell> newCells = new() { newCell };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestElectronTailBecomesWire()
        {
            HashSet<(int, int)> wires = new();
            HashSet<(int, int)> electronHeads = new();
            HashSet<(int, int)> electronTails = new() { (1, 1) };
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            wireWorld.Step();
            var newCell = new Cell((1, 1), (int)WireWorldStatus.wire);
            HashSet<Cell> newCells = new() { newCell };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestWireWithNoNeighborsRemainsWire()
        {
            HashSet<(int, int)> wires = new() { (1, 1) };
            HashSet<(int, int)> electronHeads = new();
            HashSet<(int, int)> electronTails = new() ;
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            wireWorld.Step();
            var newCell = new Cell((1, 1), (int)WireWorldStatus.wire);
            HashSet<Cell> newCells = new() { newCell };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestWireWithOneNeighboringElectronHeadBecomesHead()
        {
            HashSet<(int, int)> wires = new() { (1, 1) };
            HashSet<(int, int)> electronHeads = new() { (1, 2) };
            HashSet<(int, int)> electronTails = new();
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            wireWorld.Step();
            var newHead = new Cell((1, 1), (int)WireWorldStatus.electronHead);
            var newTail = new Cell((1, 2), (int)WireWorldStatus.electronTail);
            HashSet<Cell> newCells = new() { newHead, newTail };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestWireWithTwoNeighboringElectronHeadBecomesHead()
        {
            HashSet<(int, int)> wires = new() { (1, 1) };
            HashSet<(int, int)> electronHeads = new() { (1, 2), (2, 1) };
            HashSet<(int, int)> electronTails = new();
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            wireWorld.Step();
            var newHead = new Cell((1, 1), (int)WireWorldStatus.electronHead);
            Assert.IsTrue(wireWorld.Cells.Contains(newHead));
        }

        [TestMethod]
        public void TestWireWithMoreThanTwoNeighboringElectronHeadStaysWire()
        {
            HashSet<(int, int)> wires = new() { (1, 1) };
            HashSet<(int, int)> electronHeads = new() { (1, 2), (2, 1), (0, 1) };
            HashSet<(int, int)> electronTails = new();
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            wireWorld.Step();
            var wire = new Cell((1, 1), (int)WireWorldStatus.wire);
            Assert.IsTrue(wireWorld.Cells.Contains(wire));
        }

        [TestMethod]
        public void TestCanChangeStatusToWire()
        {
            var wireWorld = EmptyWorld();
            Assert.IsTrue(wireWorld.ChangeCellStatus(0, 0, (int)WireWorldStatus.wire));
            var newCell = new Cell((0, 0), (int)WireWorldStatus.wire);
            HashSet<Cell> newCells = new() { newCell };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestCanChangeStatusToElectronHead()
        {
            var wireWorld = EmptyWorld();
            Assert.IsTrue(wireWorld.ChangeCellStatus(0, 0, (int)WireWorldStatus.electronHead));
            var newCell = new Cell((0, 0), (int)WireWorldStatus.electronHead);
            HashSet<Cell> newCells = new() { newCell };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestCanChangeStatusToElectronTail()
        {
            var wireWorld = EmptyWorld();
            Assert.IsTrue(wireWorld.ChangeCellStatus(0, 0, (int)WireWorldStatus.electronTail));
            var newCell = new Cell((0, 0), (int)WireWorldStatus.electronTail);
            HashSet<Cell> newCells = new() { newCell };
            Assert.IsTrue(wireWorld.Cells.SetEquals(newCells));
        }

        [TestMethod]
        public void TestCanChangeStatusToEmpty()
        {
            HashSet<(int, int)> wires = new() { (0, 0) };
            HashSet<(int, int)> electronHeads = new() {  };
            HashSet<(int, int)> electronTails = new();
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            Assert.IsTrue(wireWorld.ChangeCellStatus(0, 0, (int)WireWorldStatus.empty));
            Assert.IsFalse(wireWorld.Cells.Any());
        }

        [TestMethod]
        public void TestChangeStatusReturnsFalseIfStatusWasUnchanged()
        {
            var wireWorld = EmptyWorld();
            Assert.IsFalse(wireWorld.ChangeCellStatus(0, 0, (int)WireWorldStatus.empty));
        }
        [TestMethod]
        public void TestChangeStatusReturnsFalseIfStatusIsInvalid()
        {
            int badStatus = 123;
            var wireWorld = EmptyWorld();
            Assert.IsFalse(wireWorld.ChangeCellStatus(0, 0, badStatus));
        }

        private static WireWorld EmptyWorld()
        {
            HashSet<(int, int)> wires = new();
            HashSet<(int, int)> electronHeads = new();
            HashSet<(int, int)> electronTails = new();
            var wireWorld = new WireWorld(wires, electronHeads, electronTails);
            return wireWorld;
        }
    }
}