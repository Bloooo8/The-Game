using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShowDialogueOption : DTAction
{

    public string nameOfOption;
    int index = 0;
    TextAsset[] asset=new TextAsset[4];

    public ShowDialogueOption(string name)
    {
        this.nameOfOption = name;
        
        asset = Resources.LoadAll<TextAsset>("Dialogues/" + nameOfOption);
        
    }

    public override void DoSomething()
    {

        GameObject dialWin = GameObject.Find("DialogueWindow");
        if (dialWin != null)
        {
            Text[] texts = dialWin.GetComponentsInChildren<Text>();

            for (index = 0; index < texts.Length - 1; index++)
            {
                texts[index].text = string.Empty;
              

            }

            for (index = 0; index < asset.Length ; index++)
            {
                texts[index].text = asset[index].text;
               // Debug.Log(asset[index].text);

            }
        }
        
        
        









    }
}
