using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.AI;

public class UnitAnimaterController : MonoBehaviour {
	public Animator Anim;
	public NavMeshAgent Nav;
	private UnitController Unit;

	public int AnimatorSwitch;

	private float AimVertical;
	private float AimHorizontal;

	private float HorizontalModifier;
	private float VerticalModifier;


	[Range(0.01f, 100.0f)]
	public float AimTransiton = 1f;
	private int setAim = 0;
	private float AimngLaier = 0f;

	public GameObject Targget;
	private Vector3 EnemyTargget;
	public RaycastHit Hit;
	public ParticleSystem MuzzleFlush;
    public float speed;




	void Awake () {
		AnimatorSwitch = 0;
		Anim = GetComponent<Animator> ();
		Nav = GetComponent<NavMeshAgent> ();
		Unit = GetComponent<UnitController> ();

        HorizontalModifier = Unit.AngleOfView;
		VerticalModifier = Unit.VerticalView;

		Unit.UnitMove += UnitMove;
		Unit.UnitFire += Fire;
		Unit.UnitAttack += UnitAttack;
	}

	// Update is called once per frame
	void Update () {
        if (Unit.is_Dead == true)
        {
            setAim = 2;
        }
            Targget = Unit.getEnemyInRagne();
            Anim.SetFloat("AimVertical", AimHorizontal);
            Anim.SetFloat("AimHorizontal", AimVertical);

            AimHorizontal = -(AdjustRottationN(Unit.getHorizontal()) / HorizontalModifier) + 0.5f; // cunvert the Y rotation to a value that return 0 then Angule - Horizontal Angle/2 and 1 then Angule is  Horizontal Angle/2.
            AimVertical = -(AdjustRottationN(Unit.getVertical()) / VerticalModifier) + 0.5f; // cunvert the x rotation to a value that return 0 then Angule is -View/2 and 1 then Angule is  View/2.

           UnitMovement(Nav.desiredVelocity, Nav.speed);
        switch (setAim)
        {

            case 0:
                if (Anim.GetLayerWeight(1) > 0)
                {
                    AimngLaier = Mathf.Lerp(AimngLaier, 0, Time.deltaTime * AimTransiton);
                    Anim.SetLayerWeight(1, AimngLaier);
                    Anim.SetLayerWeight(2, AimngLaier);
                }
                break;
            case 1:
                if (Anim.GetLayerWeight(1) < 1)
                {
                    AimngLaier = Mathf.Lerp(AimngLaier, 1, Time.deltaTime * AimTransiton);
                    Anim.SetLayerWeight(1, AimngLaier);
                    Anim.SetLayerWeight(2, AimngLaier);
                }
                break;
            case 2:
                if (Anim.GetLayerWeight(1) > 0)
                {
                    AimngLaier = Mathf.Lerp(AimngLaier, 0, Time.deltaTime * AimTransiton);
                    Anim.SetLayerWeight(1, AimngLaier);
                    Anim.SetLayerWeight(2, AimngLaier);
                }
                Die();
                break;

        }
        
	}


	// Use this for initialization
	public void setSwitch(int _switch, RaycastHit _Hit){
		AnimatorSwitch = _switch;
		Hit = _Hit;
	}

	public float getAimVertical(){
		return AimVertical;
	}
	public float getAimHorizontal(){
		return AimHorizontal;
	}
    void Crowch(bool _Crowch)
    {
        Anim.SetBool("Crowching", _Crowch);
    }

	void UnitMovement( Vector3 _moveMent,float Speed)
	{
        speed = (Mathf.Abs(_moveMent.z) + Mathf.Abs(_moveMent.x)) / Speed;
        Anim.SetFloat ("UnitSpeed",speed);

        if(GetComponent<UnitCharateristics>().cover == true && speed == 0){
            Anim.SetBool("Crowching", true);
        }else{
            Anim.SetBool("Crowching", false);
        }
    }


	public void UnitAttack() // When calld fanction will enable Aim animtions to look at the targert
	{
		if (Anim.GetBool ("InRange") == false) {
			Anim.SetBool ("InRange", true);
		}
		setAim = 1;
	}

	public void UnitMove() ///When colld fanction will apply Move animtions for movement.
	{
		if (Anim.GetBool ("InRange") == true) {
			Anim.SetBool ("InRange", false);
		}
		setAim = 0;
		//Targget = null;
	}
	float AdjustRottationN(float _Angle){// Adjust euler Angles rotation form 0 , 360 to -180 , 180 

		if (_Angle > 180) { //Adjust rotation from -180 ,180 to 0 , 360 on the Y Axis.
			_Angle =_Angle - 360f;
		}
		return _Angle;
	}
	public void Fire (){
		MuzzleFlush.Play ();

	}

    public void Die()
    {
        Anim.SetBool("Is_Dead", true);
    }

}
