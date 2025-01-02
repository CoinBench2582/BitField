using System.Runtime.CompilerServices;

namespace BitField
{
    public struct BitField
    {
        #region Pole
        private byte _bits;
        public const byte Length = 8;
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
                mask |= (byte)((bitsSpan[i] ? 1 : 0) << i);
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

        #region Operátory

        #endregion

        #region Interfacy a přepsání

        #endregion
    }
}
