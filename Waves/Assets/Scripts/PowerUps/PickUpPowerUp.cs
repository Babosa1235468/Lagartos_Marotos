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
    }
    public void ApplyPowerUpEffect(PowerUp.PowerUpType type)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.SpeedBoost:
                if(!playerMovement.SpeedEffectOn)
                {
                    playerMovement.ApplySpeedBoost(1.5f, 6f);
                    Destroy(transform.parent.gameObject);
                }   
                break;

            case PowerUp.PowerUpType.JumpBoost:
                if(!playerMovement.JumpEffectOn)
                {
                    playerMovement.ApplyJumpBoost(1.5f, 6f);
                    Destroy(transform.parent.gameObject);
                }
                break;

            case PowerUp.PowerUpType.DamageBoost:
                if (!playerShooting.DamageEffectOn)
                {
                    playerShooting.ApplyDamageBoost(2f, 4f);
                    Destroy(transform.parent.gameObject);
                }
                
                break;

            case PowerUp.PowerUpType.Invulnerability:
                if(!playerMovement.isInvincible)
                {
                    playerMovement.ApplyInvulnerability(4f);
                    Destroy(transform.parent.gameObject);
                }
                break;

            case PowerUp.PowerUpType.MoreHealth:
                playerMovement.AddMaxHealth(1);
                Destroy(transform.parent.gameObject);
                break;

            case PowerUp.PowerUpType.Heal:
                playerMovement.HealPlayerBackToFullHp();
                Destroy(transform.parent.gameObject);
                break;
        }
    }
    
}
