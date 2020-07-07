using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    public float speed;
    private Vector2 playerInput;


    private void Update()
    {
        transform.position += (Vector3)playerInput * speed * Time.deltaTime; 
    }

    public void Move(InputAction.CallbackContext moveInputs)
    {
        playerInput = moveInputs.ReadValue<Vector2>();
    }
}
