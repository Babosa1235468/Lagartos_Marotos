using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject owner; // who shot bullet
    public float dmg;
    public ParticleSystem damageEffect;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ignore if hitting the shooter
        if (collision.gameObject == owner)
        {
            return;
        } 

        if (collision.gameObject.tag == "Player")
        {
            // se o player estiver invencivel, nao leva dano
            if (!collision.GetComponent<PlayerMovement>().isInvincible)
            {
                Quaternion rotation = Quaternion.Euler(0, -transform.eulerAngles.z, 0);
                ParticleSystem CreatedEffectDamage = Instantiate(damageEffect, collision.transform.position, rotation);
                Destroy(CreatedEffectDamage.gameObject, 2f);
                collision.GetComponent<PlayerMovement>().Damage(dmg);
            }
            // destroy bullet after hit
            Destroy(gameObject);
        } 
    }
}
