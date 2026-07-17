using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     ボード上のブロックの移動を表す構造体。
    /// </summary>
    public readonly struct BoardBlockMove
    {
        public BoardBlockMove(BoardCellPosition from, BoardCellPosition to)
        {
            From = from;
            To = to;
        }

        /// <summary> 移動元のセルの位置。 /// </summary>
        public BoardCellPosition From { get; }

        /// <summary> 移動先のセルの位置。 /// </summary>
        public BoardCellPosition To { get; }
    }
}
