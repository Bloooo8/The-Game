using UnityEngine;
using System.Collections;

public class CoroutinesController : MonoBehaviour
{
    public static CoroutinesController instance;

    private CoroutinesController() { }

  
    public static CoroutinesController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new  GameObject("CoroutinesController").AddComponent<CoroutinesController>();
            }
            return instance;
        }
    }
}
