using UnityEngine;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///      盤面の面の水平方向の隣接関係を表す構造体。
    /// </summary>
    public readonly struct BoardHorizontalNeighbors
    {
        public BoardHorizontalNeighbors(BoardFaceId leftFaceId, BoardFaceId rightFaceId)
        {
            LeftFaceId = leftFaceId;
            RightFaceId = rightFaceId;
        }

        /// <summary> 左隣の面ID。 </summary>
        public BoardFaceId LeftFaceId { get; }

        /// <summary> 右隣の面ID。 </summary>
        public BoardFaceId RightFaceId { get; }
    }
}
