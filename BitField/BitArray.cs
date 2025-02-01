using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BitField
{
    public class BitArray : IEnumerable<bool>, IEnumerable<BitField>, ICollection
    {
        private readonly BitField[] _bitFields;

        #region Vlastnosti
        private int countOfBits;
        /// <summary>
        /// Length in bits
        /// </summary>
        public int Length => countOfBits;
        int ICollection.Count => countOfBits;
        bool ICollection.IsSynchronized => false;
        object ICollection.SyncRoot => this;
        #endregion

        /// <summary>
        /// Accesses a bit
        /// </summary>
        /// <param name="index">the index of the bit to access</param>
        /// <returns>the bit as <see langword="bool"/></returns>
        /// <exception cref="ArgumentOutOfRangeException">the <paramref name="index"/> is out of range</exception>
        public bool this[int index]
        {
            get => ((uint)index >= (uint)countOfBits)
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : _bitFields[index / 8][index % 8];
            set
            {
                if ((uint)index >= (uint)countOfBits)
                    throw new ArgumentOutOfRangeException(nameof(index));
                _bitFields[index / 8][index % 8] = value;
            }
        }

        #region Konstruktory
        /// <summary>
        /// Creates an array of bits with the specified length
        /// </summary>
        /// <param name="length">
        /// Minimal amount of bits to store.
        /// Rounded up for divisibility by 8.
        /// Must be positive.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">the <paramref name="length"/> was not positive</exception>
        public BitArray(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegativeOrZero(length, nameof(length));

            _bitFields = new BitField[(length + 7) / 8];
            countOfBits = length;
        }
        #endregion

        #region Interfacy a přepsání
        public IEnumerator<bool> GetEnumerator() => new BitArrayEnumerator(_bitFields, countOfBits);
        IEnumerator<BitField> IEnumerable<BitField>.GetEnumerator() => ((IEnumerable<BitField>)_bitFields).GetEnumerator();
        void ICollection.CopyTo(Array array, int index) => _bitFields.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion
    }

    internal struct BitArrayEnumerator : IEnumerator<bool>
    {
        private BitField[] _bits;
        private int position = -1;
        private readonly int len;

        public readonly bool Current => _bits[position / 8][position % 8];
        readonly object IEnumerator.Current => Current;

        internal BitArrayEnumerator(BitField[] target, int length)
        {
            _bits = target;
            len = length;
        }

        public void Dispose() => _bits = null!;
        public bool MoveNext() => ++position < len;
        public void Reset() => position = -1;
    }
}
