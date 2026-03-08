using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Analytics;
public class GameManager : MonoBehaviour
{
    // PAUSE MENU VARIABLES
    public GameObject pauseGameMenu;
    public GameObject gameOverScreen;
    public TextMeshProUGUI nomeJogador;
    public GameObject Mapas;
    public static GameManager instance;
    public bool isPaused = false;


    // int currentLevel = 1;
    // GameState gameState;

    private void Awake()
    {
        instance = this;
        pauseGameMenu.SetActive(false);
        gameOverScreen.SetActive(false);
        Mapas = GameObject.Find("Maps");
        nomeJogador = gameOverScreen.transform.Find("GameOverMenu/PlayerWhoWon").GetComponent<TextMeshProUGUI>();
    }

    // ------------------- PAUSE / CONTINUE FUNTIONS --------------------------

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ChangePauseMenuState();
        }
    }
    public void StartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
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
    public void EndGame(string PlayerWhoWonName)
    {
        gameOverScreen.SetActive(true);
        PauseGame();
       
        nomeJogador.text = $"{PlayerWhoWonName} \nGANHOU!!!";
    }

    // // ------------------- WORKS --------------
    // public int CurrentLevel()
    // {
    //     return currentLevel;
    // }
    // public void ChangeState(GameState newGameState)
    // {
    //     gameState = newGameState;
    //     HandleStateChanged(gameState);
    // }
    // private void HandleStateChanged(GameState gameState)
    // {
    //     switch (gameState)
    //     {
    //         case GameState.Playing:
    //             break;
    //     }
    // }
    // public enum GameState
    // {
    //     Playing,
    // }
}