using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     盤面の面を識別するための値オブジェクト。
    ///     立体のどの形状であっても対応できるように、整数値で識別する。
    /// </summary>
    public readonly struct BoardFaceId : IEquatable<BoardFaceId>
    {
        public BoardFaceId(int value)
        {
            Value = value;
        }

        /// <summary> 盤面の面を識別する値。 </summary>
        public int Value { get; }

        public bool Equals(BoardFaceId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is BoardFaceId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public override string ToString()
        {
            return $"BoardFaceId: {Value}";
        }
    }
}
