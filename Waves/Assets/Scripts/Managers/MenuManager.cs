using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager instance;
    [Header("Menus")]
    public GameObject FirstMenu;
    public GameObject ChoosePlayerModeMenu;
    public GameObject PVsPSettings;
    public GameObject PVsAiSettings;
    public GameObject MapSelectionPvP;
    public GameObject MapSelectionPvE;
    public GameObject CurrentTabOpenOnCustomizePlayer1;
    public GameObject CurrentTabOpenOnCustomizePlayer2;
    [Header("Mapas")]
    public GameObject[] Mapas;
    [Header("Textos")]
    public TextMeshProUGUI errorText;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        FirstMenu.SetActive(true);

        ChoosePlayerModeMenu.SetActive(false);
        PVsPSettings.SetActive(false);
        PVsAiSettings.SetActive(false);
        MapSelectionPvE.SetActive(false);
        MapSelectionPvP.SetActive(false);
    }

    #region ...[Scene Loads]...
    public bool TemTeclasRepetidas(KeyCode[] a, KeyCode[] b)
    {
        System.Collections.Generic.HashSet<KeyCode> usadas = new System.Collections.Generic.HashSet<KeyCode>();

        foreach (KeyCode k in a)
        {
            if (k != KeyCode.None && !usadas.Add(k))
                return true;
        }

        foreach (KeyCode k in b)
        {
            if (k != KeyCode.None && !usadas.Add(k))
                return true;
        }

        return false;
    }
    IEnumerator EsconderErro()
    {
        yield return new WaitForSeconds(3f);
        errorText.gameObject.SetActive(false);
    }
    public void LoadPlayerVsPlayerMode()
    {
        if (TemTeclasRepetidas(DataManager.instance.P1MovementControls, DataManager.instance.P1ShootingControls) ||
            TemTeclasRepetidas(DataManager.instance.P2MovementControls, DataManager.instance.P2ShootingControls))
        {
            errorText.text = "Existem teclas repetidas!\nAltere-as para começar a jogar";
            errorText.gameObject.SetActive(true);
            StartCoroutine(EsconderErro());
            return;
        }
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
    }
    public void LoadPlayerVsIAMode()
    {
        if (TemTeclasRepetidas(DataManager.instance.P1MovementControls, DataManager.instance.P1ShootingControls))
        {
            errorText.text = "Existem teclas repetidas!";
            errorText.gameObject.SetActive(true);
            StartCoroutine(EsconderErro());
            return;
        }
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
        SceneManager.LoadScene("Game_AI_Test", LoadSceneMode.Single);
    }
    #endregion

    #region ...[Menus Hide e Show --> Spaghetti]...

    public void LoadChoosePlayerModeMenu()
    {
        ChoosePlayerModeMenu.SetActive(true);
        FirstMenu.SetActive(false);
    }
    public void HideChoosePlayerModeMenu()
    {
        FirstMenu.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
    }


    public void LoadPlayerVsPlayerSettings()
    {
        PVsPSettings.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
        DataManager.instance.IsAI = false;
    }
    public void HidePlayerVsPlayerSettings()
    {
        PVsPSettings.SetActive(false);
        ChoosePlayerModeMenu.SetActive(true);
    }


    public void LoadPlayerVsAiSettings()
    {
        PVsPSettings.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
        DataManager.instance.IsAI = true;
    }
    public void HidePlayerVsAiSettings()
    {
        PVsPSettings.SetActive(false);
        ChoosePlayerModeMenu.SetActive(true);
    }


    public void ShowMapChoosingPvP()
    {
        PVsPSettings.SetActive(false);
        MapSelectionPvP.SetActive(true);
    }
    public void HideMapChoosingPvP()
    {
        PVsPSettings.SetActive(true);
        MapSelectionPvP.SetActive(false);
        Image mapChoosingImage = GameObject.Find("CanvaMenu/PVsPSettings/MapChoosing").GetComponent<Image>();
        Sprite previewSprite = DataManager.instance.MapaEscolhido.transform.Find("Preview").GetComponent<SpriteRenderer>().sprite;

        mapChoosingImage.sprite = previewSprite;
    }


    public void ShowMapChoosingPvE()
    {
        PVsAiSettings.SetActive(false);
        MapSelectionPvE.SetActive(true);
    }
    public void HideMapChoosingPvE()
    {
        PVsPSettings.SetActive(true);
        MapSelectionPvP.SetActive(false);
        Image mapChoosingImage = GameObject.Find("CanvaMenu/PVsPSettings/MapChoosing").GetComponent<Image>();
        Sprite previewSprite = DataManager.instance.MapaEscolhido.transform.Find("Preview").GetComponent<SpriteRenderer>().sprite;

        mapChoosingImage.sprite = previewSprite;
    }

    public void ShowHatsChoosing()
    {

    }
    public void ShowControlsEditing()
    {
        
    }
    public void ShowShirtsChoosing()
    {
        
    }

    #endregion


}
