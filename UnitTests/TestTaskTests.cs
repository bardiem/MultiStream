
using LyftInterview;
using System.Diagnostics;
using Stream = LyftInterview.Stream;

namespace UnitTests
{
    public class TestTaskTests
    {
        [Fact]
        public void TestTask_Tests()
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

            Stream s4 = new Stream(new List<int> { 1, 2, 3, 4, 5 });
            Stream s5 = new Stream(new List<int> { 6, 7 });
            Stream s6 = new Stream(new List<int> { 8, 9, 10 });
            Stream s7 = new Stream(new List<int> { 11, 12, 13 });
            multistream.Add(s4);
            multistream.Add(s5);
            multistream.Add(s6);
            multistream.Add(s7);

            Test(4, multistream.Read(6), new List<int> { 1, 2, 3, 4, 5, 6 }); // Read first 6
            multistream.Remove(s6); // Remove third stream
            Test(5, multistream.Read(6), new List<int> { 7, 11, 12, 13 });
            Test(6, multistream.Read(2), new List<int> { });
            Test(7, multistream.Read(5), new List<int> { });


            Stream s8 = new Stream(new List<int> { 1, 2, 3, 4, 5 });
            Stream s9 = new Stream(new List<int> { 6, 7 });
            Stream s10 = new Stream(new List<int> { 8, 9, 10 });
            Stream s11 = new Stream(new List<int> { 11, 12, 13 });
            Stream s12 = new Stream(new List<int> { 14, 15, 16 });
            multistream.Add(s8);
            multistream.Add(s9);
            multistream.Add(s10);
            multistream.Add(s11); 
            multistream.Add(s12);

            Test(4, multistream.Read(6), new List<int> { 1, 2, 3, 4, 5, 6 }); // Read first 6
            multistream.Remove(s10); // Remove third stream
            multistream.Remove(s11);
            Test(5, multistream.Read(6), new List<int> { 7, 14, 15, 16 });
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


        [Fact]
        public void Delete_ShouldBeO1WithManyStreams()
        {
            // Arrange
            var rand = new Random();

            var multistream = new MultiStream();
            

            for(int i = 0; i < 4_000_000; i++)
            {
                var newStream = new Stream(new List<int> { rand.Next(), rand.Next() });
                multistream.Add(newStream);
            }

            var testStream = new Stream(new List<int> { 1, 5, 7 });
            multistream.Add(testStream);

            for (int i = 0; i < 4_000_000; i++)
            {
                var newStream = new Stream(new List<int> { rand.Next(), rand.Next() });
                multistream.Add(newStream);
            }

            // Act
            var watch = Stopwatch.StartNew();
            multistream.Remove(testStream);
            watch.Stop();

            // Assert
            var elapsedMs = watch.ElapsedMilliseconds;
            Assert.True(elapsedMs < 5, "The actualCount was not greater than five");


        }

        [Fact]
        public void Delete_ShouldBeO1With1Stream()
        {
            // Arrange
            var multistream = new MultiStream();


            var testStream = new Stream(new List<int> { 1, 5, 7 });
            multistream.Add(testStream);

            // Act
            var watch = Stopwatch.StartNew();
            multistream.Remove(testStream);
            watch.Stop();


            // Assert
            var elapsedMs = watch.ElapsedMilliseconds;
            Assert.True(elapsedMs < 5, "The actualCount was not greater than five");

        }
    }
}