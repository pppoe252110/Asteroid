using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private Text mouseKeyboardText;
    [SerializeField] private Transform menu;
    [SerializeField] private Transform resumeButton;
    public static bool mouseInput = false;
    private bool gamePaused = false;
    private bool gameStarted = false;

    private void Start()
    {
        PlayerHealth.onPlayerDead.AddListener(delegate { SwitchResumeAvailible(); PauseGame(); });
        PauseGame();
        SwitchResumeAvailible();
        UpdateInput();
    }

    public void SwitchResumeAvailible()
    {
        resumeButton.gameObject.SetActive(!resumeButton.gameObject.activeSelf);
    }

    public void Resume()
    {
        gamePaused = false;
        Time.timeScale = 1;
        menu.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
        menu.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void NewGame()
    {
        if (gameStarted)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SwitchResumeAvailible();
            gameStarted = true;
            Resume();
        }
    }
    public void SwitchInput()
    {
        mouseInput = !mouseInput;
        UpdateInput();
    }

    private void UpdateInput()
    {
        if (mouseInput)
        {
            mouseKeyboardText.text = "Mouse";
        }
        else
        {
            mouseKeyboardText.text = "Keyboard";
        }
    }

    public void Exit()
    {
        Application.Quit();
    }
}
