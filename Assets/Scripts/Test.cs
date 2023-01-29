using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Test : MonoBehaviour {

	public AI_Army_Manager test;
    public GameObject Unit, point,target;
    public Vector3 Point;

    public void Captuer()
    {
        test.CaptureCommend(Unit, point);
    }
    public void Attack()
    {
        test.AttackCommend(Unit, target);
    }
    public void move()
    {
        test.MoveCommand(Unit, Point);
    }
}