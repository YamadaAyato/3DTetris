using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     特定の面を見るためのカメラ位置と向きを表すアンカー。
    /// </summary>
    public class BoardCameraFaceAnchor : MonoBehaviour
    {
        /// <summary> このアンカーが表す面のID。 </summary>
        public int FaceId => _faceId;

        /// <summary> カメラの位置。 </summary>
        public Vector3 Position => transform.position;

        /// <summary> カメラの回転。 </summary>
        public Quaternion Rotation => transform.rotation;

        [SerializeField, Tooltip("このアンカーが表す面のID")] private int _faceId;
    }
}
