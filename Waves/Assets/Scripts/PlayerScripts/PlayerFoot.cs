using UnityEngine;

public class PlayerFoot : MonoBehaviour
{
    
    private PlayerMovement playerMovement;
    void Awake()
    {
        playerMovement = gameObject.transform.parent.GetComponent<PlayerMovement>();
    }
    // -------------------- Collision --------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if(playerMovement.rb.linearVelocityY == 0)
            {
                playerMovement.jumpsLeft = playerMovement.maxJumps;
                playerMovement.isInAir = false;
                playerMovement.col.isTrigger = false;
            }
        }
    }
}
