using TMPro;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public GameObject MapaDefault;
    public static DataManager instance;
    [Header("Mapa Escolhido")]
    public GameObject MapaEscolhido;
    [Header("Modo Escolhido")]
    public bool IsAI;

    [Header("Player 1 Costumization")]
    public string P1Name = "Jogador 1";
    public Color P1SpriteColor;
    public SpriteRenderer P1Chapeu;
    public SpriteRenderer P1Shirt;


    [Header("Player 2 Costumization")]
    public string P2Name = "Jogador 2";
    public Color P2SpriteColor;
    public SpriteRenderer P2Chapeu;
    public SpriteRenderer P2Shirt;


    // w, a, s ,d, por ordem
    // shoot then reload

    [Header("Player 1 Controls, w ,a ,s ,d, shoot, reload")]
    public KeyCode[] P1MovementControls;
    public KeyCode[] P1ShootingControls;

    [Header("Player 2 Controls, w ,a ,s ,d, shoot, reload")]
    public KeyCode[] P2MovementControls;
    public KeyCode[] P2ShootingControls;

    [Header("Definicoes de Jogo")]
    public bool UsingPowerUps = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); // prevent duplicates
        }
        //os valores default estao colocados no inspetor
        P1SpriteColor = new Color(0.86f,0.1f,0.1f,1);
        P2SpriteColor = new Color(1,0.5f,0,1);
    }
    
    public void ResetVariables()
    {
        // Mapas e modo
        IsAI = false;
        MapaEscolhido = MapaDefault;
        P1Name = "Jogador 1";
        P2Name = "Jogador 2";
        //as cores vao ser default 
        P1Chapeu = null;
        P1Shirt = null;

        //cores vao ser difault
        P2Chapeu = null;
        P2Shirt = null;

        // Controls
        P1MovementControls = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        P1ShootingControls = new KeyCode[2] { KeyCode.T, KeyCode.R };

        P2MovementControls = new KeyCode[4] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
        P2ShootingControls = new KeyCode[2] { KeyCode.Keypad1, KeyCode.Keypad2 };

        // Game settings
        UsingPowerUps = true;
    }
}