using UnityEngine;

public class FreeCamera
{
    private float _mainSpeed = 5.0f;
    private float _shiftAdd = 25f;
    private float _maxShift = 100.0f;
    private float _camSens = 0.25f;
    private bool _rotateOnlyIfMousedown = true;
    private bool _movementStaysFlat = false;

    private Vector3 _lastMouse = new Vector3(255, 255, 255);
    private float _totalRun = 1.0f;

    public delegate void RotateCamera(Vector2 delta);
    public delegate void TranslateCamera(Vector3 velocity);

    public event RotateCamera OnRotateCamera;
    public event TranslateCamera OnTranslateCamera;

    public void Update(float deltaTime)
    {
        if (Input.GetMouseButtonDown(0))
        {
            _lastMouse = Input.mousePosition; // $CTK reset when we begin
        }

        if (!_rotateOnlyIfMousedown || _rotateOnlyIfMousedown && Input.GetMouseButton(0))
        {
            _lastMouse = Input.mousePosition - _lastMouse;
            OnRotateCamera?.Invoke(new Vector2(-_lastMouse.y * _camSens, _lastMouse.x * _camSens));
            _lastMouse = Input.mousePosition;
            //Mouse  camera angle done.  
        }

        //Keyboard commands
        var velocity = GetBaseInput();
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _totalRun += deltaTime;
            velocity *= _totalRun * _shiftAdd;
            velocity.x = Mathf.Clamp(velocity.x, -_maxShift, _maxShift);
            velocity.y = Mathf.Clamp(velocity.y, -_maxShift, _maxShift);
            velocity.z = Mathf.Clamp(velocity.z, -_maxShift, _maxShift);
        }
        else
        {
            _totalRun = Mathf.Clamp(_totalRun * 0.5f, 1f, 1000f);
            velocity *= _mainSpeed;
        }

        velocity *= deltaTime;
        // var newPosition = _transform.position;
        // if (Input.GetKey(KeyCode.Space) || movementStaysFlat && !(rotateOnlyIfMousedown && Input.GetMouseButton(1)))
        // {
        //     _transform.Translate(velocity);
        //     var position = _transform.position;
        //     newPosition.x = position.x;
        //     newPosition.z = position.z;
        //     _transform.position = newPosition;
        // }
        // else
        // {
        //     _transform.Translate(velocity);
        // }
        
        OnTranslateCamera?.Invoke(velocity);
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