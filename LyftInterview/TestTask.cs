using System.Collections.Generic;
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
    LinkedList<IStream> _lookup = new LinkedList<IStream>();
    Dictionary<IStream, LinkedListNode<IStream>> _reverseLookup = new Dictionary<IStream, LinkedListNode<IStream>>();

    public void Add(IStream stream)
    {
        var node = new LinkedListNode<IStream>(stream);
        _lookup.AddLast(node);
        _reverseLookup.Add(stream, node);
    }

    public List<int> Read(int n)
    {
        var result = new List<int>();
        int remaindingCount = n;

        while (_lookup.Count > 0)
        {
            var itemsRead = _lookup.First.Value.Read(remaindingCount);
            result.AddRange(itemsRead);

            if (itemsRead.Count == remaindingCount)
            {
                break;
            }

            Remove(_lookup.First.Value);

            remaindingCount -= itemsRead.Count;
        }

        return result;

    }

    public void Remove(IStream stream)
    {
        var node = _reverseLookup[stream];
        _lookup.Remove(node);
        _reverseLookup.Remove(stream);
    }
}