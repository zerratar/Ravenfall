using UnityEngine;
using System.Collections;

namespace StylizedGrassDemo
{
    using UnityEngine;

    [RequireComponent(typeof(Camera))]
    public class OrbitCamera : MonoBehaviour
    {
        [SerializeField] private UIManager uiManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private Transform focus = default;

        [SerializeField, Range(0f, 1f)] private float focusCentering = 0.5f;
        [SerializeField, Range(1f, 20f)] private float distance = 5f;
        [SerializeField, Min(0f)] private float focusRadius = 1f;
        [SerializeField, Range(1f, 360f)] private float rotationSpeed = 90f;
        [SerializeField, Range(-89f, 89f)]
        private float minVerticalAngle = -30f, maxVerticalAngle = 60f;
        [SerializeField, Min(0f)] private float alignDelay = 5f;
        [SerializeField, Range(0f, 90f)] private float alignSmoothRange = 45f;
        [SerializeField] private float distanceMin = 5f;
        [SerializeField] private float distanceMax = 20f;
        [SerializeField] private float zoomSpeed = 2f;

        public Vector3 focusPoint, previousFocusPoint;

        Vector2 orbitAngles = new Vector2(45f, 0f);

        float lastManualRotationTime;

        void Awake()
        {
            if (!uiManager) uiManager = FindObjectOfType<UIManager>();
            if (!playerManager) playerManager = FindObjectOfType<PlayerManager>();
            //focusPoint = focus.position;

            transform.localRotation = Quaternion.Euler(orbitAngles);
        }

        void OnValidate()
        {
            if (maxVerticalAngle < minVerticalAngle)
            {
                maxVerticalAngle = minVerticalAngle;
            }
        }

        void ConstrainAngles()
        {
            orbitAngles.x =
                Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

            if (orbitAngles.y < 0f)
            {
                orbitAngles.y += 360f;
            }
            else if (orbitAngles.y >= 360f)
            {
                orbitAngles.y -= 360f;
            }
        }

        void LateUpdate()
        {
            if (!focus)
            {
                if (playerManager && playerManager.Me) focus = playerManager.Me.transform;
                if (focus) focusPoint = focus.position;
                return;
            }

            if (!uiManager.IsMouseOverUI() && Mathf.Abs(Input.mouseScrollDelta.y) > 0)
            {
                distance -= Input.mouseScrollDelta.y * Time.deltaTime * zoomSpeed;
                distance = Mathf.Max(distanceMin, Mathf.Min(distanceMax, distance));
            }

            UpdateFocusPoint();
            Quaternion lookRotation;
            if (ManualRotation() || AutomaticRotation())
            {
                ConstrainAngles();
                lookRotation = Quaternion.Euler(orbitAngles);
            }
            else
            {
                lookRotation = transform.localRotation;
            }

            Vector3 lookDirection = lookRotation * Vector3.forward;
            Vector3 lookPosition = focusPoint - lookDirection * distance;
            transform.SetPositionAndRotation(lookPosition, lookRotation);
        }
        bool ManualRotation()
        {
            if (uiManager.HasFocus)
            {
                return false;
            }

            Vector2 input = new Vector2(
                Input.GetAxis("Vertical"),
                Input.GetAxis("Horizontal")
            );
            const float e = 0.001f;
            if (input.x < -e || input.x > e || input.y < -e || input.y > e)
            {
                orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
                lastManualRotationTime = Time.unscaledTime;
                return true;
            }
            if (input.x < e || input.x > e || input.y < e || input.y > e)
            {
                orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
                return true;
            }
            return false;
        }
        bool AutomaticRotation()
        {
            if (Time.unscaledTime - lastManualRotationTime < alignDelay)
            {
                return false;
            }
            Vector2 movement = new Vector2(
                focusPoint.x - previousFocusPoint.x,
                focusPoint.z - previousFocusPoint.z
            );
            float movementDeltaSqr = movement.sqrMagnitude;
            if (movementDeltaSqr < 0.000001f)
            {
                return false;
            }
            float headingAngle = GetAngle(movement / Mathf.Sqrt(movementDeltaSqr));
            float deltaAbs = Mathf.Abs(Mathf.DeltaAngle(orbitAngles.y, headingAngle));
            float rotationChange = rotationSpeed * Mathf.Min(Time.unscaledDeltaTime, movementDeltaSqr);
            if (deltaAbs < alignSmoothRange)
            {
                rotationChange *= deltaAbs / alignSmoothRange;
            }
            else if (180f - deltaAbs < alignSmoothRange)
            {
                rotationChange *= (180f - deltaAbs) / alignSmoothRange;
            }
            orbitAngles.y = Mathf.MoveTowardsAngle(orbitAngles.y, headingAngle, rotationChange);
            return true;
        }
        static float GetAngle(Vector2 direction)
        {
            float angle = Mathf.Acos(direction.y) * Mathf.Rad2Deg;
            return direction.x < 0f ? 360f - angle : angle;
        }

        void UpdateFocusPoint()
        {
            previousFocusPoint = focusPoint;
            Vector3 targetPoint = focus.position;
            if (focusRadius > 0f)
            {
                float distance = Vector3.Distance(targetPoint, focusPoint);
                if (distance > focusRadius)
                {
                    focusPoint = Vector3.Lerp(
                        targetPoint, focusPoint, focusRadius / distance
                    );
                }
                if (distance > 0.01f && focusCentering > 0f)
                {
                    focusPoint = Vector3.Lerp(
                        targetPoint, focusPoint,
                        Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime)
                    );
                }
            }
            else
            {
                focusPoint = targetPoint;
            }
        }
    }
}