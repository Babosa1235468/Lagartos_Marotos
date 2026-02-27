using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
        float distanceToAIWeight = 5f;
        float distanceToPlayerWeight = 5f;
        float verticalWeight = 3f;
        float jumpWeight = 8f;
        float uncertainty = 0.6f;

        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        GameObject closest = null;
        int maxPoints = -99999;
        foreach (Transform v in vertices)
        {
            float distanceToAI = Vector2.Distance(v.position, transform.position);

            // Skip the vertex we're already standing on
            if (distanceToAI < 0.7f) continue;

            int points = 0;
            float distanceToPlayer = Vector2.Distance(v.position, playerMovement.otherPlayerCol.transform.position);

            // Less penalty for being close to AI (we want to move toward player, not stay put)
            points += (int)(1f / (distanceToAI + 0.01f) * 100 * distanceToAIWeight);
            // Reward proximity to player
            points += (int)(1f / (distanceToPlayer + 0.01f) * 100f * distanceToPlayerWeight);

            float distancePlayerY = v.position.y - playerMovement.otherPlayerCol.transform.position.y;
            float distanceAIY = transform.position.y - playerMovement.otherPlayerCol.transform.position.y;

            // If both AI and vertex are below the player, prefer vertices that are higher (closer to player level)
            if (distancePlayerY < -uncertainty && distanceAIY < -uncertainty)
            {
                points += v.position.y > transform.position.y ? (int)(100 * verticalWeight) : -(int)(100 * verticalWeight);
            }
            // If both AI and vertex are above the player, prefer vertices that are lower (closer to player level)
            if (distancePlayerY > uncertainty && distanceAIY > uncertainty)
            {
                points += v.position.y < transform.position.y ? (int)(100 * verticalWeight) : -(int)(100 * verticalWeight);
            }

            // Jump reachability: only evaluate vertices that are meaningfully above the AI
            float distanceY = v.position.y - transform.position.y;
            if (distanceY > uncertainty)
            {
                points += distanceY <= maxJumpHeight ? (int)(100 * jumpWeight) : -(int)(100 * jumpWeight);
            }

            if (points > maxPoints)
            {
                maxPoints = points;
                closest = v.gameObject;
            }
        }
        return closest;
    }
    public GameObject FindFarthestVerticeFromPlayer()
    {
        float distanceToAIWeight = 5f;
        float distanceToPlayerWeight = 5f;
        float verticalWeight = 3f;
        float jumpWeight = 8f;
        float uncertainty = 0.6f;
        
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        GameObject closest = null;
        int maxPoints = -99999;
        foreach (Transform v in vertices)
        {
            float distanceToAI = Vector2.Distance(v.position, transform.position);
            // Skip the vertex we're already standing on
            if (distanceToAI < 0.5f) continue;
            int points = 0;
            float distanceToPlayer = Vector2.Distance(v.position, playerMovement.otherPlayerCol.transform.position);

            // Reward vertices FAR from AI
            points += (int)(distanceToAI * distanceToAIWeight * 10);
            // Reward vertices FAR from player
            points += (int)(distanceToPlayer * distanceToPlayerWeight * 10);

            float distancePlayerY = v.position.y - playerMovement.otherPlayerCol.transform.position.y;
            float distanceAIY = transform.position.y - playerMovement.otherPlayerCol.transform.position.y;

            // If both AI and vertex are below the player, prefer vertices that are LOWER (further from player level)
            if (distancePlayerY < -uncertainty && distanceAIY < -uncertainty)
            {
                points += v.position.y < transform.position.y ? (int)(100 * verticalWeight) : -(int)(100 * verticalWeight);
            }
            // If both AI and vertex are above the player, prefer vertices that are HIGHER (further from player level)
            if (distancePlayerY > uncertainty && distanceAIY > uncertainty)
            {
                points += v.position.y > transform.position.y ? (int)(100 * verticalWeight) : -(int)(100 * verticalWeight);
            }

            // Jump reachability: still penalize unreachable vertices — no point fleeing somewhere you can't get to
            float distanceY = v.position.y - transform.position.y;
            if (distanceY > uncertainty)
            {
                points += distanceY <= maxJumpHeight ? (int)(100 * jumpWeight) : -(int)(100 * jumpWeight);
            }
            if (points > maxPoints)
            {
                maxPoints = points;
                closest = v.gameObject;
            }
        }
        return closest;
    }
    public GameObject findClosestPowerUp()
    {
        List<Transform> powerVertices = verticesPowerUps.Where(v => v.childCount != 0).ToList();
        if (powerVertices.Count <= 0)
        {
            return null;
        }

        float distanceToAIWeight = 5f;
        float distanceToPlayerWeight = 5f;
        float verticalWeight = 3f;
        float jumpWeight = 8f;
        float uncertainty = 0.6f;
        
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        GameObject closest = null;
        int maxPoints = -99999;

        foreach (Transform v in powerVertices)
        {
            if (v == null) continue;

            float distanceToAI = Vector2.Distance(v.position, transform.position);
            if (distanceToAI < 0.5f) continue;

            int points = 0;
            float distanceToPlayer = Vector2.Distance(v.position, playerMovement.otherPlayerCol.transform.position);

            // Reward vertices close to AI
            points += (int)(1f / (distanceToAI + 0.01f) * 100f * distanceToAIWeight);
            // Slight penalty if the player is also close (they might grab it first)
            points -= (int)(1f / (distanceToPlayer + 0.01f) * 100f * distanceToPlayerWeight);

            // Jump reachability
            float distanceY = v.position.y - transform.position.y;
            if (distanceY > uncertainty)
            {
                points += distanceY <= maxJumpHeight ? (int)(100 * jumpWeight) : -(int)(100 * jumpWeight);
            }

            if (points > maxPoints)
            {
                maxPoints = points;
                closest = v.gameObject;
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
