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
        /// <param name="blockThickness"> ブロックの厚さ </param>
        /// <returns> セルのワールド座標と回転 </returns>
        public BoardCellPose ToWorldPosition(int x, int y, float cellSize, float blockThickness)
        {
            Vector3 right = _cellRight.normalized;
            Vector3 up = _cellUp.normalized;
            Vector3 forward = _cellForward.normalized;

            Vector3 position =
                transform.position +
                right * (x * cellSize) +
                up * (y * cellSize);

            Quaternion rotation = Quaternion.LookRotation(-forward, up);

            return new BoardCellPose(position, rotation);
        }

        [SerializeField] private int _faceId;
        [SerializeField] private Vector3 _cellRight = Vector3.right;
        [SerializeField] private Vector3 _cellUp = Vector3.up;
        [SerializeField] private Vector3 _cellForward = Vector3.forward;
    }
}
