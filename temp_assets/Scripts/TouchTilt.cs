/*using UnityEngine;
using UnityEngine.InputSystem;

public class TouchTilt : MonoBehaviour
{
    private PlayerControls controls;
    private Vector3 accelValue;
    public float moveSpeed = 5f;

    private float minX, maxX;  // world bounds for horizontal movement

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player1.TiltMove.performed += ctx =>
            accelValue = ctx.ReadValue<Vector3>();

        controls.Player1.TiltMove.canceled += ctx =>
            accelValue = Vector3.zero;

        if (Accelerometer.current != null && !Accelerometer.current.enabled)
            InputSystem.EnableDevice(Accelerometer.current);
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Start()
    {
        // Calculate horizontal world bounds based on camera and screen size
        Camera cam = Camera.main;

        // Bottom left corner of screen at z = player z pos
        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(0, 0, Mathf.Abs(cam.transform.position.z - transform.position.z)));
        // Top right corner of screen
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Mathf.Abs(cam.transform.position.z - transform.position.z)));

        // Set horizontal min/max limits
        minX = bottomLeft.x;
        maxX = topRight.x;
    }

    void Update()
    {
        float tiltX = Mathf.Clamp(accelValue.x, -1f, 1f);

        Vector3 move = new Vector3(tiltX * moveSpeed * Time.deltaTime, 0f, 0f);
        transform.Translate(move);

        // Clamp player x between screen bounds
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        transform.position = pos;
    }
}
*/
/*
// PlayerTiltZAxis.cs
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchTilt : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 5f;  // Movement speed
    [SerializeField] private float deadZone = 0.1f;       // Small tilt ignored

    private PlayerControls controls;
    private Vector3 accel;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Player1.TiltMove.performed += ctx => accel = ctx.ReadValue<Vector3>();
        controls.Player1.TiltMove.canceled += ctx => accel = Vector3.zero;

        if (Accelerometer.current != null && !Accelerometer.current.enabled)
            InputSystem.EnableDevice(Accelerometer.current);

        Debug.Log("TouchTilt Awake()");
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        float tilt = accel.x;

        // Apply dead zone to avoid drift
        if (Mathf.Abs(tilt) < deadZone) tilt = 0f;

        // Move along Z based on tilt
        Vector3 move = Vector3.forward * tilt * horizontalSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);
        Debug.Log("TouchTilt Update(), accel.x=" + accel.x);
    }
}
*/
/*
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchTilt : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 5f;
    [SerializeField] private float deadZone = 0.1f;
    private PlayerControls controls;
    private Vector3 accel;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Player1.TiltMove.performed += ctx => accel = ctx.ReadValue<Vector3>();
        controls.Player1.TiltMove.canceled += ctx => accel = Vector3.zero;

        if (Accelerometer.current != null && !Accelerometer.current.enabled)
            InputSystem.EnableDevice(Accelerometer.current);

        Debug.Log("TouchTilt Awake()");
    }

    void OnEnable() => controls.Enable();
    void OnDisable() => controls.Disable();

    void Update()
    {
        float tilt = accel.x;
        tilt = -tilt;
        if (Mathf.Abs(tilt) < deadZone)
            tilt = 0f;

        Vector3 move = Vector3.forward * tilt * horizontalSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        Debug.Log($"TouchTilt Update(), accel.x={accel.x}, used tilt={tilt}");
    }
}
*/
using UnityEngine;
using UnityEngine.InputSystem;

public class TouchTilt : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 15f;
    [SerializeField] private float deadZone = 5f;
    [SerializeField] private float tiltSensitivity = 0.1f;
    [SerializeField] private float maxTiltClamp = 1f;
    private PlayerControls controls;
    private Vector3 accel;

    void Awake()
    {
        controls = new PlayerControls();

      
        controls.Player1.TiltMove.performed += ctx =>
        {
            accel = ctx.ReadValue<Vector3>();
            Debug.Log("TiltMove performed: " + accel);
        };

        controls.Player1.TiltMove.canceled += ctx =>
        {
            accel = Vector3.zero;
            Debug.Log("TiltMove canceled");
        };

        
        if (Accelerometer.current != null)
        {
            if (!Accelerometer.current.enabled)
                InputSystem.EnableDevice(Accelerometer.current);

            Debug.Log("Accelerometer enabled");
        }
        else
        {
            Debug.LogWarning("Accelerometer not found. This will only work on a mobile device.");
        }

        Debug.Log("TouchTilt Awake()");
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
   
    void Update()
    {
      
        float tilt = accel.x*tiltSensitivity;

        if (Mathf.Abs(tilt) < deadZone)
            tilt = 0f;


        tilt = Mathf.Clamp(tilt, -maxTiltClamp, maxTiltClamp);

        Vector3 move = transform.right * tilt * horizontalSpeed * Time.deltaTime;
        transform.Translate(move, Space.World);

        Debug.Log($"TouchTilt Update(): accel.x={accel.x}, tilt={tilt}, move={move}");
    }

}
