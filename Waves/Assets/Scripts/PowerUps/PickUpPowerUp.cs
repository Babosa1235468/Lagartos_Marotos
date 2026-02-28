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

        //buscar o powerUp script para encontrar o respetivo tipo
        PowerUp powerUp = GetComponentInParent<PowerUp>();
        if (powerUp == null)
        {
            powerUp = GetComponentInParent<PowerUp>();
        }
        ApplyPowerUpEffect(powerUp.type);
        Destroy(transform.parent.gameObject);
    }
    public void ApplyPowerUpEffect(PowerUp.PowerUpType type)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.SpeedBoost:
                if(!playerMovement.SpeedEffectOn)
                {
                    playerMovement.ApplySpeedBoost(1.5f, 6f);
                }
                
                break;

            case PowerUp.PowerUpType.JumpBoost:
                if(!playerMovement.JumpEffectOn)
                {
                    playerMovement.ApplyJumpBoost(1.5f, 6f);
                }
                break;

            case PowerUp.PowerUpType.DamageBoost:
                playerShooting.ApplyDamageBoost(2f, 4f);
                break;

            case PowerUp.PowerUpType.Invulnerability:
                if(!playerMovement.isInvincible)
                {
                    playerMovement.ApplyInvulnerability(4f);
                }
                break;

            case PowerUp.PowerUpType.MoreHealth:
                playerMovement.AddMaxHealth(1);
                break;
            case PowerUp.PowerUpType.Heal:
                playerMovement.HealPlayerBackToFullHp();
                break;
        }
    }
    
}
