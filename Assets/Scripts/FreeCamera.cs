using System;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float mainSpeed = 5.0f;
    public float shiftAdd = 25f;
    public float maxShift = 100.0f;
    public float camSens = 0.25f;
    public bool rotateOnlyIfMousedown = true;
    public bool movementStaysFlat = false;

    private Vector3 lastMouse = new Vector3(255, 255, 255);
    private float totalRun = 1.0f;

    private Transform _transform;

    private void Awake()
    {
        _transform = transform;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMouse = Input.mousePosition; // $CTK reset when we begin
        }

        if (!rotateOnlyIfMousedown || rotateOnlyIfMousedown && Input.GetMouseButton(0))
        {
            lastMouse = Input.mousePosition - lastMouse;
            lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
            var eulerAngles = _transform.eulerAngles;
            lastMouse = new Vector3(eulerAngles.x + lastMouse.x, eulerAngles.y + lastMouse.y, 0);
            _transform.eulerAngles = lastMouse;
            lastMouse = Input.mousePosition;
            //Mouse  camera angle done.  
        }

        //Keyboard commands
        var velocity = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            totalRun += Time.deltaTime;
            velocity *= totalRun * shiftAdd;
            velocity.x = Mathf.Clamp(velocity.x, -maxShift, maxShift);
            velocity.y = Mathf.Clamp(velocity.y, -maxShift, maxShift);
            velocity.z = Mathf.Clamp(velocity.z, -maxShift, maxShift);
        }
        else
        {
            totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
            velocity *= mainSpeed;
        }

        velocity *= Time.deltaTime;
        var newPosition = _transform.position;
        if (Input.GetKey(KeyCode.Space) || movementStaysFlat && !(rotateOnlyIfMousedown && Input.GetMouseButton(1)))
        {
            _transform.Translate(velocity);
            var position = _transform.position;
            newPosition.x = position.x;
            newPosition.z = position.z;
            _transform.position = newPosition;
        }
        else
        {
            _transform.Translate(velocity);
        }
    }

    private Vector3 GetBaseInput()
    {
        var velocity = Vector3.zero;
        
        if (Input.GetKey(KeyCode.W))
        {
            velocity += Vector3.forward;
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity += Vector3.back;
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity += Vector3.right;
        }

        if (Input.GetKey(KeyCode.Q))
        {
            velocity += Vector3.down;
        }

        if (Input.GetKey(KeyCode.E))
        {
            velocity += Vector3.up;
        }

        return velocity;
    }
}