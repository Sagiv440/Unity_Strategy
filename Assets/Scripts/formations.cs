using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class formations : MonoBehaviour {

	public List<Vector3> UnitPosition;
	public Vector3 Origin = Vector3.zero;
	public Vector3 Carret = Vector3.zero;
	public int UnitCount;
	string stir; 

    public formations()
    {
        UnitPosition = new List<Vector3>();
        UnitCount = 0;
    }

	List<Vector3> getUnitPosition()
	{
		return UnitPosition;
	}
	public void setUnitPosition( List<Vector3> _list)
	{
		UnitPosition = new List<Vector3>();
		_list.Equals(UnitPosition);
	}
	public void CraetFormation (Vector3 _Origin, int Count, int type, float angule)
	{
		stir = ("");
		Origin = Vector3.zero;
		UnitPosition = new List<Vector3> ();
		switch (type) {
		case 0:
			for (int i = 0; i < Count; i++) {
				if (i == 0) {
					UnitPosition.Add (Origin);
					Carret = Origin;
				} else {
					Carret.x = Carret.x + 7;
					UnitPosition.Add (Carret);

				}
			}
			for (int i = 0; i < Count; i++) {
				UnitPosition[i] = rotationTransform( angule - Mathf.PI/2, UnitPosition[i], _Origin);
				stir +=Carret;
			}

			break;

            case 1:
                for (int i = 0; i < Count; i++)
                {
                    if (i == 0)
                    {
                        UnitPosition.Add(_Origin);
                        Carret = Origin;
                    }
                    else
                    {
                        Carret =  rotationTransform(angule - Mathf.PI / 2,Random.insideUnitCircle * 100, _Origin);
                        Carret.y = _Origin.y;
                        UnitPosition.Add(Carret);

                    }
                }
                /*for (int i = 0; i < Count; i++)
                {
                    UnitPosition[i] = rotationTransform(angule - Mathf.PI / 2, UnitPosition[i], _Origin);
                    stir += Carret;
                }*/
                break;

		}
		//Debug.Log (stir );
	}
	Vector3 rotationTransform(float angule , Vector3 origin , Vector3 _Location)
	{
		Vector3 carrent;

		carrent.x = origin.x * Mathf.Cos (angule) + origin.z * Mathf.Cos (angule + (Mathf.PI) / 2) + _Location.x;
		carrent.z = origin.x * Mathf.Sin (angule) + origin.z * Mathf.Sin (angule + (Mathf.PI) / 2) + _Location.z;
		carrent.y = origin.y;

		return carrent;
	}

}