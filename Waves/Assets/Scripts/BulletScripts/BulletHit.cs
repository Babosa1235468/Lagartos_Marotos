using UnityEngine;

public class BulletHit : MonoBehaviour
{
    public GameObject owner; // who shot bullet
    public float dmg;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // ignore if hitting the shooter
        if (collision.gameObject == owner) return;

        //6 é o player 1
        //7 é o player 2, EU ACHO, se n, é ao contrario
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7)
        {
            collision.GetComponent<PlayerMovement>().Damage(dmg);
        }

        Destroy(gameObject); // destroy bullet after hit
    }

}
