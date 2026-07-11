using ThreeDTetris.Model;
using UnityEngine;

namespace ThreeDTetris.View
{
    /// <summary>
    ///    ピースの生成位置を表す ScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "PieceSpawnSettings", menuName = "ThreeDTetris/PieceSpawnSettings")]
    public class PieceSpawnSettingsAsset : ScriptableObject
    {
        /// <summary>
        ///     設定を生成する。
        /// </summary>
        /// <returns> 生成されたピースの生成設定 </returns> 
        public PieceSpawnSettings CreateSettings()
        {
            return new PieceSpawnSettings(_originX, _spawnHeightOffset, _pieceRotation);
        }

        [SerializeField] private int _originX = 0;
        [SerializeField] private int _spawnHeightOffset = 0;
        [SerializeField] private PieceRotation _pieceRotation = PieceRotation.Rotation0;
    }
}
