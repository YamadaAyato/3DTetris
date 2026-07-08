using UnityEngine;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の面同士のつながりを提供する。
    /// </summary>
    public interface IBoardTopology
    {
        /// <summary> 盤面の寸法。 </summary>
        BoardDimensions Dimensions { get; }

        /// <summary>
        ///     指定位置の左隣の位置を取得する。
        /// </summary>
        /// <param name="position"> 確認したい位置 </param>
        /// <returns> 取得した位置 </returns>
        BoardCellPosition GetLeft(BoardCellPosition position);

        /// <summary>
        ///     指定位置の左隣の位置を取得する。
        /// </summary>
        /// <param name="position"> 確認したい位置 </param>
        /// <returns> 取得した位置 </returns>
        BoardCellPosition GetRight(BoardCellPosition position);

        /// <summary>
        ///     指定された面Idが存在するか判定する。
        /// </summary>
        /// <param name="faceId"> 判定する面Id </param>
        /// <returns> 存在するか </returns>
        bool ContainsFace(BoardFaceId faceId);
    }
}
