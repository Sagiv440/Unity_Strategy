using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour {

	public GameObject Turret;
	public GameObject MainGun;
	[Range(0.1f,10.0f)]
	public float RotationSpeed = 1;
	public int setAim = 0;
	private Quaternion Origio;


	public ParticleSystem MuzzleFlush;
	private UnitController Unit;

	// Use this for initialization
	void Awake () {
		Unit = GetComponent<UnitController> ();
		Unit.UnitMove += UnitMove;
		Unit.UnitFire += Fire;
		Unit.UnitAttack += UnitAttack;
	}

	void Update () {

		switch (setAim) {

		case 0:
			if (Turret.transform.localRotation != Quaternion.identity) {
				Turret.transform.localRotation = Quaternion.Lerp (Turret.transform.localRotation, Quaternion.identity, Time.deltaTime*RotationSpeed);
			}
			break;
		case 1:
			if (Unit.getEnemyInRagne() != null) {
				Vector3 Target = Unit.getEnemyInRagne().transform.position - this.transform.position;
				Quaternion lockRotation = Quaternion.LookRotation (Target);
				Turret.transform.rotation = Quaternion.Lerp (Turret.transform.rotation, lockRotation, Time.deltaTime*RotationSpeed);
			}
			break;

		}
		
	}
	public void UnitAttack() // When calld fanction will enable Aim animtions to look at the targert
	{
		setAim = 1;
	}

	public void UnitMove() ///When colld fanction will apply Move animtions for movement.
	{
		setAim = 0;
	}
	public void Fire (){
		MuzzleFlush.Play ();

	}
}
