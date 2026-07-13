using System;

namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///    盤面のセルの表示に必要なデータを表す構造体。
    /// </summary>
    public readonly struct BoardCellViewData : IEquatable<BoardCellViewData>
    {
        public BoardCellViewData(int faceId, int x, int y)
        {
            FaceId = faceId;
            X = x;
            Y = y;
        }

        /// <summary> 盤面のセルが属する面のID。 </summary>
        public int FaceId { get; }

        /// <summary> セルのX座標。 </summary>
        public int X { get; }

        /// <summary> セルのY座標。 </summary>
        public int Y { get; }

        public bool Equals(BoardCellViewData other)
        {
            return FaceId == other.FaceId && X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            return obj is BoardCellViewData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FaceId, X, Y);
        }

        public override string ToString()
        {
            return $"FaceId: {FaceId}, X: {X}, Y: {Y}";
        }
    }
}
