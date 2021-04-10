using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private GameObject bulletToFire;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float timeBetweenShots;

    private float shootCounter;

    public string weaponName;

    public Sprite gunUI;

    public int itemCost;

    public Sprite gunShopSprite;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerController.instance.canMove && !LevelManager.instance.isPaused)
        {
            if(shootCounter > 0)
            {
                shootCounter -= Time.deltaTime;
            }
            else
            {
                if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
                {
                    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                    shootCounter = timeBetweenShots;
                    AudioManager.instance.PlaySFX(12);
                }

                //if (Input.GetMouseButton(0))
                //{
                //    shootCounter -= Time.deltaTime;

                //    if (shootCounter <= 0)
                //    {
                //        Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
                //        shootCounter = timeBetweenShots;
                //        AudioManager.instance.PlaySFX(12);
                //    }
                //}
            }
        }
    }
}
