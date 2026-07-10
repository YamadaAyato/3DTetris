using System.Collections.Generic;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///     操作中のピースの配置可能性を判定するクラス。
    /// </summary>
    public class PiecePlecementValidator
    {
        public PiecePlecementValidator(
            BoardModel boardModel,
            PiecePositionResolver resolver)
        {
            _boardModel = boardModel ?? throw new System.ArgumentNullException(nameof(boardModel));
            _resolver = resolver ?? throw new System.ArgumentNullException(nameof(resolver));
        }

        /// <summary>
        ///     操作中のピースを配置可能かどうかを判定する。
        /// </summary>
        /// <param name="piece"> 判定対象のピース </param>
        /// <returns> 配置可能であれば true、そうでなければ false </returns>
        public bool CanPlace(ActivePiece piece)
        {
            if (piece == null)
            {
                throw new System.ArgumentNullException(nameof(piece));
            } 

            IReadOnlyList<BoardCellPosition> positions = _resolver.Resolve(piece);
            return _boardModel.CanPlace(positions);
        }

        /// <summary>
        ///     操作中のピースを配置可能かどうかを判定し、配置可能な場合は配置位置を返す。
        /// </summary>
        /// <param name="piece"> 判定対象のピース </param>
        /// <param name="positions"> 配置可能な場合の配置位置 </param>
        /// <returns> 配置可能であれば true、そうでなければ false </returns>
        public bool TryGetPlaceablePosition(ActivePiece piece, out IReadOnlyList<BoardCellPosition> positions)
        {
            if (piece == null)
            {
                throw new System.ArgumentNullException(nameof(piece));
            }

            positions = _resolver.Resolve(piece);
            return _boardModel.CanPlace(positions);
        }

        private readonly BoardModel _boardModel;
        private readonly PiecePositionResolver _resolver;
    }
}
