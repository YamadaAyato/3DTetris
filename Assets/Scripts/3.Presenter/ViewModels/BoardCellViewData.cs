namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///    盤面のセルの表示に必要なデータを表す構造体。
    /// </summary>
    public readonly struct BoardCellViewData
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
    }
}
