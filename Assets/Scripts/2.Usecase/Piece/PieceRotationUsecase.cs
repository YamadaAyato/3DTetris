using ThreeDTetris.Model;
using UnityEngine;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中のピースの回転を管理するユースケース。
    /// </summary>
    public class PieceRotationUsecase
    {
        public PieceRotationUsecase(PiecePlecementValidator placementValidator)
        {
            _placementValidator = placementValidator ?? throw new System.ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     操作中のピースを回転させる。
        /// </summary>
        /// <param name="activePiece"> 回転させる対象のピース </param>
        /// <returns> 回転が成功したかどうか </returns>
        public bool Rotate(ActivePiece activePiece)
        {
            if (activePiece == null)
            { 
                throw new System.ArgumentNullException(nameof(activePiece)); 
            }

            PieceRotation nextRotation = GetNextRotation(activePiece.Rotation);

            // 回転後の状態で配置可能かどうかを検証する。
            ActivePiece candidatePiece = new(
                activePiece.Definition, 
                activePiece.OriginFaceId,
                activePiece.OriginX,
                activePiece.OriginY, nextRotation);

            if(!_placementValidator.CanPlace(candidatePiece))
            {
                return false;
            }

            // 回転後の状態で配置可能な場合は、操作中のピースを回転させる。
            activePiece.RotationTo(nextRotation);
            return true;
        }

        private readonly PiecePlecementValidator _placementValidator;

        /// <summary>
        ///     現在の回転状態から次の回転状態を取得する。
        /// </summary>
        /// <param name="currentRotation"> 現在の回転状態 </param>
        /// <returns> 次の回転状態 </returns>
        private static  PieceRotation GetNextRotation(PieceRotation currentRotation)
        {
            return currentRotation switch
            {
                PieceRotation.Rotation0 => PieceRotation.Rotation90,
                PieceRotation.Rotation90 => PieceRotation.Rotation180,
                PieceRotation.Rotation180 => PieceRotation.Rotation270,
                PieceRotation.Rotation270 => PieceRotation.Rotation0,
                _ => throw new System.ArgumentOutOfRangeException(nameof(currentRotation), currentRotation, null)
            };
        }
    }
}
