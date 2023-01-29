 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class UnitMenu : MonoBehaviour {

	public List<GameObject> Objects;
	public List<Button> Buttons;

	void Awake (){
		for (int i = 0; i < Objects.Count; i++) {
			Objects [i].SetActive (false);
			Buttons [i] = Objects [i].GetComponent<Button> ();
		}
	}
	public void ActivatButtons(Interface _Inter){
		
		for (int i = 0; i < Objects.Count; i++) {
			Objects [i].SetActive (false);
		}
		for (int i = 0; i < _Inter.Buttons.Count && i < 12; i++) {
			Objects [_Inter.Buttons[i]].SetActive (true);
			Objects [_Inter.Buttons [i]].GetComponentInChildren<TextMeshProUGUI>().text = _Inter.ButtonText[i];
			Buttons [_Inter.Buttons [i]].onClick.RemoveAllListeners();
			Buttons [_Inter.Buttons [i]].onClick.AddListener(_Inter.Event [i].Invoke);
		}

	}

}

