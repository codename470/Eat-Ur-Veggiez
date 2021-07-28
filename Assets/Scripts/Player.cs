using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;	//Allows us to use UI.
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // Config
    [Header("Player")]
    [SerializeField] float runSpeed = 5f;  // Moving Param
    [SerializeField] Vector2 deathKick = new Vector2(2f, 2f);  // When Player Died - Get knockback
    [SerializeField] int Health = 100;

    [Header("Projectile")]
    [SerializeField] float attackSpeed = 1f;  // how fast the shoot move - probably will be static
    [SerializeField] GameObject attackPrefab;
    [SerializeField] float attackFiringDelay = 0.1f; // the delay between shots
    [SerializeField] int attackDamage = 50; // the delay between shots


    [Header("vs Enemy")]
    [SerializeField] Vector2 playerKnockback = new Vector2(0.2f, 0.2f); // When Hit by an enemy - To be completed

    public Joystick joystick;

    // Coroutines 
    Coroutine firingCoroutine; // Attack of the Player

    // Cached component references
    Rigidbody2D myRigidBody;
    Animator myAnimator;
    CapsuleCollider2D myBodyCollider;

    void Start()
    {
        // Getting comps
        myRigidBody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
    #if UNITY_STANDALONE || UNITY_WEBPLAYER || UNITY_EDITOR
        RunMobile();
    #else
        RunMobile();
    #endif
        FlipSprite();
        Fire();

    }

    private void Fire() // I want to change it to auto
    {
        if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
        {
                firingCoroutine = StartCoroutine(FireContinuously());
        }
        if (Input.GetButtonUp("Fire1") || Input.GetButtonUp("Fire2"))
        {
            StopAllCoroutines(); // Check it
        }

    }

    IEnumerator FireContinuously()
    {
            while (true)
            {
                GameObject attack = Instantiate(
                        attackPrefab,
                        transform.position,
                        Quaternion.identity) as GameObject;
                attack.GetComponent<DamageDealer>().SetDamage(attackDamage);
                if (Input.GetKey("up") )
                {
                    attack.GetComponent<Rigidbody2D>().velocity = new Vector2(0, attackSpeed);
                }
                else if (Input.GetKey("down"))
                {
                    attack.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -attackSpeed);
                }
                else if (Input.GetKey("left") )
                {
                        attack.GetComponent<Rigidbody2D>().velocity = new Vector2(-attackSpeed, 0);
                        attack.GetComponent<Rigidbody2D>().transform.Rotate(0f, 0f, 90f);
                }
                else if (Input.GetKey("right"))
                {

                        attack.GetComponent<Rigidbody2D>().velocity = new Vector2(attackSpeed, 0);
                        attack.GetComponent<Rigidbody2D>().transform.Rotate(0f, 0f, 90f);

                }
                yield return new WaitForSeconds(attackFiringDelay);
            }
        }
       
   

    private void FlipSprite()
    {
        // Check the direction of player and flip accordingly - only left and right
        bool playerHasHorizontalSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidBody.velocity.x), 1f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        beingHitRanged(damageDealer);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) { return; }
        beingHitMelee(damageDealer);
    }

    private void beingHitMelee(DamageDealer dmgDealer)
    {

        Health -= dmgDealer.GetDamage();


        if (Health <= 0)
        {
            Die();
        }

    }

    private void beingHitRanged(DamageDealer dmgDealer)
    {

        Health -= dmgDealer.GetDamage();
        dmgDealer.Hit();

        if (Health <= 0)
        {
            Die();
        }

    }


    private void Run()
    {
        //x Dir move
        float xDir = CrossPlatformInputManager.GetAxis("Horizontal"); // Value between -1 to 1
        Vector2 playerXmove = new Vector2(xDir * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerXmove;
        bool playerHorizonSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;


        //y Dir Move
        float yDir = CrossPlatformInputManager.GetAxis("Vertical"); // Value between -1 to 1
        Vector2 playerYmove = new Vector2(myRigidBody.velocity.x, yDir * runSpeed);
        myRigidBody.velocity = playerYmove;
        bool playerVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        //Start Walk Animation - Need to fix
        if (myRigidBody.velocity.x > Mathf.Epsilon || myRigidBody.velocity.x < -(Mathf.Epsilon))
        {
            myAnimator.SetBool("Move", playerHorizonSpeed);
        }
        else if (myRigidBody.velocity.y > Mathf.Epsilon || myRigidBody.velocity.y < -(Mathf.Epsilon))
            myAnimator.SetBool("Move", playerVerticalSpeed);
        else
            myAnimator.SetBool("Move", false);

    }

    private void RunMobile()
    {
        //x Dir move
        float xDir = joystick.Horizontal; // Value between -1 to 1
        Vector2 playerXmove = new Vector2(xDir * runSpeed, myRigidBody.velocity.y);
        myRigidBody.velocity = playerXmove;
        bool playerHorizonSpeed = Mathf.Abs(myRigidBody.velocity.x) > Mathf.Epsilon;


        //y Dir Move
        float yDir = joystick.Vertical; // Value between -1 to 1
        Vector2 playerYmove = new Vector2(myRigidBody.velocity.x, yDir * runSpeed);
        myRigidBody.velocity = playerYmove;
        bool playerVerticalSpeed = Mathf.Abs(myRigidBody.velocity.y) > Mathf.Epsilon;

        //Start Walk Animation - Need to fix
        if (myRigidBody.velocity.x > Mathf.Epsilon || myRigidBody.velocity.x < -(Mathf.Epsilon))
        {
            myAnimator.SetBool("Move", playerHorizonSpeed);
        }
        else if (myRigidBody.velocity.y > Mathf.Epsilon || myRigidBody.velocity.y < -(Mathf.Epsilon))
            myAnimator.SetBool("Move", playerVerticalSpeed);
        else
            myAnimator.SetBool("Move", false);

    }

    private void Die() // Actions on Death - Need to add FX, Sound and Score.
    {
        //  FindObjectOfType<GameSession>().AddToScore(scoreValue);
        gameObject.SetActive(false);
        myAnimator.SetTrigger("Death");
        GetComponent<Rigidbody2D>().velocity = deathKick;

        GameManager.instance.GameOver();

        // GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
        //  Destroy(explosion, durationOfExplosion);
        //  AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }

}
