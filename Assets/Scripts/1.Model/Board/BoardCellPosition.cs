using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面上の1マスの位置。
    /// </summary>
    public readonly struct BoardCellPosition : IEquatable<BoardCellPosition>
    {
        public BoardCellPosition(BoardFaceId faceId, int x, int y)
        {
            FaceId = faceId;
            X = x;
            Y = y;
        }

        /// <summary> 所属する面ID。 </summary>
        public BoardFaceId FaceId { get; }

        /// <summary> 面内の横座標。 </summary>
        public int X { get; }

        /// <summary> 面内の縦座標。 </summary>
        public int Y { get; }

        public bool Equals(BoardCellPosition other)
        {
            return FaceId.Equals(other.FaceId) &&
                X == other.X &&
                Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is BoardCellPosition other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FaceId, X, Y);
        }

        public override string ToString()
        {
            return $"{FaceId}({X},{Y})";
        }
    }
}
