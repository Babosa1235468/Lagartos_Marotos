using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject FirstMenu;
    public void PlayGame()
    {
        SceneManager.LoadScene("Game_Damage", LoadSceneMode.Single);
    }
    public void LoadChoosePlayerModeMenu()
    {
        Debug.Log("Tentaste esconder o menu");
        FirstMenu.SetActive(false);
    }
}
