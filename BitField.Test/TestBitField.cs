using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BitField.Tests
{
    [TestClass]
    public sealed class TestBitField
    {
        private static readonly byte testByte = 0b101010;
        private static readonly bool[] testBitArray = { false, true, false, true, false, true };

        internal static IEqualityComparer<T[]> GetArrayEqualityComparer<T>() where T : IEquatable<T>
            => EqualityComparer<T[]>.Create(
                static (first, second) =>
                {
                    if (first is null)
                        return second is null;
                    if (second is null)
                        return false;
                    if (first.Length != second.Length)
                        return false;

                    int length = first.Length;
                    Span<T> firstSpan = first;
                    Span<T> secondSpan = second;
                    for (int i = 0; i < length; i++)
                        if (!first[i].Equals(secondSpan[i]))
                            return false;
                    return true;
                });

        /// <summary>
        /// This method is called once for the test class, before any tests of the class are run.
        /// </summary>
        /// <param name="context">Context of the test</param>
        [ClassInitialize]
        public static void ClassInit(TestContext context)
        {
            // context.WriteLine($"BitField struct tests");
        }

        [TestMethod]
        public void TestIndexThrows()
        {
            BitField bits = new();

            try
            {
                _ = bits[0];
                _ = bits[7];
                bits[0] = false;
                bits[7] = false;
            }
            catch { throw new AssertFailedException(); }
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[-1]);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[8]);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[-1] = true);
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(() => bits[8] = true);
        }

        [TestMethod]
        public void TestConstructingThrows()
        {
            _ = Assert.ThrowsException<ArgumentOutOfRangeException>(static () => _ = new BitField(new bool[16]));
#pragma warning disable CS8625 // Literál null nejde převést na odkazový typ, který nemůže mít hodnotu null.
            _ = Assert.ThrowsException<ArgumentNullException>(static () => _ = new BitField(null));
#pragma warning restore CS8625 // Literál null nejde převést na odkazový typ, který nemůže mít hodnotu null.
        }

        [TestMethod]
        public void TestInitialisedValue()
        {
            BitField fromByte = new(testByte);
            BitField fromArray = new(testBitArray);

            IEnumerator<bool> first = fromByte.GetEnumerator();
            IEnumerator<bool> second = fromArray.GetEnumerator();

            while (first.MoveNext() && second.MoveNext())
                Assert.AreEqual(first.Current, second.Current);
        }

        [TestMethod]
        public void TestConvert()
        {
            BitField fromByte = (BitField)testByte;
            BitField fromArray = (BitField)testBitArray;

            byte fromArrayField = (byte)fromArray;
            bool[] fromByteField = (bool[])fromByte;

            Assert.AreEqual(testByte, fromArrayField);
            Assert.AreEqual([..testBitArray, false, false], fromByteField, comparer: GetArrayEqualityComparer<bool>());
        }

        [TestMethod]
        public void TestBitOperations()
        {
            #region Consts
            const byte low = 0b00001111;
            const byte high = 0b11110000;
            const byte zigLow = 0b01010101;
            const byte zigHigh = 0b10101010;
            const byte all = 0b11111111;
            const byte none = 0b00000000;
            #endregion
            #region Fields
            BitField Low = new(low);
            BitField High = new(high);
            BitField ZigLow = new(zigLow);
            BitField ZigHigh = new(zigHigh);
            #endregion

            #region Operations
            BitField LowAHigh = Low & High;
            BitField LowOHigh = Low | High;
            BitField LowXHigh = Low ^ High;
            BitField LowN = ~Low;
            BitField HighN = ~High;

            BitField ZigA = ZigLow & ZigHigh;
            BitField ZigO = ZigLow | ZigHigh;
            BitField ZigX = ZigLow ^ ZigHigh;
            BitField ZigLN = ~ZigLow;
            BitField ZigHN = ~ZigHigh;
            #endregion

            #region Asserts
            Assert.AreEqual(none, (byte)LowAHigh);
            Assert.AreEqual(all, (byte)LowOHigh);
            Assert.AreEqual(all, (byte)LowXHigh);
            Assert.AreEqual(high, (byte)LowN);
            Assert.AreEqual(low, (byte)HighN);

            Assert.AreEqual(none, (byte)ZigA);
            Assert.AreEqual(all, (byte)ZigO);
            Assert.AreEqual(all, (byte)ZigX);
            Assert.AreEqual(zigHigh, (byte)ZigLN);
            Assert.AreEqual(zigLow, (byte)ZigHN);
            #endregion
        }
    }
}
