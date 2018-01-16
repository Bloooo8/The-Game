using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {

    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if(anim!=null)
            anim.SetTrigger("Show");
        }

    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void StopTime()
    {
        Time.timeScale = 0.1f;
        Cursor.visible = true;
        anim.ResetTrigger("Show");
        anim.ResetTrigger("Hide");
    }

    public void StartTime()
    {
        Time.timeScale = 1;
        Cursor.visible = false;
        anim.ResetTrigger("Show");
        anim.ResetTrigger("Hide");

    }
}
