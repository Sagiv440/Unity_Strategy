using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Min_Max : MonoBehaviour {
    private PathTree path;

    public paxe findPath(PathNode pointer)
    {
        if(pointer.pCount == 0)
        {
            return new paxe(pointer.value, 0,pointer.IncBlock);
        }
        int max = findPath(pointer.pList[0]).value; ;
        byte indax = 0;
        for(byte i = 1; i < pointer.pCount; i++)
        {
            int a = findPath(pointer.pList[i]).value;
            if(max < a){
                max = a;
                indax = i;
            }
        }
        return new paxe(max + pointer.value, indax, pointer.pList[indax].IncBlock);
    }

    public PathTree Path
    {
        set
        {
            path = value;
        }
    }
}

public class PathTree
{
    private PathNode head;
    public byte layer;

    public PathTree()
    {
        head = new PathNode();
        layer = 0;
    }
    public PathTree(byte _layer)
    {
        head = new PathNode();
        layer = _layer;
    }
    public PathNode Head
    {
        get
        {
            return head;
        }
    }
}

public class PathNode
{
    public int value = 0;
    public List<PathNode> pList;
    public byte pCount = 0;
    Inc_Block com;

    public PathNode()
    {
        pList = new List<PathNode>();
        pCount = 0;
    }
    public PathNode(int _value, Inc_Block _com)
    {
        value = _value;
        com = _com;
        pList = new List<PathNode>();
        pCount = 0;
    }

    public Inc_Block IncBlock
    {
        set
        {
            com = value;
        }
        get
        {
            return com;
        }
    }

    public void addPath(int _value, Inc_Block _com)
    {
        pList.Add(new PathNode(_value, _com));
        pCount++;
    }

    public PathNode getNode(byte _num)
    {
        return pList[_num];
    }
}

