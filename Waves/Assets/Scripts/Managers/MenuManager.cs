using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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

    [Header("Mapas")]
    public GameObject[] Mapas;

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

    public void LoadPlayerVsPlayerMode()
    {
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
    }
    public void LoadPlayerVsIAMode()
    {
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
    }
    public void HidePlayerVsPlayerSettings()
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
        gameObject.transform.Find("CanvaMenu/PVsPSettigs/MapChoosing").GetComponent<Image>().sprite = DataManager.instance.MapaEscolhido.transform.Find("Preview").GetComponent<Sprite>();
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
    }
    
    #endregion


}
