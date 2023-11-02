using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Transform player;
    public RectTransform restart;
    public GameObject menu;
    public GameObject resumeButton;
    public GameObject exitButton;
    public GameObject settings;
    public GameObject message;
    public HideButtons hideButtons;
    public bool isLose = false;

    private Vector2 startPosRestart;
    private TextMeshProUGUI messageTextMesh;

     void Start()
    {
        messageTextMesh = message.GetComponent<TextMeshProUGUI>();
        startPosRestart = restart.position;
    }

    void Update()
    {
        if (player.localScale.x <= 1 && isLose == false)
        {
            StartCoroutine(YouLose());
        }
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void Pause()
    {
        Time.timeScale = 0;
        hideButtons.HideButtonss();
    }

    public void Resume()
    {
        Time.timeScale = 1;
        hideButtons.ShowButtons();
    }

    public IEnumerator YouWin()
    {
        yield return new WaitForSeconds(3f);

        menu.SetActive(true);
        resumeButton.SetActive(false);
        exitButton.SetActive(true);
        restart.position = startPosRestart;

        messageTextMesh.text = "VICTORY";
        messageTextMesh.color = new Color(0,255,0);

        Pause();

        yield return null;
    }

    public IEnumerator YouLose()
    {
        isLose = true;

        menu.SetActive(true);
        menu.GetComponent<Image>().enabled = false;
        restart.position = new Vector2(startPosRestart.x, message.transform.position.y - 300);
        resumeButton.SetActive(false);
        exitButton.SetActive(false);
        settings.SetActive(false);

        messageTextMesh.text = "Low energy";
        messageTextMesh.fontSize = 130;
        messageTextMesh.color = new Color(255, 0, 0);

        yield return null;
    }
}
