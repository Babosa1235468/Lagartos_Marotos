using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyAI : MonoBehaviour
{
    #region ...[VARIABLES]...  
    [SerializeField] private PlayerMovement playerMovement;

    public PlayerShooting playerShooting;
    public enum States
    {
        Reloading,
        Chasing,
        Shoting,
        Dying

    }

    public States currentState = States.Chasing;
    
    #endregion

    void Update()
    {
        
    }
}
