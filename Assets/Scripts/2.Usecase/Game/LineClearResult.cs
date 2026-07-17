using System;
using System.Collections.Generic;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     ラインクリアの結果を表すクラス。
    /// </summary>
    public class LineClearResult
    {
        public LineClearResult(IReadOnlyList<BoardCellPosition> removedPositions, IReadOnlyList<BoardBlockMove> movedBlocks)
        {
            RemovedPositions = removedPositions ?? throw new ArgumentNullException(nameof(removedPositions));
            MovedBlocks = movedBlocks ?? throw new ArgumentNullException(nameof(movedBlocks));
        }

        /// <summary> クリアされたブロックの位置のリスト。 /// </summary>
        public IReadOnlyList<BoardCellPosition> RemovedPositions { get; }

        /// <summary> 移動されたブロックのリスト。 /// </summary>
        public IReadOnlyList<BoardBlockMove> MovedBlocks { get; }

        /// <summary> クリアされたブロックが存在するかどうか。 /// </summary>
        public bool HasRemovedBlocks => RemovedPositions.Count > 0;
    }
}
