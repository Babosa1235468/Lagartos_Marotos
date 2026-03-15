using System;
using System.Collections;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerMovement : MonoBehaviour
{

    #region ...[VARIABLES]...  

    [Header("Player Settings")]
    public string playerName = "Player";
    public Collider2D otherPlayerCol;
    public Collider2D otherPlayerFootCol;

    [Header("Controls")]
    public KeyCode leftKey = KeyCode.A;
    public KeyCode rightKey = KeyCode.D;
    public KeyCode jumpKey = KeyCode.W;
    public KeyCode downKey = KeyCode.S;

    [Header("Components")]
    public Rigidbody2D rb;
    public Collider2D col;
    public Collider2D footCol;
    public Transform sprites;
    // public Animator animator;
    public Canvas reloadBar;
    [Header("Effects")]
    public ParticleSystem deathEffectPrefab;
    public ParticleSystem jumpBoostEffect;
    public ParticleSystem speedBoostEffect;
    public GameObject shieldInvulnerability;

    [Header("UI")]
    public Image healthBar;
    public TextMeshProUGUI playerNameTxt;
    public TextMeshProUGUI playerLivesTxt;
    public TextMeshProUGUI playerAboveHeadTxt;

    [Header("Stats (Editable)")]
    public float gravity = 2.3f;

    [Header("Jump Settings")]
    public float jumpSpeed = 6.5f;
    public int maxJumps = 2;

    [Header("Lives & Health")]
    public int maxLives = 3;
    public float maxHealthPoints = 100f;
    public float healthBarSpeed = 5f;

    [Header("Movement Settings")]
    public float currentSpeed = 0f;
    public float acceleration;
    public float deceleration;
    public float maxSpeed;
    public float maxFallSpeed = 1f;

    [Header("Health UI Colors")]
    public Color fullHealthColor = Color.green;
    public Color zeroHealthColor = Color.red;

    [Header("Spawn Settings")]
    public PlayerMovement otherPlayer;          // reference to the other player
    public float minSpawnDistance = 1.5f;       // minimum distance from other player

    // -------------------- public State --------------------
    [Header("Etc")]
    public int jumpsLeft;
    public bool isInAir;
    public bool goingDown;
    public bool isDead;
    public float respawnTimer;

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
        // Determina o número do player pela layer do GameObject
        string layerName = LayerMask.LayerToName(gameObject.layer);
        int player = layerName.Contains("1") ? 1 : 2;
        string LayerUIName = layerName + "_UI";

        // Encontra o outro player
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject p in Players)
        {
            if (p != gameObject)
            {
                otherPlayerCol = p.GetComponent<Collider2D>();
                otherPlayer = p.GetComponent<PlayerMovement>();
                otherPlayerFootCol = p.transform.Find("Foot").GetComponent<Collider2D>();
            }
        }

        playerShooting = GetComponent<PlayerShooting>();

        // Gets UI elements of the player by name
        footCol = transform.Find("Foot").GetComponent<Collider2D>();

        playerAboveHeadTxt = gameObject.transform.Find("PlayerNameCanvas/PlayerName").GetComponent<TextMeshProUGUI>();
        GameObject UIGameObject = GameObject.FindGameObjectWithTag(LayerUIName);
        healthBar = UIGameObject.transform.Find("Health/HealthBar").GetComponent<Image>();
        playerNameTxt = UIGameObject.transform.Find("PlayerName").GetComponent<TextMeshProUGUI>();
        playerLivesTxt = UIGameObject.transform.Find("PlayerLives/Text").GetComponent<TextMeshProUGUI>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        col = gameObject.GetComponent<BoxCollider2D>();
        sprites = gameObject.transform.Find("Sprites");
        // animator = gameObject.GetComponent<Animator>();
        DataManager dataManager = DataManager.instance;
        Color spriteColor = Color.white;
        ;
        if (player == 1)
        {
            if (dataManager != null)
            {
                playerAboveHeadTxt.text = dataManager.P1Name;
                playerName = dataManager.P1Name;
                spriteColor = dataManager.P1SpriteColor;
                jumpKey = dataManager.P1MovementControls[0];
                leftKey = dataManager.P1MovementControls[1];
                downKey = dataManager.P1MovementControls[2];
                rightKey = dataManager.P1MovementControls[3];
                playerShooting.shootKey = dataManager.P1ShootingControls[0];
                playerShooting.reloadKey = dataManager.P1ShootingControls[1];
                gameObject.AddComponent<PlayerManager>();
            }
        }
        else
        {
            if (dataManager != null)
            {
                playerAboveHeadTxt.text = dataManager.P2Name;
                playerName = dataManager.P2Name;
                spriteColor = dataManager.P2SpriteColor;
                jumpKey = dataManager.P2MovementControls[0];
                leftKey = dataManager.P2MovementControls[1];
                downKey = dataManager.P2MovementControls[2];
                rightKey = dataManager.P2MovementControls[3];
                playerShooting.shootKey = dataManager.P2ShootingControls[0];
                playerShooting.reloadKey = dataManager.P2ShootingControls[1];
                if (dataManager.IsAI)
                {
                    //tem de ser por esta ordem. Enemy manager precisa do component do PathFinding
                    gameObject.AddComponent<PathFinding>();
                    gameObject.AddComponent<EnemyManager>();
                }
                else
                {
                    gameObject.AddComponent<PlayerManager>();
                }
            }

        }
        foreach (Transform child in sprites)
        {
            if (!child.CompareTag("DoNotChange"))
            {
                if (child.TryGetComponent(out SpriteRenderer sr))
                    sr.color = spriteColor;
            }
        }


    }

    void Start()
    {
        ResetVariables();
        Spawn();
        Time.timeScale = 1f;
        if (otherPlayerCol != null)
        {
            Physics2D.IgnoreCollision(col, otherPlayerCol, true);
            Physics2D.IgnoreCollision(footCol, otherPlayerCol, true);
            Physics2D.IgnoreCollision(col, otherPlayerFootCol, true);
            Physics2D.IgnoreCollision(footCol, otherPlayerFootCol, true);
            GameObject[] grounds = GameObject.FindGameObjectsWithTag("Ground");
            foreach (GameObject g in grounds)
            {
                Physics2D.IgnoreCollision(col, g.GetComponent<Collider2D>(), true);
            }
        }
    }

    void Update()
    {
        UpdateHealthBar();
        if (isDead) return;

        if (rb.linearVelocityY < -maxFallSpeed)
        {
            rb.linearVelocityY = -maxFallSpeed;
        }
    }

    public void ResetVariables()
    {
        rb.gravityScale = gravity;
        maxLives = 3;
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
        respawnTimer = 3f;

        // Movement
        maxSpeed = 2.8f;
        acceleration = 4f;
        deceleration = 6f;
        currentSpeed = 0f;

        // Scale
        scale = sprites.transform.localScale;
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
            footCol.isTrigger = true;
            isInAir = true;
            goingDown = true;
            jumpsLeft = 1;
            Invoke(nameof(EnableCollider), 0.3f);
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
        // animator.SetFloat("Speed", Math.Abs(currentSpeed));
        rb.linearVelocity = new Vector2(currentSpeed, rb.linearVelocity.y);
    }

    // 1 FOR RIGHT -1 FOR LEFT
    public void FlipSprite(int direction)
    {
        Vector3 scale = sprites.localScale;
        scale.x = Mathf.Abs(scale.x) * direction;
        sprites.localScale = scale;
    }

    public void HandleFalling()
    {
        if (isInAir && rb.linearVelocity.y <= 0 && !goingDown)
        {
            footCol.isTrigger = false;
        }


        if (rb.linearVelocity.y < 0 && !isInAir)
        {
            isInAir = true;
            jumpsLeft = 1;
        }
    }

    public void EnableCollider()
    {
        footCol.isTrigger = false;
        goingDown = false;
    }

    // -------------------- Health & Lives --------------------
    public void Damage(float dmg)
    {
        if (isInvincible || isDead) return;
        currentHealthPoints = Mathf.Max(0, currentHealthPoints - dmg);
        targetHealthFill = (float)currentHealthPoints / maxHealthPoints;
        if (currentHealthPoints <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        currentLives--;
        if (playerLivesTxt != null)
        {
            playerLivesTxt.text = $"{currentLives}";
        }
        playerAboveHeadTxt.gameObject.SetActive(false);

        if (isDead) return;

        changePlayerState(true);

        ParticleSystem DeathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(DeathEffect.gameObject, 2f);

        isDead = true;

        if (currentLives <= 0)
        {
            StartCoroutine(GameOver());
            return;
        }

        StartCoroutine(RespawnCoroutine());
    }
    public IEnumerator GameOver()
    {
        float timeUntilShowScreen = 2f;
        while (timeUntilShowScreen > 0)
        {
            timeUntilShowScreen -= Time.deltaTime;
            yield return null;
        }
        GameManager.instance.EndGame(otherPlayer.playerName);
    }
    public IEnumerator RespawnCoroutine()
    {
        isInvincible = true;
        float timer = respawnTimer;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            yield return null;
        }
        Spawn();
        FixReload();
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
        playerAboveHeadTxt.gameObject.SetActive(true);
        changePlayerState(false);
        gameObject.SetActive(true);
        reloadBar.enabled = false;
        isInvincible = false;
        currentHealthPoints = maxHealthPoints;
        targetHealthFill = 1f;
        currentHealthFill = 1f;

        if (playerLivesTxt != null)
        {
            playerLivesTxt.text = $"{currentLives}";
        }
        if (playerNameTxt != null)
        {
            playerNameTxt.text = $"{playerName}";
        }

        Vector3 spawnPos;
        bool SpawnInGround = false;
        if (otherPlayer != null)
        {
            do
            {
                SpawnInGround = false;
                spawnPos = new Vector3(Random.Range(-2.8f, 1.4f), 5f, -0.685f);

                //VERIFICA SE HA CHAO EM BAIXO
                RaycastHit2D[] hits = Physics2D.RaycastAll(spawnPos, new Vector2(0, -1), 100f, LayerMask.GetMask("Ground"));
                if (hits.Count() > 0)
                {
                    SpawnInGround = true;
                }
            } while ((Vector3.Distance(spawnPos, otherPlayer.transform.position) < minSpawnDistance) || !SpawnInGround);
        }
        else
        {
            spawnPos = new Vector3(Random.Range(-3f, 1.5f), 3f, -0.685f);
        }

        transform.position = spawnPos;
        rb.linearVelocity = Vector2.zero;

        isDead = false;
    }
    public void changePlayerState(bool disable)
    {
        SpriteRenderer[] renderers = sprites.GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in renderers)
        {
            sr.enabled = !disable;
        }
        col.enabled = !disable;
        footCol.enabled = !disable;
        reloadBar.enabled = !disable;

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
        if (SpeedEffectOn)
        {
            CancelInvoke(nameof(EndSpeedBoost));
            maxSpeed -= speedDiference;
        }
        else
        {
            speedBoostEffect.Play();
        }
        SpeedEffectOn = true;
        speedDiference = (maxSpeed * multValue) - maxSpeed;
        maxSpeed += speedDiference;
        Invoke(nameof(EndSpeedBoost), time);
    }
    public void EndSpeedBoost()
    {
        speedBoostEffect.Stop();
        SpeedEffectOn = false;
        maxSpeed -= speedDiference;
    }
    public void ApplyJumpBoost(float multValue, float time)
    {
        if (JumpEffectOn)
        {
            CancelInvoke(nameof(EndJumpBoost));
            jumpSpeed -= jumpDiference;
        }
        else
        {
            jumpBoostEffect.Play();
        }
        JumpEffectOn = true;
        jumpDiference = (jumpSpeed * multValue) - jumpSpeed;
        jumpSpeed += jumpDiference;
        Invoke(nameof(EndJumpBoost), time);
    }
    public void EndJumpBoost()
    {
        jumpBoostEffect.Stop();
        JumpEffectOn = false;
        jumpSpeed -= jumpDiference;
    }
    public void ApplyInvulnerability(float time)
    {
        if (isInvincible)
        {
            CancelInvoke(nameof(EndInvulnerability));
        }
        else
        {
            shieldInvulnerability.SetActive(true);
        }
        isInvincible = true;
        Invoke(nameof(EndInvulnerability), time);
    }
    public void EndInvulnerability()
    {
        shieldInvulnerability.SetActive(false);
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
        targetHealthFill = 1f;
    }
    #endregion
}