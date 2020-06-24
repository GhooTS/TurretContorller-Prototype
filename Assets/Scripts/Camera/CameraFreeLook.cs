using UnityEngine;
using UnityEngine.InputSystem;

namespace GTCamera2D
{
    [RequireComponent(typeof(Camera))]
    public class CameraFreeLook : MonoBehaviour
    {
        [Tooltip("main if not set")]
        new public Camera camera;
        public Vector3 Offset = new Vector3(0, 0, -10);

        [Header("Move options")]
        public float speed = 25.0f;
        public bool limitCameraMovement = false;
        public bool countCameraSize = false;
        public MovementLimiter moveLimitation;
        public bool useUnscaledDeltaTime = false;
        private Vector2 playerInput;

        private void Awake()
        {

            if (camera == null)
            {
                camera = GetComponent<Camera>();
            }
        }

        private void LateUpdate()
        {
            Move();
        }

        private void Move()
        {
            Vector3 newPosition = GetPlayerInputs(transform.position, playerInput);

            //Limt camera movement to bounds
            newPosition = GetNewPosition(newPosition);

            //set new camera position
            transform.position = newPosition + Offset;
        }

        public void Move(InputAction.CallbackContext moveInputs)
        {
            playerInput = moveInputs.ReadValue<Vector2>();
        }

        private Vector3 GetNewPosition(Vector3 newPosition)
        {
            if (limitCameraMovement)
            {
                if (countCameraSize)
                {
                    //Get size of orthographic camera
                    Vector2 cameraSize = new Vector2(camera.orthographicSize * camera.aspect, camera.orthographicSize);
                    //if camera size is greater than camera movement bounds, set camera position to the center of the bounds
                    if (cameraSize.x * 2 > moveLimitation.bounds.size.x + moveLimitation.margin
                        || cameraSize.y * 2 > moveLimitation.bounds.size.y + moveLimitation.margin)
                    {
                        newPosition = moveLimitation.bounds.center;
                    }
                    else
                    {
                        newPosition = moveLimitation.GetPosition(newPosition, cameraSize);
                    }
                }
                else
                {
                    newPosition = moveLimitation.GetPosition(newPosition);
                }
            }
            else
            {
                newPosition.z = 0;
            }

            return newPosition;
        }

        private Vector3 GetPlayerInputs(Vector3 newPosition,Vector2 direction)
        {
            var deltaTime = useUnscaledDeltaTime ? Time.unscaledDeltaTime : Time.deltaTime;
            newPosition.x += direction.x * speed * deltaTime;
            newPosition.y += direction.y * speed * deltaTime;
            return newPosition;
        }

        private void OnDrawGizmosSelected()
        {
            moveLimitation.DrawGizmo(Color.green);
        }

    }
}