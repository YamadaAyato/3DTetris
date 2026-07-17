using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     操作中のミノが、盤面上のどのマスを占有しているかを計算するクラス。
    /// </summary>
    public class PiecePositionResolver
    {
        public PiecePositionResolver(IBoardTopology topology)
        {
            _topology = topology ?? throw new ArgumentNullException(nameof(topology));
        }

        /// <summary>
        ///     操作中のミノが現在占有している盤面セル一覧を取得する。
        /// </summary>
        /// <param name="activePiece"> 操作中のミノ </param>
        /// <returns> ミノが占有している板面セル一覧 </returns>
        public IReadOnlyList<BoardCellPosition> Resolve(ActivePiece activePiece)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            // ミノ定義から、基準点を原点とした相対セルの一覧を取得する。
            IReadOnlyList<PieceCellOffset> cellOffsets = activePiece.Definition.Cells;
            BoardCellPosition[] positions = new BoardCellPosition[cellOffsets.Count];

            BoardCellPosition originPosition = new(
                activePiece.OriginFaceId,
                activePiece.OriginX,
                activePiece.OriginY);

            // 相対セルを盤面セルへ変換した結果を格納していく・
            for (int i = 0; i < cellOffsets.Count; i++)
            {
                // 現在の回転状態に合わせてミノ内の相対位置を回転。
                PieceCellOffset rotatedOffset = RotateOffset(cellOffsets[i], activePiece.Rotation);

                BoardCellPosition horizontalPosition = MoveHorizontal(originPosition, rotatedOffset.X);

                // X方向のズレは面マタギがあるのでBoardTopology経由で移動。
                positions[i] = new BoardCellPosition(
                    horizontalPosition.FaceId,
                    horizontalPosition.X,
                    horizontalPosition.Y + rotatedOffset.Y);
            }

            return positions;
        }

        private readonly IBoardTopology _topology;

        /// <summary>
        ///     指定された横方向のずれだけ盤面上を移動する。
        /// </summary>
        /// <param name="originPosition"> 移動開始位置 </param>
        /// <param name="offsetX"> 横方向のずれ </param>
        /// <returns> 移動後のセル位置 </returns>
        private BoardCellPosition MoveHorizontal(BoardCellPosition originPosition, int offsetX)
        {
            BoardCellPosition current = originPosition;

            if (offsetX > 0)
            {
                for (int i = 0; i < offsetX; i++)
                {
                    current = _topology.GetRight(current);
                }

                return current;
            }

            if (offsetX < 0)
            {
                for (int i = 0; i < -offsetX; i++)
                {
                    current = _topology.GetLeft(current);
                }
            }
            return current;
        }

        /// <summary>
        ///     ミノ内の相対位置を、指定された回転状態に合わせて回転する。
        /// </summary>
        /// <param name="offset"> 回転前の相対位置 </param>
        /// <param name="rotation"> 適用する回転状態 </param>
        /// <returns> 回転後の相対位置 </returns>
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
    }
}
