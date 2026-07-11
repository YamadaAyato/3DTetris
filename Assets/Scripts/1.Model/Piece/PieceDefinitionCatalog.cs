using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     使用可能なピース定義のカタログを管理するクラス。
    /// </summary>
    public class PieceDefinitionCatalog
    {
        public PieceDefinitionCatalog(IReadOnlyList<PieceDefinition> pieceDefinitions)
        {
            if (pieceDefinitions == null)
            {
                throw new ArgumentNullException(nameof(pieceDefinitions));
            }

            if (pieceDefinitions.Count == 0)
            {
                throw new ArgumentException("ピース定義が空です。", nameof(pieceDefinitions));
            }

            // 外側の List 変更で中身が変わらないようにコピー。
            PieceDefinition[] copiedDefinitions = new PieceDefinition[pieceDefinitions.Count];
            _pieceDefinitionMap = new Dictionary<PieceId, PieceDefinition>();

            for (int i = 0; i < pieceDefinitions.Count; i++)
            {
                var definition = pieceDefinitions[i];

                if (definition == null)
                {
                    throw new ArgumentException("ピース定義にnullが含まれています。", nameof(pieceDefinitions));
                }

                if (_pieceDefinitionMap.ContainsKey(definition.Id))
                {
                    throw new ArgumentException($"ピースID {definition.Id} が重複しています。", nameof(pieceDefinitions));
                }

                copiedDefinitions[i] = definition;
                _pieceDefinitionMap.Add(definition.Id, definition);
            }

            _pieceDefinitions = Array.AsReadOnly(copiedDefinitions);
        }

        /// <summary> 使用可能なピース定義の一覧。 </summary>
        public IReadOnlyList<PieceDefinition> PieceDefinitions => _pieceDefinitions;

        /// <summary> 使用可能なピース定義の数。 </summary>
        public int Count => _pieceDefinitions.Count;

        /// <summary>
        ///     指定された PieceId に対応する PieceDefinition を取得します。
        /// </summary>
        /// <param name="id"> 取得するピースのID </param>
        /// <returns> 指定されたIDに対応するPieceDefinition </returns>
        public PieceDefinition GetById(PieceId id)
        {
            if (_pieceDefinitionMap.TryGetValue(id, out var definition))
            {
                return definition;
            }
            else
            {
                throw new KeyNotFoundException($"PieceDefinition with id {id} not found.");
            }
        }

        /// <summary>
        ///     指定された PieceId に対応する PieceDefinition を取得できるかどうかを試みます。
        /// </summary>
        /// <param name="id"> 取得するピースのID </param>
        /// <param name="definition"> 取得できた場合に設定されるPieceDefinition </param>
        /// <returns> 指定されたIDに対応するPieceDefinitionを取得できた場合はtrue、それ以外の場合はfalse </returns>
        public bool TryGetById(PieceId id, out PieceDefinition definition)
        {
            return _pieceDefinitionMap.TryGetValue(id, out definition);
        }

        private readonly ReadOnlyCollection<PieceDefinition> _pieceDefinitions;
        private readonly Dictionary<PieceId, PieceDefinition> _pieceDefinitionMap;
    }
}
