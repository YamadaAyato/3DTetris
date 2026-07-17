using System;

namespace ThreeDTetris.Model
{
    /// <summary>
    ///     ミノの種類を識別するID。
    /// </summary>
    public readonly struct PieceId : IEquatable<PieceId>
    {
        public PieceId(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("ミノIDが空です。", nameof(value));
            }

            Value = value;
        }

        /// <summary> ミノを識別する値。 </summary>
        public string Value { get; }

        public bool Equals(PieceId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            return obj is PieceId other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}
