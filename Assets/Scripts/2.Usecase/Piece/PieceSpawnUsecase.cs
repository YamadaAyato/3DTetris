using System;
using ThreeDTetris.Model;

namespace ThreeDTetris.Usecase
{
    /// <summary>
    ///    ピースを指定された位置に生成し、配置可能かどうかを検証するユースケース。
    /// </summary>
    public class PieceSpawnUsecase
    {
        public PieceSpawnUsecase(IBoardTopology boardTopology, PiecePlacementValidator placementValidator)
        {
            _boardTopology = boardTopology ?? throw new ArgumentNullException(nameof(boardTopology));
            _placementValidator = placementValidator ?? throw new ArgumentNullException(nameof(placementValidator));
        }

        /// <summary>
        ///     指定された位置に新しいアクティブピースを生成し、配置可能かどうかを検証する。
        /// </summary>
        /// <param name="definition"> ピースの定義 </param>
        /// <param name="spawnFaceId"> ピースの生成面ID </param>
        /// <param name="spawnSettings"> ピースの生成設定 </param>
        /// <param name="activePiece"> 生成されたアクティブピース </param>
        /// <returns> 配置可能であればtrue、そうでなければfalse </returns>
        public bool TrySpawn(
            PieceDefinition definition,
            BoardFaceId spawnFaceId,
            PieceSpawnSettings spawnSettings,
            out ActivePiece activePiece)
        {
            // 引数の検証
            if (definition == null)
            {
                throw new ArgumentNullException(nameof(definition));
            }

            if (!_boardTopology.ContainsFace(spawnFaceId))
            {
                throw new InvalidOperationException($"面ID {spawnFaceId} は存在しません。");
            }

            if (spawnSettings.OriginX < 0 ||
                spawnSettings.OriginX >= _boardTopology.Dimensions.FaceWidth)
            {
                throw new InvalidOperationException($"X座標 {spawnSettings.OriginX} は面ID {spawnFaceId} の範囲外です。");
            }

            int originY = _boardTopology.Dimensions.FaceHeight + spawnSettings.SpawnHeightOffset;

            // 新しいアクティブピースを作成する。
            var candidatePiece = new ActivePiece(definition, spawnFaceId, spawnSettings.OriginX, originY, spawnSettings.InitialRotation);

            // 配置可能かどうかを検証する
            if (!_placementValidator.CanOccupy(candidatePiece))
            {
                activePiece = null;
                return false;
            }

            activePiece = candidatePiece;
            return true;
        }

        private readonly IBoardTopology _boardTopology;
        private readonly PiecePlacementValidator _placementValidator;
    }
}
