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
    public void UpdateVertices()
    {
        verticesPowerUps = GameObject.FindGameObjectsWithTag("PowerUpVertice").Select(p => p.GetComponent<Transform>()).ToArray();
        vertices = GameObject.FindGameObjectsWithTag("Vertices").Select(p => p.GetComponent<Transform>()).ToArray();
    }
    //Encontra o vertice mais proximo do player ao qual a ia consegue chegar
    public GameObject FindClosestVerticeToPlayer()
    {
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        float doubleJumpHeight = maxJumpHeight + playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        GameObject closest = vertices.OrderBy(p => Vector2.Distance(playerMovement.otherPlayer.transform.position,p.position)).First().gameObject;
        GameObject nextVetice = closest;
        float uncertainty = 0.4f;

        // É possivel chegar ao vertice a saltar?
        float distanceY = closest.transform.position.y - transform.position.y;
        if (distanceY > uncertainty && !canJumpTo(closest.GetComponent<Collider2D>(),2))
        {
            float minDistance = 9999f;
            foreach (Transform v in vertices)
            {
                float distanceToAI = Vector2.Distance(v.transform.position, transform.position);
                float distanceAIY = v.transform.position.y - transform.position.y;
                if (distanceAIY > uncertainty)
                {
                    if(distanceAIY < doubleJumpHeight)
                    {
                        if(distanceToAI < minDistance)
                        {
                            minDistance = distanceToAI;
                            nextVetice = v.gameObject;
                        }
                    }
                }
            }
        }
        return nextVetice;
    }
    //Encontra o vertice mais longe do player ao qual a ia consegue chegar
    public GameObject FindFarthestVerticeFromPlayer()
    {
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        float doubleJumpHeight = maxJumpHeight + playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        GameObject farthest = vertices.OrderBy(p => Vector2.Distance(playerMovement.otherPlayer.transform.position,p.position)).Reverse().First().gameObject;
        GameObject nextVetice = farthest;
        float uncertainty = 0.4f;

        // É possivel chegar ao vertice a saltar?
        float distanceY = farthest.transform.position.y - transform.position.y;
        if (distanceY > uncertainty && !canJumpTo(farthest.GetComponent<Collider2D>(),2))
        {
            float minDistance = 9999f;
            foreach (Transform v in vertices)
            {
                float distanceToAI = Vector2.Distance(v.transform.position, transform.position);
                float distanceAIY = v.transform.position.y - transform.position.y;
                if (distanceAIY > uncertainty)
                {
                    if(distanceAIY < doubleJumpHeight)
                    {
                        if(distanceToAI < minDistance)
                        {
                            minDistance = distanceToAI;
                            nextVetice = v.gameObject;
                        }
                    }
                }
            }
        }
        return nextVetice;
    }
    //Encontra o vertice mais perto do power up ao qual a ia consegue chegar
    public GameObject findClosestPowerUp()
    {
        List<Transform> powerVertices = verticesPowerUps.Where(v => v.childCount != 0).ToList();
        if (powerVertices.Count <= 0)
        {
            return null;
        }

        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        float doubleJumpHeight = maxJumpHeight + playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        float timeToMax = playerMovement.jumpSpeed / effectiveGravity;
        float horizontalDistanceAtMax = Mathf.Abs(playerMovement.currentSpeed) * timeToMax + 0.5f * Mathf.Abs(playerMovement.acceleration) * timeToMax * timeToMax;

        
        GameObject closestPowerUp = powerVertices.OrderBy(p => Vector2.Distance(transform.position,p.position)).First().gameObject;
        float distanceY = closestPowerUp.transform.position.y - transform.position.y;
        float uncertainty = 0.4f;
        if(distanceY > uncertainty && canJumpTo(closestPowerUp.GetComponent<Collider2D>(), 2) || (closestPowerUp.transform.position.x > transform.position.x - horizontalDistanceAtMax && closestPowerUp.transform.position.x < transform.position.x + horizontalDistanceAtMax) )
        {
            return closestPowerUp;
        }
        GameObject closestVertice = vertices.OrderBy(p => Vector2.Distance(closestPowerUp.transform.position,p.position)).First().gameObject;
        GameObject nextVetice = closestVertice;

        // É possivel chegar ao vertice a saltar?
        distanceY = closestVertice.transform.position.y - transform.position.y;
        if (distanceY > uncertainty && !canJumpTo(closestVertice.GetComponent<Collider2D>(),2))
        {
            float minDistance = 9999f;
            foreach (Transform v in vertices)
            {
                float distanceToAI = Vector2.Distance(v.transform.position, transform.position);
                float distanceAIY = v.transform.position.y - transform.position.y;
                if (distanceAIY > uncertainty)
                {
                    if(distanceAIY < doubleJumpHeight)
                    {
                        if(distanceToAI < minDistance)
                        {
                            minDistance = distanceToAI;
                            nextVetice = v.gameObject;
                        }
                    }
                }
            }
        }
        return nextVetice;
    }
    //O player está no los do player
    public bool PlayerInLOS()
    {
        float facing = playerShooting.sprites.transform.localScale.x > 0 ? 1f : -1f;
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
    public bool PlayerBehind()
    {
        float facing = playerShooting.sprites.transform.localScale.x > 0 ? -1f : 1f;
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
    public bool canJumpTo(Collider2D jumpTo, int amountOfJump)
    {
        if(jumpTo.transform.position.y <= transform.position.y) return false;
        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        float doubleJumpHeight = maxJumpHeight + playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        float timeToMax = playerMovement.jumpSpeed / effectiveGravity;
        float horizontalDistanceAtMax = Mathf.Abs(playerMovement.currentSpeed) * timeToMax + 0.5f * Mathf.Abs(playerMovement.acceleration) * timeToMax * timeToMax;

        float enemyY = transform.position.y;
        float enemyX = transform.position.x;
        float colliderY = jumpTo.gameObject.transform.position.y;
        float colliderX = jumpTo.gameObject.transform.position.x;

        if(amountOfJump == 2)
        {
            return (enemyY + doubleJumpHeight > colliderY && colliderX > enemyX - horizontalDistanceAtMax && colliderX < enemyX + horizontalDistanceAtMax) ? true : false;
        }
        else
        {
            return (enemyY + maxJumpHeight > colliderY && colliderX > enemyX - horizontalDistanceAtMax && colliderX < enemyX + horizontalDistanceAtMax) ? true : false;
        }
    }
}
