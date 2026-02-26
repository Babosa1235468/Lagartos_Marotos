using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private float radiusDetection = .7f;
    Transform[] vertices;

    void Start()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Players)
        { 
            if (p != gameObject)
            {
                player = p;
            }
        }

        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerShooting = gameObject.GetComponent<PlayerShooting>();
        vertices = GameObject.FindGameObjectsWithTag("Vertices").Select(p => p.GetComponent<Transform>()).ToArray();
    }
    //Finds and returns the closest Vertice to the AI and Player
    public GameObject FindClosestVerticeToPlayer()
    {
        Vector2 direction = (player.transform.position - transform.position).normalized;
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radiusDetection, direction);
        float minDistance = 99999f;
        GameObject closest = null;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider != null && hit.collider.gameObject != gameObject && hit.collider.tag == "Vertices" )
            {
                float distance = Vector2.Distance(playerMovement.otherPlayer.gameObject.transform.position, hit.collider.transform.position);

                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = hit.collider.transform.gameObject;
                }
            }
        }
        return closest;
    }
    public GameObject FindFarthestVerticeFromPlayer()
    {
        float maxDistance = -99999f;
        GameObject closest = null;
        foreach (Transform v in vertices)
        {
            if (v != null)
            {
                float distance = Vector2.Distance(playerMovement.otherPlayer.gameObject.transform.position, v.position);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    closest = v.gameObject;
                }
            }
        }
        return closest;
    }
    public bool PlayerInLOS()
    {
        float facing = playerShooting.spriteHolder.transform.localScale.x > 0 ? 1f : -1f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, new Vector2(facing, 0));
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.gameObject != gameObject && hit.collider.gameObject.tag == "Player")
            {
                return true;
            }
        }
        return false;
    }
}
