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
    public GameObject Mapa;
    public static GameManager instance;
    public bool isPaused = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

    }
    void Start()
    {
        if (!DataManager.instance.UsingPowerUps)
        {
            GameObject.Find("PowerUpManager").gameObject.SetActive(false);
        }
        else
        {
            PowerUpManager pum = GameObject.Find("PowerUpManager").GetComponent<PowerUpManager>();
            pum.UpdateVertices();
        }

        if (DataManager.instance.IsAI)
        {
            GameObject aiPlayer = GameObject.FindGameObjectWithTag("Player2");

            if (aiPlayer != null)
            {
                PathFinding pf = aiPlayer.GetComponent<PathFinding>();
                pf.UpdateVertices();
            }
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
        Time.timeScale = 1f;
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