using System;
using System.Collections;

namespace BitField
{
    /// <summary>
    /// Enumerates upon the underlying bits of a <see cref="BitField"/>
    /// </summary>
    internal struct BitEnumerator : IEnumerator<bool>
    {
        private byte _bits;
        private int _index = -1;

        /// <summary>
        /// Returns the current bit as a <see langword="bool"/>
        /// </summary>
        public readonly bool Current => (_bits & (1 << _index)) != 0;

        readonly object IEnumerator.Current => Current;

        internal BitEnumerator(BitField bitField) => this._bits = (byte)bitField;

        /// <summary>
        /// Moves to the first bit
        /// </summary>
        /// <returns>If we are within bounds</returns>
        public bool MoveNext()
        {
            _index++;
            return _index < BitField.Length;
        }

        /// <summary>
        /// Prepares for new enumeration.
        /// </summary>
        /// <remarks>First bit cannot be yielded before the first call of <see cref="MoveNext"/></remarks>
        public void Reset() => _index = -1;

        /// <summary>
        /// Invalidates the enumerator (will yield only <see langword="false"/> now)
        /// </summary>
        public void Dispose() => _bits = 0;
    }
}
