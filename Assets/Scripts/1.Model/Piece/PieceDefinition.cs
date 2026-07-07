using System;
using System.Collections.Generic;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ミノの形状を定義するクラス。
    /// </summary>
    public class PieceDefinition
    {
        public PieceDefinition(PieceId id, IReadOnlyList<PieceCellOffset> cellOffsets)
        {
            if (cellOffsets == null)
            {
                throw new ArgumentNullException(nameof(cellOffsets));
            }

            if (cellOffsets.Count == 0)
            {
                throw new ArgumentException("ミノを構成するセルが存在しません。", nameof(cellOffsets));
            }

            Id = id;
            _cellOffsets = new PieceCellOffset[cellOffsets.Count];

            // 外側の List 変更で中身が変わらないようにコピー。
            for (int i = 0; i < cellOffsets.Count; i++)
            {
                _cellOffsets[i] = cellOffsets[i];
            }
        }

        private readonly PieceCellOffset[] _cellOffsets;

        /// <summary> ミノの識別ID。 </summary>
        public PieceId Id { get; }

        /// <summary> ミノを構成するセルの相対位置一覧。 </summary>
        public IReadOnlyList<PieceCellOffset> Cells => _cellOffsets;
    }
}
