using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinPressE : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Recognised collision");
        // check world flip is on
        if (GameController.instance != null && (GameController.instance.IsInWorldFlip == true))
        {
            Debug.Log("In world flip");
            // check to make sure specific objects
            if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("TimmyBody"))
            {
                Debug.Log("Succesful tags");
                // check for key press
                Debug.Log("Trying to load scene");
                // load end scene
                SceneManager.LoadScene("GameWin");
            }
        }

    }
}
