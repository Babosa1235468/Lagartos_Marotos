using System.Xml.XPath;
using UnityEngine;
using UnityEngine.UI;

public class PickUpPowerUp : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    void OnTriggerEnter2D(Collider2D collision)
    {
        playerMovement = collision.GetComponent<PlayerMovement>();
        playerShooting = collision.GetComponent<PlayerShooting>();
        // caso toque em outras cenas
        if (playerMovement == null || playerShooting == null ) return;

        //buscar ao pai o powerUp script para ir buscar o respetivo powerUp
        PowerUp powerUp = GetComponent<PowerUp>();
        if (powerUp == null)
        {
            powerUp = GetComponentInParent<PowerUp>();
        }
        ApplyPowerUpEffect(powerUp.type);
        Destroy(gameObject);
    }
    public void ApplyPowerUpEffect(PowerUp.PowerUpType type)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.SpeedBoost:
                playerMovement.ApplySpeedBoost(1.5f, 6f);
                break;

            case PowerUp.PowerUpType.JumpBoost:
                playerMovement.ApplyJumpBoost(1.3f, 6f);
                break;

            case PowerUp.PowerUpType.DamageBoost:
                playerShooting.ApplyDamageBoost(2f, 4f);
                break;

            case PowerUp.PowerUpType.Invulnerability:
                playerMovement.ApplyInvulnerability(4f);
                break;

            case PowerUp.PowerUpType.MoreHealth:
                playerMovement.AddMaxHealth(1);
                break;
        }
    }
    
}
