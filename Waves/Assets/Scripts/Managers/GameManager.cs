using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Analytics;
public class GameManager : MonoBehaviour
{
    // PAUSE MENU VARIABLES
    public GameObject dataManager;
    public GameObject pauseGameMenu;
    public GameObject gameOverScreen;
    public TextMeshProUGUI nomeJogador;
    public GameObject Mapa;
    public static GameManager instance;
    public bool isPaused = false;


    // int currentLevel = 1;
    // GameState gameState;

    private void Awake()
    {
        dataManager = GameObject.Find("DataManager");
        instance = this;
        pauseGameMenu.SetActive(false);
        gameOverScreen.SetActive(false);

        //ir buscar o mapa e colocar o gameobject escolhido pelo player
        Mapa = GameObject.Find("Map/MapToLoad");
        foreach (Transform child in Mapa.transform)
        {
            Destroy(child.gameObject);
        }
        Instantiate(DataManager.instance.MapaEscolhido, Mapa.transform);
        
        nomeJogador = gameOverScreen.transform.Find("GameOverMenu/PlayerWhoWon").GetComponent<TextMeshProUGUI>();

        //resetar os vertices do pwoerup e path finding
        PowerUpManager pum = GameObject.Find("PowerUpManager").GetComponent<PowerUpManager>();
        pum.UpdateVertices();

        if (DataManager.instance.IsAI)
        {
            
        }
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
        DataManager.instance.ResetVariables();
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
}