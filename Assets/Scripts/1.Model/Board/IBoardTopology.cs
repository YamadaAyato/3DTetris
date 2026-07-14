using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の面同士のつながりを提供する。
    /// </summary>
    public interface IBoardTopology
    {
        /// <summary> 盤面の面ID一覧。 </summary>
        IReadOnlyList<BoardFaceId> FaceIds { get; }

        /// <summary> 初期操作面の面ID。 </summary>
        BoardFaceId InitialFaceId { get; }

        /// <summary>
        ///     指定位置の左隣の位置を取得する。
        /// </summary>
        /// <param name="position"> 確認したい位置 </param>
        /// <returns> 取得した位置 </returns>
        BoardCellPosition GetLeft(BoardCellPosition position);

        /// <summary>
        ///     指定位置の右隣の位置を取得する。
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

        /// <summary>
        ///     指定された面Idの盤面の寸法を取得する。
        /// </summary>
        /// <param name="faceId"> 寸法を取得する面Id </param>
        /// <returns> 指定された面Idの盤面の寸法 </returns>
        BoardDimensions GetDimensions(BoardFaceId faceId);

        /// <summary>
        ///     指定された面Idの盤面の幅を取得する。
        /// </summary>
        /// <param name="faceId"> 幅を取得する面Id </param>
        /// <returns> 指定された面Idの盤面の幅 </returns>
        int GetFaceWidth(BoardFaceId faceId);

        /// <summary>
        ///     指定された面Idの盤面の高さを取得する。
        /// </summary>
        /// <param name="faceId"> 高さを取得する面Id </param>
        /// <returns> 指定された面Idの盤面の高さ </returns>
        int GetFaceHeight(BoardFaceId faceId);

        /// <summary>
        ///     指定された面Idの盤面の左右隣の面Idを取得する。
        /// </summary>
        /// <param name="faceId"> 左右隣の面Idを取得する面Id </param>
        /// <returns> 指定された面Idの盤面の左右隣の面Id </returns>
        BoardHorizontalNeighbors GetHorizontalNeighbors(BoardFaceId faceId);
    }
}
