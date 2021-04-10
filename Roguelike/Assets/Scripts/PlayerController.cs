using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    [SerializeField]
    private float moveSpeed;

    private Vector2 moveInput;

    private Rigidbody2D theRB;

    public Transform gunArm;

    //private Camera theCam;

    private Animator anim;

    //[SerializeField]
    //private GameObject bulletToFire;

    //[SerializeField]
    //private Transform firePoint;

    //[SerializeField]
    //private float timeBetweenShots;

    //private float shootCounter;

    public SpriteRenderer bodySR;

    private float activeMoveSpeed;

    public float dashSpeed = 8f;

    public float dashLength = 0.5f;

    public float dashCooldown = 1f;

    public float dashInvincibility = 0.5f;

    [HideInInspector]
    public float dashCounter;

    private float dashCoolCounter;

    [HideInInspector]
    public bool canMove = true;

    public List<Gun> availableGuns = new List<Gun>();
    
    [HideInInspector]
    public int currentGun;

    private void Awake()
    {
        instance = this;
        
        DontDestroyOnLoad(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        theRB = GetComponent<Rigidbody2D>();
        //theCam = Camera.main;

        anim = GetComponent<Animator>();
        activeMoveSpeed = moveSpeed;

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && !LevelManager.instance.isPaused)
        {

            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");

            theRB.velocity = moveInput.normalized * activeMoveSpeed;

            Vector3 mousePos = Input.mousePosition;
            Vector3 screenPoint = CameraController.instance.mainCamera.WorldToScreenPoint(transform.localPosition);

            if (mousePos.x < screenPoint.x)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
                gunArm.localScale = new Vector3(-1f, -1f, 1f);
            }
            else
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                gunArm.localScale = new Vector3(1f, 1f, 1f);
            }

            Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            gunArm.rotation = Quaternion.Euler(0, 0, angle);

            //if (Input.GetMouseButtonDown(0))
            //{
            //    Instantiate(bulletToFire, firePoint.position, firePoint.rotation);
            //    shootCounter = timeBetweenShots;
            //    AudioManager.instance.PlaySFX(12);
            //}

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

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if(availableGuns.Count > 0)
                {
                    currentGun++;
                    if(currentGun >= availableGuns.Count)
                    {
                        currentGun = 0;
                    }

                    SwitchGun();
                }
                else
                {
                    Debug.LogError("Player has no guns!");
                }
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (dashCoolCounter <= 0 && dashCounter <= 0)
                {
                    activeMoveSpeed = dashSpeed;
                    dashCounter = dashLength;

                    anim.SetTrigger("dash");

                    PlayerHealthController.instance.MakeInvincible(dashInvincibility);

                    AudioManager.instance.PlaySFX(8);
                }
            }

            if (dashCounter > 0)
            {
                dashCounter -= Time.deltaTime;
                if (dashCounter <= 0)
                {
                    activeMoveSpeed = moveSpeed;
                    dashCoolCounter = dashCooldown;
                }
            }

            if (dashCoolCounter > 0)
            {
                dashCoolCounter -= Time.deltaTime;
            }

            if (moveInput != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
        else
        {
            theRB.velocity = Vector2.zero;
            anim.SetBool("isMoving", false);
        }
    }
    
    public void SwitchGun()
    {
        foreach(Gun theGun in availableGuns)
        {
            theGun.gameObject.SetActive(false);
        }
        availableGuns[currentGun].gameObject.SetActive(true);

        UIController.instance.currentGun.sprite = availableGuns[currentGun].gunUI;
        UIController.instance.gunText.text = availableGuns[currentGun].weaponName;
    }
}
