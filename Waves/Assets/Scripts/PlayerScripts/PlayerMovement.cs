using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{

    #region ...[VARIABLES]...  

    [Header("Player Settings")]
    [SerializeField] public string playerName = "Player";
    [SerializeField] public Collider2D otherPlayerCol;

    [Header("Controls")]
    [SerializeField] public KeyCode leftKey = KeyCode.A;
    [SerializeField] public KeyCode rightKey = KeyCode.D;
    [SerializeField] public KeyCode jumpKey = KeyCode.W;
    [SerializeField] public KeyCode downKey = KeyCode.S;

    [Header("Components")]
    [SerializeField] public Rigidbody2D rb;
    [SerializeField] public Collider2D col;
    [SerializeField] public Transform spriteHolder; // assign SpriteHolder in Inspector

    [Header("UI")]
    [SerializeField] public Image healthBar;
    [SerializeField] public TextMeshProUGUI playerNameTxt;
    [SerializeField] public TextMeshProUGUI playerLivesTxt;

    [Header("Stats (Editable)")]
    [SerializeField] public float gravity = 2.3f;

    [Header("Jump Settings")]
    [SerializeField] public float jumpSpeed = 6.5f;
    [SerializeField] public int maxJumps = 2;

    [Header("Lives & Health")]
    [SerializeField] public int maxLives = 3;
    [SerializeField] public float maxHealthPoints = 100f;
    [SerializeField] public float healthBarSpeed = 5f;

    [Header("Movement Settings")]
    [SerializeField] public float currentSpeed = 0f;
    [SerializeField] public float acceleration;
    [SerializeField] public float deceleration;
    [SerializeField] public float maxSpeed;

    [Header("Health UI Colors")]
    [SerializeField] public Color fullHealthColor = Color.green;
    [SerializeField] public Color zeroHealthColor = Color.red;

    [Header("Spawn Settings")]
    public PlayerMovement otherPlayer;          // reference to the other player
    public float minSpawnDistance = 1.5f;       // minimum distance from other player

    // -------------------- public State --------------------
    public int jumpsLeft;
    public bool isInAir;
    public bool goingDown;
    public bool isDead;

    public int currentLives;
    public float currentHealthPoints;




    public float targetHealthFill;
    public float currentHealthFill;

    public Vector3 scale;


    public PlayerShooting playerShooting;
    #endregion

    // -------------------- Core Variables --------------------
    void Awake()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        string LayerUIName = string.Empty;
        foreach (GameObject p in Players)
        { 
            if (p != gameObject)
            {
                otherPlayerCol = p.GetComponent<Collider2D>();
                otherPlayer = p.GetComponent<PlayerMovement>();
                
            }
        }
        LayerUIName = LayerMask.LayerToName(gameObject.layer) + "_UI";

        playerShooting = GetComponent<PlayerShooting>();

        // Gets UI elements of the player by name
        GameObject UIGameObject = GameObject.FindGameObjectWithTag(LayerUIName);
        healthBar = UIGameObject.transform.Find("Health/HealthBar").GetComponent<Image>();
        playerNameTxt = UIGameObject.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        playerLivesTxt = UIGameObject.transform.Find("PlayerLives/Text").GetComponent<TextMeshProUGUI>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        spriteHolder = gameObject.GetComponentInChildren<SpriteRenderer>().transform;
    }
    void Start() 
    {
        ResetVariables();
        Spawn();
        Time.timeScale = 1f;
        if (otherPlayerCol != null)
        {
            Physics2D.IgnoreCollision(col, otherPlayerCol, true);
        }
    }
    void Update()
    {
        UpdateHealthBar();
    }
    public void ResetVariables()
    {
        rb.gravityScale = gravity;

        // Jump
        jumpsLeft = maxJumps;
        isInAir = false;
        goingDown = false;

        // State
        isDead = false;

        // Lives & Health
        currentLives = maxLives;
        currentHealthPoints = maxHealthPoints;
        targetHealthFill = 1f;
        currentHealthFill = 1f;

        // Movement
        maxSpeed = 2.8f;
        acceleration = 4f;
        deceleration = 6f;
        currentSpeed = 0f;

        // Scale
        scale = spriteHolder.transform.localScale;
    }

    public void UpdateHealthBar()
    {
        if (healthBar == null) return;

        currentHealthFill = Mathf.Lerp(currentHealthFill, targetHealthFill, Time.deltaTime * healthBarSpeed);
        healthBar.fillAmount = currentHealthFill;
        healthBar.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealthFill);
    }

    // -------------------- Player Movement Actions --------------------
    public void HandleJump()
    {
        if (jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            jumpsLeft--;
            isInAir = true;
            //col.isTrigger = true;
        }
    }

    public void HandleDropThrough()
    {
        if (!isInAir)
        {
            col.isTrigger = true;
            isInAir = true;
            goingDown = true;
            jumpsLeft = 1;
            Invoke(nameof(EnableCollider), 0.24f);
        }
    }

    public void HandleMovement()
    {
        float targetSpeed = 0f;

        if (Input.GetKey(leftKey))
        {
            targetSpeed = -maxSpeed;
            FlipSprite(-1);
        }
        else if (Input.GetKey(rightKey))
        {
            targetSpeed = maxSpeed;
            FlipSprite(1);
        }

        float smoothFactor = (targetSpeed == 0f) ? deceleration : acceleration;
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, smoothFactor * Time.deltaTime);

        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    // 1 FOR RIGHT -1 FOR LEFT
    public void FlipSprite(int direction)
    {
        Vector3 scale = spriteHolder.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        spriteHolder.localScale = scale;
    }

    public void HandleFalling()
    {
        if (isInAir && rb.linearVelocity.y <= 0 && !goingDown)
        {
            col.isTrigger = false;
        }


        if (rb.linearVelocity.y < 0 && !isInAir)
        {
            isInAir = true;
            jumpsLeft = 1;
        }
    }

    // -------------------- Collision --------------------
    public void OnCollisionStay2D(Collision2D collision)
    {
        jumpsLeft = maxJumps;
        isInAir = false;
        col.isTrigger = false;
    }

    public void EnableCollider()
    {
        col.isTrigger = false;
        goingDown = false;
    }

    // -------------------- Health & Lives --------------------
    public void Damage(float dmg)
    {
        currentHealthPoints = Mathf.Max(0, currentHealthPoints - dmg);
        targetHealthFill = (float)currentHealthPoints / maxHealthPoints;
        if (currentHealthPoints <= 0) Death();
    }

    public void Death()
    {
        if (isDead) return;
        isDead = true;
        currentLives--;

        if (currentLives <= 0)
        {
        }
        else
        {
            Spawn();
            FixReload();
        }
    }

    public void FixReload()
    {
        playerShooting.remainingBullets = playerShooting.magazineSize;
        playerShooting.reloadBar.enabled = false;
        playerShooting.isReloading = false;
        playerShooting.bulletsTxt.text = $"{playerShooting.remainingBullets}";
    }
    // -------------------- Spawn --------------------
    public void Spawn()
    {
        gameObject.SetActive(true);
        isDead = false;
        currentHealthPoints = maxHealthPoints;
        targetHealthFill = 1f;
        currentHealthFill = 1f;

        if (playerLivesTxt != null)
            playerLivesTxt.text = $"{currentLives}";

        if (playerNameTxt != null)
            playerNameTxt.text = $"{playerName}";


        Vector3 spawnPos;
        if (otherPlayer != null)
        {
            do
            {
                spawnPos = new Vector3(Random.Range(-2.8f, 1.4f), 10f, -0.685f);
            } while (Vector3.Distance(spawnPos, otherPlayer.transform.position) < minSpawnDistance);
        }
        else
        {
            spawnPos = new Vector3(Random.Range(-3f, 1.5f), 3f, -0.685f);
        }

        transform.position = spawnPos;
        rb.linearVelocity = Vector2.zero;
    }
    #region ...[Power Ups Section]...
    #region ...[Variaveis]...
    public bool isInvincible = false;
    public float speedDiference;
    public float jumpDiference;
    public bool SpeedEffectOn = false;
    public bool JumpEffectOn = false;
    #endregion
    public void ApplySpeedBoost(float multValue, float time)
    {
        SpeedEffectOn = true;
        speedDiference = (maxSpeed*multValue) - maxSpeed;
        maxSpeed += speedDiference;
        Invoke(nameof(EndSpeedBoost),time);
    }
    public void EndSpeedBoost()
    {
        SpeedEffectOn = false;
        maxSpeed -= speedDiference;
    }
    public void ApplyJumpBoost(float multValue, float time)
    {
        JumpEffectOn = true;
        jumpDiference = (jumpSpeed*multValue) - jumpSpeed;
        jumpSpeed += jumpDiference;
        Invoke(nameof(EndJumpBoost),time);
    }
    public void EndJumpBoost()
    {
        JumpEffectOn = false;
        jumpSpeed -= jumpDiference;
    }
    public void ApplyInvulnerability(float time)
    {
        isInvincible = true;
        Invoke(nameof(EndInvulnerability),time);
    }
    public void EndInvulnerability()
    {
        isInvincible = false;
    }
    public void AddMaxHealth(int livesAmmount)
    {
        currentLives += livesAmmount;
        playerLivesTxt.text = $"{currentLives}";   
    }
    public void HealPlayerBackToFullHp()
    {
        currentHealthPoints = maxHealthPoints;
        UpdateHealthBar();
    }
    #endregion
}