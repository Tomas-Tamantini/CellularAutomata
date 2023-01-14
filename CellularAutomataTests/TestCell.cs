namespace CellularAutomataTests
{
    [TestClass]
    public class TestCell
    {
        [TestMethod]
        public void TestCellsWithSameStatusAndPositionAreEqual()
        {
            var c1 = new Cell((10, 10), 1);
            var c2 = new Cell((10, 10), 1);
            var c3 = new Cell((10, 10), 2);
            var c4 = new Cell((9, 10), 1);
            Assert.AreEqual(c1, c2);
            Assert.AreNotEqual(c1, c3);
            Assert.AreNotEqual(c1, c4);
        }

        [TestMethod]
        public void TestCellsWithSameStatusAndPositionHaveSameHash()
        {
            var c1 = new Cell((10, 10), 1);
            var c2 = new Cell((10, 10), 1);
            var c3 = new Cell((10, 10), 2);
            var c4 = new Cell((9, 10), 1);
            Assert.AreEqual(c1.GetHashCode(), c2.GetHashCode());
            Assert.AreNotEqual(c1.GetHashCode(), c3.GetHashCode());
            Assert.AreNotEqual(c1.GetHashCode(), c4.GetHashCode());
        }
    }
}
