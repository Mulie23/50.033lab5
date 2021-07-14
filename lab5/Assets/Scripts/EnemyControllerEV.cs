using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyControllerEV : MonoBehaviour
{
	// events to subscribe
    public UnityEvent onPlayerDeath;
    public UnityEvent onEnemyDeath;
    
    public GameConstants gameConstants;
	private int moveRight;
	private float originalX;
	private Vector2 velocity;
	private Rigidbody2D enemyBody;
    private SpriteRenderer enemySprite;
	private bool isMoving = true;

	private GameObject spawnManager;
	
	void Start()
	{
		enemyBody = GetComponent<Rigidbody2D>();
        enemySprite = GetComponent<SpriteRenderer>();

		// spawnManager = GameObject.Find("EmptySpawnManager");

		// get the starting position
		originalX = transform.position.x;
	
		// randomise initial direction
		moveRight = Random.Range(0, 2) ==  0  ?  -1  :  1;
		
		// compute initial velocity
		ComputeVelocity();


	}
	void correctDirection(){
		if (velocity.x > 0)
        {
            enemySprite.flipX = true;
        }
        if (velocity.x < 0 )
        {
            enemySprite.flipX = false;
        }
	} 
	void ComputeVelocity()
	{
		velocity = new Vector2((moveRight) * gameConstants.maxOffset / gameConstants.enemyPatroltime, 0);
	}
  
	void  MoveEnemy()
	{
		enemyBody.MovePosition(enemyBody.position + velocity * Time.fixedDeltaTime);
	}

	void  Update()
	{
	    correctDirection();
		if (isMoving) {
			if (Mathf.Abs(enemyBody.position.x - originalX) < gameConstants.maxOffset)
			{// move gomba
				MoveEnemy();
			}
			else
			{
				// change direction
				moveRight *= -1;
				ComputeVelocity();
				MoveEnemy();
			}
		}

	}

    void  OnTriggerEnter2D(Collider2D other){
		// check if it collides with Mario
		if (other.gameObject.tag  ==  "Player"){
			// check if collides on top
			float yoffset = (other.transform.position.y - this.transform.position.y);
			if (yoffset  >  0.75f){
				KillSelf();
				// spawnManager.GetComponent<SpawnManagerEV>().spawnNewEnemy();
			}
			else{
				// hurt player
				onPlayerDeath.Invoke();
			}
		}

		if (other.gameObject.CompareTag("Pipe") || other.gameObject.CompareTag("Enemy")) {
            moveRight *= -1;
			ComputeVelocity();
			MoveEnemy();
            Debug.Log("Mushroom collides with Pillar and change direction");
        }
	}

    void  KillSelf(){
		// enemy dies
		onEnemyDeath.Invoke();
		StartCoroutine(flatten());
		Debug.Log("Kill sequence ends");
	}

    IEnumerator flatten(){
		Debug.Log("Flatten starts");
		int steps =  5;
		float stepper =  1.0f/(float) steps;

		for (int i =  0; i  <  steps; i  ++){
			this.transform.localScale  =  new  Vector3(this.transform.localScale.x, this.transform.localScale.y  -  stepper, this.transform.localScale.z);

			// make sure enemy is still above ground
			this.transform.position  =  new  Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
			yield  return  null;
		}
		Debug.Log("Flatten ends");
		this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
		yield  break;
	}

    // animation when player is dead
    public void  PlayerDeathResponse(){
        Debug.Log("Enemy killed Mario");
        // do whatever you want here, animate etc
		isMoving = false;
		InvokeRepeating("FlipXpos", 0, 0.2f);
        
    }

	void FlipXpos() {
        if (enemySprite.flipX) {
            enemySprite.flipX = false;
        } else {
            enemySprite.flipX = true;
        }
    }

}
