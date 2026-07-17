using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    public class DataDrivenBoardTopology : IBoardTopology
    {
        public DataDrivenBoardTopology(BoardShapeDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary> 盤面の面ID一覧。 </summary>
        public IReadOnlyList<BoardFaceId> FaceIds => _definition.FaceIds;

        /// <summary> 盤面の初期面ID。 </summary>
        public BoardFaceId InitialFaceId => _definition.InitialFaceId;

        public BoardCellPosition GetLeft(BoardCellPosition position)
        {
            ValidatePosition(position);

            // 今の位置が左端でないなら同じ面の -1 の位置を返す。
            if (position.X > 0)
            {
                return new BoardCellPosition(position.FaceId, position.X - 1, position.Y);
            }

            // 左端なら左隣の面を返す。
            BoardHorizontalNeighbors neighbors = GetHorizontalNeighbors(position.FaceId);
            int targetWidth = GetFaceWidth(neighbors.LeftFaceId);
            return new BoardCellPosition(neighbors.LeftFaceId, targetWidth - 1, position.Y);
        }

        public BoardCellPosition GetRight(BoardCellPosition position)
        {
            ValidatePosition(position);

            int currentFaceWidth = GetFaceWidth(position.FaceId);

            // 今の位置が右端でないなら同じ面の +1 の位置を返す。
            if (position.X < currentFaceWidth - 1)
            {
                return new BoardCellPosition(position.FaceId, position.X + 1, position.Y);
            }

            // 右端なら右隣の面を返す。
            BoardHorizontalNeighbors neighbors = GetHorizontalNeighbors(position.FaceId);
            return new BoardCellPosition(neighbors.RightFaceId, 0, position.Y);
        }

        public bool ContainsFace(BoardFaceId faceId)
        {
            return _definition.DimentionsByFaceId.ContainsKey(faceId);
        }

        public BoardDimensions GetDimensions(BoardFaceId faceId)
        {
            if (_definition.DimentionsByFaceId.TryGetValue(faceId, out var dimensions))
            {
                return dimensions;
            }
            else
            {
                throw new InvalidOperationException($"面ID {faceId} の寸法情報が定義されていません。");
            }
        }

        public int GetFaceWidth(BoardFaceId faceId)
        {
            return GetDimensions(faceId).FaceWidth;
        }

        public int GetFaceHeight(BoardFaceId faceId)
        {
            return GetDimensions(faceId).FaceHeight;
        }

        public BoardHorizontalNeighbors GetHorizontalNeighbors(BoardFaceId faceId)
        {
            if (_definition.HorizontalNeighborsByFaceId.TryGetValue(faceId, out var neighbors))
            {
                return neighbors;
            }
            else
            {
                throw new InvalidOperationException($"面ID {faceId} の水平隣接情報が定義されていません。");
            }
        }

        private readonly BoardShapeDefinition _definition;

        /// <summary>
        ///     指定された盤面位置がトポロジー上で扱える位置か検証する。
        /// </summary>
        /// <param name="position"> 検証対象の盤面位置 </param>
        private void ValidatePosition(BoardCellPosition position)
        {
            if (!ContainsFace(position.FaceId))
            {
                throw new InvalidOperationException($"面ID {position.FaceId} は存在しません。");
            }

            int width = GetFaceWidth(position.FaceId);

            if (position.X < 0 || position.X >= width)
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"X座標 {position.X} は盤面幅の範囲外です。");
            }
        }
    }
}
