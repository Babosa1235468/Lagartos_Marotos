using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

public class PlayerShooting : MonoBehaviour
{
    [Header("UI")]
    public Image reloadFill;
    public Canvas reloadBar;
    public Transform spriteHolder;
    public TMP_Text bulletsTxt;

    [Header("Shooting Settings")]
    public GameObject bullet;
    public float bulletSpeed = 6.5f;
    public float bulletDamage = 25f;
    public KeyCode shootKey = KeyCode.Space;
    public KeyCode reloadKey = KeyCode.R;
    public float fireRate = 0.2f;          // one shot per 0.3s
    public int magazineSize = 5;           // bullets per magazine
    public float reloadTime = 1.2f;

    public int remainingBullets;
    public float nextFireTime;
    public bool isReloading = false;
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
        Debug.Log(bulletDamage);
        if (newBullet.TryGetComponent<BulletHit>(out var hit))
        {
            hit.owner = gameObject;
            hit.dmg = CalculateDamage();
            Debug.Log(hit.dmg);
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
