using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     固定済みブロック等の盤面の状態を表すモデルクラス。
    ///     クラス内で使われているoccupiedは占有中かを表す単語。
    /// </summary>
    public class BoardModel
    {
        public BoardModel(IBoardTopology boardTopology)
        {
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
        }

        /// <summary> 盤面の寸法 </summary>
        public BoardDimensions Dimensions => _boardTopology.Dimensions;

        /// <summary>
        ///     指定された盤面位置に固定済みブロックが存在するかを判定する。
        /// </summary>
        /// <param name="position"> 確認対象の盤面の位置 </param>
        /// <returns> 固定済みブロックが存在するか </returns>
        public bool IsOccupied(BoardCellPosition position)
        {
            ValidatePosition(position);

            return _occupiedPositions.Contains(position);
        }

        /// <summary>
        ///     指定された盤面位置一覧にブロックを占有できるか判定する。
        /// </summary>
        /// <param name="positions"> 確認対象の盤面位置一覧 </param>
        /// <returns> ブロックを占有できるか </returns>
        public bool CanOccupy(IReadOnlyList<BoardCellPosition> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            HashSet<BoardCellPosition> checkedPositions = new();

            for (int i = 0; i < positions.Count; i++)
            {
                BoardCellPosition position = positions[i];

                // 存在しない面IDや範囲外の座標の場合は占有できない。
                if (!_boardTopology.ContainsFace(position.FaceId))
                {
                    return false;
                }

                if (position.X < 0 || position.X >= Dimensions.FaceWidth)
                {
                    return false;
                }

                // Y座標が負の値の場合は占有できない。
                if (position.Y < 0)
                {
                    return false;
                }

                // 同じミノ内で同じ座標が重複している場合も占有できない。
                if (!checkedPositions.Add(position))
                {
                    return false;
                }

                // 盤面より上にあるセルは、固定済みブロックとの衝突判定を行わない。
                // 上に積んでゲームオーバーの判定は別の箇所で行う。
                if (position.Y >= Dimensions.FaceHeight)
                {
                    continue;
                }

                // すでに固定済みブロックがある場合占有できない。
                if (_occupiedPositions.Contains(position))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     指定された盤面位置一覧にブロックを配置できるか判定する。
        /// </summary>
        /// <param name="positions"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool CanPlaceBlocks(IReadOnlyList<BoardCellPosition> positions)
        {
            if (positions == null)
            {
                throw new ArgumentNullException(nameof(positions));
            }

            HashSet<BoardCellPosition> checkedPositions = new();

            for (int i = 0; i < positions.Count; i++)
            {
                BoardCellPosition position = positions[i];

                // 置く位置が範囲外でないか。
                if (!IsValidPosition(position))
                {
                    return false;
                }

                // すでに固定済みブロックがある場合配置できない。
                if (_occupiedPositions.Contains(position))
                {
                    return false;
                }

                // 同じミノ内で同じ座標が重複している場合も配置できない。
                if (!checkedPositions.Add(position))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     指定された位置一覧に固定済みブロックを配置する。
        /// </summary>
        /// <param name="positions"> 配置する盤面位置一覧  </param>
        public void PlaceBlocks(IReadOnlyList<BoardCellPosition> positions)
        {
            if (!CanPlaceBlocks(positions))
            {
                throw new InvalidOperationException("指定されたい位置にブロックを配置できません。");
            }

            for (int i = 0; i < positions.Count; i++)
            {
                _occupiedPositions.Add(positions[i]);
            }
        }

        /// <summary>
        ///     指定された盤面位置の固定済みブロックを削除する。
        /// </summary>
        /// <param name="position"> 削除対象の盤面位置 </param>
        public void RemoveBlock(BoardCellPosition position)
        {
            ValidatePosition(position);

            _occupiedPositions.Remove(position);
        }

        /// <summary>
        ///     存在ブロックをすべて削除する。
        /// </summary>
        public void Clear()
        {
            _occupiedPositions.Clear();
        }

        /// <summary>
        ///     固定済みブロックの盤面位置一覧をコピーして取得する。
        /// </summary>
        /// <returns> 固定済みブロックの盤面位置一覧 </returns>
        public IReadOnlyList<BoardCellPosition> GetOccupiedPositions()
        {
            var positions = new BoardCellPosition[_occupiedPositions.Count];
            _occupiedPositions.CopyTo(positions);

            return positions;
        }

        private readonly IBoardTopology _boardTopology;
        private readonly HashSet<BoardCellPosition> _occupiedPositions = new();

        /// <summary>
        ///     指定された盤面位置が有効範囲内か判定する。
        ///     配置できるか等のチェック用に使用する。
        /// </summary>
        /// <param name="position"> 判定対象の盤面位置 </param>
        /// <returns> 有効範囲内か </returns>
        private bool IsValidPosition(BoardCellPosition position)
        {
            if (!_boardTopology.ContainsFace(position.FaceId))
            {
                return false;
            }

            if (position.X < 0 || position.X >= Dimensions.FaceWidth)
            {
                return false;
            }

            if (position.Y < 0 || position.Y >= Dimensions.FaceHeight)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        ///     指定された盤面位置が有効範囲内か検証する。
        ///     呼び出し側のバグなどの確認に使用する。
        /// </summary>
        /// <param name="position"> 検証対象の盤面位置 </param>
        private void ValidatePosition(BoardCellPosition position)
        {
            if (!_boardTopology.ContainsFace(position.FaceId))
            {
                throw new InvalidOperationException($"面ID {position.FaceId} は存在しません。");
            }

            if (position.X < 0 || position.X >= Dimensions.FaceWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"X座標 {position.X} は盤面幅の範囲外です。");
            }

            if (position.Y < 0 || position.Y >= Dimensions.FaceHeight)
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"Y座標 {position.Y} は盤面高さの範囲外です。");
            }
        }
    }
}
