using System;
using System.Collections;
using System.Numerics;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class EnemyManager : MonoBehaviour
{
    #region ...[VARIABLES]...  
    private PlayerMovement playerMovement;
    private PlayerShooting playerShooting;
    private PathFinding pathFinding;
    private float heightDiference = .3f;
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
        Dying

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
        if(currentState == States.Chasing)
        {
            MoveToClosestVertice();
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
            case States.Running:
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
    //Moves the player to the closest vertice
    void MoveToClosestVertice()
    {
        Collider2D AICollider = playerMovement.col;
        Collider2D moveCollider = pathFinding.FindClosestVerticeToPlayer().GetComponent<Collider2D>();
        if (pathFinding.PlayerInLOS())
        {
            Debug.Log("In Los");
            currentState = States.Shooting;
            return;
        }
        float colliderY = moveCollider.gameObject.transform.position.y;
        float colliderX = moveCollider.gameObject.transform.position.x;
        float enemyY = gameObject.transform.position.y;
        float enemyX = gameObject.transform.position.x;

        if(enemyY > colliderY + heightDiference)
        {
            playerMovement.HandleDropThrough();
        }
        else if(enemyY < colliderY)
        {
            playerMovement.HandleJump();
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
    
}
