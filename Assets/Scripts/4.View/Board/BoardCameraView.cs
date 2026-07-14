using DG.Tweening;
using System.Collections.Generic;
using ThreeDTetris.Presenter;
using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     操作面に対応するカメラのビューを表すクラス。
    /// </summary>
    public class BoardCameraView : MonoBehaviour, IBoardCameraView
    {
        /// <summary> 現在フォーカスされている FaceId を取得する。 </summary>
        public int CurrentFaceId => _currentFaceId;

        public void FocusFace(int faceId)
        {
            if (_cameraRig == null)
            {
                Debug.LogWarning("Camera rig is not assigned.", this);
                return;
            }

            if (!_faceAnchorMap.TryGetValue(faceId, out var targetAnchor))
            {
                Debug.LogWarning($"FaceId {faceId} does not have a corresponding anchor.", this);
                return;
            }

            _currentFaceId = faceId;

            if (_snapOnFirstFocus && !_hasFocusedOnce)
            {
                SetRigPose(targetAnchor);
                _hasFocusedOnce = true;
                return;
            }

            PlayTransition(targetAnchor);
            _hasFocusedOnce = true;
        }

        [SerializeField, Tooltip("カメラのリグ")] private Transform _cameraRig;
        [SerializeField, Tooltip("各操作面に対応するアンカーのリスト")] private List<BoardCameraFaceAnchor> _faceAnchors = new();

        [Header("Transition Settings")]
        [SerializeField, Tooltip("トランジションの時間")] private float _transitionDuration = 1.0f;
        [SerializeField, Tooltip("トランジションのイージング")] private Ease _transitionEase = Ease.Linear;
        [SerializeField, Tooltip("最初のフォーカス時にスナップするかどうか")] private bool _snapOnFirstFocus = true;

        private readonly Dictionary<int, BoardCameraFaceAnchor> _faceAnchorMap = new();

        private Sequence _transitionSequence;
        private bool _hasFocusedOnce = false;
        private int _currentFaceId = -1;

        private void Awake()
        {
            BuildAnchorDicitionary();
        }

        private void OnDestroy()
        {
            KillTransition();
        }

        /// <summary>
        ///     登録されている BoardCameraFaceAnchor を FaceId をキーとして辞書に変換する
        /// </summary>
        private void BuildAnchorDicitionary()
        {
            _faceAnchorMap.Clear();

            for (int i = 0; i < _faceAnchors.Count; i++)
            {
                var faceAnchor = _faceAnchors[i];

                if (faceAnchor == null)
                {
                    continue;
                }

                if (_faceAnchorMap.ContainsKey(faceAnchor.FaceId))
                {
                    Debug.LogWarning($"Duplicate face anchor found for FaceId {faceAnchor.FaceId}. Ignoring this anchor.");
                    continue;
                }
                _faceAnchorMap[faceAnchor.FaceId] = faceAnchor;
            }
        }

        /// <summary>
        ///     カメラリグを指定されたアンカーの位置と回転に設定する。
        /// </summary>
        /// <param name="anchor"> 設定するアンカー </param>
        private void SetRigPose(BoardCameraFaceAnchor anchor)
        {
            KillTransition();

            _cameraRig.position = anchor.Position;
            _cameraRig.rotation = anchor.Rotation;
        }

        /// <summary>
        ///     指定されたアンカーに向けてカメラリグをトランジションさせる。
        /// </summary>
        /// <param name="targetAnchor"> トランジション先のアンカー </param>
        private void PlayTransition(BoardCameraFaceAnchor targetAnchor)
        {
            KillTransition();

            _transitionSequence = DOTween.Sequence()
                .SetEase(_transitionEase)
                .Join(_cameraRig.DOMove(targetAnchor.Position, _transitionDuration))
                .Join(_cameraRig.DORotateQuaternion(targetAnchor.Rotation, _transitionDuration));
        }

        /// <summary>
        ///     現在再生中のトランジションを停止する。
        /// </summary>
        private void KillTransition()
        {
            if (_transitionSequence == null)
            {
                return;
            }

            if (_transitionSequence.IsActive())
            {
                _transitionSequence.Kill();
            }

            _transitionSequence = null;
        }
    }
}
