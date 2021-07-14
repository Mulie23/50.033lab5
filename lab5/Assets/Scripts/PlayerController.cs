using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    private AudioSource[] sounds;
    private AudioSource audio1;
    private AudioSource audio2;
    private AudioSource audio3;

    public float speed;
    public float maxSpeed = 50;
    // public Transform enemyLocation;
    // public Text scoreText;
    // private int score = 0;
    // private bool countScoreState = false;
    public float upSpeed = 30;
    private Rigidbody2D marioBody;

    private float moveHorizontal;

    private bool onGroundState = false;
    // Start is called before the first frame update
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;

    private int firstLand = 0;
    private  Animator marioAnimator;


    private ParticleSystem  dustCloud;
    private GameObject cameraManager;

    void  Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate =  30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
        marioAnimator  =  GetComponent<Animator>();
        sounds = GetComponents<AudioSource>();
        audio1 = sounds[0];
        audio2 = sounds[1];
        audio3 = sounds[2];
        dustCloud = GetComponentInChildren<ParticleSystem>();
        GameManager.OnPlayerDeath  +=  PlayerDiesSequence;
        cameraManager = GameObject.Find("Main Camera");

    }


    // FixedUpdate may be called once per frame. See documentation for details.
    void FixedUpdate(){
        // dynamic rigidbody
        float moveHorizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(moveHorizontal) > 0){
            Vector2 movement = new Vector2(moveHorizontal, 0);
            if (marioBody.velocity.magnitude < maxSpeed)
                    marioBody.AddForce(movement * speed);
        }
        if (Input.GetKeyUp("a") || Input.GetKeyUp("d")){
            // stop
            marioBody.velocity = Vector2.zero;
        }
        if (Input.GetKeyDown("space") && onGroundState)
        {
            marioBody.AddForce(Vector2.up * upSpeed, ForceMode2D.Impulse);
            onGroundState = false;
            // countScoreState = true; //check if Gomba is underneath
        }
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground") )
        {
            firstLand += 1;
            if (firstLand > 1)
            {
                dustCloud.Play();
                audio2.PlayOneShot(audio2.clip);
            }
            onGroundState = true; // back on ground
            // countScoreState = false; // reset score state
            // scoreText.text = "Score: " + score.ToString();
        };
        // if (col.gameObject.CompareTag("Enemy"))
        // {
        //     Time.timeScale =0;
        // };
        if (col.gameObject.CompareTag("Obstacles") && Mathf.Abs(marioBody.velocity.y)<0.01f)
        {
            onGroundState = true; 
            marioAnimator.SetBool("onGround", onGroundState);
        };

        if (col.gameObject.CompareTag("Pipe") && Mathf.Abs(marioBody.velocity.y)<0.01f)
        {
            onGroundState = true; 
            marioAnimator.SetBool("onGround", onGroundState);
        };

    }
    void  PlayerDiesSequence(){
        // Mario dies
        Debug.Log("Mario dies");

        StartCoroutine(PlayerDieAnimation());
        this.gameObject.transform.GetComponent<BoxCollider2D>().enabled = false;

        cameraManager.GetComponent<CameraController>().stopBackgroundSound();
        audio3.PlayOneShot(audio3.clip);

    }
    void  PlayJumpSound(){
        audio1.PlayOneShot(audio1.clip);
    }
    void Update(){
    // toggle state
        if (Input.GetKeyDown("a") && faceRightState){
            faceRightState = false;
            marioSprite.flipX = true;
            if (Mathf.Abs(marioBody.velocity.x) >  1.0){
                marioAnimator.SetTrigger("onSkid");
            } 
        }

        if (Input.GetKeyDown("d") && !faceRightState){
            faceRightState = true;
            marioSprite.flipX = false;
            if (Mathf.Abs(marioBody.velocity.x) >  1.0){
                marioAnimator.SetTrigger("onSkid");
            } 
        }
        marioAnimator.SetFloat("xSpeed", Mathf.Abs(marioBody.velocity.x));
        marioAnimator.SetBool("onGround", onGroundState);
        if (Input.GetKeyDown("z")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.Z,this.gameObject);
        }

        if (Input.GetKeyDown("x")){
            CentralManager.centralManagerInstance.consumePowerup(KeyCode.X,this.gameObject);
        }
        // when jumping, and Gomba is near Mario and we haven't registered our score
    //     if (!onGroundState && countScoreState)
    //     {
    //         if (Mathf.Abs(transform.position.x - enemyLocation.position.x) < 0.5f)
    //         {
    //             countScoreState = false;
    //             score++;
    //             Debug.Log(score);
    //         }
    //     }
    //     if (Input.GetKeyDown("r"))
    //     {
    //         SceneManager.LoadScene(0);
    //     }
    // }
    // called when the cube hits the floor
    }
    IEnumerator PlayerDieAnimation(){
		for (int i =  0; i  < 2; i  ++){

			this.transform.position  =  new  Vector3(this.transform.position.x, this.transform.position.y + 1, this.transform.position.z);
			yield  return  null;
		}
		// this.gameObject.SetActive(false);
		Debug.Log("Enemy returned to pool");
        
		yield  break;
	}
}
