using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitController : MonoBehaviour
{

    private byte Dead = 0;

    public int IronNumber; // holds the unit number in the current select commend line
    public byte weapon_type = 0; // 
    public float AttackTime; // dittermens how much delay between to whate between attacks
    [Range(0.0f, 360.0f)]
    public float AngleOfView; // dittermens the viewing angle of the unit
    [Range(0.0f, 180.0f)]
    public float VerticalView; // dittermens the viewing angle of the unit
    [Range(0.01f, 100.0f)]
    public float RotatingSpeed = 1f;
    private float time = 0;
    private float Dtime = 0; 

    private string EnemyTag;
    private string NeutralTag;
    private string MainTag;

    public float HorizontalAngle; // hold the Horizontal Angle between the unit and the current Enemy ("Enemy")
    public float VerticalAngle; // hold the Vertical Angle between the unit and the current Enemy ("Enemy")


    public GameObject Enemy;// hold the current Enemy in view
    public GameObject TEnemy;// hold the Enemy assind by the play for the unit to attack
    public GameObject AEnemy;// hold the current Enemy in view
    public GameObject frendly;


    public float Dis_form_Targget = 0;
    public float Dis_for_Malae;

    public bool inSight = false;
    public bool Cowching = false;
    public bool Active = false;
    public bool unitSelect = false;
    public bool is_Dead = false;
    public NavMeshAgent Nav;
    public Animator Anim;

    public UnitCharateristics Charater; // hold the Charateristics of the spasifice unit.

    private Vector3 point;
    private Vector3 greed = Vector3.zero;

    public List<string> visbye; //hold the tag to all the factions that see this unit
    private UnitList list; // hold a referns to the main commend line.
    private UnitNode Node; // hond a referns to the personl commend node in the commend line.
    public int CommendSelect = 0; // dittrmens what commned is runing in the update function. 

    // Use this for initialization

    //--------------------
    //this functions used to courdinate with ather functions that need to run when they are cald
    public delegate void move();
    public event move UnitMove; //execiut functions that need to run when unit is moving

    public delegate void fire();
    public event fire UnitFire; //execiut functions that need to run when unit is Fireing

    public delegate void attact(); //execiut functions that need to run when unit is attacking
    public event attact UnitAttack;
    //--------------------


    void Awake()
    {
        gameObject.tag = GetComponentInParent<Manager>().Unit; // atach the unit to the commanding manager ( user / AI);

        this.EnemyTag = GetComponentInParent<Manager>().Enemy; // the manager defines to the unint the enemy tag 
        this.NeutralTag = GetComponentInParent<Manager>().Neutralbuilding; // the manager defines to the unit the capture points and natural bildings tag
        this.MainTag = this.EnemyTag; // reset the uint to defalt surtch enemy tags;

        Charater = GetComponent<UnitCharateristics>(); // hold a refernce to the UnitCharateristics object of the unit;
        GetComponentInParent<Manager>().Army.Add(this.gameObject); //assing the Unit GameObject refernce to the Manager GameObject list
        GetComponentInParent<Manager>().Army_Power += Charater.Power; //adds the units power to the Manager Army Power level 

        Charater.Weapon = set_weapon( weapon_type ); // set the weapon of the unit

        Anim = GetComponent<Animator>(); // hold a refernce to the Animator component
        Nav = GetComponent<NavMeshAgent>(); // holds a refernce to NavMeshAget component
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<MouseController>().MouseSelect += DselectUnit; /// ????
		Charater.TypeOfUnit = 0;
        CommendSelect = 0;
    }

    weapon set_weapon(byte set)
    {
        switch (set)
        {
            case 0:
                return  new bulter();

            case 1:
                return new plasma();
            
            case 2:
                return new Canon();
        }
        return null;
    }

    public void setIronNumber(int _IronNumber)
    {
        IronNumber = _IronNumber;
    }


    public UnitNode node
    {
        set
        {
            Node = value;
        }
        get
        {
            return Node;
        }
    }

    public void subscribeTo(UnitList _list)// Assing Unit Fouctions to unit list event.
    {
        _list.MoveUnit += MoveCommend;
        _list.AttackEnemy += AttackCommend;
        _list.CapturePoint += CaptureCommend;
        list = _list;
    }

    public void setEnemyInRagne(GameObject _Enemy)
    {
        Enemy = _Enemy;
        UnitController current = null;
        if (_Enemy != null && _Enemy.tag == EnemyTag) {
            current = _Enemy.GetComponent<UnitController>();
            if (current != null)
                current.visbye.Add(this.tag);
        }
    }

    public bool isActive
    {
        get
        {
            return Active;
        }
    }

    public void setFrendly(GameObject _Frand)
    {
        frendly = _Frand;
    }

    public GameObject getEnemyInRagne (){
		return Enemy;
	}
	//---------------------------------------------------------------

	public GameObject getTEnemy (){
		return TEnemy;
	}
	//----------------------------------------------------------------
	public void setAEnemyInRagne (GameObject _AEnemy){
		AEnemy = _AEnemy;
	}
	public GameObject getAEnemyInRagne (){
		return AEnemy;
	}

	public void setHorizontal (float _HorizontalAngle){ 
		HorizontalAngle = _HorizontalAngle;
	}
	public float getHorizontal (){
		return HorizontalAngle;
	}

	public void setVertical (float _VerticalAngle){
		VerticalAngle = _VerticalAngle;
	}
	public float getVertical (){
		return VerticalAngle;
	}

	public void setEnemyTag (string _tag){
		EnemyTag = _tag;
	}
	public string getEnemyTag (){
		return EnemyTag;
	}

    public void setMainTag(string _tag)
    {
        MainTag = _tag;
    }
    public string getMainTag()
    {
        return MainTag;
    }

    public void set_Distans(float _Dis_form_Targget)
    {
        Dis_form_Targget = _Dis_form_Targget;
    }

    public void SelectUnit(){//Marck Unit is Selected.
		unitSelect = true;
		if(Nav.enabled == false){
			Nav.enabled = true;
		}
	}
	public void DselectUnit(){// Marck Unit is Not Selected.
		unitSelect = false;
	}

	// Update is called once per frame
	void Update () {

		if (is_Dead == false) {
            Dtime = Enemy == null && Dtime <= 10 ? Dtime+Time.deltaTime : 0; // timer for count time out of enemy sight
            if (Dtime >= 10) // if unit is not spoted a
                visbye.Clear();
            
       
			switch (CommendSelect) { // this switch controlle the different biaveyers of the individuale units and how it shold execute the selected commend.
			case 0: // Updating when useing MoveCommend Logic.
                    
                    Active = false; // note that the Unit is idel ;
                    MoveToTargget();

				if (AEnemy != null && Mathf.Abs (Nav.desiredVelocity.normalized.z) + Mathf.Abs (Nav.desiredVelocity.normalized.x) == 0 && Enemy == null) {
					Vector3 Targget = AEnemy.transform.position - this.transform.position;
					Quaternion LocalRotation = Quaternion.LookRotation (Targget);
					transform.rotation = Quaternion.Slerp (this.transform.rotation, LocalRotation, Time.deltaTime * RotatingSpeed);
				}
				break;

			case 1: // Updating useing AttackCommend Logic.

                    Active = true; // note that the Unit is in activity ;

                    if (TEnemy != null) { // tiels the Unit witch Enemy to fallow.
					if (TEnemy == Enemy) {
						if (Nav.enabled == true) {
							Nav.enabled = false;
						}
						AttackEnemy ();
					} else {
						if (Nav.enabled == false) {
							Nav.enabled = true;
						}
						FallowTargget (TEnemy); // when Enemy is in range Aime and fire.

					}

				} else {
					CommendSelect = 0;
				}

				break;

			case 2: // Activated when Capture commend is called .
                    UnitMove();
                    Active = true; // note that the Unit is in activity ;

                    if (AEnemy == TEnemy)
                    {
                        if (TEnemy.GetComponent<neutraleBiulding>().unitManager == null)
                        {
                            if (Dis_form_Targget <= Dis_for_Malae)
                            {
                                TEnemy.GetComponent<neutraleBiulding>().StartCapture(this.gameObject);
                                Cowching = true;
                               
                            }
                            else
                            {
                                Cowching = false;
                            }
                        }
                        else if (TEnemy.GetComponent<neutraleBiulding>().unitManager == this.GetComponentInParent<Manager>())
                        {

                            Vector3 vec = this.transform.position;
                            vec.z -= 5;
                            Nav.SetDestination(vec);
                            TEnemy = null;
                            MainTag = EnemyTag;
                            Cowching = false;
                            CommendSelect = 0;
                        }
                        else
                        {
                            if(TEnemy.GetComponent<neutraleBiulding>().unit == null || TEnemy.GetComponent<neutraleBiulding>().unit.GetComponent<Manager>() == this.GetComponent<Manager>())
                            {
                                if (Dis_form_Targget <= Dis_for_Malae)
                                {
                                    TEnemy.GetComponent<neutraleBiulding>().StartCapture(this.gameObject);
                                    Cowching = true;
                                }
                                else
                                {
                                    Cowching = false;
                                }
                            }
                            else
                            {
                                AttackCommend(TEnemy.GetComponent<neutraleBiulding>().unit.GetComponent<Collider>(), new formations());
                            }
                        }

                    }
                   
				break;
			}// End of main Switch
		} else {
            switch (Dead)
            {
                case 0:
                    Die();
                    break;
                case 1:
                    time = time + Time.deltaTime;
                    if (time > 2)
                    {
                        Destroy(Nav);
                        Destroy(gameObject.GetComponent<Rigidbody>());
                        Destroy(gameObject.GetComponent<CapsuleCollider>());
                        Destroy(gameObject.GetComponentInChildren<EnemyDetecter>());
                        Destroy(gameObject.GetComponent<UnitCharateristics>());
                        Destroy(gameObject.GetComponent<UnitAnimaterController>());
                        Destroy(gameObject.GetComponent<UnitController>());
                        

                    }
                    break;
            }
        }
	}
    

	void Fire(UnitCharateristics _Charater){
		//if (Enemy.GetComponent<UnitCharateristics> () != null) {
			time += Time.deltaTime;
			if (time > AttackTime) {
				UnitFire ();
				Charater.applyDamage (_Charater , Charater.Weapon);
				time = 0;
			}
		/*} else {
			Debug.Log ("Cannot fire on the enemy");
		}*/
	}

	void OnDisable()// Disconact from Commend Line Befor Death.
	{
		//GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<MouseController> ().MouseSelect  -= DselectUnit;
		if (list != null) { //if Unit is assind to the carrent commend line on Daeth Unit will Desubscribe from it.
			list.MoveUnit -= MoveCommend;
			list.AttackEnemy -= AttackCommend;
            list.Remove (Node);
		}
        
    }
		
	//unit halif

	//wan Right Mouse Button Prest on terrain and unit is selected.
	//move unit to mouse point on terrain.
	public void MoveCommend(Collider _collider, formations _formation)
	{
       
        CommendSelect = 0;
		Debug.Log ("Rager Moveing");
		MoveUnit (_formation.UnitPosition[IronNumber-1]);
		TEnemy = null;

        MainTag = EnemyTag;
        Cowching = false;
    }
    public void MoveCommend(Vector3 Traget)
    {

        CommendSelect = 0;
        Debug.Log("Rager Moveing");
        MoveUnit(Traget);
        TEnemy = null;

        MainTag = EnemyTag;
        Cowching = false;
    }

    //wan Right Mouse Button Prest on enemy and unit is selected.
    //move unit to attack enemy.
    public void AttackCommend(Collider _collider, formations _formation)
	{
     
        MainTag = EnemyTag;
        TEnemy = _collider.gameObject;
		CommendSelect = 1;
		Debug.Log ("For the Emparer");

        MainTag = EnemyTag;
        Cowching = false;
    }
    public void AttackCommend(Collider _collider)
    {

        MainTag = EnemyTag;
        TEnemy = _collider.gameObject;
        CommendSelect = 1;
        Debug.Log("For the Emparer");

        MainTag = EnemyTag;
        Cowching = false;
    }

    public void CaptureCommend(Collider _collider, formations _formation)
	{
        TEnemy = null;
        MainTag = NeutralTag;
        CommendSelect = 2;
        TEnemy = _collider.gameObject;
        Debug.Log ("Capturing the point");
        MoveUnit(_formation.UnitPosition[IronNumber - 1]);

    }
    public void CaptureCommend(Collider _collider, Vector3 Target)
    {
        TEnemy = null;
        MainTag = NeutralTag;
        CommendSelect = 2;
        TEnemy = _collider.gameObject;
        Debug.Log("Capturing the point");
        MoveUnit(Target);

    }

    //wan rechriet camend is aplied unit will move to stating point.

    void MoveUnit (Vector3 _point)
	{
		if (Nav.enabled == false) {
			Nav.enabled = true;
		}
		if (UnitMove != null) {
			UnitMove ();
		}

        Debug.Log(gameObject + "move to" + _point);
		Nav.stoppingDistance = 0;
		Nav.SetDestination (_point);
	}

	void FallowTargget (GameObject _Enemy)
	{
		if (Nav.enabled == false) {
			Nav.enabled = true;
		}
		if (_Enemy != null) {
			UnitMove ();
			Nav.SetDestination (_Enemy.transform.position);
		}
	}

	void AttackEnemy ()
	{
			UnitAttack (); 
			Fire (Enemy.GetComponent<UnitCharateristics> ());
	}

    void MoveToTargget()
    {
        if (Enemy != null)
        {
            
            UnitAttack();
            Fire(Enemy.GetComponent<UnitCharateristics>());
        }
        else
        {
            //view.UpdateTargget (); //Update the targget when old targget is no longer in sight.
            if (UnitMove != null)
            {
                UnitMove();
            }
        }
    }

	public void Die(){
        GetComponentInParent<Manager>().Army_Power -= Charater.Power;
        gameObject.tag = "Untagged";
        this.GetComponentInParent<Manager>().UnitDid(this.gameObject);
        Destroy(gameObject);

    }


	//
	//
	//
	 
}
