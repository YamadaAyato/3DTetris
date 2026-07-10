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
            PiecePlecementValidator placementValidator)
        {
            _boardTopology = boardTopology ?? throw new System.ArgumentNullException(nameof(boardTopology));
            _placementValidator = placementValidator ?? throw new System.ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     操作中ミノを左に移動する。
        /// </summary>
        /// <param name="activePiece"> 操作中ミノ </param>
        /// <returns> 移動に成功した場合は true、それ以外の場合は false </returns>
        public bool MoveLeft(ActivePiece activePiece) => Move(activePiece, PieceMoveDirection.Left);

        /// <summary>
        ///     操作中ミノを右に移動する。
        /// </summary>
        /// <param name="activePiece"> 操作中ミノ </param>
        /// <returns> 移動に成功した場合は true、それ以外の場合は false </returns>
        public bool MoveRight(ActivePiece activePiece) => Move(activePiece, PieceMoveDirection.Right);

        private readonly IBoardTopology _boardTopology;
        private readonly PiecePlecementValidator _placementValidator;

        /// <summary>
        ///     操作中ミノの現在の基準位置を盤面位置として生成する。
        ///     読みやすくするためのヘルパーメソッド。
        /// </summary>
        /// <param name="piece"> 操作中ミノ </param>
        /// <returns> 盤面位置 </returns>
        private static BoardCellPosition CreateOriginPosition(ActivePiece piece)
        {
            return new BoardCellPosition(
                piece.OriginFaceId,
                piece.OriginX,
                piece.OriginY);
        }

        /// <summary>
        ///     操作中ミノを指定された方向に移動する。
        /// </summary>
        /// <param name="activePiece"> 操作中ミノ </param>
        /// <param name="direction"> 移動方向 </param>
        /// <returns> 移動に成功した場合は true、それ以外の場合は false </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        private bool Move(ActivePiece activePiece, PieceMoveDirection direction)
        {
            if (activePiece == null)
            {
                throw new System.ArgumentNullException(nameof(activePiece));
            }

            var originPosition = CreateOriginPosition(activePiece);
            BoardCellPosition movedOrigin = GetMovedOrigin(originPosition, direction);

            return TryMove(activePiece, movedOrigin);
        }

        /// <summary>
        ///     指定された方向に移動した場合の盤面位置を取得する。
        /// </summary>
        /// <param name="origin"> 現在の盤面位置 </param>
        /// <param name="direction"> 移動方向 </param>
        /// <returns> 移動後の盤面位置 </returns>
        /// <exception cref="System.ArgumentOutOfRangeException"></exception>
        private BoardCellPosition GetMovedOrigin(BoardCellPosition origin, PieceMoveDirection direction)
        {
            return direction switch
            {
                PieceMoveDirection.Left => _boardTopology.GetLeft(origin),
                PieceMoveDirection.Right => _boardTopology.GetRight(origin),
                _ => throw new System.ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        }

        /// <summary>
        ///     操作中ミノを指定された盤面位置に移動する。
        /// </summary>
        /// <param name="activePiece"> 操作中ミノ </param>
        /// <param name="movedOrigin"> 移動先の盤面位置 </param>
        /// <returns> 移動に成功した場合は true、それ以外の場合は false </returns>
        private bool TryMove(ActivePiece activePiece, BoardCellPosition movedOrigin)
        {
            // 本体を移動させる前に、移動先に置けるかどうかを確認するための候補ものを生成する。
            ActivePiece candidatePosition = new ActivePiece(
                activePiece.Definition,
                movedOrigin.FaceId,
                movedOrigin.X,
                movedOrigin.Y,
                activePiece.Rotation);

            // 移動先に置けるかどうかを確認する。
            if (!_placementValidator.CanPlace(candidatePosition))
            {
                return false;
            }

            // 移動先に置ける場合は、実際に操作中ミノを移動させる。
            activePiece.MoveTo(
                movedOrigin.FaceId,
                movedOrigin.X,
                movedOrigin.Y);

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
