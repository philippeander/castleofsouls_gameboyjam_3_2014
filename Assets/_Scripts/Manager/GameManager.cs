using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	
	void Start () {
        InputManager.Instance.Start_BtnAction += Start_Btn;
        InputManager.Instance.Select_BtnAction += Select_Btn;
    }
	
	
	void Update () {
		
	}

    public void Start_Btn() {
        ScreenShake.Instance.Shake();
    }
    public void Select_Btn() {

    }

}
