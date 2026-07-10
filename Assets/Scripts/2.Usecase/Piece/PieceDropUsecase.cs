using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中のピースの落下を管理するユースケース。
    /// </summary>
    public class PieceDropUsecase
    {
        public PieceDropUsecase(PiecePlecementValidator placementValidator)
        {
            _placementValidator = placementValidator ?? throw new ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     操作中のピースを下方向に1ステップ落下させる。
        /// </summary>
        /// <param name="activePiece"> 操作中のピース </param>
        /// <returns> ピースが1ステップ落下できたかどうか </returns>
        public bool DropOneStep(ActivePiece activePiece)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            // 下方向に1ステップ移動させるため、Y座標を1減少させる。
            int candidateY = activePiece.OriginY - 1;

            ActivePiece candidatePiece = CreateCandiateWithY(activePiece, candidateY);

            // 移動後の状態で配置可能かどうかを検証する。
            if (!_placementValidator.CanPlace(candidatePiece))
            {
                return false;
            }

            // 移動後の状態で配置可能な場合は、操作中のピースを下方向に移動させる。
            activePiece.MoveTo(candidatePiece.OriginFaceId, candidatePiece.OriginX, candidatePiece.OriginY);
            return true;
        }

        /// <summary>
        ///     操作中のピースを下方向に落下させ、最終的な落下量を返す。
        /// </summary>
        /// <param name="activePiece"> 操作中のピース </param>
        /// <returns> 最終的な落下量 </returns>
        public int DropToBottom(ActivePiece activePiece)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            int dropCount = 0;

            while (DropOneStep(activePiece))
            {
                dropCount++;
            }

            return dropCount;
        }

        private readonly PiecePlecementValidator _placementValidator;

        /// <summary>
        ///     指定されたY座標を使用して、操作中のピースの候補を作成する。
        /// </summary>
        /// <param name="activePiece"> 操作中のピース </param>
        /// <param name="originY"> 使用するY座標 </param>
        /// <returns> 指定されたY座標を使用した操作中のピースの候補 </returns>
        private static ActivePiece CreateCandiateWithY(ActivePiece activePiece, int originY)
        {
            return new ActivePiece(
                activePiece.Definition,
                activePiece.OriginFaceId,
                activePiece.OriginX,
                originY, // 指定されたY座標を使用
                activePiece.Rotation);
        }
    }
}
