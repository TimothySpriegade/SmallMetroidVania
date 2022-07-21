using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //comps
    private Rigidbody2D _rb;
    //vars
    [SerializeField] Vector2 _movementInput;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField]private float _movementSpeed;
    private float _targetSpeed;
    private float _accelRate;
    private float _movement;
    private float _speedDif;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //input Handler
        _movementInput.x = Input.GetAxisRaw("Horizontal");
    }
    
    void FixedUpdate()
    {
        //movement
        _targetSpeed = _movementInput.x * _movementSpeed;
        _speedDif = _targetSpeed - _rb.velocity.x;
        _accelRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? acceleration : decceleration;
        _movement = Mathf.Pow(Mathf.Abs(_speedDif) + _accelRate, velPower) * Mathf.Sign(_speedDif);
        _rb.AddForce(_movement*Vector2.right);

    }

}
