using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ピースの落下設定を表す構造体。
    /// </summary>
    public readonly struct PieceFallSettings
    {
        public PieceFallSettings(float fallIntervalSeconds)
        {
            if (fallIntervalSeconds <= 0f)
            {
                throw new ArgumentOutOfRangeException(nameof(fallIntervalSeconds), "落下間隔は正の値でなければなりません。");
            }
            FallIntervalSeconds = fallIntervalSeconds;
        }

        /// <summary> ピースの落下間隔（秒）を取得します。 /// </summary>  
        public float FallIntervalSeconds { get; }
    }
}
