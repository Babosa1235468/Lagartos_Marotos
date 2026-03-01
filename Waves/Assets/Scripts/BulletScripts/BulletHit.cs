using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject owner; // who shot bullet
    public float dmg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ignore if hitting the shooter
        if (collision.gameObject == owner) return;

        if (collision.gameObject.tag == "Player")
        {
            Debug.Log(collision.GetComponent<PlayerMovement>().isInvincible);
            // se o player estiver invencivel, nao leva dano
            if (collision.GetComponent<PlayerMovement>().isInvincible)
            {
                collision.GetComponent<PlayerMovement>().Damage(dmg);
            }
            // destroy bullet after hit
            Destroy(gameObject);
        } 
    }
}
