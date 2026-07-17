using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の面の幅と高さを表す構造体。
    /// </summary>
    public readonly struct BoardDimensions
    {
        public BoardDimensions(int faceWidth, int height)
        {
            if (faceWidth <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(faceWidth), "面の横幅は1以上である必要があります。");
            }

            if (height <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height), "盤面の高さは1以上である必要があります。");
            }

            FaceWidth = faceWidth;
            FaceHeight = height;
        }

        /// <summary> 各面の横幅。 </summary>
        public int FaceWidth { get; }

        /// <summary> 盤面の高さ。 </summary>
        public int FaceHeight { get; }

        /// <summary>
        ///     指定された座標が寸法内におさまっているか判定する。
        /// </summary>
        /// <param name="x"> 指定したx座標 </param>
        /// <param name="y"> 指定したy座標 </param>
        /// <returns> 寸法内におさまっているか </returns>
        public bool Contains(int x, int y)
        {
            return x >= 0 &&
                x < FaceWidth &&
                y >= 0 &&
                y < FaceHeight;
        }
    }
}
