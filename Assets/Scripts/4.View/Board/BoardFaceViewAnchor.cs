using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     盤面の1つの面を表すビューのアンカー。
    /// </summary>
    public class BoardFaceViewAnchor : MonoBehaviour
    {
        /// <summary> 対応する面IDを取得する。 /// </summary>
        public int FaceId => _faceId;

        /// <summary>
        ///     指定されたセルのワールド座標を取得する。
        /// </summary>
        /// <param name="x"> セルのX座標 </param>
        /// <param name="y"> セルのY座標 </param>
        /// <param name="cellSize"> セルのサイズ </param>
        /// <returns> セルのワールド座標 </returns>
        public Vector3 ToWorldPosition(int x, int y,float cellSize)
        {
            return transform.position + 
                _cellRight * x * cellSize +
                _cellUp * y * cellSize;
        }

        [SerializeField] private int _faceId;
        [SerializeField] private Vector3 _cellRight = Vector3.right;
        [SerializeField] private Vector3 _cellUp = Vector3.up;
    }
}
