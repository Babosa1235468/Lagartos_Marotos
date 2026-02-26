using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Image = UnityEngine.UI.Image;

public class PlayerManager : MonoBehaviour
{
    [Header("PlayerScripts")]
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    void Awake()
    {
        
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerShooting = gameObject.GetComponent<PlayerShooting>();

        
    }
    // -------------------- Main Functions --------------------

    void Update()
    {
        playerMovement.UpdateHealthBar();
        if (!playerShooting.isReloading)
        {
            playerShooting.HandleTryShooting();
            playerShooting.HandleTryReloading(); 
        }
        
        if (!playerMovement.isDead)
        {
            playerMovement.HandleJump();
            playerMovement.HandleDropThrough();
            playerMovement.HandleMovement();
            playerMovement.HandleFalling();
        }
    }
}
