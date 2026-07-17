using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ピースの生成位置を表す構造体。
    /// </summary>
    public readonly struct PieceSpawnSettings
    {
        public PieceSpawnSettings(int originX, int spawnHeightOffset, PieceRotation initialRotation)
        {
            if (spawnHeightOffset < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(spawnHeightOffset), "スポーン位置のオフセットは0以上である必要があります。");
            }

            OriginX = originX;
            SpawnHeightOffset = spawnHeightOffset;
            InitialRotation = initialRotation;
        }

        /// <summary> ピースの生成位置の基準となるX座標。 </summary>
        public int OriginX { get; }

        /// <summary> 盤面上端から何マス上にピースを生成するかのオフセット。 </summary>
        public int SpawnHeightOffset { get; }

        /// <summary> ピースの生成時の回転状態。 </summary>
        public PieceRotation InitialRotation { get; }
    }
}
