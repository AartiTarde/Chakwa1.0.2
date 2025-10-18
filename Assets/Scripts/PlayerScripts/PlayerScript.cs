//using UnityEngine;
//using Chakwa;

//namespace Chakwa.Player
//{
//    public class PlayerScript : MonoBehaviour
//    {
//        public static PlayerScript Instance;
//        private GameObject lastTrigger = null;
//        private bool waitingForTurnInput = false;
//        private Path path;

//        // Touch support
//        private Vector2 startTouchPosition;
//        private float minSwipeDistance = 50f;

//        public SoundDatabase SoundDatabase;
//        void Awake()
//        {
//            Instance = this;
//        }

//        void Start()
//        {
//            path = Path.Instance;
//        }

//        private void OnTriggerEnter(Collider other)
//        {
//            if (path == null) return;
//            if (other.CompareTag("TileTrigger"))
//            {
//                path.SpawnNextTile();
//                Debug.Log("TileTrigger activated: Spawned next tile.");
//                Destroy(other.gameObject);
//            }

//            else if (other.CompareTag("TIntersectionTrigger"))
//            {
//                waitingForTurnInput = true;

//                Debug.Log("Player reached T intersection - waiting for input (Left or Right arrow).");
//            }
//            if (lastTrigger != null && !waitingForTurnInput)
//            {
//                Destroy(lastTrigger);
//            }
//            lastTrigger = other.gameObject;
//        }

//        void Update()
//        {

//            if (waitingForTurnInput)
//            {
//                if (Input.GetKeyDown(KeyCode.A)) // Player wants to turn LEFT
//                {

//                    Path.Instance.TriggerTurn(true);
//                    SoundManager.Instance.PlaySound(SoundDatabase.playerturnSound);

//                    waitingForTurnInput = false;
//                    Destroy(lastTrigger);
//                    lastTrigger = null;
//                    Debug.Log("Player chose LEFT turn.");

//                }
//                else if (Input.GetKeyDown(KeyCode.D)) // Player wants to turn RIGHT
//                {
//                    SoundManager.Instance.PlaySound(SoundDatabase.playerturnSound);
//                    Path.Instance.TriggerTurn(false);

//                    waitingForTurnInput = false;
//                    Destroy(lastTrigger);
//                    lastTrigger = null;

//                    Debug.Log("Player chose RIGHT turn.");

//                }
//            }
//        }
//    }
//}

//using UnityEngine;
//using UnityEngine.InputSystem; // <--- needed for the new Input System
//using Chakwa;

//namespace Chakwa.Player
//{
//    public class PlayerScript : MonoBehaviour
//    {
//        public static PlayerScript Instance;
//        private GameObject lastTrigger = null;
//        private bool waitingForTurnInput = false;
//        private Path path;

//        public SoundDatabase SoundDatabase;

//        void Awake() => Instance = this;
//        void Start() => path = Path.Instance;

//        private void OnTriggerEnter(Collider other)
//        {
//            if (path == null) return;

//            if (other.CompareTag("TileTrigger"))
//            {
//                path.SpawnNextTile();
//                Destroy(other.gameObject);
//            }
//            else if (other.CompareTag("TIntersectionTrigger"))
//            {
//                waitingForTurnInput = true;
//                Debug.Log("Player reached T intersection - waiting for input (A=Left / D=Right).");
//            }

//            if (lastTrigger != null && !waitingForTurnInput)
//                Destroy(lastTrigger);

//            lastTrigger = other.gameObject;
//        }

//        void Update()
//        {
//            if (!waitingForTurnInput) return;

//            bool leftPressed = Input.GetKeyDown(KeyCode.A) // old input
//                               || (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame); // new input

//            bool rightPressed = Input.GetKeyDown(KeyCode.D) // old input
//                                || (Keyboard.current != null && Keyboard.current.dKey.wasPressedThisFrame); // new input

//            if (leftPressed)
//            {
//                Debug.Log("[DEBUG] Left input detected (hybrid)");
//                path.TriggerTurn(true); // true = left turn
//                SoundManager.Instance.PlaySound(SoundDatabase.playerturnSound);
//                waitingForTurnInput = false;
//                Destroy(lastTrigger); lastTrigger = null;
//                Debug.Log("Player chose LEFT turn.");
//            }
//            else if (rightPressed)
//            {
//                Debug.Log("[DEBUG] Right input detected (hybrid)");
//                path.TriggerTurn(false); // false = right turn
//                SoundManager.Instance.PlaySound(SoundDatabase.playerturnSound);
//                waitingForTurnInput = false;
//                Destroy(lastTrigger); lastTrigger = null;
//                Debug.Log("Player chose RIGHT turn.");
//            }
//        }
//    }
//}
using UnityEngine;
using UnityEngine.InputSystem;
using Chakwa;

namespace Chakwa.Player
{
    public class PlayerScript : MonoBehaviour
    {
        public static PlayerScript Instance;
        private GameObject lastTrigger = null;
        private bool waitingForTurnInput = false;
        private Path path;

       
       

        public SoundDatabase SoundDatabase;

        void Awake() => Instance = this;
        void Start() => path = Path.Instance;

        private void OnTriggerEnter(Collider other)
        {
            if (path == null) return;

            if (other.CompareTag("TileTrigger"))
            {
                path.SpawnNextTile();
                Destroy(other.gameObject);
                return;
            }

            if (other.CompareTag("TIntersectionTrigger"))
            {
                waitingForTurnInput = true;
               
            }

            if (lastTrigger != null && !waitingForTurnInput)
                Destroy(lastTrigger);

            lastTrigger = other.gameObject;
        }

        void Update()
        {
            //if (!waitingForTurnInput) return;

            //bool leftPressed = Input.GetKeyDown(KeyCode.A)
            //                   || (Keyboard.current != null && Keyboard.current.aKey.wasPressedThisFrame);
            //bool rightPressed = Input.GetKeyDown(KeyCode.D)
            //                    || (Keyboard.current != null && Keyboard.current.dKey.wasPressedThisFrame);

            //if (leftPressed)
            //{
            //    bufferedTurnDirection = 1;
            //    Debug.Log("[Turn Buffer] LEFT turn buffered.");
            //}
            //else if (rightPressed)
            //{
            //    bufferedTurnDirection = -1;
            //    Debug.Log("[Turn Buffer] RIGHT turn buffered.");
            //}

            //if (bufferedTurnDirection != 0)
            //{
            //    float distToCenter = Vector3.Distance(transform.position, turnPointCenter);
            //    Debug.Log($"[DEBUG] distToCenter = {distToCenter:F2}, turnPointRadius = {turnPointRadius:F2}");

            //    if (distToCenter <= turnPointRadius)
            //    {
            //        bool turnLeft = bufferedTurnDirection == 1;
            //        path.TriggerTurn(turnLeft);

            //        SoundManager.Instance?.PlaySound(SoundDatabase.playerturnSound);
            //        Debug.Log($"[Auto Turn] Player reached turn point radius. Turned {(turnLeft ? "LEFT" : "RIGHT")}.");

            //        waitingForTurnInput = false;
            //        bufferedTurnDirection = 0;
            //        if (lastTrigger != null)
            //        {
            //            Destroy(lastTrigger);
            //            lastTrigger = null;
            //        }
            //    }
            //}
        }

        void OnDrawGizmosSelected()
        {
            if (waitingForTurnInput)
            {
                Gizmos.color = Color.yellow;
               
            }
        }
    }
}

