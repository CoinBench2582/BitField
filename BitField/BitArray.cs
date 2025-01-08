using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BitField
{
    public class BitArray : /*IEnumerable<bool>,*/ IEnumerable<BitField>, ICollection
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
        /// Is rounded up for divisibility by 8.
        /// Must not be negative.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">the <paramref name="length"/> was negative</exception>
        public BitArray(int length)
        {
            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be non-negative.");

            _bitFields = new BitField[(length + 7) / 8];
        }
        #endregion

        #region Interfacy a přepsání
        IEnumerator<BitField> IEnumerable<BitField>.GetEnumerator() => ((IEnumerable<BitField>)_bitFields).GetEnumerator();
        void ICollection.CopyTo(Array array, int index) => _bitFields.CopyTo(array, index);
        public IEnumerator GetEnumerator() => _bitFields.GetEnumerator();
        #endregion
    }
}
