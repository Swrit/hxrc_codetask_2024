using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script handles player input.
/// Accessed via static instance (InputManager.Instance)
/// </summary>
public class InputManager : MonoBehaviour
{
    //The static instance to access InputManager
    static public InputManager Instance { get; private set; }

    //This event is invoked when player clicks left mouse button
    public event EventHandler OnLeftMouseClick;

    private void Awake()
    {
        //Set static instance
        if (Instance == null) Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        //If player clicks mouse, invoke the event
        if (Input.GetMouseButtonDown(0)) OnLeftMouseClick?.Invoke(this, EventArgs.Empty);
    }
}
