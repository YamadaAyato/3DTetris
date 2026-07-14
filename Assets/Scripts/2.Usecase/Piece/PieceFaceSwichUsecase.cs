using System;
using ThreeDTetris.Model;
using UnityEngine;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中のピースを別の面に移動するユースケース。
    /// </summary>
    public class PieceFaceSwichUsecase
    {
        public PieceFaceSwichUsecase(IBoardTopology boardTopology, PiecePlacementValidator placementValidator)
        {
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
            _placementValidator = placementValidator ?? throw new ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     操作中のピースを別の面に移動できるか試みます。
        /// </summary>
        /// <param name="activePiece">　操作中のピース　</param>
        /// <param name="targetFaceId">　移動先の面のID　</param>
        /// <returns>　移動に成功した場合はtrue、失敗した場合はfalse　</returns>
        public bool TrySwitchFace(ActivePiece activePiece, BoardFaceId targetFaceId)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            if (!_boardTopology.ContainsFace(targetFaceId))
            {
                return false;
            }

            // 移動先の面の幅を取得し、操作中のピースのX座標が移動先の面の幅を超えないように調整します。
            int targetWidth = _boardTopology.GetFaceWidth(targetFaceId);
            int candidateX = Mathf.Min(activePiece.OriginX, targetWidth - 1);

            // 移動先の面の高さを取得し、操作中のピースのY座標が移動先の面の高さを超えないように調整します。
            int maxUpOffset = _boardTopology.GetFaceHeight(targetFaceId) + 1;

            // 移動先の面のY座標を上方向にずらしながら、操作中のピースが移動先の面に収まるかどうかを確認します。
            for (int upOffset = 0; upOffset <= maxUpOffset; upOffset++)
            {
                int candidateY = activePiece.OriginY + upOffset;

                ActivePiece candidatePiece = new ActivePiece(
                    activePiece.Definition,
                    targetFaceId,
                    candidateX,
                    candidateY,
                    activePiece.Rotation);

                if (!_placementValidator.CanOccupy(candidatePiece))
                {
                    continue;
                }

                activePiece.MoveTo(targetFaceId, candidateX, candidateY);
                return true;
            }

            return false;
        }

        private readonly IBoardTopology _boardTopology;
        private readonly PiecePlacementValidator _placementValidator;
    }
}
