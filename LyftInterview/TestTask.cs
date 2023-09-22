using System.Collections.Generic;
using System.Collections.Specialized;
using System.Xml.Linq;

namespace LyftInterview;

public interface IStream
{
    List<int> Read(int n);
}

public interface IMultiStream
{
    List<int> Read(int n);

    void Add(IStream stream);

    void Remove(IStream stream);
}

public class Stream : IStream
{
    // Do not modify this class
    private List<int> elements;
    private int currIndex;

    public Stream(List<int> input)
    {
        elements = input;
        currIndex = 0;
    }

    public List<int> Read(int n)
    {
        List<int> output = new List<int>();
        while (currIndex < elements.Count && output.Count < n)
        {
            output.Add(elements[currIndex]);
            currIndex++;
        }

        return output;
    }
}

public class MultiStream : IMultiStream
{
    OrderedDictionary _lookup = new OrderedDictionary();

    public void Add(IStream stream)
    {
        _lookup.Add(stream, stream);
    }

    public List<int> Read(int n)
    {
        var result = new List<int>();
        int remaindingCount = n;

        while (_lookup.Count > 0)
        {
            var first = (IStream)_lookup[0];
            var itemsRead = first.Read(remaindingCount);
            result.AddRange(itemsRead);

            if (itemsRead.Count == remaindingCount)
            {
                break;
            }

            Remove(first);

            remaindingCount -= itemsRead.Count;
        }

        return result;

    }

    public void Remove(IStream stream)
    {
        _lookup.Remove(stream);
    }
}