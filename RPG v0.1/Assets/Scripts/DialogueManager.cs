using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {


    GameObject window;

    public int optionIndex;

    Button[] buttons;

    public string option1 = "D1O1", option2 = "D1O2", option3 = "D1O3";


    void Start () {


        buttons = GetComponentsInChildren<Button>();
        window =GameObject.Find("DialogueWindow");
        window.SetActive(false);
        
		
	}
    private void Update()
    {

        foreach(Button button in buttons)
        {

            if (button.GetComponentInChildren<Text>().text == string.Empty)
                button.interactable = false;
            else
                button.interactable = true;

        }
    }

    public void AssignDialogue(string nameOfDialogue)
    {
        option1 = nameOfDialogue + "/" + nameOfDialogue + "O1";
        option2 = nameOfDialogue + "/" + nameOfDialogue + "O2";
        option3 = nameOfDialogue + "/" + nameOfDialogue + "O3";

    }
    public void ShowWindow()
    {
        window.SetActive(true);
        Cursor.visible = true;
    }

    public void OnClick(int index)
    {
        if (EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text.StartsWith(" "))
        {
            GetComponentInChildren<SelfDestroy>().TurnOff();
            
        }
        else
        {
            optionIndex = index;

            option1 += "O" + index.ToString();
            option2 += "O" + index.ToString();
            option3 += "O" + index.ToString();
        }
           

        


    }
   
}
