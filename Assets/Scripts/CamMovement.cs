using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovement : MonoBehaviour {
	public float offset;
	public float camSpeed = 0;
	private Vector3 CamAxe;
	private Vector3 Border;
	public Vector3 MousePos;
	public string Tag;
	public int daedZone;
	public float CamHit;

    public float top_B = 0, Buttom_B = 0, Left_B = 0, Right_B = 0;

    private MouseController MouseC;
	Camera mainCamera;


	public float RFrach = 0.0f; // mouse rifrach

	public string getTag (){
		return tag;
	}
	public  void setTag (string _tag){
		Tag = _tag;
	}
	public void restTag (){
		Tag = "PlayGround";
	}


	//set the distans form the sceen age form witch the camera star to move
	public void setBorderMoveZome(int _deadZone){
		daedZone = _deadZone;
	}


	public Vector3 getCamAxe(){// get camera global psition
		return CamAxe;
	}


	public void setMouseContrller(MouseController _MouseC){//asine a new mause controller
		MouseC = _MouseC;
	}


	public void MoveToTarget(Vector3 Target){
		CamAxe.x = Target.x;
		CamAxe.z = Target.z - offset;
	}
	// Use this for initialization
	void Start () {
		CamHit = gameObject.transform.position.y;// hight of the camera from the terrane 
		mainCamera = Camera.main;// camera refarens 
		CamAxe = gameObject.transform.position;// globale position of the camera
		setMouseContrller(GetComponent<MouseController> ());//refarens for the mause comtroller script
		Border = new Vector3 (mainCamera.pixelWidth, mainCamera.pixelHeight, 0f);//hold the far ages valuse of the screen

	}
	// Update is called once per frame
	void Update () {
		CamHightFromTerrain ();

	}
	void FixedUpdate() {
		CamMoveMent (MouseC);
		gameObject.transform.position = CamAxe;
	}

	void OnGUI()
	{
		RFrach = RFrach + Time.deltaTime;

		if (RFrach > 0.1f)
		{
			Event e = Event.current;
			Debug.Log(e.mousePosition);
			MousePos = e.mousePosition;
			RFrach = 0.0f;
		}
	}

	void CamMoveMent(MouseController _MouseC){
		if (MousePos.y <= 0 + daedZone && CamAxe.z < top_B) { 
			CamAxe.z = CamAxe.z + camSpeed * Time.deltaTime;
		}
		if (MousePos.y >= mainCamera.pixelHeight - daedZone && CamAxe.z > Buttom_B) { 
			CamAxe.z = CamAxe.z - camSpeed * Time.deltaTime;
		}
		if (MousePos.x <= 0 + daedZone && CamAxe.x > Left_B) { 
			CamAxe.x = CamAxe.x - camSpeed * Time.deltaTime;
		}
		if (MousePos.x >= mainCamera.pixelWidth - daedZone && CamAxe.x < Right_B) { 
			CamAxe.x = CamAxe.x + camSpeed * Time.deltaTime;
		}
	}
	void CamHightFromTerrain(){
		Ray ray = new Ray (transform.position,Vector3.down);
		RaycastHit hit;

		if(Physics.Raycast(ray ,out hit , 1000f)){
			if(hit.collider.tag == Tag)
			setCamY(CamHit + hit.point.y);
		}
	}
	//set the hit of the camera
	void setCamY(float _y){
		CamAxe.y = _y;
	}
}
