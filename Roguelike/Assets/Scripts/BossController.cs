using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;

    public BossAction[] actions;

    private int currentAction;

    private float actionCounter;

    private float shotCounter;

    public Rigidbody2D theRB;

    private Vector2 moveDirection;

    public int currentHealth;

    public GameObject deathEffect;

    public GameObject levelExit;

    public GameObject hitEffect;

    public BossSequence[] sequences;
    public int currentSequence;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        actions = sequences[currentSequence].actions;

        actionCounter = actions[currentAction].actionLength;

        UIController.instance.bossHealthBar.maxValue = currentHealth;
        UIController.instance.bossHealthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(actionCounter > 0)
        {
            actionCounter -= Time.deltaTime;

            moveDirection = Vector2.zero;

            if (actions[currentAction].shouldMove)
            {
                if (actions[currentAction].shouldChasePlayer)
                {
                    moveDirection = PlayerController.instance.transform.position - transform.position;
                    moveDirection.Normalize();
                }

                if (actions[currentAction].moveToPoints && Vector3.Distance(transform.position, actions[currentAction].pointToMoveTo.position) > 0.5f)
                {
                    moveDirection = actions[currentAction].pointToMoveTo.position - transform.position;
                    moveDirection.Normalize();
                }
            }

            theRB.velocity = moveDirection * actions[currentAction].moveSpeed;

            if (actions[currentAction].shouldShoot)
            {
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0)
                {
                    shotCounter = actions[currentAction].timeBetweenShots;

                    foreach (Transform t in actions[currentAction].shotPoints)
                    {
                        Instantiate(actions[currentAction].itemToShot, t.position, t.rotation);
                    }
                }
            }
        }
        else
        {
            currentAction++;

            if(currentAction >= actions.Length)
            {
                currentAction = 0;
            }

            actionCounter = actions[currentAction].actionLength;
        }
    }

    public void TakeDamage(int damageAmmount)
    {
        currentHealth -= damageAmmount;

        if(currentHealth <= 0)
        {
            gameObject.SetActive(false);

            Instantiate(deathEffect, transform.position, transform.rotation);

            if(Vector3.Distance(PlayerController.instance.transform.position, levelExit.transform.position) < 2f)
            {
                levelExit.transform.position += new Vector3(4f, 0f, 0f);
            }

            levelExit.SetActive(true);

            UIController.instance.bossHealthBar.gameObject.SetActive(false);
        }
        else
        {
            if (currentHealth <= sequences[currentSequence].endSequenceHealth && currentSequence < sequences.Length - 1)
            {
                currentSequence++;
                actions = sequences[currentSequence].actions;
                currentAction = 0;
                actionCounter = actions[currentAction].actionLength;
            } 
        }

        UIController.instance.bossHealthBar.value = currentHealth;
    }
}

[System.Serializable]
public class BossAction
{
    public float actionLength;
    public bool shouldMove;
    public bool shouldChasePlayer;
    public bool moveToPoints;
    public float moveSpeed;
    public Transform pointToMoveTo;

    public bool shouldShoot;
    public GameObject itemToShot;
    public float timeBetweenShots;
    public Transform[] shotPoints;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]

    public int endSequenceHealth;

    public BossAction[] actions;
}