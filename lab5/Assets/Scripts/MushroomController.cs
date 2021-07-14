using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomController : MonoBehaviour
{
    public Rigidbody2D rigidBody;
    int direction = 1;
    public float speed = 4f;
    bool Move = true;
    private AudioSource mushAudio;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        rigidBody.AddForce(Vector2.up * 20, ForceMode2D.Impulse);
        mushAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (Move)
        {
            rigidBody.velocity = new Vector2(speed * direction, rigidBody.velocity.y);
        }
    }


    void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.CompareTag("Pipe"))
        {
            direction *= -1;
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
