using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    // Config
    [Header("Enemy")]
    [SerializeField] float moveSpeed = 2f;  // Moving Param
    [SerializeField] Vector2 deathKick = new Vector2(2f, 2f);  // When Player Died - Get knockback
    [SerializeField] int Health = 100;

    [Header("Projectile")]
    [SerializeField] float attackSpeed = 1f;  // how fast the shoot move - probably will be static
    [SerializeField] GameObject attackPrefab;
    [SerializeField] float attackFiringDelay = 0.1f; // the delay between shots


    // Local vars

    private Transform target;
    private Vector2 myPos;
    DamageDealer damageDealer;
    GameObject gameManager;
    BoardManager board;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindWithTag("GameManager");

        if (gameManager != null)
            board = gameManager.GetComponent<BoardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        myPos = transform.position;
        transform.position = Vector2.MoveTowards(myPos, target.transform.position, moveSpeed * Time.deltaTime);
        
    }

    private void OnTriggerEnter2D(Collider2D other) // Destroy Prjectile on hit.
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer) {
            Debug.Log("Enter here.");
            return; }
        beingHitRanged(damageDealer);

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

    private void Die() // Actions on Death - Need to add FX, Sound and Score.
    {
        //  FindObjectOfType<GameSession>().AddToScore(scoreValue);
        board.setEnemyCount();
        Destroy(gameObject);
       // GameObject explosion = Instantiate(deathVFX, transform.position, transform.rotation);
      //  Destroy(explosion, durationOfExplosion);
      //  AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
    }
}
