using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour {

	public void TurnOff()
    {
        DialogueManager manager= GameObject.Find("UI").GetComponent<DialogueManager>();
        manager.option1 = "D1O1";
        manager.option2 = "D1O2";
        manager.option3 = "D1O3";

        gameObject.SetActive(false);
        Cursor.visible = false;

    }
}
