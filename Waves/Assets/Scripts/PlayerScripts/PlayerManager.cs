using System.Linq;
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
        if (!playerMovement.isDead)
        {
            if (!playerShooting.isReloading)
            {
                if(Input.GetKey(playerShooting.shootKey))
                {
                    playerShooting.HandleTryShooting();
                }
                if(Input.GetKeyDown(playerShooting.reloadKey))
                {
                    playerShooting.HandleTryReloading(); 
                }
            }
            
            if (Input.GetKeyDown(playerMovement.jumpKey))
            {
                playerMovement.HandleJump();
            }
            if (Input.GetKeyDown(playerMovement.downKey))
            {
                playerMovement.HandleDropThrough();
            }
            playerMovement.HandleMovement();
            playerMovement.HandleFalling();
        }
    }
}
