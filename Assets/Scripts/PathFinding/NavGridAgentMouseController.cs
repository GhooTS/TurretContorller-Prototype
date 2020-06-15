using UnityEngine;
using UnityEngine.InputSystem;

namespace Nav2D.TestUtility
{
    public class NavGridAgentMouseController : MonoBehaviour
    {
        public NavGridAgent agent;

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                agent.SetDestination(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()));
            }
        }
    }
}