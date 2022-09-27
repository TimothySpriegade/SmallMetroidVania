
using System.ComponentModel;
using System.Timers;
using System.Runtime.InteropServices.WindowsRuntime;
using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Movement : MonoBehaviour
{
    #region vars
    [SerializeField] private float _coyoteTime = 0.1f;
    private float _coyoteTimeCounter;
    private float _jumpBufferTime = 0.1f;
    private float _jumpBufferCounter;
    #endregion

    #region Components
    private Rigidbody2D _rb;
    public Animator _animator;
    #endregion
    
    #region SerializedVars
    [SerializeField] Vector2 _movementInput;
    [SerializeField] private float acceleration;
    [SerializeField] private float decceleration;
    [SerializeField] private float velPower;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private float _jumpforce;

    #region Groundcheck
    [SerializeField] private float _groundCheckRadius = 0.5f;
    [SerializeField] private Transform _groundCheckObject;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _frictionAmount;
    [SerializeField] private float _groundedGravaty;
    [SerializeField] private float _defaultGravity;

    #endregion
    
    #endregion

    #region Variables
    private float _targetSpeed;
    private float _accelRate;
    private float _movement;
    private float _speedDif;
    private bool _facingRight = false;
    public bool _isGrounded;
    public bool _isJumping;

    #endregion
    
    #region Updates

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //input Handler
        _movementInput.x = Input.GetAxisRaw("Horizontal");
       #region  Timer
        if(_isGrounded)
        {
            _coyoteTimeCounter = _coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }
       #endregion 
        #region Jump
        
        if (Input.GetKeyDown("space"))
        {
            _jumpBufferCounter = _jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if(_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f)
        {
            _rb.velocity = new Vector2(_rb.velocity.x, _jumpforce);
            _jumpBufferCounter = 0f;
        }


        if (Input.GetKeyUp("space")&& _rb.velocity.y > 0f)
        {
             _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y *0.5f);
             _coyoteTimeCounter = 0f;
        }

    
        #endregion

    }
    
    void FixedUpdate()
    {
        
        isGrounded();

        #region Movement
        _targetSpeed = _movementInput.x * _movementSpeed;
        _speedDif = _targetSpeed - _rb.velocity.x;
        _accelRate = (Mathf.Abs(_targetSpeed) > 0.01f) ? acceleration : decceleration;
        _movement = Mathf.Pow(Mathf.Abs(_speedDif) + _accelRate, velPower) * Mathf.Sign(_speedDif);
        _rb.AddForce(_movement*Vector2.right);

        if (_isGrounded && Mathf.Abs(_movementInput.x)< 0.1f)
        {
            float amount = Mathf.Min(Mathf.Abs(_rb.velocity.x), Mathf.Abs(_frictionAmount));
            amount *= Mathf.Sign(_rb.velocity.x);
            _rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
        #endregion

        flip();
        #region Groundcheck
        if (_isGrounded)
        {
            _isJumping = false;
            _rb.gravityScale = _groundedGravaty;
        }
        else
        {
            _isJumping = true;
            _rb.gravityScale = _defaultGravity;
        }
        #endregion

        //anims

    }

    #endregion

    #region Methods
    void flip()
    {
        if (_movementInput.x < 0 && !_facingRight)
        {
            flipIt();
        }
        if (_movementInput.x > 0 && _facingRight)
        {
            flipIt();
        }
    }
    void flipIt()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _facingRight = !_facingRight;
    }

    void isGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheckObject.position, _groundCheckRadius, _groundLayer);
    }

   

    #endregion
   

}
