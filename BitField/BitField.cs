using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace BitField
{
    public struct BitField : IEnumerable<bool>, IEquatable<BitField>, IBitwiseOperators<BitField, BitField, BitField>, IMinMaxValue<BitField>
    {
        #region Pole
        private byte _bits;
        public const byte Length = 8;
        internal static readonly BitField Empty = new();

        public static BitField MaxValue { get; } = new(byte.MaxValue);
        public static BitField MinValue => Empty;
        #endregion

        #region Vlastnosti
        /// <summary>
        /// Accesses bits stored in the field
        /// </summary>
        /// <param name="index">
        /// Index of the bit to access.
        /// Mustn't be greater than or equal to <see cref="Length"/>
        /// </param>
        /// <returns>Returns the stored bit as a <see langword="bool"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if the index is negative or more than <c>8</c></exception>
        public bool this[int index]
        {
            readonly get => Get(index);
            set => Set(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly bool Get(int index) =>
            (uint)index >= Length
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : (_bits & (1 << index)) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(int index, bool value)
        {
            _ = ((uint)index >= Length ? (int?)null : 0) ?? throw new ArgumentOutOfRangeException(nameof(index));

            if (value)
                _bits |= (byte)(1 << index);
            else
                _bits &= (byte)~(1 << index);
        }
        #endregion

        #region Konstruktory
        /// <summary>
        /// Initialises an empty field of 8 bits
        /// </summary>
        public BitField() => _bits = 0;

        /// <summary>
        /// Initialises a field of bits from the provided <paramref name="bits"/>
        /// </summary>
        /// <param name="bits"></param>
        public BitField(byte bits) => _bits = bits;

        /// <summary>
        /// Initialises a field of bits from the provided <paramref name="bits"/>
        /// </summary>
        /// <param name="bits">Array of bits to store</param>
        /// <exception cref="ArgumentNullException">The provided <paramref name="bits"/> array is <see langword="null"/></exception>
        /// <exception cref="ArgumentOutOfRangeException">The provided <paramref name="bits"/> array is longer than <see cref="Length"/></exception>
        public BitField(bool[] bits)
        {
            int len = (bits ?? throw new ArgumentNullException(nameof(bits))).Length;
            _ = (len > Length ? (int?)null : 0)
                    ?? throw new ArgumentOutOfRangeException(nameof(bits), $"The provided array is too long!");

            Span<bool> bitsSpan = bits;
            byte mask = 0;
            for (int i = 0; i < len; i++)
                if (bitsSpan[i])
                    mask |= (byte)(1 << i);
            _bits = mask;
        }

        /// <summary>
        /// Initialises a field of bits with values copied from the provided <paramref name="bitField"/>
        /// </summary>
        /// <param name="bitField">field of bits to copy</param>
        public BitField(BitField bitField) => _bits = bitField._bits;
        #endregion

        #region Metody

        #endregion

        #region Přetypování
        public static explicit operator byte(BitField bitField) => bitField._bits;
        public static explicit operator BitField(byte bits) => new(bits);

        public static explicit operator BitField(bool[] bits) => new(bits);
#pragma warning disable IDE0305 // Zjednodušit inicializaci kolekce
        public static explicit operator bool[](BitField bitField) => bitField.ToArray();
#pragma warning restore IDE0305 // Zjednodušit inicializaci kolekce

        /// <summary>
        /// Collects the stored bits as <see langword="bool"/>s into an array
        /// </summary>
        /// <returns><see langword="bool"/>[] containing 8 bits</returns>
        public readonly bool[] ToArray()
        {
            bool[] bits = new bool[Length];
            Span<bool> bitsSpan = bits;
            for (int i = 0; i < Length; i++)
                bitsSpan[i] = (this._bits & (1 << i)) != 0;
            return bits;
        }
        #endregion

        #region Operátory
        /// <summary>
        /// Compares if the underlying bit are the same
        /// </summary>
        public static bool operator ==(BitField left, BitField right) => left._bits == right._bits;
        /// <summary>
        /// Compares if the underlying bit are different
        /// </summary>
        public static bool operator !=(BitField left, BitField right) => left._bits != right._bits;

        /// <summary>
        /// Computes the bit AND between the underlying bits
        /// </summary>
        /// <returns>New field containing the resulting bits in order</returns>
        public static BitField operator &(BitField left, BitField right) => new((byte)(left._bits & right._bits));
        /// <summary>
        /// Computes the bit OR between the underlying bits
        /// </summary>
        /// <returns>New field containing the resulting bits in order</returns>
        public static BitField operator |(BitField left, BitField right) => new((byte)(left._bits | right._bits));
        /// <summary>
        /// Computes the bit XOR between the underlying bits
        /// </summary>
        /// <returns>New field containing the resulting bits in order</returns>
        public static BitField operator ^(BitField left, BitField right) => new((byte)(left._bits ^ right._bits));
        /// <summary>
        /// Computes the complementary bits for the underlying bits
        /// </summary>
        /// <returns>New field containing the complementary bits in order</returns>
        public static BitField operator ~(BitField value) => new((byte)~value._bits);
        #endregion

        #region Interfacy a přepsání
        public readonly IEnumerator<bool> GetEnumerator() => new Enumerator(this);
        readonly IEnumerator IEnumerable.GetEnumerator() => new Enumerator(this);

        /// <summary>
        /// Returns a value indicating if the underlying bit fields are the same.
        /// </summary>
        /// <returns>if the underlying bits are the same</returns>
        public static bool Equals(BitField left, BitField right) => left._bits == right._bits;
        /// <summary>
        /// Returns if the underlying bit fields are the same.
        /// </summary>
        /// <param name="other">field of bits to compare to</param>
        /// <returns>the underlying bits are the same</returns>
        public readonly bool Equals(BitField other) => this._bits == other._bits;

        public override readonly string ToString() => _bits.ToString();
        public override readonly bool Equals(object? obj) => obj is BitField bitField && this._bits == bitField._bits;
        public override readonly int GetHashCode() => _bits.GetHashCode();
        #endregion

        /// <summary>
        /// Enumerates upon the underlying bits of a <see cref="BitField"/>
        /// </summary>
        private struct Enumerator : IEnumerator<bool>
        {
            private byte _bits;
            private int _index = -1;

            public readonly bool Current => (_bits & (1 << _index)) != 0;

            readonly object IEnumerator.Current => Current;

            internal Enumerator(BitField bitField) => this._bits = bitField._bits;

            public void Dispose() => _bits = 0;

            public bool MoveNext()
            {
                _index++;
                return _index < Length;
            }

            public void Reset() => _index = -1;
        }
    }
}
