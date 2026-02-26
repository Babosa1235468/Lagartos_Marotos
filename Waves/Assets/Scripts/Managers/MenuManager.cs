using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject FirstMenu;
    public GameObject ChoosePlayerModeMenu;
    
    void Start()
    {
        FirstMenu.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
    }
    public void LoadPlayerVsPlayerMode()
    {
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
    }
    public void LoadChoosePlayerModeMenu()
    {
        ChoosePlayerModeMenu.SetActive(true);
        FirstMenu.SetActive(false);
    }
    public void LoadPlayerVsIAMode()
    {
        SceneManager.LoadScene("Game_AI_Test", LoadSceneMode.Single);
    }
    public void HideChoosePlayerModeMenu()
    {
        FirstMenu.SetActive(true);
        ChoosePlayerModeMenu.SetActive(false);
    }
}
