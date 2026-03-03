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
    public float bulletDamage = 10f;
    public KeyCode shootKey = KeyCode.Space;
    public KeyCode reloadKey = KeyCode.R;
    public float fireRate = 0.2f;          // one shot per 0.3s
    public int magazineSize = 5;           // bullets per magazine
    public float reloadTime = 1.2f;

    public int remainingBullets;
    public float nextFireTime;
    public bool isReloading = false;

    void Awake() 
    {
        string LayerUIName = string.Empty;
        
        LayerUIName = LayerMask.LayerToName(gameObject.layer) + "_UI";

        // Gets UI elements of the player by name
        GameObject UIGameObject = GameObject.FindGameObjectWithTag(LayerUIName);
        bulletsTxt = UIGameObject.transform.Find("BulletsLeft/Text").GetComponent<TextMeshProUGUI>();
    }
    
    void Start()
    {
        reloadBar.enabled = false;
        remainingBullets = magazineSize;
        bulletsTxt.text = $"{remainingBullets}";
    }

    public void HandleTryReloading()
    {
        if (isReloading) return;

        if (magazineSize != remainingBullets)
        {
            StartCoroutine(Reload());
        }
    }
    
    public void HandleTryShooting()
    {
        if (isReloading) return;

        if (Time.time >= nextFireTime)
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
    
    public IEnumerator Reload()
    {
        isReloading = true;
        reloadBar.enabled = true;

        float elapsed = 0f;

        while (elapsed < reloadTime)
        {
            elapsed += Time.deltaTime;
            if (reloadFill != null)
            {
                reloadFill.fillAmount = Mathf.Clamp01(elapsed / reloadTime);
            }

            yield return null;
        }

        remainingBullets = magazineSize;
        isReloading = false;

        if (reloadFill != null)
        {
            reloadFill.fillAmount = 0f;
        }

        reloadBar.enabled = false;
        bulletsTxt.text = $"{remainingBullets}";
    }

    public void Shoot()
    {
        GameObject newBullet = Instantiate(bullet, spriteHolder.transform.position, Quaternion.identity);
        if (newBullet.TryGetComponent<BulletHit>(out var hit))
        {
            hit.owner = gameObject;
            hit.dmg = bulletDamage;
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
    
    #region ...[Power Up Effects]...
    #region ...[Variables]...
    public float bulletDamageDifference;
    public bool DamageEffectOn = false;
    #endregion
    public void ApplyDamageBoost(float multValue, float time)
    {
        DamageEffectOn = true;
        bulletDamageDifference = (bulletDamage*multValue) - bulletDamage;
        bulletDamage += bulletDamageDifference;
        Invoke(nameof(EndDamageBoost), time);
         
        }
    public void EndDamageBoost()
    {
        bulletDamage -= bulletDamageDifference;
        DamageEffectOn = false;
    }
    #endregion
}
