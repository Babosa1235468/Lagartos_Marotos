using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;
    [Header("Mapa Escolhido")]
    public GameObject MapaEscolhido;
    [Header("Cores dos Players")]
    public Color P1SpriteColor;
    public Color P2SpriteColor;
    [Header("Player 1 Controls")]
    public KeyCode[] P1Controls;
    [Header("Player 2 Controls")]
    public KeyCode[] P2Controls;
    [Header("Definicoes de Jogo")]
    public bool UsingPowerUps = true;

    void Awake()
    {
        //criar isto estático
        instance = this;

        // meter para nao destruir entre cenas
        DontDestroyOnLoad(gameObject);
    }
    public void ChangeMap(GameObject Mapa)
    {
        MapaEscolhido = Mapa;
    }
}