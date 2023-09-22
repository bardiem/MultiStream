﻿using System.Collections.Generic;
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
    private long _firstIndex = 0;
    private long _lastIndex = -1;
    private long _length = 0;

    private Dictionary<long, IStream> _lookup = new Dictionary<long, IStream>();
    private Dictionary<IStream, long> _reverseLookup = new Dictionary<IStream, long>();

    public void Add(IStream stream)
    {
        if(_length == long.MaxValue - 1)
        {
            throw new OverflowException("Multistream is full, you can't add a new stream");
        }

        _lastIndex += 1;
        _length++;

        _lookup.Add(_lastIndex, stream);
        _reverseLookup.Add(stream, _lastIndex);

        RewriteIfNeeded();
    }

    public List<int> Read(int n)
    {
        var result = new List<int>();
        int remaindingCount = n;

        while (_lookup.Count > 0)
        {
            var first = _lookup[_firstIndex];
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
        var id = _reverseLookup[stream];

        if(_length == 1)
        {
            _firstIndex = 0;
            _lastIndex = -1;
            _length = 0;
        } else if(id == _firstIndex)
        {
            _firstIndex++;
            _length--;
        } else if(id == _lastIndex)
        {
            _lastIndex--;
            _length--;
        }

        _lookup.Remove(id);
        _reverseLookup.Remove(stream);
    }

    private void RewriteIfNeeded()
    {
        if (_lastIndex < long.MaxValue)
        {
            return;
        }

        var newLookup = new Dictionary<long, IStream>();
        var newReverse = new Dictionary<IStream, long>();

        long i = 0;
        foreach (var pair in _lookup)
        {
            newLookup.Add(i, pair.Value);
            newReverse.Add(pair.Value, i);
            i++;
        }

        _lookup = newLookup;
        _reverseLookup = newReverse;

        _firstIndex = 0;
        _lastIndex = _lookup.Count - 1;
        _length = _lookup.Count;
    }
}