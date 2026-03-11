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
        if (playerMovement == null || playerShooting == null) return;

        //buscar o powerUp script para encontrar o respetivo tipo
        PowerUp powerUp = GetComponentInParent<PowerUp>();
        if (powerUp == null)
        {
            powerUp = GetComponentInParent<PowerUp>();
        }
        ApplyPowerUpEffect(powerUp.type);
        //Resetar os vertices da IA, nao deve de causar problemas
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in players)
        {
            PathFinding pf = p.GetComponent<PathFinding>();
            if (pf != null) pf.UpdateVertices();
        }
    }
    public void ApplyPowerUpEffect(PowerUp.PowerUpType type)
    {
        switch (type)
        {
            case PowerUp.PowerUpType.SpeedBoost:
                playerMovement.ApplySpeedBoost(1.5f, 6f);
                Destroy(transform.parent.gameObject);
                break;

            case PowerUp.PowerUpType.JumpBoost:
                playerMovement.ApplyJumpBoost(1.5f, 6f);
                Destroy(transform.parent.gameObject);
                break;

            case PowerUp.PowerUpType.DamageBoost:
                playerShooting.ApplyDamageBoost(2f, 4f);
                Destroy(transform.parent.gameObject);
                break;

            case PowerUp.PowerUpType.Invulnerability:
                playerMovement.ApplyInvulnerability(4f);
                Destroy(transform.parent.gameObject);
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
