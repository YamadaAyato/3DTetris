using System.Collections.Generic;
using UnityEngine;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ピース定義のカタログを表す ScriptableObject。
    /// </summary>
    [CreateAssetMenu(fileName = "PieceDefinitionCatalog", menuName = "ThreeDTetris/PieceDefinitionCatalog")]
    public class PieceDefinitionCatalogAsset : ScriptableObject
    {
        /// <summary>
        ///     ピース定義カタログを生成する。
        /// </summary>
        /// <returns> 生成されたピース定義カタログ </returns>
        public PieceDefinitionCatalog CreateCatalog()
        {
            if (_pieceDefinitions == null || _pieceDefinitions.Count == 0)
            {
                throw new System.Exception("PieceDefinitionCatalogAssetにPieceDefinitionAssetが設定されていません。");
            }

            // PieceDefinitionAssetからPieceDefinitionを生成する
            var definitions = new PieceDefinition[_pieceDefinitions.Count];

            for (int i = 0; i < definitions.Length; i++)
            {
                var definitionAsset = _pieceDefinitions[i];

                if (definitionAsset == null)
                {
                    throw new System.Exception("PieceDefinitionCatalogAssetにnullのPieceDefinitionAssetが含まれています。");
                }

                var definition = definitionAsset.CreateDefinition();
                definitions[i] = definition;
            }

            return new PieceDefinitionCatalog(definitions);
        }

        [SerializeField] private List<PieceDefinitionAsset> _pieceDefinitions = new();
    }
}
