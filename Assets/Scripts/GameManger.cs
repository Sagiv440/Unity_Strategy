using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {

    public int max_Army_Power = 1000; 
    public List<GameObject> points;

    public List<GameObject> Points
    {
        get
        {
            return points;
        }
    }

    public int Max_Army_Power
    {
        get
        {
            return max_Army_Power;
        }
    }
}
