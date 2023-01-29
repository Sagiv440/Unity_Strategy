using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Resorce : MonoBehaviour {

	public float AmountPerSec;
	public float Value;

	public TextMeshProUGUI A_PerSec;
	public TextMeshProUGUI A_Value;

    
    void Update(){
		A_PerSec.text = ((int)AmountPerSec).ToString();
		A_Value.text = ((int)Value).ToString();
	}

}
