using System;
using System.Collections;
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
    [SerializeField] private bool isReacting = false;
    [Header("AI Settings")]
    [SerializeField] private float reactionTime = .2f;
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
        if(!isReacting)
        {
            if(pathFinding.findClosestPowerUp() != null)
            {
                if (IsPowerUpCloser())
                {
                    currentState = States.GrabbingPowerUp;
                }
                else
                {
                    currentState = States.Chasing;
                }
            }
            if(pathFinding.PlayerInLOS() && currentState == States.Shooting)
            {
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
                StartCoroutine(StartingState());
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
    IEnumerator StartingState()
    {
        yield return new WaitForSeconds(1.5f);
        currentState = States.Chasing;

    }
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
    //Moves the ai to the closest player vertice
    void MoveToClosestVertice()
    {
        if(!playerMovement.isInAir)
        {
            GameObject moveTo = pathFinding.FindClosestVerticeToPlayer();
            if(moveTo != null)
            {
                moveColliderPlayer = moveTo.GetComponent<Collider2D>();
            }
        }
        if (pathFinding.PlayerInLOS())
        {
            currentState = States.Shooting;
            return;
        }
        MoveTowards(moveColliderPlayer);
    }
    //Moves the ai to the Farthest player vertice
    void runAway()
    {   
        if(!playerMovement.isInAir)
        {
            GameObject moveTo = pathFinding.FindFarthestVerticeFromPlayer();
            if(moveTo != null)
            {
                moveColliderPlayer = moveTo.GetComponent<Collider2D>();
            }
        }
        MoveTowards(moveColliderPlayer);
    }
    //Moves the ai to the power up vertice
    void GrabPowerUp()
    {
        if(!playerMovement.isInAir)
        {
            GameObject moveTo = pathFinding.findClosestPowerUp();
            if(moveTo != null)
            {
                moveColliderPowerUp = moveTo.GetComponent<Collider2D>();
            }
        }
        MoveTowards(moveColliderPowerUp);
        if (moveColliderPowerUp == null || moveColliderPowerUp != null && gameObject.GetComponent<Collider2D>().IsTouching(moveColliderPowerUp))
        {
            currentState = States.Chasing;
        }
    }
    //Moves the player towards a vertice
    void MoveTowards(Collider2D moveCollider)
    {
        if(moveCollider == null) return;
        float colliderY = moveCollider.gameObject.transform.position.y;
        float colliderX = moveCollider.gameObject.transform.position.x;
        float enemyY = gameObject.transform.position.y;
        float enemyX = gameObject.transform.position.x;

        if(enemyY > colliderY + heightDiference)
        {
            playerMovement.HandleDropThrough();
            Debug.Log("Cair");
        }
        else if(enemyY < colliderY - heightDiference)
        {
            playerMovement.HandleJump();
            Debug.Log("Salto");
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
