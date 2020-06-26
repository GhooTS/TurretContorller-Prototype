﻿using UnityEngine;
using UnityEngine.InputSystem;

public class LocationTargetSelector : MonoBehaviour
{
    private Unit actionSource;
    private ActionContainer actionContainer;
    private bool isActive = false;
    private Vector2 location;
    new private Camera camera;

    private void Awake()
    {
        camera = Camera.main;
    }


    private void Update()
    {
        if (isActive == false) return;

        location = camera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        actionContainer.UpdateView(location);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Applay();
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            CancelAction();
        }
    }

    public void Applay()
    {
        actionSource.EnqueueAction(actionContainer.GetQueueAction(new ActionTarget { targetLocation = location }));
        Deactivate();
    }

    public void Activate(Unit actionSource, ActionContainer actionContainer)
    {
        this.actionSource = actionSource;
        this.actionContainer = actionContainer;
        actionContainer.ActiveView(actionSource);
        isActive = true;
    }

    private void Deactivate()
    {
        actionContainer.ClearView();
        actionSource = null;
        actionContainer = null;
        isActive = false;
    }

    private void CancelAction()
    {
        Deactivate();
    }

}