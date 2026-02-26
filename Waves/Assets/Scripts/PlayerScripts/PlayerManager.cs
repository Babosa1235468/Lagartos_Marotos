using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [Header("PlayerScripts")]
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    void Awake()
    {
        
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerShooting = gameObject.GetComponent<PlayerShooting>();

        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        int x = 0;
        string LayerUIName = string.Empty;
        foreach (GameObject p in Players)
        { 
            if (p != gameObject)
            {
                x++;
                playerMovement.otherPlayerCol = p.GetComponent<Collider2D>();
                LayerUIName = LayerMask.LayerToName(gameObject.layer) + "_UI";
                Debug.Log(LayerUIName);
            }
        }
        
        playerMovement.playerShooting = GetComponent<PlayerShooting>();
        
    }
    // -------------------- Main Functions --------------------
    void Start()
    {
        playerShooting.reloadBar.enabled = false;
        playerShooting.remainingBullets = playerShooting.magazineSize;
        playerShooting.bulletsTxt.text = $"{playerShooting.remainingBullets}";

        playerMovement.ResetVariables();
        playerMovement.Spawn();
        Time.timeScale = 1f;
        if (playerMovement.otherPlayerCol != null)
        {
            Physics2D.IgnoreCollision(playerMovement.col, playerMovement.otherPlayerCol, true);
        }
            

    }

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
