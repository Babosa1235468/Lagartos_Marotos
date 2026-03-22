using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public GameObject MapaDefault;
    public Sprite MapaDefaultSprite;
    public static DataManager instance;
    [Header("Mapa Escolhido")]
    public GameObject MapaEscolhido;
    [Header("Modo Escolhido")]
    public bool IsAI;
    [Header("Player Costumization Default")]
    public Sprite defaultMouth;

    [Header("Player 1 Costumization")]
    public string P1Name = "Jogador 1";
    public Color P1SpriteColor;
    public Sprite P1Chapeu;
    public Sprite P1Mouth;


    [Header("Player 2 Costumization")]
    public string P2Name = "Jogador 2";
    public Color P2SpriteColor;
    public Sprite P2Chapeu;
    public Sprite P2Mouth;


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
        if(GameObject.FindGameObjectWithTag("MapSelect") != null)
        {
            GameObject.FindGameObjectWithTag("MapSelect").GetComponent<Image>().sprite = MapaDefaultSprite;
        }
        P1Name = "Jogador 1";
        P2Name = "Jogador 2";
        //as cores vao ser default 
        P1Chapeu = null;
        P1Mouth = defaultMouth;

        //cores vao ser difault
        P2Chapeu = null;
        P2Mouth = defaultMouth;

        // Controls
        P1MovementControls = new KeyCode[4] { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        P1ShootingControls = new KeyCode[2] { KeyCode.T, KeyCode.R };

        P2MovementControls = new KeyCode[4] { KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow };
        P2ShootingControls = new KeyCode[2] { KeyCode.Keypad1, KeyCode.Keypad2 };

        // Game settings
        UsingPowerUps = true;

        P1SpriteColor = new Color(0.86f,0.1f,0.1f,1);
        P2SpriteColor = new Color(1,0.5f,0,1);

        ChangeSpritesToDefault();
    }

    private void ChangeSpritesToDefault()
    {
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player1"))
        {
            foreach (Transform child in player.transform.Find("Sprites"))
            {
                if (!child.CompareTag("DoNotChange"))
                {
                    if (child.TryGetComponent(out SpriteRenderer sr))
                        sr.color = P1SpriteColor;
                }
            }
            player.transform.Find("Sprites/Mouth").GetComponent<SpriteRenderer>().sprite = defaultMouth;
            player.transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = null;
        }
        foreach (GameObject player in GameObject.FindGameObjectsWithTag("Player2"))
        {
            foreach (Transform child in player.transform.Find("Sprites"))
            {
                if (!child.CompareTag("DoNotChange"))
                {
                    if (child.TryGetComponent(out SpriteRenderer sr))
                        sr.color = P2SpriteColor;
                }
            }
            player.transform.Find("Sprites/Mouth").GetComponent<SpriteRenderer>().sprite = defaultMouth;
            player.transform.Find("Sprites/Hat").GetComponent<SpriteRenderer>().sprite = null;
        }
    }
}