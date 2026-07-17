using System.Collections.Generic;
using UnityEngine;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ピースの定義を表す ScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "PieceDefinition", menuName = "ThreeDTetris/PieceDefinition")]
    public class PieceDefinitionAsset : ScriptableObject
    {
        /// <summary>
        ///     ピース定義を生成する。
        /// </summary>
        /// <returns> 生成されたピース定義 </returns>
        public PieceDefinition CreateDefinition()
        {
            if (string.IsNullOrEmpty(_id))
            {
                throw new System.Exception("PieceDefinitionAssetのIDが設定されていません。");
            }

            if (_cellOffsets == null || _cellOffsets.Count == 0)
            {
                throw new System.Exception($"PieceDefinitionAsset({_id})のセルオフセットが設定されていません。");
            }

            PieceCellOffset[] cellOffsets = new PieceCellOffset[_cellOffsets.Count];
            HashSet<PieceCellOffset> offsetSet = new();

            for (int i = 0; i < _cellOffsets.Count; i++)
            {
                var offsetData = _cellOffsets[i];

                if (offsetData == null)
                {
                    throw new System.Exception($"PieceDefinitionAsset({_id})のセルオフセットにnullが含まれています。インデックス: {i}");
                }

                var offset = new PieceCellOffset(offsetData.X, offsetData.Y);
                if (!offsetSet.Add(offset))
                {
                    throw new System.Exception($"PieceDefinitionAsset({_id})のセルオフセットに重複があります。オフセット: ({offset.X}, {offset.Y})");
                }
                cellOffsets[i] = offset;
            }

            return new PieceDefinition(new PieceId(_id.Trim()), cellOffsets);
        }

        [SerializeField] private string _id;
        [SerializeField] private List<PieceCellOffsetData> _cellOffsets = new();
    }
}
