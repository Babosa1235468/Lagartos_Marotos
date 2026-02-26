using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    #region ...[VARIABLES]...  
    private PlayerMovement playerMovement;

    private PlayerShooting playerShooting;
    void Awake()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
        playerShooting = gameObject.GetComponent<PlayerShooting>();

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
        Dying

    }

    public States currentState = States.Starting;
    
    #endregion

    void Start()
    {
        stateControl();
    }
    void Update()
    {
        playerMovement.UpdateHealthBar();
    }
    void stateControl()
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
            case States.Shooting:
                StartCoroutine(ShootingState());
                break;
            case States.Dying:
                StartCoroutine(DyingState());
                break;
        }
    }
    #region States
    IEnumerator StartingState()
    {
        yield return new WaitForSeconds(2F);
        currentState = States.Chasing;



        stateControl();
    }
    IEnumerator ReloadingState()
    {
        yield return new WaitForSeconds(playerShooting.reloadTime);
        currentState = States.Chasing;



        stateControl();
    }

    IEnumerator ChasingState()
    {
        yield return new WaitForSeconds(0.3f);

        stateControl();
    }

    IEnumerator ShootingState()
    {
        yield return new WaitForSeconds(0.3f);

        stateControl();
    }

    IEnumerator DyingState()
    {
        yield return new WaitForSeconds(0.3f);

        stateControl();
    }
    
    #endregion
}
