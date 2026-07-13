using System.Collections.Generic;

namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///     操作中ピースの表示を担当するビューのインターフェース。
    /// </summary>
    public interface IActivePieceView
    {
        /// <summary>
        ///     操作中ピースの表示を更新する。
        /// </summary>
        /// <param name="cellViewDatas"></param>
        void Render(IReadOnlyList<BoardCellViewData> cellViewDatas);

        /// <summary>
        ///     操作中ピースの表示をクリアする。
        /// </summary>
        void Clear();
    }
}
