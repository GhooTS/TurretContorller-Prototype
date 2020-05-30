﻿using UnityEngine;
using UnityEngine.InputSystem;

public class AgentTest : MonoBehaviour
{
    public Agent agent;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            agent.SetDestination(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
        }
    }
}
