using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiuldingLogic : MonoBehaviour {

	public bool is_Dead = false;

	public Manager player;
	public UnitCharateristics Charater;
	public Transform SponPoint;


	public List<GameObject> Sponers;
	public SponeList list;
	public bool BiuldingSelect;
	private int mode = 0;

	private GameObject Sponer;
	private float delay = 0 ;

	public bool taskApprove;


	void Awake(){
		gameObject.tag = GetComponentInParent<Manager>().Unit;
		Charater = GetComponent<UnitCharateristics> ();
		player = GetComponentInParent<Manager> ();
		Charater.TypeOfUnit = 1;
	}

	public void SponMenu(int _SponSelect){

        UnitCharateristics Sponers_char = Sponers[_SponSelect].GetComponent<UnitCharateristics>();
    
        if (player.Credit >= Sponers_char.Credits && 
            player.Energy >= Sponers_char.Energy  &&
            player.Army_Power + Sponers_char.Power <= player.Game_Manger.Max_Army_Power) {
			if (list == null) {
				list = new SponeList ();
				list.add (_SponSelect);
			} else {
				list.add (_SponSelect);
			}
			player.Credit -= Sponers_char.Credits;
			player.Energy -= Sponers_char.Energy; 
        }
	}

	//this function is Spon Unit 

	void Update(){

		if (is_Dead == false) {
			if (list != null) {
			

				if (list.front == null && list.back == null) {
					list = null;

				} else {
					delay = delay + Time.deltaTime;

					if (delay > Sponers [list.back.Sponer].GetComponent<UnitCharateristics> ().TimeToCreat) {
						Instantiate (Sponers [list.back.Sponer], SponPoint);
						list.remove ();
						delay = 0f;
					}

				}
			}
		} else {
			Destroy (gameObject);
		}

		switch (mode) {
		case 0:

			break;
		case 1:
			
			break;
		}
	}

}
/*---------------------------------------------------------------------------------------------------------------------------------------
 * class SponeList 
 * this class reprosent a list of units request by the user/AI to spone to the game.
 * 
 * variables:
 * front - hold the first node of the list 
 * back - hold the last node of the list
 * count - hold how mach nods the are in the list
 * 
 *---------------------------------------------------------------------------------------------------------------------------------------*/
public class SponeList{

	public SponeNode front;
	public SponeNode back;
	private int count;

	public SponeList(){
		front = null;
		back = null;
		count = 0;
	}
	public void add (int _Sponer){

		if (front == null && back == null) {
			front = new SponeNode (_Sponer);
			back = front;
			count++;
		} else {
			SponeNode current = new SponeNode (_Sponer);
			current.next = front;
			front.last = current;
			front = current;
			count++;
		}
		Debug.Log("Unit number "+count+" is added");
	}
	public void removeFromeTheFornt (){
		if (front == back && front != null) {
			front = null;
			back = null;
			count--;
		}
		if (front != back) {
			front = front.next;
			count--;
		}
	}
	public void remove(){
		if (front == back && front != null) {
			front = null;
			back = null;
			count--;
		}
		if (front != back) {
			back = back.last;
			count--;
		}
		Debug.Log("Unit number "+count+" was Spond");
	}

}
public class SponeNode{
	
	public SponeNode last;
	public SponeNode next;
	public int Sponer;


	public SponeNode(int _Sponer){
		next = null;
		last = null;
		Sponer = _Sponer;
	}
	public void setSponer(int  _Sponer){
		Sponer = _Sponer;
	}
	public int getSponer(){
		return Sponer;
	}
}
