using System;
using System.Collections;
using System.Linq;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class EnemyManager : MonoBehaviour
{
    #region ...[VARIABLES]...  
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private PathFinding pathFinding;
    private float heightDiference = .5f;
    Collider2D moveColliderPlayer = null;
    Collider2D moveColliderPowerUp = null;
    private bool jumping = false;
    [SerializeField] private bool isReacting = false;
    [Header("AI Settings")]
    [SerializeField] private float reactionTime = .2f;
    bool respawning = false;
    bool wasInvincible;
    void Awake()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerShooting = gameObject.GetComponent<PlayerShooting>();
        pathFinding = gameObject.GetComponent<PathFinding>();

        playerMovement.downKey = KeyCode.None;
        playerMovement.jumpKey = KeyCode.None;
        playerMovement.leftKey = KeyCode.None;
        playerMovement.rightKey = KeyCode.None;

        playerShooting.shootKey = KeyCode.None;
        playerShooting.reloadKey = KeyCode.None;
    }
    public enum States
    {
        Starting,
        Reloading,
        Chasing,
        Shooting,
        Running,
        Dying,
        PlayerDeath,
        GrabbingPowerUp

    }

    [SerializeField] private States _currentState = States.Starting;
    public States currentState
    {
        get => _currentState;
        set
        {
            if (_currentState != value)
            {
                _currentState = value;
                OnStateChanged();
            }
        }
    }
    
    #endregion

    void Start()
    {
        OnStateChanged();
    }
    void Update()
    {
        bool isDead = playerMovement.isDead;
        if (!isDead)
        {
            if(currentState == States.Dying)
            {
                if(!respawning)
                {
                    StartCoroutine(waitTillRespawn());
                }
                return;
            }
            if(currentState == States.Starting)
            {
                
                if(playerMovement.rb.linearVelocityY == 0)
                {
                    currentState = States.Chasing;
                }
            }
            bool isInvincible = playerMovement.otherPlayer.isInvincible;
            if (isInvincible)
            {
                playerShooting.HandleTryReloading();
                if (pathFinding.findClosestPowerUp() != null)
                {
                    currentState = States.GrabbingPowerUp;
                }
                else
                {
                    currentState = States.Running;
                }
            }
            else
            {
                if(!isReacting)
                {
                    if(pathFinding.findClosestPowerUp() != null)
                    {
                        if (IsPowerUpCloser())
                        {
                            currentState = States.GrabbingPowerUp;
                            playerShooting.HandleTryReloading();
                        }
                        else
                        {
                            currentState = States.Chasing;
                        }
                    }
                    int facingBehind = playerShooting.spriteHolder.transform.localScale.x > 0 ? -1 : 1;
                    if (pathFinding.PlayerBehind())
                    {
                        playerMovement.FlipSprite(facingBehind);
                    }
                    if(pathFinding.PlayerInLOS()  && currentState == States.Shooting)
                    {
                        MoveToClosestVertice();
                        if(playerShooting.remainingBullets <= 0)
                        {
                            currentState = States.Reloading;
                        }
                        else
                        {
                            playerShooting.HandleTryShooting();
                        }
                    }
                    else if(!pathFinding.PlayerInLOS() && currentState == States.Shooting)
                    {
                        currentState = States.Chasing;
                    }
                    StartCoroutine(ReactionTime());
                }
            }
            if (wasInvincible && !isInvincible)
            {
                currentState = States.Chasing;
            }
            wasInvincible = isInvincible;
            if(currentState == States.Chasing)
            {
                MoveToClosestVertice();
            }
            if(currentState == States.Running)
            {
                runAway();
            }
            if(currentState == States.GrabbingPowerUp)
            {
                GrabPowerUp();
            }
        }
        else
        {
            currentState = States.Dying;
        }
        
    }
    IEnumerator waitTillRespawn()
    {
        respawning = true;
        yield return new WaitForSeconds(.3f);
        respawning = false;
        currentState = States.Starting;
    }
    IEnumerator ReactionTime()
    {
        isReacting = true;

        yield return new WaitForSeconds(reactionTime); 

        isReacting = false;
    }
    void OnStateChanged()
    {
        switch (currentState) {
            case States.Starting:
                StartCoroutine(Spawning());
                break;
             case States.Reloading:
                StartCoroutine(ReloadingState());
                break;
            case States.Chasing:
                StartCoroutine(ChasingState());
                break;
            case States.Dying:
                StartCoroutine(DyingState());
                break;
        }
    }
    #region States
    IEnumerator ReloadingState()
    {
        currentState = States.Running;
        yield return StartCoroutine(playerShooting.Reload());
        currentState = States.Chasing;
    }

    IEnumerator ChasingState()
    {
        MoveToClosestVertice();
        yield return null;
    }

    IEnumerator DyingState()
    {
        yield return null;
    }
    
    #endregion
    IEnumerator Spawning()
    {
        yield return null;
    }
   //Move a ia em direção ao player
    void MoveToClosestVertice()
    {
        if(playerMovement.rb.linearVelocityY == 0)
        {
            moveColliderPlayer = pathFinding.FindClosestVerticeToPlayer().GetComponent<Collider2D>();
        }
        if (pathFinding.PlayerInLOS())
        {
            currentState = States.Shooting;
            return;
        }
        MoveTowards(moveColliderPlayer);
    }
    //Faz a ia fugir do player
    void runAway()
    {   
        if(playerMovement.rb.linearVelocityY == 0)
        {
            moveColliderPlayer = pathFinding.FindFarthestVerticeFromPlayer().GetComponent<Collider2D>();
        }
        MoveTowards(moveColliderPlayer);
    }
    //Move a ia em direção a um powerUp
    void GrabPowerUp()
    {
        
        if(playerMovement.rb.linearVelocityY == 0)
        {
            GameObject powerUp = pathFinding.findClosestPowerUp();
            if(powerUp != null)
            {
                moveColliderPowerUp = powerUp.GetComponent<Collider2D>();
            }
        }
        MoveTowards(moveColliderPowerUp);
        if (moveColliderPowerUp == null || moveColliderPowerUp != null && playerMovement.col.IsTouching(moveColliderPowerUp))
        {
            currentState = States.Chasing;
        }
    }
    IEnumerator SecondJumpWait(float timeToMax)
    {
        yield return new WaitForSeconds(timeToMax);
        playerMovement.HandleJump();
    }
    
    //Move a ia em direção a um vertice
    void MoveTowards(Collider2D moveCollider)
    {
        if(moveCollider == null) return;
        if(moveCollider.IsTouching(playerMovement.col)) return;
        Debug.Log("Pai:" + moveCollider.transform.parent.parent);
        Debug.Log("Filho: " + moveCollider);

        float colliderY = moveCollider.gameObject.transform.position.y;
        float colliderX = moveCollider.gameObject.transform.position.x;
        float enemyY = transform.position.y;
        float enemyX = transform.position.x;

        float effectiveGravity = Mathf.Abs(playerMovement.gravity * Physics2D.gravity.y);
        float maxJumpHeight = playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);
        float doubleJumpHeight = maxJumpHeight + playerMovement.jumpSpeed * playerMovement.jumpSpeed / (2 * effectiveGravity);

        float timeToMax = playerMovement.jumpSpeed / effectiveGravity;
        float direction = Mathf.Sign(colliderX - enemyX);
        float horizontalDistanceAtMax = direction * (Mathf.Abs(playerMovement.currentSpeed) * timeToMax + 0.5f * Mathf.Abs(playerMovement.acceleration) * timeToMax * timeToMax);

        if(playerMovement.rb.linearVelocityY > 0)
        {
            jumping = true;
        }
        else
        {
            jumping = false;
        }
        RaycastHit2D[] hitsFloor = Physics2D.RaycastAll(transform.position,new Vector2(direction,-1),2f,LayerMask.GetMask("Ground"));
        if(enemyY > colliderY + heightDiference && playerMovement.rb.linearVelocityY == 0)
        {
            RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position,Vector2.down,10f,LayerMask.GetMask("Ground"));
            if(hits.Count() >= 2 || (colliderX > enemyX - horizontalDistanceAtMax && colliderX < enemyX + horizontalDistanceAtMax))
            {
                playerMovement.HandleDropThrough();
            }
        }
        else if(!jumping && pathFinding.canJumpTo(moveCollider,1) && playerMovement.jumpsLeft >= 1)
        {
            jumping = true;
            playerMovement.HandleJump();
        } // se apenas um salto não for suficiente faz dois saltos
        else if((!jumping && pathFinding.canJumpTo(moveCollider,2) || hitsFloor.Count() <= 0) && playerMovement.jumpsLeft >= 2)
        {
            jumping = true;
            playerMovement.HandleJump();
            StartCoroutine(SecondJumpWait(timeToMax));
        }
        float targetSpeed = 0f;
        if(enemyX > colliderX)
        {
            targetSpeed = -playerMovement.maxSpeed;
            playerMovement.FlipSprite(-1);
        }
        else if(enemyX < colliderX)
        {
            targetSpeed = playerMovement.maxSpeed;
            playerMovement.FlipSprite(1);
        }
        float smoothFactor = (targetSpeed == 0f) ? playerMovement.deceleration : playerMovement.acceleration;
        playerMovement.currentSpeed = Mathf.Lerp(playerMovement.currentSpeed, targetSpeed, smoothFactor * Time.deltaTime);

        playerMovement.rb.linearVelocity = new Vector2(playerMovement.currentSpeed, playerMovement.rb.linearVelocity.y);
    }
    // O power up esta mais perto que o player?
    bool IsPowerUpCloser()
    {
        Collider2D temp = pathFinding.findClosestPowerUp()?.GetComponent<Collider2D>();
        Collider2D playerColliderVertice = playerMovement.otherPlayer.GetComponent<Collider2D>();
        Vector2 moveColliderVector = temp.transform.position;
        Vector2 playerColliderVerticeVector = playerColliderVertice.transform.position;
        if(playerColliderVerticeVector == null || (moveColliderVector != null && Vector2.Distance(playerColliderVerticeVector, transform.position) > Vector2.Distance(moveColliderVector, transform.position)))
        {
            return true;
        }
        return false;
    }
}
