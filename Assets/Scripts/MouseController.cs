using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MouseController : MonoBehaviour {
	


	public float Range = 100f;
	private Vector3 MousePos, HitPoint, HitDir, LastPoint;
	private Vector2 P_one;
	public string PlayGround; //hold the tag of the PlayGround
	public string Unit; // hold the tag of the Units that bilong to the player
	public string Enemy; // hold the tag of the Units that bilong to the Enemy.

	public Manager maneger;

	public GameObject test;

	private UnitList list;
	public formations Formation;
	public UnitMenu Menu;
	private Ray mouseRay;
	private RaycastHit Hit;
	public float angule;
	public int count;
	private bool Shift;

    public byte formation_Type = 0;


	//----------------------
	//this veriables rapresent resorces which the player have.
	public int Credits = 100; //Cradits are the main resorce whish the player will use to buy Units and biuldings.
	public int Energy = 100; // Energy is a resorce thet the player will use to upgrade or modifay Unit and biuldings.
	public int UnitCapacity = 0; // this value represent the carrent amunt of unit that the player have in the Game.
	public int MaxUnitCapacity = 30; // this value repercent the Maxsemum amunt of unit the player can have in the game (this value can be incress in Game).

	public delegate void MouseSelecter();
	public event MouseSelecter MouseSelect;


	// Use this for initialization
	void Awake () 
	{
		maneger = GameObject.FindGameObjectWithTag ("Player").GetComponent<Manager> ();
		Formation = GetComponent<formations> ();
	}
	 

	public Vector3 getHitPoint () 
	{
		return HitPoint; 
	}
	public Vector3 getMousePosition () 
	{
		return MousePos; 
	}
	public string getPlayGroundTag ()
	{
		return PlayGround;
	}
	public string getUnitTag ()
	{
		return Unit;
	}
	public string getEnemyTag ()
	{
		return Enemy;
	}
	public  void setPlayGroundTag (string _tag)
	{
		PlayGround = _tag;
	}
	public  void setUnitTag (string _tag)
	{
		Unit = _tag;
	}
	public  void setEnemyTag (string _tag)
	{
		Enemy = _tag;
	}
	public void restAllTags ()
	{
		LastPoint = transform.position;
	}

	// Update is called once per frame

	void Update() 
	{
		RightMouseClick ("Fire1");
		LeftMouseClick ("Fire2");
	}


	void OnGUI()
	{
			Event e = Event.current;
			MousePos = e.mousePosition;
	}
	void LeftMouseClick (string _Button)
	{
		if (Input.GetButtonDown (_Button) == true) {
			Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit _Hit;

			if (Physics.Raycast (mouseRay, out _Hit, Range)) {
				HitPoint = Hit.point;
				P_one = (Vector2)MousePos;
				Hit = _Hit;
			}
		}
		//when right mouse button is rellest it shot out a second ray form the mause saves the point of impact and use the target point and the carrent point to get an angule
		//then in it right mouse commend and genorate a formtion in the recevd angule.

		if (Input.GetButtonUp (_Button) == true) {

			HitDir = MousePos;
			if (P_one != (Vector2)MousePos) {

				//Cuceulate the angule from the Hit point to the point witch the mouse is draging toword.

				angule = Mathf.Acos ((HitDir.x - P_one.x) / (Mathf.Sqrt (Mathf.Pow (HitDir.y - P_one.y, 2) + Mathf.Pow (HitDir.x - P_one.x, 2))));
				if (HitDir.y < P_one.y) {
					angule = Mathf.PI + (Mathf.PI - angule);
				}

		
				//Cuceulate the angule from the Hit point to the last Hit point.

				angule = Mathf.Acos ((LastPoint.x - HitPoint.x) / (Mathf.Sqrt (Mathf.Pow (LastPoint.z - HitPoint.z, 2) + Mathf.Pow (LastPoint.x - HitPoint.x, 2))));
				if (LastPoint.z < HitPoint.z) {
					angule = Mathf.PI + (Mathf.PI - angule);
				}
			}


			MouseLeftCommend (Hit, maneger.PlayGround, maneger.Enemy);
			LastPoint = HitPoint;
		}
	}
	void RightMouseClick (string _Button)
	{
		if (Input.GetButtonDown (_Button) == true) {
			Ray mouseRay = Camera.main.ScreenPointToRay (Input.mousePosition);
			RaycastHit _Hit;

			if (Physics.Raycast (mouseRay, out _Hit, Range)) {
				Hit = _Hit; 
				MouseRightCommend (Hit, maneger.Unit, Shift);
			}
		}
		//Right mouse button functions : when the right mouse is prest dwon it shot out a ray form the mouse and store the target adders in the class.

	
		if (Input.GetButton("Fire3") == true){//Shift button function
			Shift = true;
		}else {
			Shift = false;
		}
	}

	//-------------------------------------------------------------------------
	//this function control all right mouse button functions.
	//-------------------------------------------------------------------------
	void MouseLeftCommend(RaycastHit _Hit, string _PlayGround, string _Enemy)
	{
		if (_Hit.collider.tag != maneger.Unit) 
		{

			if (list != null) {
				//Creat and save a formation
				list.SendCommendToUnits (_Hit.collider, setFormation (_Hit, formation_Type, angule));//send a commend to units.
			}
		}
	}

	void MouseRightCommend(RaycastHit _Hit, string _Unit, bool _Shift)
	{
		if (_Hit.collider.tag == _Unit) {
			int _type = _Hit.collider.GetComponent<UnitCharateristics> ().TypeOfUnit;
			if (_type == 0 & _Shift == true) {//Select and save a Unit in a list to issue a command. 
				shiftSelect (list, _Hit);
			}

			if (_type == 0 & _Shift != true) {//Select a Unit to issue a command.

				if (MouseSelect != null) { // Deselect all the Unit that where selected 
					MouseSelect ();
				}

				Select (list, _Hit);
				Menu.ActivatButtons (_Hit.collider.gameObject.GetComponent<UnitCharateristics> ().Inter);
			}

			if (_type == 1) {
				Debug.Log ("It is a Biulding");
				Menu.ActivatButtons (_Hit.collider.gameObject.GetComponent<UnitCharateristics> ().Inter); // Adjust Unit menu to acording to Unit Specification.

				//_Hit.collider.gameObject.GetComponent<BiuldingLogic> ().SponMenu (0);
			}
		}
	}


	formations setFormation(RaycastHit _Hit,int typeOfFormetion, float _angule)
	{
		//Formation.transform.position = _Hit.point;
		formations _Formation = new formations();
		_Formation.CraetFormation (_Hit.point ,list.getCount (), typeOfFormetion,_angule );
		return _Formation;
	}
	void shiftSelect(UnitList _list ,RaycastHit _Hit)  //Select a unit to receve Commend and or add Unit to commend line to send commend to multible Unit.
	{
		if (_list == null)
		{
			Select (_list, _Hit);
			//count = _list.NodeCount ();
		} else {
			_list.add (_Hit);
			//c  ount = _list.NodeCount ();
		}
	}
	void Select(UnitList _list ,RaycastHit _Hit)  // Select only one unit to receve commend .
	{
		list = new UnitList (maneger.Unit, maneger.Enemy);
		list.add ( _Hit);
		//count = list.NodeCount ();
	}
}
//-----------------------------------------------------------------------------
//A class that represent List that store data in a form of a Magzine.
//Useing a sub class unit Node as a contener to hold the addreas of the Unit Selected.
//-----------------------------------------------------------------------------
public class UnitList //Represents a collection of magzines. "represent the main commend line"
{
	private UnitNode Node;
	public int count;

	public string PlayGround;
	public string Unit;
	public string Enemy;


	public UnitList(string _Unit, string _Enemy)  //Sets up an initiallyempty list of units.
	{
		Node = null;
		PlayGround = "PlayGround";
		Unit = _Unit;
		Enemy= _Enemy;
	}
	//fuctions that are used to send commend to the verious unit that are currently listed in the commend line.
	//---------------------------------------------------------------------------
	public delegate void MouseSelecter(Collider _collider ,formations _formation);
	public event MouseSelecter MoveUnit; // Send move commend.
	public event MouseSelecter AttackEnemy; // send attack commend.
	public event MouseSelecter CapturePoint; // send capture commend.
	//---------------------------------------------------------------------------
	

	public void add(RaycastHit _Hit) { // this function Adds a Unit node to the commend line.
		
		UnitNode current = null;
		if (_Hit.collider.GetComponent<UnitController> ().unitSelect != true) {
			if (Node == null) {
				Node = new UnitNode(_Hit.collider.gameObject,1);
				count++;
			} else {
				current = new UnitNode (_Hit.collider.gameObject, Node.getIronNumber() + 1); 
				current.next = Node;
				Node.last = current;
				Node = current;
				count++;
			}
			Node.getUnit().GetComponent<UnitController> ().subscribeTo (this);// Assine the selected unit commend function to Unitlist maine commend function.
			Node.getUnit().GetComponent<UnitController> ().SelectUnit (); // mark unit as selected.
			Node.getUnit().GetComponent<UnitController> ().node = Node; //Assine a refarens of the node to it's Unit.
			Node.getUnit().GetComponent<UnitController> ().setIronNumber(Node.getIronNumber());
		}
	}

    // this function Removes a Unit node from commend line.
    // ---------------------------------------------------
    public void Remove(UnitNode node){ 
		UnitNode current;

		if (node.last == null && node.next == null) {// If this node is the only node on the commend line.
			Node = null;
			Debug.Log ("node number " + node.getIronNumber () + " Has Bin Removed");
		}


		else if (node.last == null) {// If this node is the last.
			node.next.last = null;
			Debug.Log ("node number " + node.getIronNumber () + " Has Bin Removed");
		}


		else if (node.next == null) {// If this node is the first.
			node.last.next = null;
			current = node.last;
			while (current != null) { //Here the loop update all the Iron numbers(the numbers that indecate where the unit stand in the formation).
				current.IronNumber --;
				current.getUnit().GetComponent<UnitController> ().setIronNumber(current.getIronNumber());
				current = current.last;
			}
			Debug.Log ("node number " + node.getIronNumber () + " Has Bin Removed");
		}


		else if (node.last != null && node.last != null) {// If this node is in the commend line.
			node.next.last = node.last;
			node.last.next = node.next;
			current = node.last;
			while (current != null) { //Here the loop update all the Iron numbers(the numbers that indecate where the unit stand in the formation).
				current.IronNumber --;
				current.getUnit().GetComponent<UnitController> ().setIronNumber(current.getIronNumber());
				current = current.last;
			}
			Debug.Log ("node number " + node.getIronNumber () + " Has Bin Removed");
		}
	}
		
	public int getCount(){ //return the number of unit that are carrent select.
		return count;
	}
		
	public void SendCommendToUnits(Collider _collider,formations _formation)
	{ 
		if (_collider.tag == Enemy) {
			AttackEnemy (_collider ,_formation);
		}

		if (_collider.tag == PlayGround) {
			MoveUnit (_collider ,_formation);
		}
		if (_collider.tag == "N_building") {
			CapturePoint (_collider ,_formation);
		}
	}

	public int  NodeCount()
	{
		UnitNode current = null;
		count = 0;
		current = Node;

			while (current != null)
		{
				current = current.next;
				count++;
		}
		return count;
	}
		
		/*while(current.next !=null){
			current = current.next;
				current.next = node;
		}*/
}


//-----------------------------------------------------------------------------
//An inner class that represent a node in the UnitList class("magzine")list.
//The public variables are accessed by the Unitlist class.
//-----------------------------------------------------------------------------
public class UnitNode 
{
	public int IronNumber;
	public UnitNode next;
	public UnitNode last;
	private GameObject Unit;


	public UnitNode(GameObject _Unit,int _IronNumber){//Set up the node.
		next = null; // Hold the addres of the next Node in the data mag. 
		last = null; // Hold the addres of the last Node in the data mag.
		Unit = _Unit; // Assine a Unit GameObject to the Node.
		IronNumber =_IronNumber; //The tag number of the unit.
	}
	public GameObject getUnit (){//Return the Unit assine to the Node.
		return Unit;
	}
	public int getIronNumber(){
		return IronNumber;
	}
	public void setUnit (GameObject _Unit){//Assine a new unit to the Node.
		Unit = _Unit;
	}
	void OnDisable()
	{
		Unit.GetComponent<UnitController> ().DselectUnit ();
	}

}
