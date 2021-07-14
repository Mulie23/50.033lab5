using System.Collections;
using UnityEngine;

public class RedMushroom : MonoBehaviour, ConsumableInterface
{
    public Rigidbody2D rigidBody;
    int direction = 1;
    public float speed = 4f;
    bool Move = true;
	public Texture t;
    private int index = 1;
    private AudioSource mushAudio;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        mushAudio = GetComponent<AudioSource>();
    }
    private void FixedUpdate()
    {
        if (Move)
        {
            rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);
        }
    }

	public void consumedBy(GameObject player){
		// give player jump boost
		player.GetComponent<PlayerController>().upSpeed  +=  10;
		StartCoroutine(removeEffect(player));
	}

	IEnumerator removeEffect(GameObject player){
		yield return new WaitForSeconds(5.0f);
		player.GetComponent<PlayerController>().upSpeed -= 10;
	}

    void  OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player")){
            // update UI
            CentralManager.centralManagerInstance.addPowerup(t, index, this);
            GetComponent<Collider2D>().enabled = false;
        }
        if (col.gameObject.CompareTag("Pipe"))
        {
            direction *= -1;
        }
    }
}