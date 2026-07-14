namespace ThreeDTetris.Presenter
{
    /// <summary>
    ///     操作面に合わせてカメラの視点を変更するビューを表すインターフェース。
    /// </summary>
    public interface IBoardCameraView
    {
        /// <summary>
        ///     指定された面にカメラの視点を合わせる。
        /// </summary>
        /// <param name="faceId"> カメラの視点を合わせる面のID </param>
        void FocusFace(int faceId);
    }
}
