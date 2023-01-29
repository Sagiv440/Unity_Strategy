using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class coverMachnice : MonoBehaviour {
    private string Ground = "PlayGround";


    void OnTriggerEnter(Collider other)
    {
        if (other.tag != Ground)
        {
            if (other.gameObject.GetComponent<UnitCharateristics>() != null)
            {
                other.gameObject.GetComponent<UnitCharateristics>().cover = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag != Ground)
        {
            other.gameObject.GetComponent<UnitCharateristics>().cover = false;
        }
    }
}
