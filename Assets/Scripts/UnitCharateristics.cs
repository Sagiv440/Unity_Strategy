using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitCharateristics : MonoBehaviour {

	private byte typeOfUnit = 0; // this integer determens wether the unit is a Biuding or a Character.
    public string ClassTag = "";

    //--------------------------------------------------------------
    //this determens the definsive and offensive Charateristic
    //-------------------------------------------------------------
    public int power = 0; // over all power of the unit
    private float max_health;
	public float HealthAmount;
	public float ArmorAmount;
	public float damage;
	[Range (0f,100f)]
	public float Accuracy; // how accurat a Unit is during combat
	[Range (0f,100f)]
	public float CriticalHit; // how offen a hit conflict havey damege
	[Range (0f,100f)]
	public float RegularHit;  // how offen a hit conflict  damege 
	public float TimeToCreat = 1f;

    public bool cover = false;

    weapon weapon = null;
    


	//--------------------------------------------------------------
	//this diterment the value amunt that is recuierd to produs the unit
	//-------------------------------------------------------------
	public int Credits = 0; 
	public int Energy = 0;

	//--------------------------------------------------------------
	//this veribles ditermins what is the functions and abiltes of the Unit
	//-------------------------------------------------------------
	public List<int> Buttons;
	public List<string> ButtonText;
	public List<UnityEvent> Events;

	public Interface Inter;

    public byte TypeOfUnit
    {
        get
        {
            return typeOfUnit;
        }
        set
        {
            typeOfUnit = value;
        }
    }
    public int Power
    {
        get
        {
            return power;
        }
        set
        {
            power = value;
        }
    }
    public float Armor
    {
        get
        {
            return ArmorAmount;
        }
    }
    public float Damage
    {
        get
        {
            return damage;
        }
    }
    public float Health
    {
        get
        {
            return HealthAmount;
        }
        set
        {
            HealthAmount = value;
        }
    }
    public float Max_Health
    {
        get
        {
            return max_health;
        }
    }
    public weapon Weapon
    {
        get
        {
            return weapon;
        }
        set
        {
            weapon = value;
        }
    }

	public float getCritical(){
		return CriticalHit;
	}
	public float getReglar(){
		return RegularHit;
	}

	void Awake (){

		Inter = new Interface (ButtonText,Events);
		if (Inter == null) {
			Debug.Log ("Inter is missing");
		}
        max_health = HealthAmount;
	}

    public void TakeDamage(float _Damage)
    { // Get Damage form enemy attacks.
        if (_Damage < 0f)
        {
            HealthAmount += _Damage;
        }
        if (HealthAmount <= 0f)
        {
            switch (typeOfUnit)
            {
                case 0:
                    GetComponent<UnitController>().is_Dead = true;
                    break;
                case 1:
                    GetComponent<BiuldingLogic>().is_Dead = true;
                    break;
            }
        }


    }

    public void ApplyDamage(UnitCharateristics _Enemy){ // Apply Damage to Enemy.

		float _Damage = (_Enemy.Armor - Damage) * DamageModal (Accuracy, _Enemy.getCritical(), _Enemy.getReglar());
		if (_Damage < 0) {
			_Enemy.TakeDamage (_Damage);
		} else {
			Debug.Log ("Armor is to strong");
		}
	}

    //Return the amount of damage based on the stats that you enterd -- Note  this function is shit (--broken--) and need to be Rewirtten !!!
    float DamageModal(float _Accuracy ,float _Critical ,float _Regular){ 
		float z = 0;
		if (_Accuracy <= 100f) {
			if (_Regular < 100f && _Critical < 100f) {
				float x = Random.Range (0, 100);
				float y = Random.Range (0, 100);
				if (x < _Accuracy) {
					if (y >= _Critical) {
						z = 1.5f;
					}
					if (y >= _Regular && y < _Critical) {
						z = 1.0f;
					}
					if (y < _Regular) {
						z = 0.2f;
					}
				} else {
					z = 0f;
				}
			} else {
				Debug.Log ("Damage Modale Balans is above 100%");
				return 0;
			}
		} else {
			Debug.Log ("Accuracy is above 100%");
			return 0;
		} 
		return z;
	}

    public void applyDamage(UnitCharateristics _Enemy, weapon Weapon)
    {
        float Damage = 0;
        range gun = null; // holds Range weapon
        maile sowrd = null;// holds maile weapon

        try { sowrd = (maile)Weapon;} // try to convert the Weapon class to a Maile class
        catch (System.InvalidCastException){sowrd = null;} // if the Weapon is not a Maile baset class maile equals null;
        try { gun = (range)Weapon; } // try to convert the Weapon class to a Maile class
        catch (System.InvalidCastException) { gun = null; }// if the Weapon is not a Maile baset class range equals null;

        byte type = (gun == null) ? (byte)1 : (byte)0;
        if (gun == null)
        {
            type = 1;
        }
        switch (type)
        {
            case 0:
                Damage = RangeDamage(_Enemy, gun);
                break;
            case 1:
                Damage = MaileDamage(_Enemy, sowrd);
                break;
        }

        _Enemy.TakeDamage(-Damage);
    }

    float MaileDamage(UnitCharateristics _Enemy, maile sowrd)
    {
        return sowrd.Damage;
    }
    float RangeDamage(UnitCharateristics _Enemy, range gun)
    {
        return gun.Damage;
    }

    public void test()
    {
        weapon sowerd = new maile();
        weapon bulter = new range();
        Debug.Log("The unit " + this.GetComponent<GameObject>() + " helth is " + this.Health);
        applyDamage(this, sowerd);
        Debug.Log("The unit"+ this.GetComponent<GameObject>() + "helth after tacing damage " + this.Health);
    }
}

/*
 * this class hold all the special abilitys of the unit or biulding thet will be display an the menu scrren whan you click on it
 */
public class Interface
{
	public List<int> Buttons;
	public List<string> ButtonText;
	public List<UnityEvent> Event;


	public Interface (List<int> _Buttons,List<string> _ButtonText, List<UnityEvent> _Event){

		Buttons = _Buttons;
		ButtonText = _ButtonText;
		Event = _Event;
	}

	public Interface (List<string> _ButtonText, List<UnityEvent> _Event){

		Event = _Event;
		Buttons = new List<int> ();
		int sum;

		for (int i = 0; i < Event.Count; i++) {
			Buttons.Add (i);
		}

		ButtonText = _ButtonText;
		sum = Event.Count - ButtonText.Count;
		if (sum > 0) {
			for (int i = 0; i < sum; i++) {
				ButtonText.Add ("Emptey");
			}
		}

		Debug.Log (Event.Count + " Events of ");
		Debug.Log (ButtonText.Count + " Buttons of ");


	}

}



public class weapon
{

    public int Damage = 0;
    public float write_of_Attack = 1;
}

public class maile : weapon
{

}
public class range : weapon
{
    public int offective_range = 0;
    public int Accurecy = 0;
}
public class projctile_lancher : weapon
{
    public GameObject projctile = null;
    public float max_dis = 50;
    public float Area_of_affect = 1;
}

public class bulter : range
{
    public bulter()
    {
        base.offective_range = 200;
        base.Accurecy = 80;
        base.Damage = 10;
        base.write_of_Attack = 0.5f;
    }
}
public class plasma : range
{
    public plasma()
    {
        base.offective_range = 200;
        base.Accurecy = 80;
        base.Damage = 80;
        base.write_of_Attack = 3f;
    }
}
public class Canon : range
{
    public Canon()
    {
        base.offective_range = 400;
        base.Accurecy = 80;
        base.Damage = 70;
        base.write_of_Attack = 1f;
    }
}
public class Hand_Garnade : projctile_lancher
{
    
}




