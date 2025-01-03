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
    }
}
