using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    #region ...[VARIABLES]...  
    [Header("Player Settings")]
    [SerializeField] private string playerName = "Player";
    [SerializeField] private Collider2D otherPlayerCol;

    [Header("Controls")]
    [SerializeField] private KeyCode leftKey = KeyCode.A;
    [SerializeField] private KeyCode rightKey = KeyCode.D;
    [SerializeField] private KeyCode jumpKey = KeyCode.W;
    [SerializeField] private KeyCode downKey = KeyCode.S;

    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D col;
    [SerializeField] private Transform spriteHolder; // assign SpriteHolder in Inspector

    [Header("UI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI playerNameTxt;
    [SerializeField] private TextMeshProUGUI playerLivesTxt;

    [Header("Stats (Editable)")]
    [SerializeField] private float gravity = 2.3f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpSpeed = 6.5f;
    [SerializeField] private int maxJumps = 2;

    [Header("Lives & Health")]
    [SerializeField] private int maxLives = 3;
    [SerializeField] private float maxHealthPoints = 100f;
    [SerializeField] private float healthBarSpeed = 5f;

    [Header("Movement Settings")]
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float maxSpeed;

    [Header("Health UI Colors")]
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color zeroHealthColor = Color.red;

    [Header("Spawn Settings")]
    public PlayerMovement otherPlayer;          // reference to the other player
    public float minSpawnDistance = 1.5f;       // minimum distance from other player

    // -------------------- Private State --------------------
    private int jumpsLeft;
    private bool isInAir;
    private bool goingDown;
    private bool isDead;

    public int currentLives;
    public float currentHealthPoints;




    private float targetHealthFill;
    private float currentHealthFill;

    private Vector3 scale;


    private PlayerShooting playerShooting;
    #endregion


    void Awake()
    {
        playerShooting = GetComponent<PlayerShooting>();
    }
    // -------------------- Main Functions --------------------
    void Start()
    {
        ResetVariables();
        Spawn();
        Time.timeScale = 1f;
        if (otherPlayerCol != null)
            Physics2D.IgnoreCollision(col, otherPlayerCol, true);

    }
    void Update()
    {
        UpdateHealthBar();
        if (isDead || GameManager.instance.isPaused) return;
        HandleJump();
        HandleDropThrough();
        HandleMovement();
        HandleFalling();
    }

    // -------------------- Core Variables --------------------
    public void ResetVariables()
    {
        rb.gravityScale = gravity;
        playerShooting.bulletDamage = 20f;
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

    private void UpdateHealthBar()
    {
        if (healthBar == null) return;

        currentHealthFill = Mathf.Lerp(currentHealthFill, targetHealthFill, Time.deltaTime * healthBarSpeed);
        healthBar.fillAmount = currentHealthFill;
        healthBar.color = Color.Lerp(zeroHealthColor, fullHealthColor, currentHealthFill);
    }

    // -------------------- Player Movement Actions --------------------
    private void HandleJump()
    {
        if (Input.GetKeyDown(jumpKey) && jumpsLeft > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpSpeed);
            jumpsLeft--;
            isInAir = true;
            col.isTrigger = true;
        }
    }

    private void HandleDropThrough()
    {
        if (Input.GetKeyDown(downKey) && !isInAir)
        {
            col.isTrigger = true;
            isInAir = true;
            goingDown = true;
            jumpsLeft = 1;
            Invoke(nameof(EnableCollider), 0.24f);
        }
    }

    private void HandleMovement()
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
    private void FlipSprite(int direction)
    {
        Vector3 scale = spriteHolder.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        spriteHolder.localScale = scale;
    }

    private void HandleFalling()
    {
        if (isInAir && rb.linearVelocity.y <= 0 && !goingDown)
            col.isTrigger = false;

        if (rb.linearVelocity.y < 0 && !isInAir)
        {
            isInAir = true;
            jumpsLeft = 1;
        }
    }

    // -------------------- Collision --------------------
    private void OnCollisionStay2D(Collision2D collision)
    {
        jumpsLeft = maxJumps;
        isInAir = false;
        col.isTrigger = false;
    }

    private void EnableCollider()
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
            FinishRound();
        }
        else
        {
            Spawn();
            FixReload();
        }
    }

    private void FixReload()
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
                spawnPos = new Vector3(Random.Range(-3f, 1.5f), 3f, -0.685f);
            } while (Vector3.Distance(spawnPos, otherPlayer.transform.position) < minSpawnDistance);
        }
        else
        {
            spawnPos = new Vector3(Random.Range(-3f, 1.5f), 3f, -0.685f);
        }

        transform.position = spawnPos;
        rb.linearVelocity = Vector2.zero;
    }

    public void FinishRound()
    {
        GameManager.instance.ChangeState(GameManager.GameState.CardSelecting);
    }
}
