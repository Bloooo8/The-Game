using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour
{

    DTNode dialogTree;
    public Transform player;
    DialogueManager manager;
    // Use this for initialization
    void Start()
    {

        manager = GameObject.Find("UI").GetComponent<DialogueManager>();
        dialogTree = new DecideDialogOption(50,new ShowDialogueOption("D1O1"),
            new ShowDialogueOption("D1O2"),
            new ShowDialogueOption("D1O3"));

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < 2.0f)
        {
            manager.ShowWindow();


            dialogTree.Decide();

        }
            
    }
}
