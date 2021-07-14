using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 0.0f;
    }
    // Start is called before the first frame update
    void Start(){

    }
    public void StartButtonClicked()
    {
        foreach (Transform eachChild in transform)
        {
            if (eachChild.name != "Score" && eachChild.name != "Powerups")
            {
                Debug.Log("Child found. Name: " + eachChild.name);
                // disable them
                eachChild.gameObject.SetActive(false);
                Time.timeScale = 1.0f;
            }
        }
    }
    void Update(){
        
    }
    // Update is called once per frame
}
