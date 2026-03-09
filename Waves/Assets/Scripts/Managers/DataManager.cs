using UnityEngine;

public class DataManager : MonoBehaviour
{

    public static DataManager instance;
    [Header("Mapa Escolhido")]
    public GameObject MapaEscolhido;


    [Header("Player 1 Costumization")]
    public Color P1SpriteColor;
    public SpriteRenderer P1Chapeu;
    public SpriteRenderer P1Shirt;


    [Header("Player 2 Costumization")]
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
    }
    public void ChangeMap(GameObject Mapa)
    {
        MapaEscolhido = Mapa;
    }
}