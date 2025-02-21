using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private float _moveSpeed = 5f ;
    [SerializeField] private float _gravity = -20f;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _jumpSpeed = 15f;

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveVelocity;
    Vector3 turnVelocity;
    Vector3 direction;

    private CharacterController _controller;

    // Start is called before the first frame update
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        direction = new Vector3 (horizontalInput, 0 ,verticalInput);
        direction.Normalize();

        if(_controller.isGrounded)
        {
            moveVelocity = transform.forward * _moveSpeed * verticalInput;
            turnVelocity = transform.up * _rotationSpeed * horizontalInput;

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                moveVelocity.y = _jumpSpeed;
            }
        }
        moveVelocity.y += _gravity * Time.deltaTime;

        _controller.Move(moveVelocity * Time.deltaTime);
        transform.Rotate(turnVelocity * Time.deltaTime);
    }
}
