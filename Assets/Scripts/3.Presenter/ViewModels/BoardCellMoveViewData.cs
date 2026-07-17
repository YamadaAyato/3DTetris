using UnityEngine;

namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///     盤面のセルの移動を表す構造体。
    /// </summary>
    public readonly struct BoardCellMoveViewData
    {
        public BoardCellMoveViewData(BoardCellViewData from, BoardCellViewData to)
        {
            From = from;
            To = to;
        }

        /// <summary> 移動元のセルの表示データ。 /// </summary>
        public BoardCellViewData From { get; }

        /// <summary> 移動先のセルの表示データ。 /// </summary>
        public BoardCellViewData To { get; }
    }
}
