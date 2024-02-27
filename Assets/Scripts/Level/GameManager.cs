using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    public Image Pause;
    public Image Quit;
    public PlayerController player;

    public bool paused;
    public bool quitMenu;

    private void Start(){
        canvas.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnPause(){
        paused = !paused;
        if(paused){
            Time.timeScale = 0;
            canvas.enabled = true;
        } else {
            Time.timeScale = 1;
            canvas.enabled = false;
        }
    }
    public void Reload(){
        SceneManager.LoadScene("MainLevel");
    }
    public void OnOptions(){
        Debug.Log("Not Implemented");
    }
    public void OnQuitMenu(){
        quitMenu = !quitMenu;
        if(quitMenu){
            Quit.enabled = true;
            for(int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                Pause.gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }
        } else{
            Quit.enabled = false;
            for(int i = 0; i < this.gameObject.transform.childCount; i++)
            {
                Pause.gameObject.transform.GetChild(i).gameObject.SetActive(true);
            } 
        }
    }
    public void OnQuit(){
        Application.Quit();
    }
}
