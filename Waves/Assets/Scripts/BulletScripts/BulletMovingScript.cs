using UnityEngine;

public class BulletMovingScript : MonoBehaviour
{
    public float moveSpeed;
    public Vector2 moveDirection = Vector2.zero;

    void Update()
    {
        if (GameManager.instance.isPaused) return;
        transform.position += (Vector3)moveDirection * moveSpeed * Time.deltaTime;

        // delete when too far
        if (Mathf.Abs(transform.position.x) > 20f || Mathf.Abs(transform.position.y) > 20f)
        {
            Destroy(gameObject);
        }
    }
}
