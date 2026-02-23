using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // PAUSE MENU VARIABLES
    public GameObject pauseGameMenu;
    public static GameManager instance;
    public bool isPaused = false;


    int currentLevel = 1;
    GameState gameState;

    private void Awake()
    {
        instance = this;
    }
    // Just to prevent future 
    private void Start()
    {
        pauseGameMenu.SetActive(false);
    }

    // ------------------- PAUSE / CONTINUE FUNTIONS --------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseMenuState();
        }
    }
    public void ChangePauseMenuState()
    {
        if (pauseGameMenu.activeSelf == false)
        {
            pauseGameMenu.SetActive(true);
            PauseGame();
        }
        else
        {
            pauseGameMenu.SetActive(false);
            ContinueGame();
        }
    }
    public void ResumeGame()
    {
        ContinueGame();
        pauseGameMenu.SetActive(false);
    }
    public void QuitGame()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
    }
    private void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
    }

    // ------------------- WORKS --------------
    public int CurrentLevel()
    {
        return currentLevel;
    }
    public void ChangeState(GameState newGameState)
    {
        gameState = newGameState;
        HandleStateChanged(gameState);
    }
    private void HandleStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Playing:
                CardManager.instance.HideCardSelection();
                break;
            case GameState.CardSelecting:
                CardManager.instance.RandomizeNewCards();
                break;
        }
    }
    public enum GameState
    {
        Playing,
        CardSelecting
    }
}