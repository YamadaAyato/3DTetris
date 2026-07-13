using System.Collections.Generic;
using ThreeDTetris.Presenter;
using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     操作中ピースの表示を担当するビュー。
    /// </summary>
    public class ActivePieceView : MonoBehaviour, IActivePieceView
    {
        public void Render(IReadOnlyList<BoardCellViewData> cellViewDatas)
        {
            if (cellViewDatas == null)
            {
                Clear();
                return;
            }

            EnsureBlockCount(cellViewDatas.Count);

            for (int i = 0; i < _activeBlocks.Count; i++)
            {
                bool isActive = i < cellViewDatas.Count;
                _activeBlocks[i].gameObject.SetActive(isActive);

                if (!isActive)
                {
                    continue;
                }

                Vector3 position = ConvertToLocalPosition(cellViewDatas[i]);
                _activeBlocks[i].SetPosition(position);
            }
        }

        public void Clear()
        {
            for (int i = 0; i < _activeBlocks.Count; i++)
            {
                _activeBlocks[i].gameObject.SetActive(false);
            }
        }

        [SerializeField] private BoardCellBlockView _blockPrefab;
        [SerializeField] private Transform _parentTransform;
        [SerializeField] private float _cellSize = 1f;

        private readonly List<BoardCellBlockView> _activeBlocks = new();

        /// <summary>
        ///     必要なブロック数を確保する。足りない場合はプレハブから生成する。
        /// </summary>
        /// <param name="count"> 確保するブロックの数 </param>
        private void EnsureBlockCount(int count)
        {
            while (_activeBlocks.Count < count)
            {
                BoardCellBlockView block = Instantiate(_blockPrefab, _parentTransform);
                _activeBlocks.Add(block);
            }
        }

        /// <summary>
        ///     BoardCellViewDataの座標をローカル座標に変換する。
        /// </summary>
        /// <param name="cell"> 変換するBoardCellViewData </param>
        /// <returns> ローカル座標 </returns>
        private Vector3 ConvertToLocalPosition(BoardCellViewData cell)
        {
            float faceOffsetX = cell.FaceId * 10f * _cellSize;

            return new Vector3(faceOffsetX + cell.X * _cellSize, cell.Y * _cellSize, 0f);
        }
    }
}
