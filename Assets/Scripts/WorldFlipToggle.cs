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
        _image = gameObject.GetComponent<Image>();

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
            }

            if (_image != null)
            {
                _image.enabled = true;
            }
      
        }

        if (GameController.instance != null && (GameController.instance.IsInWorldFlip == false || Input.GetKeyDown(KeyCode.X)))  
        {
            // hide if not in world flip.
            if (_renderer != null)
            {
                _renderer.enabled = false;
            }

            if (_image != null)
            {
                _image.enabled = false;
            }
        }
    }

}
