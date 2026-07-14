namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の制御状態を表すモデル。
    /// </summary>
    public class BoardControllStateModel
    {
        public BoardControllStateModel(BoardFaceId currentFaceId)
        {
            CurrentFaceId = currentFaceId;
        }

        /// <summary> 現在の盤面の面ID。 </summary>
        public BoardFaceId CurrentFaceId { get; private set; }

        /// <summary> 現在の盤面の面IDを設定する。 </summary>
        /// <param name="faceId"> 設定する面ID </param>
        public void SetCurrentFaceId(BoardFaceId faceId)
        {
            CurrentFaceId = faceId;
        }
    }
}
