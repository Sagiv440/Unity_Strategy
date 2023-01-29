using System.Collections;
using System.Collections.Generic;
using Gamekit3D;
using UnityEngine;


public class EnemyDetecter : MonoBehaviour {

	private UnitController Unit;
	private float timer ;
	[Range(0.0f, 3.0f)]
	public float RefrachTime = 0.0f;
	public float heightOffset = 0.0f;
	public float detectionRadius = 10;
	public float detectionAngle = 270;
    public float detectionDis = 10;
    public float maxHeightDifference = 1.0f;
	public LayerMask viewBlockerLayerMask;
	public SphereCollider sphereCollider;


	bool canSee; 
	bool inSight;
	public float View;

	void Start()
	{
		Unit = GetComponentInParent<UnitController> ();
		detectionAngle = Unit.AngleOfView;
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == Unit.getEnemyTag ()) {
            Unit.setEnemyInRagne (Detect (Unit.gameObject, other.gameObject,detectionAngle, false));
			Debug.Log (Detect (Unit.gameObject, other.gameObject,detectionAngle, false));
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.tag == Unit.getEnemyTag ()) {
			if (Unit.getEnemyInRagne () == other.gameObject) {
               // other.gameObject.GetComponent<UnitController>().visbye.Remove(this.tag);
                Unit.setEnemyInRagne (null);
			}
			if (Unit.getAEnemyInRagne () != null) {
                Unit.setAEnemyInRagne (null);
			}
		} 
	}
	//--------------------------------------------------------------------------------------------------------------------------------m
	//this function is the main function of the class 
	//It uses the biuld in OnTriggerStay function to detect the What Enemy Type GameObjects are in the arriea of the associated Unit
	//and a priy assined logic to choce what emeny GameObject to attack.


	void OnTriggerStay(Collider other)
	{
		/*
		if (other.tag == Unit.getEnemyTag ()) {
			Unit.inSight = true; // if Enemy is in Sight 
			timer += Time.deltaTime;

			if (timer > RefrachTime) {
				
				Unit.setAEnemyInRagne(Detect (this.gameObject, other.gameObject,360.0f, true));
				//newEne = Detect (this.gameObject, other.gameObject, detectionAngle, false);

				if (Unit.getTEnemy() == null) { // if Player or AI have not specify an Enemy use this mefeteds
					
					if (Unit.getEnemyInRagne () == null) { // If Enemy is in range and the Unit has no privias enemy the return Enemy to Unit.
						Unit.setEnemyInRagne (Detect (this.gameObject, other.gameObject, detectionAngle, false));
					}

					if (Detect (this.gameObject, Unit.getEnemyInRagne (), detectionAngle, false) == null) { // If Enemy is no longer in sight retern null to unit.
						Unit.setEnemyInRagne (null);
					}

				}
                else {// If Unit have a specific Enemy to lock for 
					
					if (Unit.getTEnemy() != Unit.getEnemyInRagne ()) { // If the last Enemy is not the specifed Enemy set as null. 
						Unit.setEnemyInRagne (null);
					}
					if (Unit.getEnemyInRagne () == null) { //if Unit detedct eneny and do not have any target in sight then then function reterns the New enemy.
						Unit.setEnemyInRagne (Detect (this.gameObject, Unit.getTEnemy().gameObject, detectionAngle, false));
					}
					if (Detect (this.gameObject, Unit.getEnemyInRagne (), detectionAngle, false) == null) { // If Enemy is no longer in sight retern null to unit.
						Unit.setEnemyInRagne (null);
					}
				}
					timer = 0;
			}
		} else {
            // this statment is if the Enemy is dead to stop detedect him. 
            if (Unit.getEnemyInRagne() != null) 
            {
                if (Unit.getEnemyInRagne().tag != Unit.getEnemyTag())
                {
                    Unit.setEnemyInRagne(null);
                }
            }
			Unit.inSight = false; //if Enemy is noit in Sight
		}
        */
        DetectGameCollider(other ,Unit.getMainTag());
    

    }

    void DetectGameCollider(Collider other ,string _tag)
    {
        if (other.tag == _tag)
        {
            Unit.inSight = true; // if Enemy is in Sight 
            timer += Time.deltaTime;

            if (timer > RefrachTime)
            {

                Unit.setAEnemyInRagne(Detect(this.gameObject, other.gameObject, 360.0f, true));
                //newEne = Detect (this.gameObject, other.gameObject, detectionAngle, false);

                if (Unit.getTEnemy() == null)
                { // if Player or AI have not specify an Enemy use this mefeteds

                    if (Unit.getEnemyInRagne() == null)
                    { // If Enemy is in range and the Unit has no privias enemy the return Enemy to Unit.
                        Unit.setEnemyInRagne(Detect(this.gameObject, other.gameObject, detectionAngle, false));
                    }

                    if (Detect(this.gameObject, Unit.getEnemyInRagne(), detectionAngle, false) == null)
                    { // If Enemy is no longer in sight retern null to unit.
                        Unit.setEnemyInRagne(null);
                    }

                }
                else
                {// If Unit have a specific Enemy to lock for 

                    if (Unit.getTEnemy() != Unit.getEnemyInRagne())
                    { // If the last Enemy is not the specifed Enemy set as null. 
                        Unit.setEnemyInRagne(null);
                    }
                    if (Unit.getEnemyInRagne() == null)
                    { //if Unit detedct eneny and do not have any target in sight then then function reterns the New enemy.
                        Unit.setEnemyInRagne(Detect(this.gameObject, Unit.getTEnemy().gameObject, detectionAngle, false));
                    }
                    if (Detect(this.gameObject, Unit.getEnemyInRagne(), detectionAngle, false) == null)
                    { // If Enemy is no longer in sight retern null to unit.
                        Unit.setEnemyInRagne(null);
                    }
                }
                timer = 0;
            }
        }
        else
        {
            // this statment is if the Enemy is dead to stop detedect him. 
            if (Unit.getEnemyInRagne() != null)
            {
                if (Unit.getEnemyInRagne().tag != _tag)
                {
                    Unit.setEnemyInRagne(null);
                }
            }
            Unit.inSight = false; //if Enemy is noit in Sight
        }
    }

    
    //----------------------------------------------------------------------------------------------------------------------------------m

    //----------------------------------------------------------------------------------------------------------------------------------------------------------
    //This function acts as a sensor to detect if the GameObject assind to the "target" variable is in the angule relative to the "detector" GameObjcet variable.
    //----------------------------------------------------------------------------------------------------------------------------------------------------------
    public GameObject Detect(GameObject detector, GameObject target, float _detectionAngle, bool useRangeToDetect = false)
	{
		if (target == null || detector == null) {
			return null;
		}
        

		Vector3 eyePos = detector.transform.position + Vector3.up * heightOffset; 
		Vector3 toPlayer = target.transform.position - eyePos;
		Vector3 toPlayerTop = target.transform.position + Vector3.up * heightOffset - eyePos;

        

        Vector3 toPlayerFlat = toPlayer;
		toPlayerFlat.y = 0;


		if (useRangeToDetect == false) {

            Unit.set_Distans((this.transform.position - target.transform.position).sqrMagnitude);

            if (Vector3.Dot (toPlayerFlat.normalized, detector.transform.forward) >
			    Mathf.Cos (_detectionAngle * 0.5f * Mathf.Deg2Rad)) {

				canSee = false;

				Vector3 EnemyTargget = rotationTransform (this.transform.rotation.eulerAngles.y * Mathf.Deg2Rad, target.gameObject.transform.position - this.transform.position); // Culculate the reletive position of the targget form the Unit perspective.
                Quaternion LocalRotation = Quaternion.LookRotation (EnemyTargget); // Culculate the rotation Using the LookRotation function.
				float _angule = Mathf.Cos (_detectionAngle * 0.5f * Mathf.Deg2Rad);

				Unit.setVertical (LocalRotation.eulerAngles.x); //the  angle of the enemy relevent to the unit.
				Unit.setHorizontal (LocalRotation.eulerAngles.y); //the Horizontal angle of the enemy relevent to the unit.
                
                

				Debug.DrawRay (eyePos, toPlayer, Color.blue);
				Debug.DrawRay (eyePos, toPlayerTop, Color.blue);

				/*canSee |= !Physics.Raycast (eyePos, toPlayer.normalized, detectionRadius,
					viewBlockerLayerMask, QueryTriggerInteraction.Ignore);*/

				canSee |= !Physics.Raycast (eyePos, toPlayerTop.normalized, toPlayerTop.magnitude,
					viewBlockerLayerMask, QueryTriggerInteraction.Ignore);

				if (canSee)
					return target;
			}
			return null;
		} else {

			canSee = false;

			Debug.DrawRay (eyePos, toPlayer, Color.blue);
			Debug.DrawRay (eyePos, toPlayerTop, Color.blue);

			/*canSee |= !Physics.Raycast (eyePos, toPlayer.normalized, detectionRadius,
				viewBlockerLayerMask, QueryTriggerInteraction.Ignore);*/

			canSee |= !Physics.Raycast (eyePos, toPlayerTop.normalized, toPlayerTop.magnitude,
				viewBlockerLayerMask, QueryTriggerInteraction.Ignore);

			if (canSee) {
				return target;
			}
		
			return null;
		}
	}

	//this function tack a Globle Vector and return a transformd Vector bacist on the angule.

	Vector3 rotationTransform(float angule , Vector3 origin)
	{
		Vector3 carrent;

		carrent.x = origin.x * Mathf.Cos (angule) + origin.z * Mathf.Cos (angule + (Mathf.PI) / 2);
		carrent.z = origin.x * Mathf.Sin (angule) + origin.z * Mathf.Sin (angule + (Mathf.PI) / 2);
		carrent.y = origin.y;

		return carrent;
	}
}
