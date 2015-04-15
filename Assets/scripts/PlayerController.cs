using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //TODO add inputManager
    public Vector2 moving = new Vector2 ();
    public bool running = false;

    // Use this for initialization
    void Start ()
    {
	
    }
	
    // Update is called once per frame
    void Update ()
    {
        moving.x = moving.y = 0;
        running = false;

        if (Input.GetKey ("right")) {
            moving.x += 1;
        }
        if (Input.GetKey ("left")) {
            moving.x += -1;
        }

        if (Input.GetKeyDown ("up")) {
            moving.y = 1;
        }
        if (Input.GetKeyUp ("up")) {
            moving.y = 2;
        }

        if (Input.GetKey ("left shift")) {
            running = true;
        }
    }
}
