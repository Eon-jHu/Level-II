using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldFlipToggle : MonoBehaviour
{
    private Renderer _renderer;
   // private GameController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();

        if (_renderer == null)
        {
            Debug.LogError("Renderer component not found on this object.");
        }

        //_controller = GetComponent<GameController>();

        //if (_controller == null)
        //{
        //    Debug.LogError("Controller component not found on this object.");
        //}
    }

    // Update is called once per frame
    void Update()
    {
        // show in world flip.
        if (GameController.instance != null && (GameController.instance.IsInWorldFlip == true || Input.GetKeyDown(KeyCode.Z)))
        {
            // show
            _renderer.enabled = true;
        }

        if (GameController.instance != null && (GameController.instance.IsInWorldFlip == false || Input.GetKeyDown(KeyCode.X)))  
        {
            // hide if not in world flip.
            Debug.Log("Entered world flip hiding");
            // hide
            _renderer.enabled = false;
        }
    }
}
