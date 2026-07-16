using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中のピースの回転を管理するユースケース。
    /// </summary>
    public class PieceRotationUsecase
    {
        public PieceRotationUsecase(
            IBoardTopology boardTopology,
            PiecePlacementValidator placementValidator)
        {
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
            _placementValidator = placementValidator ?? throw new ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     操作中のピースを回転させる。
        /// </summary>
        /// <param name="activePiece">回転させる対象のピース。</param>
        /// <returns>回転が成功した場合は true。</returns>
        public bool Rotate(ActivePiece activePiece)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            PieceRotation nextRotation = GetNextRotation(activePiece.Rotation);

            // 回転後に、隣接面の端セルより奥へ入り込む場合は回転しない。
            if (!CanRotateWithinSharedEdgeRange(activePiece, nextRotation))
            {
                return false;
            }

            ActivePiece candidatePiece = new ActivePiece(
                activePiece.Definition,
                activePiece.OriginFaceId,
                activePiece.OriginX,
                activePiece.OriginY,
                nextRotation);

            if (!_placementValidator.CanOccupy(candidatePiece))
            {
                return false;
            }

            activePiece.RotationTo(nextRotation);
            return true;
        }

        private readonly IBoardTopology _boardTopology;
        private readonly PiecePlacementValidator _placementValidator;

        /// <summary>
        ///     回転後のピース全体が、現在面と隣接面の共有端の範囲内に収まるか判定する。
        /// </summary>
        /// <param name="activePiece">操作中ミノ。</param>
        /// <param name="rotation">確認する回転状態。</param>
        /// <returns>共有端の範囲内に収まる場合は true。</returns>
        private bool CanRotateWithinSharedEdgeRange(
            ActivePiece activePiece,
            PieceRotation rotation)
        {
            int faceWidth = _boardTopology.GetFaceWidth(activePiece.OriginFaceId);

            GetRotatedXRange(
                activePiece.Definition,
                rotation,
                out int minOffsetX,
                out int maxOffsetX);

            int minCellX = activePiece.OriginX + minOffsetX;
            int maxCellX = activePiece.OriginX + maxOffsetX;

            // -1 は左隣面の右端セルとして許可する。
            // faceWidth は右隣面の左端セルとして許可する。
            // それより外側は、隣接面へ深く入り込むため許可しない。
            return minCellX >= -1 &&
                   maxCellX <= faceWidth;
        }

        /// <summary>
        ///     回転後のピースのX方向範囲を取得する。
        /// </summary>
        /// <param name="definition">ピース定義。</param>
        /// <param name="rotation">回転状態。</param>
        /// <param name="minX">最小Xオフセット。</param>
        /// <param name="maxX">最大Xオフセット。</param>
        private static void GetRotatedXRange(
            PieceDefinition definition,
            PieceRotation rotation,
            out int minX,
            out int maxX)
        {
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            PieceCellOffset firstOffset = RotateOffset(
                definition.Cells[0],
                rotation);

            minX = firstOffset.X;
            maxX = firstOffset.X;

            for (int i = 1; i < definition.Cells.Count; i++)
            {
                PieceCellOffset offset = RotateOffset(
                    definition.Cells[i],
                    rotation);

                if (offset.X < minX)
                {
                    minX = offset.X;
                }

                if (offset.X > maxX)
                {
                    maxX = offset.X;
                }
            }
        }

        /// <summary>
        ///     ミノ内の相対位置を、指定された回転状態に合わせて回転する。
        /// </summary>
        /// <param name="offset">回転前の相対位置。</param>
        /// <param name="rotation">適用する回転状態。</param>
        /// <returns>回転後の相対位置。</returns>
        private static PieceCellOffset RotateOffset(PieceCellOffset offset, PieceRotation rotation)
        {
            switch (rotation)
            {
                case PieceRotation.Rotation0:
                    return offset;

                case PieceRotation.Rotation90:
                    return new PieceCellOffset(-offset.Y, offset.X);

                case PieceRotation.Rotation180:
                    return new PieceCellOffset(-offset.X, -offset.Y);

                case PieceRotation.Rotation270:
                    return new PieceCellOffset(offset.Y, -offset.X);

                default:
                    throw new ArgumentOutOfRangeException(nameof(rotation), rotation, "未対応の回転状態です。");
            }
        }

        /// <summary>
        ///     現在の回転状態から次の回転状態を取得する。
        /// </summary>
        /// <param name="currentRotation">現在の回転状態。</param>
        /// <returns>次の回転状態。</returns>
        private static PieceRotation GetNextRotation(PieceRotation currentRotation)
        {
            return currentRotation switch
            {
                PieceRotation.Rotation0 => PieceRotation.Rotation90,
                PieceRotation.Rotation90 => PieceRotation.Rotation180,
                PieceRotation.Rotation180 => PieceRotation.Rotation270,
                PieceRotation.Rotation270 => PieceRotation.Rotation0,
                _ => throw new ArgumentOutOfRangeException(nameof(currentRotation), currentRotation, "未対応の回転状態です。")
            };
        }
    }
}