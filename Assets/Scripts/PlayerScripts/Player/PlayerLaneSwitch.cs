//using UnityEngine;
//using System.Collections;
//using UnityEngine.InputSystem;

//namespace Chakwa.Player
//{
//    public class PlayerLaneSwitch : MonoBehaviour, PlayerControls.IPlayerActions
//    {
//        [Header("Tile Detection")]
//        public Tile currentTile;
//        public float raycastDistance = 20f;

//        [Header("Lane Settings")]
//        public Lane currentLane = Lane.Center;
//        public float laneSwitchSpeed = 8f;
//        public float laneSnapDistance = 0.05f;

//        [Header("Swipe Settings")]
//        public float minSwipeDistance = 80f;

//        private PlayerControls controls;
//        private Vector2 swipeDelta;
//        private bool isSwitching = false;
//        private Vector3 targetPosition;
//        private Vector3 lateralMove = Vector3.zero;

//        // Exposed for PlayerMovement
//        public Vector3 GetLateralMoveDelta() => lateralMove;

//        private void Awake()
//        {
//            controls = new PlayerControls();
//            controls.Player.SetCallbacks(this); // Connect input callbacks
//        }

//        private void OnEnable() => controls.Player.Enable();
//        private void OnDisable() => controls.Player.Disable();

//        private void Start()
//        {
//            TryDetectTile();

//            if (currentTile != null)
//            {
//                targetPosition = currentTile.GetLaneWorldPosition(currentLane);
//                transform.position = targetPosition;
//                Debug.Log($"✅ PlayerLaneSwitch initialized on tile {currentTile.name}");
//            }
//        }

//        private void Update()
//        {
//            TryDetectTile();
//            FollowLanePosition();

//            // Editor test keys
//            if (Keyboard.current != null)
//            {
//                if (Keyboard.current.leftArrowKey.wasPressedThisFrame) MoveLeft();
//                if (Keyboard.current.rightArrowKey.wasPressedThisFrame) MoveRight();
//            }
//        }

//        // --------------------------------------------------------------------
//        // 🔹 Input Action Callbacks (from PlayerControls)
//        // --------------------------------------------------------------------

//        public void OnSwipe(InputAction.CallbackContext ctx)
//        {
//            swipeDelta = ctx.ReadValue<Vector2>();
//            if (!ctx.performed) return;

//            if (swipeDelta.magnitude < minSwipeDistance) return;

//            bool horizontal = Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y);

//            if (horizontal)
//            {
//                if (swipeDelta.x > 0)
//                {
//                    MoveRight();
//                    Debug.Log("👉 Swipe RIGHT detected (Input System)");
//                }
//                else
//                {
//                    MoveLeft();
//                    Debug.Log("👈 Swipe LEFT detected (Input System)");
//                }
//            }
//        }

//        public void OnJump(InputAction.CallbackContext ctx) { }
//        public void OnTouchDelta(InputAction.CallbackContext ctx) { }
//        public void OnTurnLeft(InputAction.CallbackContext ctx) { }
//        public void OnTurnRight(InputAction.CallbackContext ctx) { }
//        public void OnTiltMove(InputAction.CallbackContext ctx) { }

//        // --------------------------------------------------------------------
//        // 🔹 Lane Switching Logic (Z-axis movement)
//        // --------------------------------------------------------------------
//        public void MoveLeft()
//        {
//            if (isSwitching || currentTile == null) return;
//            Lane newLane = (Lane)Mathf.Clamp((int)currentLane - 1, -1, 1);
//            StartLaneChange(newLane);
//        }

//        public void MoveRight()
//        {
//            if (isSwitching || currentTile == null) return;
//            Lane newLane = (Lane)Mathf.Clamp((int)currentLane + 1, -1, 1);
//            StartLaneChange(newLane);
//        }

//        private void StartLaneChange(Lane newLane)
//        {
//            if (currentTile == null || newLane == currentLane) return;

//            currentLane = newLane;
//            targetPosition = currentTile.GetLaneWorldPosition(currentLane);

//            // ✅ Move only along Z axis (keep X constant)
//            targetPosition.x = transform.position.x;
//            targetPosition.y = transform.position.y;

//            Debug.Log($"➡️ Switching to {currentLane} | Target={targetPosition}");
//            StopAllCoroutines();
//            StartCoroutine(SmoothLaneMove());
//        }

//        private IEnumerator SmoothLaneMove()
//        {
//            isSwitching = true;

//            Vector3 startPos = transform.position;
//            Vector3 endPos = targetPosition;

//            float elapsed = 0f;
//            float duration = Mathf.Max(0.05f, Vector3.Distance(startPos, endPos) / laneSwitchSpeed);

//            while (elapsed < duration)
//            {
//                elapsed += Time.deltaTime;
//                Vector3 newPos = Vector3.Lerp(startPos, endPos, elapsed / duration);

//                // Only allow Z-axis movement
//                newPos.x = transform.position.x;
//                newPos.y = transform.position.y;

//                // Calculate movement delta for CharacterController
//                lateralMove = newPos - transform.position;
//                transform.position = newPos;

//                yield return null;
//            }

//            transform.position = endPos;
//            lateralMove = Vector3.zero;
//            isSwitching = false;
//        }

//        private void FollowLanePosition()
//        {
//            if (isSwitching || currentTile == null) return;

//            Vector3 correct = currentTile.GetLaneWorldPosition(currentLane);
//            correct.x = transform.position.x;
//            correct.y = transform.position.y;

//            if (Vector3.Distance(targetPosition, correct) > 0.05f)
//                targetPosition = correct;
//        }

//        // --------------------------------------------------------------------
//        // 🔹 Tile Detection (Raycast Down)
//        // --------------------------------------------------------------------
//        private void TryDetectTile()
//        {
//            Vector3 origin = transform.position + Vector3.up * 2f;

//            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance))
//            {
//                Tile detected = hit.collider.GetComponentInParent<Tile>();
//                if (detected != null && detected != currentTile)
//                {
//                    currentTile = detected;
//                    targetPosition = currentTile.GetLaneWorldPosition(currentLane);

//                    targetPosition.x = transform.position.x;
//                    targetPosition.y = transform.position.y;

//                    Debug.Log($"🟩 Detected Tile: {detected.name}");
//                }
//            }
//        }
//    }
//}

/*
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace Chakwa.Player
{
    public class PlayerLaneSwitch : MonoBehaviour, PlayerControls.IPlayerActions
    {
        [Header("Tile Detection")]
        public Tile currentTile;
        public float raycastDistance = 20f;

        [Header("Lane Settings")]
        public Lane currentLane = Lane.Center; // Left, Center, Right (but forward/backward now)
        public float laneSwitchSpeed = 8f;
        public float laneSnapDistance = 0.05f;
        public float laneOffset = 2.5f; // Z offset between lanes

        [Header("Touch Settings")]
        public float tapDetectionCooldown = 0.15f;

        private PlayerControls controls;
        private bool isSwitching = false;
        private Vector3 targetPosition;
        private Vector3 forwardMove = Vector3.zero;
        private float lastTapTime = 0f;

        private void Awake()
        {
            controls = new PlayerControls();
            controls.Player.SetCallbacks(this);
        }

        private void OnEnable() => controls.Player.Enable();
        private void OnDisable() => controls.Player.Disable();

        private void Start()
        {
            TryDetectTile();

            if (currentTile != null)
            {
                targetPosition = GetLaneWorldPosition(currentLane);
                transform.position = targetPosition;
                Debug.Log($" PlayerLaneSwitch initialized on tile {currentTile.name}");
            }
        }

        private void Update()
        {
            TryDetectTile();
            FollowLanePosition();
            HandleTapLaneSwitch();

            // Editor test keys
            if (Keyboard.current != null)
            {
                if (Keyboard.current.upArrowKey.wasPressedThisFrame) MoveForwardLane();
                if (Keyboard.current.downArrowKey.wasPressedThisFrame) MoveBackwardLane();
            }
        }

        private void HandleTapLaneSwitch()
        {
            if (Touchscreen.current == null) return;

            var touch = Touchscreen.current.primaryTouch;
            if (!touch.press.wasPressedThisFrame) return;
            if (Time.time - lastTapTime < tapDetectionCooldown) return;

            Vector2 tapPosition = touch.position.ReadValue();
            float screenHeight = Screen.height;
            float sectionHeight = screenHeight / 3f;

            // Bottom third = backward lane
            if (tapPosition.y < sectionHeight)
            {
                MoveBackwardLane();
                Debug.Log(" Tap detected in BACK lane zone");
            }
            // Middle third = center lane
            else if (tapPosition.y < sectionHeight * 2f)
            {
                MoveCenterLane();
                Debug.Log(" Tap detected in CENTER lane zone");
            }
            // Top third = forward lane
            else
            {
                MoveForwardLane();
                Debug.Log(" Tap detected in FORWARD lane zone");
            }

            lastTapTime = Time.time;
        }

        // --------------------------------------------------------------------
        // 🔹 Lane Switching
        // --------------------------------------------------------------------
        public void MoveForwardLane()
        {
            if (isSwitching) return;
            Lane newLane = Lane.Right; // reuse enum — “Right” = forward
            StartLaneChange(newLane);
        }

        public void MoveCenterLane()
        {
            if (isSwitching) return;
            Lane newLane = Lane.Center;
            StartLaneChange(newLane);
        }

        public void MoveBackwardLane()
        {
            if (isSwitching) return;
            Lane newLane = Lane.Left; // reuse enum — “Left” = backward
            StartLaneChange(newLane);
        }

        private void StartLaneChange(Lane newLane)
        {
            if (currentTile == null || newLane == currentLane) return;

            currentLane = newLane;
            targetPosition = GetLaneWorldPosition(currentLane);

            Debug.Log($"➡️ Switching to {currentLane} | Target={targetPosition}");
            StopAllCoroutines();
            StartCoroutine(SmoothLaneMove());
        }

        private IEnumerator SmoothLaneMove()
        {
            isSwitching = true;

            Vector3 startPos = transform.position;
            Vector3 endPos = targetPosition;

            float elapsed = 0f;
            float duration = Mathf.Max(0.05f, Vector3.Distance(startPos, endPos) / laneSwitchSpeed);

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                Vector3 newPos = Vector3.Lerp(startPos, endPos, elapsed / duration);

                // Lock x & y to prevent vertical drift
                newPos.x = transform.position.x;
                newPos.y = transform.position.y;

                transform.position = newPos;
                yield return null;
            }

            transform.position = endPos;
            isSwitching = false;
        }

        private void FollowLanePosition()
        {
            if (isSwitching) return;

            Vector3 correct = GetLaneWorldPosition(currentLane);
            correct.x = transform.position.x;
            correct.y = transform.position.y;

            if (Vector3.Distance(targetPosition, correct) > laneSnapDistance)
                targetPosition = correct;
        }

        // --------------------------------------------------------------------
        // 🔹 Tile Detection
        // --------------------------------------------------------------------
        private void TryDetectTile()
        {
            Vector3 origin = transform.position + Vector3.up * 2f;

            if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, raycastDistance))
            {
                Tile detected = hit.collider.GetComponentInParent<Tile>();
                if (detected != null && detected != currentTile)
                {
                    currentTile = detected;
                    targetPosition = GetLaneWorldPosition(currentLane);
                    targetPosition.x = transform.position.x;
                    targetPosition.y = transform.position.y;
                    Debug.Log($"🟩 Detected Tile: {detected.name}");
                }
            }
        }

        // --------------------------------------------------------------------
        // 🔹 Lane Position on Z Axis
        // --------------------------------------------------------------------
        private Vector3 GetLaneWorldPosition(Lane lane)
        {
            Vector3 basePos = currentTile != null ? currentTile.transform.position : transform.position;
            float zOffset = 0f;

            switch (lane)
            {
                case Lane.Left: zOffset = -laneOffset; break; // backward
                case Lane.Center: zOffset = 0f; break;
                case Lane.Right: zOffset = laneOffset; break; // forward
            }

            return basePos + transform.forward * zOffset;
        }

        // --------------------------------------------------------------------
        // 🔹 Input Action Callbacks
        // --------------------------------------------------------------------
        public void OnSwipe(InputAction.CallbackContext ctx) { }
        public void OnJump(InputAction.CallbackContext ctx) { }
        public void OnTouchDelta(InputAction.CallbackContext ctx) { }
        public void OnTurnLeft(InputAction.CallbackContext ctx) { }
        public void OnTurnRight(InputAction.CallbackContext ctx) { }
        public void OnTiltMove(InputAction.CallbackContext ctx) { }
    }
}
*/