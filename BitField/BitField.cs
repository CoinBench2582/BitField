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
        /// <returns>Returns the stored bit as a <see cref="bool"/></returns>
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
        
        #endregion

        #region Metody

        #endregion

        #region Operátory

        #endregion

        #region Interfacy a přepsání

        #endregion
    }
}
