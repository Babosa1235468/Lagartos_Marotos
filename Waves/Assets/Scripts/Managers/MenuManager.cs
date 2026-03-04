using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [Header("Menus")]
    public GameObject FirstMenu;
    public GameObject ChoosePlayerModeMenu;
    public GameObject PVsPSettings;
    public GameObject PVsAiSettings;
    public GameObject MapSelection;

    [Header("Mapas")]
    public GameObject[] Mapas;

     void Start()
    {
        FirstMenu.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
        PVsPSettings.SetActive(false);
        PVsAiSettings.SetActive(false);
        MapSelection.SetActive(false);
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
        MapSelection.SetActive(true);
    }
    public void HideMapChoosingPvP()
    {
        PVsPSettings.SetActive(true);
        MapSelection.SetActive(false);
    }
    public void ShowMapChoosingPvE()
    {
        PVsAiSettings.SetActive(false);
        MapSelection.SetActive(true);
    }
    public void HideMapChoosingPvE()
    {
        PVsAiSettings.SetActive(true);
        MapSelection.SetActive(false);
    }
}
