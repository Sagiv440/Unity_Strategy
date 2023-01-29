using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour {

    private GameManger game_Manger;
    public GameObject PlayerCamera;
	public List<GameObject> Army;
    public List<GameObject> Enemys;
    

    public List<UnitList> Squads;
    public List<GameObject> Coustructon_biulding;

    public int army_Power = 0;

	public int credit = 100;
	public int energy = 100;

	public string PlayGround = "PlayGround"; //hold the tag of the PlayGround
	public string Unit; // hold the tag of the Units that bilong to the player
	public string Enemy; // hold the tag of the Units that bilong to the Enemy.
	public string Neutralbuilding ="N_building"; // hold the tag of the Neutral Buildings.


	// Use this for initialization
	void Start () {

        game_Manger = GetComponentInParent<GameManger>();
        

        //Gmanger = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManger>();
      //  Points = Gmanger.Points;
    }

	// Update is called once per frame
	void Update () {

	}

    public GameManger Game_Manger
    {
        get
        {
            return game_Manger;
        }
    } 

    public int Army_Power
    {
        get
        {
            return army_Power;
        }
        set
        {
            army_Power = value;
        }
    }

    public void RiprtingIn(GameObject Unit)
    {
        if(Army == null)
        {
            Army = new List<GameObject>();
        }
        Army.Add(Unit);
    }

    public void UnitDid(GameObject Unit)
    {
        if(Army.Contains(Unit) == true) Army.Remove(Unit);
    }

    public int Credit
    {
        get
        {
            return credit;
        }
        set
        {
            credit = value;
        }
    }

    public int Energy
    {
        get
        {
            return energy;
        }
        set
        {
            energy = value;
        }
    }
}

