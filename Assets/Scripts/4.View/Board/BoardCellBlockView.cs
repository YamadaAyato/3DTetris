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
        public void SetPosition(Vector3 position)
        {
            transform.localPosition = position;
        }
    }
}
