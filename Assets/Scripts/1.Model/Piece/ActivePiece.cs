using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     現在アクティブなミノの情報を表すクラス。
    /// </summary>
    public class ActivePiece
    {
        public ActivePiece(
            PieceDefinition definition,
            BoardFaceId originFaceId,
            int originX,
            int originY,
            PieceRotation rotation)
        {
            Definition = definition ?? throw new ArgumentNullException(nameof(definition));
            OriginFaceId = originFaceId;
            OriginX = originX;
            OriginY = originY;
            Rotation = rotation;
        }

        /// <summary> ミノの形状定義。 </summary>
        public PieceDefinition Definition { get; }

        /// <summary> 基準位置が存在する面のID。 </summary>
        public BoardFaceId OriginFaceId { get; private set; }

        /// <summary> 基準位置のX座標。 </summary>
        public int OriginX { get; private set; }
        
        /// <summary> 基準位置のY座標。 </summary>
        public int OriginY { get; private set; }

        /// <summary> ミノの回転状態。 </summary>
        public PieceRotation Rotation { get; private set; }

        /// <summary>
        ///    基準位置を設定する。
        /// </summary>
        /// <param name="originFaceId"> 基準位置が存在する面のID </param>
        /// <param name="originX"> 基準位置のX座標 </param>
        /// <param name="originY"> 基準位置のY座標 </param>
        public void MoveTo(BoardFaceId originFaceId, int originX, int originY)
        {
            OriginFaceId = originFaceId;
            OriginX = originX;
            OriginY = originY;
        }

        /// <summary>
        ///     ミノの回転状態を設定する。
        /// </summary>
        /// <param name="rotation"> ミノの回転状態。 </param>
        public void RotationTo(PieceRotation rotation)
        {
            Rotation = rotation;
        }
    }
}
