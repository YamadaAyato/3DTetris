using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ミノのセルの相対座標を表す値オブジェクト。
    /// </summary>
    public readonly struct PieceCellOffset : IEquatable<PieceCellOffset>
    {
        public PieceCellOffset(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary> ミノのセルの相対座標のX成分。 </summary>
        public int X { get; }

        /// <summary> ミノのセルの相対座標のY成分。 </summary>
        public int Y { get; }

        public bool Equals(PieceCellOffset other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is PieceCellOffset other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
