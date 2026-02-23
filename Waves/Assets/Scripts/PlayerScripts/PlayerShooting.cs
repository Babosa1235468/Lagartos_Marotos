using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] public Image reloadFill;
    [SerializeField] public Canvas reloadBar;
    [SerializeField] public Transform spriteHolder;
    [SerializeField] public TMP_Text bulletsTxt;

    [Header("Shooting Settings")]
    [SerializeField] public GameObject bullet;
    [SerializeField] public float bulletSpeed = 5.4f;
    [SerializeField] public float bulletDamage = 20f;
    [SerializeField] public KeyCode shootKey = KeyCode.Space;
    [SerializeField] public KeyCode reloadKey = KeyCode.R;
    [SerializeField] public float fireRate = 0.3f;          // one shot per 0.3s
    [SerializeField] public int magazineSize = 5;           // bullets per magazine
    [SerializeField] public float reloadTime = 1.5f;

    [SerializeField] public int remainingBullets;
    [SerializeField] public float nextFireTime;
    [SerializeField] public bool isReloading = false;
    void Start()
    {
        reloadBar.enabled = false;
        remainingBullets = magazineSize;
        bulletsTxt.text = $"{remainingBullets}";
    }

    void Update()
    {
        if (isReloading) return;
        HandleTryShooting();
        HandleTryReloading();
    }
    private void HandleTryReloading()
    {
        if (Input.GetKeyDown(reloadKey) && magazineSize != remainingBullets)
        {
            StartCoroutine(Reload());
        }
    }
    private void HandleTryShooting()
    {
        if (isReloading) return;

        if (Input.GetKey(shootKey) && Time.time >= nextFireTime)
        {
            if (remainingBullets > 0)
            {
                Shoot();
                remainingBullets--;
                bulletsTxt.text = $"{remainingBullets}";
                nextFireTime = Time.time + fireRate;
            }

            if (remainingBullets <= 0 && !isReloading)
            {
                StartCoroutine(Reload());
            }
        }
    }
    private IEnumerator Reload()
    {
        isReloading = true;
        reloadBar.enabled = true;

        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            if (reloadFill != null)
                reloadFill.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
            yield return null;
        }

        remainingBullets = magazineSize;
        isReloading = false;

        if (reloadFill != null)
            reloadFill.fillAmount = 0f;

        reloadBar.enabled = false;
        bulletsTxt.text = $"{remainingBullets}";
    }

    private void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, spriteHolder.transform.position, Quaternion.identity);

        if (newBullet.TryGetComponent<BulletHit>(out var hit))
        {
            hit.owner = gameObject;
            hit.dmg = CalculateDamage();
        }
            

        if (newBullet.TryGetComponent<BulletMovingScript>(out var bm))
        {
            bm.moveSpeed = bulletSpeed;

            float facing = spriteHolder.transform.localScale.x > 0 ? 1f : -1f;
            bm.moveDirection = new Vector2(facing, 0);

            float angle = Mathf.Atan2(bm.moveDirection.y, bm.moveDirection.x) * Mathf.Rad2Deg;
            newBullet.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
        
    }
    private float CalculateDamage()
    {
        float damage = bulletDamage;
        return damage;
    }
}
