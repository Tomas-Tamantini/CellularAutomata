namespace CellularAutomataModels
{
    public class Cell : IEquatable<Cell?>
    {
        public readonly (int, int) position;
        public readonly int status;
        public Cell((int, int) position, int status)
        {
            this.position = position;
            this.status = status;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Cell);
        }

        public bool Equals(Cell? other)
        {
            return other is not null &&
                   position.Equals(other.position) &&
                   status == other.status;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(position, status);
        }

        public static bool operator ==(Cell? left, Cell? right)
        {
            return EqualityComparer<Cell>.Default.Equals(left, right);
        }

        public static bool operator !=(Cell? left, Cell? right)
        {
            return !(left == right);
        }

        public static IEnumerable<(int, int)> NeighboringPositions((int, int) position)
        {
            var (x, y) = position;
            for (var dx = -1; dx <= 1; dx++)
            {
                for (var dy = -1; dy <= 1; dy++)
                {
                    if (dx != 0 || dy != 0)
                    {
                        yield return (x + dx, y + dy);
                    }
                }
            }
        }
    }
}