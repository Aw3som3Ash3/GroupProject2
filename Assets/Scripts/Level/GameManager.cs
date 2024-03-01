using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject Pause;
    public GameObject Quit;
    public PlayerController player;
    public GameObject Win;
    public Button[] buttons;

    public bool paused;
    public bool quitMenu;

    public bool QuitMenu
    {
        get { return quitMenu; }
        set { quitMenu = value; }
    }

    private void Start(){
        Pause.SetActive(false);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void OnPause(){
        paused = !paused;
        if (quitMenu)
        {
            Quit.SetActive(false);
            quitMenu = false;
            foreach (Button currBtn in buttons)
            {
                currBtn.interactable = true;
            }
        }
        if(paused)
        {
            Pause.SetActive(true);
            Time.timeScale =0;
            canvas.enabled = true;
        } else {
            Time.timeScale = 1;
            Pause.SetActive(false);
        }
    }
    public void Reload()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainLevel");
    }
    public void OnOptions(){
        Debug.Log("Not Implemented");
    }
    public void OnQuit(){
        Application.Quit();
        Debug.Log("EndGame");
    }

    public void LoadLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(5f);
        Win.SetActive(true);
        Time.timeScale = 0f;
        player.GetComponent<PlayerController>().actions.Disable();
    }

    
}
