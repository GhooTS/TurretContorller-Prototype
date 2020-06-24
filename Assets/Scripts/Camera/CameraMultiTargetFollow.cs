using System.Collections.Generic;
using UnityEngine;

namespace GTCamera2D
{
    public class CameraMultiTargetFollow : MonoBehaviour
    {
        [SerializeField]
        new private Camera camera;
        [SerializeField]
        private List<GameObjectBounds> targets;

        [Header("Zoom Options")]
        public bool enableZooming = true;
        [Range(0, 1)]
        public float zoomSmoothTime = 0.12f;
        [Range(0, 100)]
        public float oneTargetZoom = 7;
        [Range(0, 100)]
        public float minZoom = 3;
        [Range(0, 100)]
        public float maxZoom = 10;
        [Range(0, 100)]
        public float margin = 2;
        public bool marginInZoomRange = false;

        [Header("Move Options")]
        public bool enableMoveing = true;
        public Vector3 offset;
        [Range(0, 1)]
        public float moveSmoothTime = 0.12f;
        public bool followInBounds = false;
        public Transform moveBoundsCenterTarget;
        public MovementLimiter moveLimitation;
        private Vector3 velocity;


        private void Start()
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
        }

        private void LateUpdate()
        {
            MoveAndRotateCamera();
        }

        private void MoveAndRotateCamera()
        {
            if (targets.Count == 0)
                return;

            if (targets.Count == 1)
            {
                FollowTargets(targets[0].transform.position);
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, oneTargetZoom, zoomSmoothTime);
                return;
            }

            Bounds targetBounds = targets[0].GetBounds();
            for (int i = 1; i < targets.Count; i++)
            {
                targetBounds.Encapsulate(targets[i].GetBounds());
            }
            Vector3 moveLocation = targetBounds.center;

            if (enableMoveing)
            {
                if (followInBounds)
                {
                    moveLocation = moveLimitation.GetPosition(moveLocation);
                }
                FollowTargets(moveLocation);
            }
            if (enableZooming)
            {
                if (followInBounds)
                {
                    AdjustZoom(targetBounds.extents.x + Mathf.Abs(moveLocation.x - targetBounds.center.x),
                               targetBounds.extents.y + Mathf.Abs(moveLocation.y - targetBounds.center.y));
                }
                else
                {
                    AdjustZoom(targetBounds.extents.x, targetBounds.extents.y);
                }

            }
        }

        public void AddTarget(GameObjectBounds target)
        {
            if (targets.Contains(target) == false)
            {
                targets.Add(target);
            }
            else
            {
                Debug.Log($"{target.name} already in follow list");
            }
        }

        public void RemoveTarget(GameObjectBounds target)
        {
            targets.Remove(target);
        }

        private void FollowTargets(Vector3 targetLocation)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetLocation + offset, ref velocity, moveSmoothTime);
        }

        private void AdjustZoom(float maxHorizontal, float maxVertical)
        {
            float cameraZoom = Mathf.Max(maxVertical, maxHorizontal * Screen.height / Screen.width);

            if (marginInZoomRange)
            {
                cameraZoom = Mathf.Clamp(cameraZoom + margin, minZoom, maxZoom);
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraZoom, zoomSmoothTime);
            }
            else
            {
                cameraZoom = Mathf.Clamp(cameraZoom, minZoom, maxZoom);
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, cameraZoom + margin, zoomSmoothTime);
            }
        }

        private void OnDrawGizmos()
        {
            if (followInBounds)
            {
                moveLimitation.DrawGizmo(Color.green);
            }
            Vector2 aspectVec = Vector2.zero;
            if (camera == null)
            {
                camera = Camera.main;
            }
            aspectVec.x = camera.pixelWidth / (float)camera.pixelHeight;
            aspectVec.y = 1;

            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(transform.position, aspectVec * (minZoom + margin) * 2);
            if (marginInZoomRange == false)
            {
                Gizmos.DrawWireCube(transform.position, aspectVec * (maxZoom + margin) * 2);
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(transform.position, aspectVec * minZoom * 2);
            Gizmos.DrawWireCube(transform.position, aspectVec * maxZoom * 2);

        }
    }
}