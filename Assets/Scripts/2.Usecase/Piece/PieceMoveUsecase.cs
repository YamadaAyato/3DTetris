using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中ミノの左右移動を行うユースケース。
    /// </summary>
    public class PieceMoveUsecase
    {
        public PieceMoveUsecase(
            IBoardTopology boardTopology,
            PiecePlacementValidator placementValidator)
        {
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
            _placementValidator = placementValidator ?? throw new ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     操作中ミノを左に移動する。
        /// </summary>
        /// <param name="activePiece">操作中ミノ。</param>
        /// <returns>移動に成功した場合は true、それ以外の場合は false。</returns>
        public bool MoveLeft(ActivePiece activePiece) => Move(activePiece, PieceMoveDirection.Left);

        /// <summary>
        ///     操作中ミノを右に移動する。
        /// </summary>
        /// <param name="activePiece">操作中ミノ。</param>
        /// <returns>移動に成功した場合は true、それ以外の場合は false。</returns>
        public bool MoveRight(ActivePiece activePiece) => Move(activePiece, PieceMoveDirection.Right);

        private readonly IBoardTopology _boardTopology;
        private readonly PiecePlacementValidator _placementValidator;

        /// <summary>
        ///     操作中ミノを指定された方向に移動する。
        /// </summary>
        /// <param name="activePiece">操作中ミノ。</param>
        /// <param name="direction">移動方向。</param>
        /// <returns>移動に成功した場合は true、それ以外の場合は false。</returns>
        private bool Move(ActivePiece activePiece, PieceMoveDirection direction)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            int movedOriginX = GetMovedOriginX(
                activePiece.OriginX,
                direction);

            // 通常移動では操作面を切り替えない。
            // ただし、ピースの端セルだけは隣接面の端セルと共有できる。
            if (!CanMoveWithinSharedEdgeRange(activePiece, movedOriginX))
            {
                return false;
            }

            return TryMove(
                activePiece,
                activePiece.OriginFaceId,
                movedOriginX,
                activePiece.OriginY);
        }

        /// <summary>
        ///     指定された方向に移動した場合の基準X座標を取得する。
        /// </summary>
        /// <param name="originX">現在の基準X座標。</param>
        /// <param name="direction">移動方向。</param>
        /// <returns>移動後の基準X座標。</returns>
        private static int GetMovedOriginX(int originX, PieceMoveDirection direction)
        {
            return direction switch
            {
                PieceMoveDirection.Left => originX - 1,
                PieceMoveDirection.Right => originX + 1,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, "未対応の移動方向です。")
            };
        }

        /// <summary>
        ///     ピース全体が現在面と隣接面の共有端の範囲内に収まるか判定する。
        /// </summary>
        /// <param name="activePiece">操作中ミノ。</param>
        /// <param name="originX">確認する基準X座標。</param>
        /// <returns>共有端の範囲内に収まる場合は true。</returns>
        private bool CanMoveWithinSharedEdgeRange(ActivePiece activePiece, int originX)
        {
            int faceWidth = _boardTopology.GetFaceWidth(activePiece.OriginFaceId);

            // 通常移動では、基準X座標は必ず現在面の中に残す。
            // ピースのセルだけが隣接面の端セルへはみ出すことを許可する。
            if (originX < 0 || originX >= faceWidth)
            {
                return false;
            }

            GetRotatedXRange(
                activePiece.Definition,
                activePiece.Rotation,
                out int minOffsetX,
                out int maxOffsetX);

            int minCellX = originX + minOffsetX;
            int maxCellX = originX + maxOffsetX;

            // -1 は左隣面の右端セルとして許可する。
            // faceWidth は右隣面の左端セルとして許可する。
            // それより外側は、隣接面へ深く入り込むため許可しない。
            return minCellX >= 0 &&
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
        ///     操作中ミノを指定された基準位置に移動する。
        /// </summary>
        /// <param name="activePiece">操作中ミノ。</param>
        /// <param name="originFaceId">移動先の基準面ID。</param>
        /// <param name="originX">移動先の基準X座標。</param>
        /// <param name="originY">移動先の基準Y座標。</param>
        /// <returns>移動に成功した場合は true、それ以外の場合は false。</returns>
        private bool TryMove(
            ActivePiece activePiece,
            BoardFaceId originFaceId,
            int originX,
            int originY)
        {
            // 本体を移動させる前に、移動先に置けるかどうかを確認するための候補ミノを生成する。
            ActivePiece candidatePiece = new ActivePiece(
                activePiece.Definition,
                originFaceId,
                originX,
                originY,
                activePiece.Rotation);

            // 移動先に置けるかどうかを確認する。
            if (!_placementValidator.CanOccupy(candidatePiece))
            {
                return false;
            }

            // 移動先に置ける場合は、実際に操作中ミノを移動させる。
            activePiece.MoveTo(
                originFaceId,
                originX,
                originY);

            return true;
        }

        /// <summary>
        ///     ピースの移動方向を表す列挙型。
        /// </summary>
        private enum PieceMoveDirection
        {
            Left,
            Right
        }
    }
}