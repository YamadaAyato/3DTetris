using System.Collections.Generic;
using ThreeDTetris.Presenter;
using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///     ゲーム盤面のビューを表すクラス。
    /// </summary>
    public class GameBoardView : MonoBehaviour, IGameBoardView
    {
        /// <summary>
        ///     操作中のピースを描画する。
        /// </summary>
        /// <param name="cells"> 描画するピースのセル情報のリスト </param>
        public void RenderActivePiece(IReadOnlyList<BoardCellViewData> cells)
        {
            // ピースが存在しない場合は、操作中のピースをクリアする
            if (cells == null || cells.Count == 0)
            {
                ClearActivePiece();
                return;
            }

            EnsureActiveBlockCount(cells.Count);

            // 描画するセルの数に応じて、アクティブブロックの表示・非表示を切り替える
            for (int i = 0; i < _activeBlocks.Count; i++)
            {
                bool isActive = i < cells.Count;
                _activeBlocks[i].gameObject.SetActive(isActive);

                if (!isActive)
                {
                    continue;
                }

                // アクティブブロックの親をアクティブブロックルートに設定し、位置を更新する
                _activeBlocks[i].transform.SetParent(_activeBlockRoot, worldPositionStays: true);
                BoardCellPose pose = _boardShapeViewLayout.ConvertToWorldPosition(cells[i]);
                _activeBlocks[i].SetPosition(pose.Position, pose.Rotation);
            }
        }

        /// <summary>
        ///     操作中のピースをクリアする。
        /// </summary>
        public void ClearActivePiece()
        {
            foreach (var block in _activeBlocks)
            {
                block.gameObject.SetActive(false);
            }
        }

        /// <summary>
        ///     操作中のピースを固定ブロックとして確定する。
        /// </summary>
        /// <param name="cells"> 確定するピースのセル情報のリスト </param>
        public void CommitActivePieceAsFixedBlock(IReadOnlyList<BoardCellViewData> cells)
        {
            if (cells == null || cells.Count == 0)
            {
                ClearActivePiece();
                return;
            }

            // アクティブブロックの数を確保する。
            EnsureActiveBlockCount(cells.Count);

            // 確定するセルの数に応じて、アクティブブロックを固定ブロックとして確定する。
            for (int i = 0; i < cells.Count; i++)
            {
                // 確定するセル情報と対応するアクティブブロックを取得する。
                BoardCellViewData cellData = cells[i];
                BoardCellBlockView blockView = _activeBlocks[i];

                // すでに固定ブロックが存在する場合は、警告を出力して既存の固定ブロックを削除する。
                if (_fixedBlocks.ContainsKey(cellData))
                {
                    Debug.LogWarning($"Cell {cellData} is already occupied by a fixed block.", this);
                    Destroy(_fixedBlocks[cellData].gameObject);
                    _fixedBlocks.Remove(cellData);
                }

                // 固定ブロックとして確定するために、アクティブブロックを有効化し、親を固定ブロックルートに設定し、位置を更新する。
                blockView.gameObject.SetActive(true);
                blockView.transform.SetParent(_fixedBlockRoot, worldPositionStays: true);
                BoardCellPose pose = _boardShapeViewLayout.ConvertToWorldPosition(cellData);
                blockView.SetPosition(pose.Position, pose.Rotation);

                _fixedBlocks.Add(cellData, blockView);
            }

            // 確定したセルの数に応じて、アクティブブロックのリストから確定した分を削除する。
            _activeBlocks.RemoveRange(0, cells.Count);
        }

        /// <summary>
        ///     固定ブロックを削除する。
        /// </summary>
        /// <param name="cells"> 削除する固定ブロックのセル情報のリスト </param>
        public void RemoveFixedBlock(IReadOnlyList<BoardCellViewData> cells)
        {
            if (cells == null)
            {
                return;
            }

            // 指定されたセル情報に対応する固定ブロックを削除する。
            for (int i = 0; i < cells.Count; i++)
            {
                BoardCellViewData cell = cells[i];

                if (!_fixedBlocks.TryGetValue(cell, out BoardCellBlockView blockView))
                {
                    continue;
                }

                _fixedBlocks.Remove(cell);
                Destroy(blockView.gameObject);
            }
        }

        /// <summary>
        ///     固定ブロックを移動する。
        /// </summary>
        /// <param name="moves"> 移動するブロックのセル情報のリスト </param>
        public void MoveFixedBlock(IReadOnlyList<BoardCellMoveViewData> moves)
        {
            if (moves == null || moves.Count == 0)
            {
                return;
            }

            BoardCellBlockView[] movingBlocks = new BoardCellBlockView[moves.Count];

            for (int i = 0; i < movingBlocks.Length; i++)
            {
                BoardCellViewData from = moves[i].From;

                if (!_fixedBlocks.TryGetValue(from, out BoardCellBlockView block))
                {
                    Debug.LogWarning($"[{nameof(GameBoardView)}] 移動元のブロックが見つかりません");
                    continue;
                }

                movingBlocks[i] = block;
                _fixedBlocks.Remove(from);
            }

            for (int i = 0; i < movingBlocks.Length; i++)
            {
                BoardCellBlockView block = movingBlocks[i];

                if (block == null)
                {
                    continue;
                }

                BoardCellViewData to = moves[i].To;

                if (_fixedBlocks.ContainsKey(to))
                {
                    Debug.LogWarning($"[{nameof(GameBoardView)}] 移動先のブロックがすでに存在します");
                    Destroy(_fixedBlocks[to].gameObject);
                    _fixedBlocks.Remove(to);
                }

                _fixedBlocks.Add(to, block);
                BoardCellPose pose = _boardShapeViewLayout.ConvertToWorldPosition(to);
                block.SetPosition(pose.Position, pose.Rotation);
            }
        }

        [SerializeField, Tooltip("ブロックのプレハブ")] private BoardCellBlockView _blockPrefab;
        [SerializeField, Tooltip("アクティブブロックのルート")] private Transform _activeBlockRoot;
        [SerializeField, Tooltip("固定ブロックのルート")] private Transform _fixedBlockRoot;
        [SerializeField, Tooltip("盤面の形状ビューのレイアウト")] private BoardShapeViewLayout _boardShapeViewLayout;

        private readonly List<BoardCellBlockView> _activeBlocks = new();
        private readonly Dictionary<BoardCellViewData, BoardCellBlockView> _fixedBlocks = new();

        /// <summary>
        ///     アクティブブロックの数を確保する。
        /// </summary>
        /// <param name="count"> 確保するアクティブブロックの数 </param>
        private void EnsureActiveBlockCount(int count)
        {
            while (_activeBlocks.Count < count)
            {
                BoardCellBlockView block = Instantiate(_blockPrefab, _activeBlockRoot);
                _activeBlocks.Add(block);
            }
        }
    }
}
