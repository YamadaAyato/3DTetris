using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     盤面状の1つのセルを表すブロックのビュー。
    /// </summary>
    public class BoardCellBlockView : MonoBehaviour
    {
        /// <summary>
        ///     ブロックの位置を設定する。
        /// </summary>
        /// <param name="position"> 設定する位置 </param>
        /// <param name="rotation"> 設定する回転 </param>
        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }
    }
}
