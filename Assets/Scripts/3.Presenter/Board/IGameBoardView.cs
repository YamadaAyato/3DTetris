using System.Collections.Generic;

namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///     ゲーム盤面のビューを表すインターフェース。
    /// </summary>
    public interface IGameBoardView
    {
        /// <summary>
        ///     操作中のピースを描画する。
        /// </summary>
        /// <param name="cells"> 描画するピースのセル情報のリスト </param>
        void RenderActivePiece(IReadOnlyList<BoardCellViewData> cells);

        /// <summary>
        ///     操作中のピースを固定ブロックとして確定する。
        /// </summary>
        /// <param name="cells"> 確定するピースのセル情報のリスト </param>
        void CommitActivePieceAsFixedBlock(IReadOnlyList<BoardCellViewData> cells);

        /// <summary>
        ///     固定ブロックを削除する。
        /// </summary>
        /// <param name="cells"> 削除するブロックのセル情報のリスト </param>
        void RemoveFixedBlock(IReadOnlyList<BoardCellViewData> cells);

        /// <summary>
        ///     固定ブロックを移動する。
        /// </summary>
        /// <param name="from"> 移動元のブロックのセル情報 </param>
        /// <param name="to"> 移動先のブロックのセル情報 </param>
        void MoveFixedBlock(BoardCellViewData from, BoardCellViewData to);

        /// <summary>
        ///     操作中のピースをクリアする。
        /// </summary>
        void ClearActivePiece();
    }
}
