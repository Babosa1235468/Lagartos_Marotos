using System.Collections;
using System.Collections.Generic;
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
    public TextMeshProUGUI PvPerrorText;
    public TextMeshProUGUI PvEerrorText;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

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

    public void CloseGame()
    {
        // will only work on build
        Application.Quit();
        // para o editor
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void ChangeMap(GameObject Mapa)
    {
        DataManager.instance.MapaEscolhido = Mapa;
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
    IEnumerator EsconderErroPvP()
    {
        yield return new WaitForSeconds(3f);
        PvPerrorText.gameObject.SetActive(false);
    }
    IEnumerator EsconderErroPvE()
    {
        yield return new WaitForSeconds(3f);
        PvEerrorText.gameObject.SetActive(false);
    }
    public void LoadPlayerVsPlayerMode()
    {
        if (TemTeclasRepetidas(DataManager.instance.P1MovementControls, DataManager.instance.P1ShootingControls) ||
            TemTeclasRepetidas(DataManager.instance.P1MovementControls, DataManager.instance.P2ShootingControls) ||
            TemTeclasRepetidas(DataManager.instance.P1MovementControls, DataManager.instance.P2MovementControls) ||
            TemTeclasRepetidas(DataManager.instance.P1ShootingControls, DataManager.instance.P2ShootingControls) ||
            TemTeclasRepetidas(DataManager.instance.P2ShootingControls, DataManager.instance.P2MovementControls) ||
            TemTeclasRepetidas(DataManager.instance.P1ShootingControls, DataManager.instance.P2MovementControls))
        {
            PvPerrorText.text = "Existem teclas repetidas!\nAltere-as para começar a jogar";
            PvPerrorText.gameObject.SetActive(true);
            StartCoroutine(EsconderErroPvP());
            return;
        }
        DataManager.instance.P1Name = GameObject.FindGameObjectWithTag("P1NamePvP").GetComponent<TextMeshProUGUI>().text;
        DataManager.instance.P2Name = GameObject.FindGameObjectWithTag("P2NamePvP").GetComponent<TextMeshProUGUI>().text;
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
    }
    public void LoadPlayerVsIAMode()
    {
        if (TemTeclasRepetidas(DataManager.instance.P1MovementControls, DataManager.instance.P1ShootingControls))
        {
            PvEerrorText.text = "Existem teclas repetidas!\nAltere-as para começar a jogar";
            PvEerrorText.gameObject.SetActive(true);
            StartCoroutine(EsconderErroPvE());
            return;
        }
        DataManager.instance.P1Name = GameObject.FindGameObjectWithTag("P1NamePvE").GetComponent<TextMeshProUGUI>().text;
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
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
        
        DataManager.instance.ResetVariables();
        PVsPSettings.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
        DataManager.instance.IsAI = false;
        GameObject.FindGameObjectWithTag("P1Shoot").GetComponent<TextMeshProUGUI>().text = "T";
        GameObject.FindGameObjectWithTag("P1Reload").GetComponent<TextMeshProUGUI>().text = "R";
        GameObject.FindGameObjectWithTag("P1Jump").GetComponent<TextMeshProUGUI>().text = "W";
        GameObject.FindGameObjectWithTag("P1Down").GetComponent<TextMeshProUGUI>().text = "S";
        GameObject.FindGameObjectWithTag("P1Left").GetComponent<TextMeshProUGUI>().text = "A";
        GameObject.FindGameObjectWithTag("P1Right").GetComponent<TextMeshProUGUI>().text = "D";

        GameObject.FindGameObjectWithTag("P2Shoot").GetComponent<TextMeshProUGUI>().text = "Keypad1";
        GameObject.FindGameObjectWithTag("P2Reload").GetComponent<TextMeshProUGUI>().text = "Keypad2";
        GameObject.FindGameObjectWithTag("P2Jump").GetComponent<TextMeshProUGUI>().text = "↑";
        GameObject.FindGameObjectWithTag("P2Down").GetComponent<TextMeshProUGUI>().text = "↓";
        GameObject.FindGameObjectWithTag("P2Left").GetComponent<TextMeshProUGUI>().text = "←";
        GameObject.FindGameObjectWithTag("P2Right").GetComponent<TextMeshProUGUI>().text = "→";
    }
    public void HidePlayerVsPlayerSettings()
    {
        PVsPSettings.SetActive(false);
        ChoosePlayerModeMenu.SetActive(true);
    }


    public void LoadPlayerVsAiSettings()
    {
       

        DataManager.instance.ResetVariables();
        PVsAiSettings.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
        DataManager.instance.IsAI = true;
        GameObject.FindGameObjectWithTag("P1Shoot").GetComponent<TextMeshProUGUI>().text = "T";
        GameObject.FindGameObjectWithTag("P1Reload").GetComponent<TextMeshProUGUI>().text = "R";
        GameObject.FindGameObjectWithTag("P1Jump").GetComponent<TextMeshProUGUI>().text = "W";
        GameObject.FindGameObjectWithTag("P1Down").GetComponent<TextMeshProUGUI>().text = "S";
        GameObject.FindGameObjectWithTag("P1Left").GetComponent<TextMeshProUGUI>().text = "A";
        GameObject.FindGameObjectWithTag("P1Right").GetComponent<TextMeshProUGUI>().text = "D";
    }
    public void HidePlayerVsAiSettings()
    {
        PVsAiSettings.SetActive(false);
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
        PVsAiSettings.SetActive(true);
        MapSelectionPvE.SetActive(false);

        Image mapChoosingImage = GameObject.Find("CanvaMenu/PVsAiSettings/MapChoosing").GetComponent<Image>();
        Sprite previewSprite = DataManager.instance.MapaEscolhido.transform.Find("Preview").GetComponent<SpriteRenderer>().sprite;

        mapChoosingImage.sprite = previewSprite;
    }
    public void ShowHatsChoosing(GameObject card)
    {
       ShowTab("Hats",card);
    }
    public void ShowControlsEditing(GameObject card)
    {
        ShowTab("Controls",card);
    }
    public void ShowMouthChoosing(GameObject card)
    {
        ShowTab("Mouths",card);
    }
    private void ShowTab(string tab,GameObject card)
    {
        List<GameObject> tabs = new List<GameObject>();
        foreach (Transform child in card.transform) {
            if(child.gameObject.tag == "Tab")
            {
                tabs.Add(child.gameObject);
                
            }
        }
        
        if(tabs != null)
        {
            foreach (GameObject item in tabs)
            {
                if(item.gameObject.name == tab)
                {
                    foreach(Transform menu in item.transform)
                    {
                        if(menu.gameObject.name == "Menu")
                        {
                            menu.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    foreach(Transform menu in item.transform)
                    {
                        if(menu.gameObject.name == "Menu")
                        {
                            menu.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        
    }

    #endregion


}
