using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の位相構造を生成するための定義クラス。
    /// </summary>
    public class BoardTopologyDefinition
    {
        public BoardTopologyDefinition(
            BoardDimensions dimensions,
            IReadOnlyCollection<BoardFaceId> faceIds,
            IReadOnlyDictionary<BoardFaceId, BoardHorizontalNeighbors> neighborsByFaceId)
        {
            if (faceIds == null)
            {
                throw new ArgumentNullException(nameof(faceIds));
            }

            if (neighborsByFaceId == null)
            {
                throw new ArgumentNullException(nameof(neighborsByFaceId));
            }

            if (faceIds.Count == 0)
            {
                throw new ArgumentException("面IDが存在しません。", nameof(faceIds));
            }

            Dimensions = dimensions;
            _faceIds = new HashSet<BoardFaceId>(faceIds);
            _neighborsByFaceId = new Dictionary<BoardFaceId, BoardHorizontalNeighbors>(neighborsByFaceId);

            Validate();
        }

        /// <summary> 盤面の寸法。 </summary>
        public BoardDimensions Dimensions { get; }

        /// <summary>
        ///     指定された面IDが存在するか判定する。
        /// </summary>
        /// <param name="faceId"> 指定した面ID </param>
        /// <returns> 存在するか </returns>
        public bool ContainsFace(BoardFaceId faceId)
        {
            return _faceIds.Contains(faceId);
        }

        /// <summary>
        ///     指定された面IDの左右隣接面を取得する。
        /// </summary>
        /// <param name="faceId"> 指定した面ID </param>
        /// <returns> 水平方向の隣接関係 </returns>
        public BoardHorizontalNeighbors GetHorizontalNeighbors(BoardFaceId faceId)
        {
            if (_neighborsByFaceId.TryGetValue(faceId, out BoardHorizontalNeighbors neighbors))
            {
                return neighbors;
            }

            throw new InvalidOperationException($"面ID {faceId} の接続情報が存在しません。");
        }

        private readonly HashSet<BoardFaceId> _faceIds;
        private readonly Dictionary<BoardFaceId, BoardHorizontalNeighbors> _neighborsByFaceId;

        /// <summary>
        ///     存在するかのチェック。
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        private void Validate()
        {
            foreach (var faceId in _faceIds)
            {
                if (!_neighborsByFaceId.ContainsKey(faceId))
                {
                    throw new InvalidOperationException($"面ID {faceId} の接続情報が存在しません。");
                }

                BoardHorizontalNeighbors neighbors = _neighborsByFaceId[faceId];

                if (!_faceIds.Contains(neighbors.LeftFaceId))
                {
                    throw new InvalidOperationException($"面ID {faceId} の左隣 {neighbors.LeftFaceId} が存在しません。");
                }

                if (!_faceIds.Contains(neighbors.RightFaceId))
                {
                    throw new InvalidOperationException($"面ID {faceId} の右隣 {neighbors.RightFaceId} が存在しません。");
                }
            }
        }
    }
}
