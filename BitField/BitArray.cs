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
        /// <summary>
        /// Length in bits
        /// </summary>
        public int Length => _bitFields.Length * 8;
        int ICollection.Count => Length;
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
            get => ((uint)index >= (uint)Length)
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : _bitFields[index / 8][index % 8];
            set
            {
                if ((uint)index >= (uint)Length)
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
        /// Must not be negative.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">the <paramref name="length"/> was negative</exception>
        public BitArray(int length)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(length, nameof(length));

            _bitFields = new BitField[(length + 7) / 8];
        }
        #endregion

        #region Interfacy a přepsání
        public IEnumerator<bool> GetEnumerator() => new BitArrayEnumerator(_bitFields);
        IEnumerator<BitField> IEnumerable<BitField>.GetEnumerator() => ((IEnumerable<BitField>)_bitFields).GetEnumerator();
        void ICollection.CopyTo(Array array, int index) => _bitFields.CopyTo(array, index);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        #endregion

        private struct BitArrayEnumerator : IEnumerator<bool>
        {
            private BitField[] _bits;
            private int position = -1;

            private readonly int Len => _bits.Length;
            public readonly bool Current => _bits[position / 8][position % 8];
            readonly object IEnumerator.Current => Current;

            internal BitArrayEnumerator(BitField[] target) => _bits = target;

            public void Dispose() => _bits = null!;
            public bool MoveNext() => ++position < Len;
            public void Reset() => position = -1;
        }
    }
}
