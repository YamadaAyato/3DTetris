using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の形状を定義するクラス。
    /// </summary>
    public class BoardShapeDefinition
    {
        public BoardShapeDefinition(
            IReadOnlyList<BoardFaceId> faceIds,
            IReadOnlyDictionary<BoardFaceId, BoardDimensions> dimentionsByFaceId,
            IReadOnlyDictionary<BoardFaceId, BoardHorizontalNeighbors> horizontalNeighborsByFaceId,
            BoardFaceId initialFaceId)
        {
            if (faceIds == null)
            {
                throw new ArgumentNullException(nameof(faceIds));
            }

            if (dimentionsByFaceId == null)
            {
                throw new ArgumentNullException(nameof(dimentionsByFaceId));
            }

            if (horizontalNeighborsByFaceId == null)
            {
                throw new ArgumentNullException(nameof(horizontalNeighborsByFaceId));
            }

            if (faceIds.Count == 0)
            {
                throw new ArgumentException("面IDのリストは空であってはなりません。", nameof(faceIds));
            }

            FaceIds = new List<BoardFaceId>(faceIds).AsReadOnly();
            DimentionsByFaceId = new Dictionary<BoardFaceId, BoardDimensions>(dimentionsByFaceId);
            HorizontalNeighborsByFaceId = new Dictionary<BoardFaceId, BoardHorizontalNeighbors>(horizontalNeighborsByFaceId);
            InitialFaceId = initialFaceId;

            Validate();
        }

        /// <summary> 盤面の面IDのリスト。 /// </summary>
        public IReadOnlyList<BoardFaceId> FaceIds { get; }

        /// <summary> 面IDに対応する寸法の辞書。 /// </summary>
        public IReadOnlyDictionary<BoardFaceId, BoardDimensions> DimentionsByFaceId { get; }

        /// <summary> 面IDに対応する水平隣接情報の辞書。 /// </summary>
        public IReadOnlyDictionary<BoardFaceId, BoardHorizontalNeighbors> HorizontalNeighborsByFaceId { get; }

        /// <summary> 初期面ID。 /// </summary>
        public BoardFaceId InitialFaceId { get; }

        /// <summary>
        ///     このBoardShapeDefinitionの整合性を検証します。
        /// </summary>
        public void Validate()
        {
            HashSet<BoardFaceId> faceIdSet = new HashSet<BoardFaceId>(FaceIds);

            if (faceIdSet.Count != FaceIds.Count)
            {
                throw new InvalidOperationException("FaceIdsに重複が含まれています。");
            }

            if (!faceIdSet.Contains(InitialFaceId))
            {
                throw new InvalidOperationException("InitialFaceIdがFaceIdsに含まれていません。");
            }

            for (int i = 0; i < FaceIds.Count; i++)
            {
                var faceId = FaceIds[i];

                if (!DimentionsByFaceId.ContainsKey(faceId))
                {
                    throw new InvalidOperationException($"面ID {faceId} に対応する寸法が定義されていません。");
                }

                if (!HorizontalNeighborsByFaceId.TryGetValue(faceId, out var neighbors))
                {
                    throw new InvalidOperationException($"面ID {faceId} に対応する水平隣接情報が定義されていません。");
                }

                if (!faceIdSet.Contains(neighbors.LeftFaceId))
                {
                    throw new InvalidOperationException($"面ID {faceId} の左隣接面ID {neighbors.LeftFaceId} が FaceIds に含まれていません。");
                }

                if (!faceIdSet.Contains(neighbors.RightFaceId))
                {
                    throw new InvalidOperationException($"面ID {faceId} の右隣接面ID {neighbors.RightFaceId} が FaceIds に含まれていません。");
                }
            }
        }
    }
}
