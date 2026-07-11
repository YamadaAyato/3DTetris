using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    public class DataDrivenBoardTopology : IBoardTopology
    {
        public DataDrivenBoardTopology(BoardTopologyDefinition definition)
        {
            _definition = definition ?? throw new ArgumentNullException(nameof(definition));
        }

        /// <summary> 盤面の寸法。 </summary>
        public BoardDimensions Dimensions => _definition.Dimensions;

        /// <summary> 盤面の面ID一覧。 </summary>
        public IReadOnlyList<BoardFaceId> FaceIds => _definition.FaceIds;

        public BoardCellPosition GetLeft(BoardCellPosition position)
        {
            ValidatePosition(position);

            // 今の位置が左端でないなら同じ面の -1 の位置を返す。
            if (position.X > 0)
            {
                return new BoardCellPosition(position.FaceId, position.X - 1, position.Y);
            }

            // 左端なら左隣の面を返す。
            BoardHorizontalNeighbors neighbors = _definition.GetHorizontalNeighbors(position.FaceId);
            return new BoardCellPosition(neighbors.LeftFaceId, Dimensions.FaceWidth - 1, position.Y);
        }

        public BoardCellPosition GetRight(BoardCellPosition position)
        {
            ValidatePosition(position);

            // 今の位置が右端でないなら同じ面の +1 の位置を返す。
            if (position.X < Dimensions.FaceWidth - 1)
            {
                return new BoardCellPosition(position.FaceId, position.X + 1, position.Y);
            }

            // 右端なら右隣の面を返す。
            BoardHorizontalNeighbors neighbors = _definition.GetHorizontalNeighbors(position.FaceId);
            return new BoardCellPosition(neighbors.RightFaceId, 0, position.Y);
        }

        public bool ContainsFace(BoardFaceId faceId)
        {
            return _definition.ContainsFace(faceId);
        }

        private readonly BoardTopologyDefinition _definition;

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

            if (position.X < 0 || position.X >= Dimensions.FaceWidth)
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"X座標 {position.X} は盤面幅の範囲外です。");
            }
        }
    }
}
