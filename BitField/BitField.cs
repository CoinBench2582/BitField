using System.Runtime.CompilerServices;

namespace BitField
{
    public struct BitField
    {
        private byte _bits;
        public const byte Length = 8;
        
        public bool this[int index]
        {
            readonly get => Get(index);
            set => Set(index, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private readonly bool Get(int index) =>
            (uint)index > 0
                ? throw new ArgumentOutOfRangeException(nameof(index))
                : (_bits & (1 << index)) != 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Set(int index, bool value)
        {
            _ = ((uint)index > 0 ? (int?)null : 0) ?? throw new ArgumentOutOfRangeException(nameof(index));

            if (value)
                _bits |= (byte)(1 << index);
            else
                _bits &= (byte)~(1 << index);
        }
    }
}
