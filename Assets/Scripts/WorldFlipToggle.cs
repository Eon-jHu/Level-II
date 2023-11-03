using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldFlipToggle : MonoBehaviour
{
    private Renderer _renderer;
    private Image _image;

   // private GameController _controller;

    // Start is called before the first frame update
    void Awake()
    {
        _renderer = gameObject.GetComponent<Renderer>();

        if (_renderer == null)
        {
            Debug.LogError("Renderer component not found on " + gameObject.name);
        }

        _image = gameObject.GetComponent<Image>();

        if (_renderer == null)
        {
            Debug.LogError("Image component not found on " + gameObject.name);
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
            if (_renderer != null)
            {
                _renderer.enabled = true;
                Debug.Log(gameObject.name + "'s Renderer has been enabled!");
            }

            if (_image != null)
            {
                _image.enabled = true;
                Debug.Log(gameObject.name + "'s Image has been enabled!");
            }
      
        }

        if (GameController.instance != null && (GameController.instance.IsInWorldFlip == false || Input.GetKeyDown(KeyCode.X)))  
        {
            // hide if not in world flip.
            Debug.Log("Entered world flip hiding");
            // hide
            if (_renderer != null)
            {
                _renderer.enabled = false;
                Debug.Log(gameObject.name + "'s Renderer has not been enabled!");
            }

            if (_image != null)
            {
                _image.enabled = false;
                Debug.Log(gameObject.name + "'s Image has been not enabled!");
            }
        }
    }

}
