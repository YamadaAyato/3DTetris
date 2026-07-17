using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///    操作中のピースを固定済みのブロックとして盤面に登録するユースケース。
    /// </summary>
    public class PieceLockUsecase
    {
        public PieceLockUsecase(BoardModel boardModel, PiecePositionResolver piecePositionResolver)
        {
            _boardModel = boardModel ?? throw new ArgumentNullException(nameof(boardModel));
            _piecePositionResolver = piecePositionResolver ?? throw new ArgumentNullException(nameof(piecePositionResolver));
        }

        /// <summary>
        ///     操作中のピースをボードの現在位置に固定する。
        /// </summary>
        /// <param name="activePiece"> 配置するアクティブなピース </param>
        /// <returns> 配置に成功した場合はtrue、失敗した場合はfalse </returns>
        public bool Lock(ActivePiece activePiece)
        {
            if (activePiece == null)
            {
                throw new ArgumentNullException(nameof(activePiece));
            }

            // アクティブなピースのブロックの位置を取得する。
            var positions = _piecePositionResolver.Resolve(activePiece);

            // ブロックを配置できるかどうかを確認する。
            if (!_boardModel.CanPlaceBlocks(positions))
            {
                return false;
            }

            // 配置できる場合は、ブロックを配置する。
            _boardModel.PlaceBlocks(positions);
            return true;
        }

        private readonly BoardModel _boardModel;
        private readonly PiecePositionResolver _piecePositionResolver;
    }
}
