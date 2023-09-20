
using LyftInterview;
using Stream = LyftInterview.Stream;

namespace UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void TestTaskTests()
        {
            // Test that Stream is working as expected
            Stream stream = new Stream(new List<int> { 1, 2, 3, 4, 5, 6 });

            Test(0, stream.Read(1), new List<int> { 1 });
            Test(1, stream.Read(2), new List<int> { 2, 3 });
            Test(2, stream.Read(6), new List<int> { 4, 5, 6 });
            Test(3, stream.Read(6), new List<int> { });

            // Test that MultiStream is working as expected
            MultiStream multistream = new MultiStream();
            Stream s1 = new Stream(new List<int> { 1, 2, 3, 4, 5 });
            Stream s2 = new Stream(new List<int> { 6, 7 });
            Stream s3 = new Stream(new List<int> { 8, 9, 10 });
            multistream.Add(s1);
            multistream.Add(s2);
            multistream.Add(s3);

            Test(4, multistream.Read(6), new List<int> { 1, 2, 3, 4, 5, 6 }); // Read first 6
            multistream.Remove(s3); // Remove third stream
            Test(5, multistream.Read(6), new List<int> { 7 });
            Test(6, multistream.Read(2), new List<int> { });
            Test(7, multistream.Read(5), new List<int> { });
        }

        static void Test(int test_case, List<int> actual, List<int> expected)
        {
            Assert.Equivalent(actual, expected);
            //if (!actual.SequenceEqual(expected))
            //{
            //    Console.WriteLine("Test Case " + test_case + ": FAILED");
            //}
            //else
            //{
            //    Console.WriteLine("Test Case " + test_case + ": SUCCESS");
            //}
        }
    }
}