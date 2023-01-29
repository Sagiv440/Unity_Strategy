using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResorceManeger : MonoBehaviour {


	public Resorce Credits;
	public Resorce Power;
	public Manager manager;

	// Use this for initialization
	void Start () {
		manager = GetComponentInParent<Manager> ();
	}
	
	// Update is called once per frame
	void Update () {
		Credits.Value = manager.Credit;
		Power.Value = manager.Energy;
	}
}
