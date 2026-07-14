using System;
using System.Collections.Generic;
using ThreeDTetris.Presenter;
using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     盤面の形状を表すビューのレイアウトを管理するクラス。
    /// </summary>
    public class BoardShapeViewLayout : MonoBehaviour
    {
        /// <summary>
        ///     BoardCellViewData の座標をワールド座標に変換する。
        /// </summary>
        /// <param name="boardCellViewData"> 変換する BoardCellViewData </param>
        /// <returns> ワールド座標 </returns>
        public Vector3 ConvertToWorldPosition(BoardCellViewData boardCellViewData)
        {
            if (!_faceAnchorMap.TryGetValue(boardCellViewData.FaceId, out var faceAnchor))
            {
                throw new InvalidOperationException($"FaceId{boardCellViewData.FaceId} のアンカーが見つかりません。");
            }

            return faceAnchor.ToWorldPosition(boardCellViewData.X, boardCellViewData.Y, _cellSize);
        }

        [SerializeField, Tooltip("盤面の各面のアンカー情報")] private List<BoardFaceViewAnchor> _faceAnchors = new();
        [SerializeField, Tooltip("セルのサイズ")] private float _cellSize = 1f;

        private readonly Dictionary<int, BoardFaceViewAnchor> _faceAnchorMap = new();

        private void Awake()
        {
            _faceAnchorMap.Clear();

            // FaceId をキーにして BoardFaceViewAnchor をマップに登録する
            for (int i = 0; i < _faceAnchors.Count; i++)
            {
                var faceAnchor = _faceAnchors[i];

                if (faceAnchor == null)
                {
                    continue;
                }

                _faceAnchorMap[faceAnchor.FaceId] = faceAnchor;
            }
        }
    }
}
