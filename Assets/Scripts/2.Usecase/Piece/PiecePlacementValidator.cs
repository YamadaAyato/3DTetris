using System;
using System.Collections.Generic;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中のピースの配置可能性を判定するクラス。
    /// </summary>
    public class PiecePlacementValidator
    {
        public PiecePlacementValidator(
            BoardModel boardModel,
            PiecePositionResolver resolver)
        {
            _boardModel = boardModel ?? throw new ArgumentNullException(nameof(boardModel));
            _resolver = resolver ?? throw new ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        ///     操作中のピースを占有可能かどうかを判定する。
        /// </summary>
        /// <param name="piece"> 判定対象のピース </param>
        /// <returns> 占有可能であれば true、そうでなければ false </returns>
        public bool CanOccupy(ActivePiece piece)
        {
            if (piece == null)
            {
                throw new ArgumentNullException(nameof(piece));
            }

            IReadOnlyList<BoardCellPosition> positions = _resolver.Resolve(piece);
            return _boardModel.CanOccupy(positions);
        }

        private readonly BoardModel _boardModel;
        private readonly PiecePositionResolver _resolver;
    }
}
