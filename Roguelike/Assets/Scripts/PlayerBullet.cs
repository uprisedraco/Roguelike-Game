using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    [SerializeField]
    private float speed = 7.5f;

    private Rigidbody2D rb;

    [SerializeField]
    private GameObject impactEffect;

    [SerializeField]
    private int damageToGive = 50;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(gameObject);

        AudioManager.instance.PlaySFX(4);


        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyController>().DamageEnemy(damageToGive);
        }

        if(other.tag == "Boss")
        {
            BossController.instance.TakeDamage(damageToGive);
            Instantiate(BossController.instance.hitEffect, transform.position, transform.rotation);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
