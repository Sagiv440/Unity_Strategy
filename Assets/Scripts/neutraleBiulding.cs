using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class neutraleBiulding : MonoBehaviour {
	public byte Biuding_purpose = 0;
	public Manager unitManager;
    public GameObject unit;
    public bool isCoptoring = false;

    public float time =0;
	public float Refile_time;
    public float Capture_time;

    public bool IsCoptoring
    {
        get
        {
            return isCoptoring;
        }

        set
        {
            isCoptoring = value;
        }
    }


    // Use this for initialization
    void Start () {
		
	}
	public void StartCapture(GameObject Unit)
    {
        unit = Unit;
        if (unitManager == null)
        {
            Biuding_purpose = 2;
        }
        else if(unitManager != unit.GetComponentInParent<Manager>())
        {
            Biuding_purpose = 3;
        }
    }
    

	// Update is called once per frame
	void Update () {

		switch (Biuding_purpose) {
        case 0:
                if(IsCoptoring == true)
                {
                    IsCoptoring = false;
                }
            break;
		case 1: // in this instans the biulding is acting as a resorce point
			if (unitManager != null) {
				time = time + Time.deltaTime;
				if(time > Refile_time){
				unitManager.Credit += 10;
					time = 0;
				}
			}
                if (IsCoptoring == true)
                {
                    IsCoptoring = false;
                }
                break;
		case 2:  // biulding is been capturd by a team
                time = time + Time.deltaTime;
                if (time > Capture_time)
                {
                    unitManager = unit.GetComponentInParent<Manager>();
                    time = 0;
                    Biuding_purpose = 1;
                    unit = null;
                }
                if (unit != null)
                {
                    if (unit.GetComponent<UnitController>().CommendSelect != 2)
                    {
                        IsCoptoring = false;
                        unit = null;
                        Biuding_purpose = 0;
                    } // if Unit stops to capture the point mid way the capture prosas will reset
                }
                break;
		case 3:// biulding is been libereted form the enemy team
                time = time + Time.deltaTime;
                if (time > Capture_time)
                {
                    unitManager = null;
                    time = 0;
                    Biuding_purpose = 2;
                }
                if (unit != null)
                {
                    if (unit.GetComponent<UnitController>().CommendSelect != 2)
                    {
                        IsCoptoring = false;
                        unit = null;
                        Biuding_purpose = 1;
                    }// if Unit stops to libereate the point mid way the the point will cary on suppling points to the last team that capture it.
                }
                break;
		}
	}
    
}
