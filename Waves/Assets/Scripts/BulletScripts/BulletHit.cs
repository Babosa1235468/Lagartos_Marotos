using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject owner; // who shot bullet
    public float dmg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ignore if hitting the shooter
        if (collision.gameObject == owner) return;

        //6 if o player 1
        //7 se o player 2, EU ACHO, se n, e ao contrario
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7)
        {
            // se o player estiver invencibel, nao leva dano
            if (!collision.GetComponent<PlayerMovement>().isInvincible)
            {
                collision.GetComponent<PlayerMovement>().Damage(dmg);
            }
            // destroy bullet after hit
            Destroy(gameObject);
        } 
    }
}
