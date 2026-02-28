using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using Color = UnityEngine.Color;

public class PathFinding : MonoBehaviour
{
    private GameObject player;
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private float radiusDetection = 0.8f;
    private Transform[] vertices;
    private Transform[] verticesPowerUps;

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
        
        verticesPowerUps = GameObject.FindGameObjectsWithTag("PowerUpVertice").Select(p => p.GetComponent<Transform>()).ToArray();
        vertices = GameObject.FindGameObjectsWithTag("Vertices").Select(p => p.GetComponent<Transform>()).ToArray();
    }
    //Finds and returns the closest Vertice to the AI and Player
    public GameObject FindClosestVerticeToPlayer()
    {
        if(playerMovement.otherPlayer.GetComponent<PlayerMovement>().isInAir || playerMovement.isInAir) return null;
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        GameObject closest = vertices.OrderBy(p => Vector2.Distance(playerMovement.otherPlayer.transform.position,p.position)).First().gameObject;
        GameObject nextVetice = closest;
        float uncertainty = 0.5f;

        // Jump reachability: only evaluate vertices that are meaningfully above the AI
        float distanceY = closest.transform.position.y - transform.position.y;
        if (distanceY > uncertainty)
        {
            float minDistance = 9999f;
            foreach (Transform v in vertices)
            {
                float distanceToAI = Vector2.Distance(v.transform.position, transform.position);
                float distanceAIY = v.transform.position.y - transform.position.y;
                if (distanceAIY > uncertainty)
                {
                    if(distanceAIY < maxJumpHeight)
                    {
                        if(distanceToAI < minDistance)
                        {
                            minDistance = distanceAIY;
                            nextVetice = v.gameObject;
                        }
                    }
                }
            }
        }
        return nextVetice;
    }
    public GameObject FindFarthestVerticeFromPlayer()
    {
        if(playerMovement.otherPlayer.GetComponent<PlayerMovement>().isInAir  || playerMovement.isInAir) return null;
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        GameObject farthest = vertices.OrderBy(p => Vector2.Distance(playerMovement.otherPlayer.transform.position,p.position)).Reverse().First().gameObject;
        GameObject nextVetice = farthest;
        float uncertainty = 0.5f;

        // Jump reachability: only evaluate vertices that are meaningfully above the AI
        float distanceY = farthest.transform.position.y - transform.position.y;
        if (distanceY > uncertainty)
        {
            float minDistance = 9999f;
            foreach (Transform v in vertices)
            {
                float distanceToAI = Vector2.Distance(v.transform.position, transform.position);
                float distanceAIY = v.transform.position.y - transform.position.y;
                if (distanceAIY > uncertainty)
                {
                    if(distanceAIY < maxJumpHeight)
                    {
                        if(distanceToAI < minDistance)
                        {
                            minDistance = distanceAIY;
                            nextVetice = v.gameObject;
                        }
                    }
                }
            }
        }
        return nextVetice;
    }
    public GameObject findClosestPowerUp()
    {
        if(playerMovement.isInAir) return null;
        List<Transform> powerVertices = verticesPowerUps.Where(v => v.childCount != 0).ToList();
        if (powerVertices.Count <= 0)
        {
            return null;
        }

        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        GameObject closest = powerVertices.OrderBy(p => Vector2.Distance(playerMovement.otherPlayer.transform.position,p.position)).First().gameObject;
        GameObject nextVetice = closest;
        float uncertainty = 0.5f;

        // Jump reachability: only evaluate vertices that are meaningfully above the AI
        float distanceY = closest.transform.position.y - transform.position.y;
        if (distanceY > uncertainty)
        {
            float minDistance = 9999f;
            foreach (Transform v in powerVertices)
            {
                float distanceToAI = Vector2.Distance(v.transform.position, transform.position);
                float distanceAIY = v.transform.position.y - transform.position.y;
                if (distanceAIY > uncertainty)
                {
                    if(distanceAIY < maxJumpHeight)
                    {
                        if(distanceToAI < minDistance)
                        {
                            minDistance = distanceAIY;
                            nextVetice = v.gameObject;
                        }
                    }
                }
            }
        }
        return nextVetice;
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
