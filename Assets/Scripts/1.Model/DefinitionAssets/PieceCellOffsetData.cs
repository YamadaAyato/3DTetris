using System;
using UnityEngine;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ピースセルの相対位置データを表すクラス。
    ///     Editor上でのデータ定義用に使用される。
    /// </summary>
    [Serializable]
    public class PieceCellOffsetData
    {
        /// <summary> X方向のオフセット値 </summary>
        public int X => _xOffset;

        /// <summary> Y方向のオフセット値 </summary>
        public int Y => _yOffset;

        [SerializeField] private int _xOffset;
        [SerializeField] private int _yOffset;
    }
}
