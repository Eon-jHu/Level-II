using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalGameOver : MonoBehaviour
{

   // public Rigidbody2D playerRb;
    public GameObject gameManager;
    public GameObject gameOverPanel;
    public GameObject restartButton;
    public GameObject errorPanel;

    //public void StopPlayer()
    //{
    //    Debug.Log("Stopped Player");
    //    if (playerRb != null)
    //    {
    //        playerRb.velocity = Vector2.zero;
    //        playerRb.angularVelocity = 0f;
    //        playerRb.isKinematic = true;
    //    }
    //    else
    //    {
    //        Debug.LogError("Player Rigidbody is not assigned.");
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.SetActive(false);
            gameOverPanel.SetActive(true);
            restartButton.SetActive(true);

            Debug.Log("Entered proximity");
            //StopPlayer();
        }
    }

    private IEnumerator LoadLevelAfterDelayCoroutine()
    {
        // Pause for 5 seconds
        yield return new WaitForSeconds(3f);

        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        errorPanel.SetActive(true);
        Debug.Log("Restart Button Pressed");
        StartCoroutine(LoadLevelAfterDelayCoroutine());
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
