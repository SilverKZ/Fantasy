using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 3f;
    [SerializeField] private float _runSpeed = 4f;
    [SerializeField] private float _jumpForce = 7f;
    [SerializeField] private float _gravity = 15f;
    [SerializeField] private float _angularSpeed = 5f;

    private CharacterController _characterController;
    private Animator _animator;
    private Transform _mainCamera;
    private Vector3 _direction;
    private float _horizontal;
    private float _vertical;
    private float _run;
    private bool _isAttack;

    private bool Idle => _horizontal == 0f && _vertical == 0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main.transform;
        _isAttack = false;
    }

    private void Update()
    {
        Rotate();
        Attack();

        if (_isAttack == false) 
            Move();        
    }

    private void Rotate()
    {
        if (Idle) return;

        Vector3 target = _mainCamera.forward;
        target.y = 0;
        Quaternion look = Quaternion.LookRotation(target);
        float speed = _angularSpeed * Time.deltaTime;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, look, speed);
    }

    private void Move()
    {
        if (_characterController.isGrounded)
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            _run = Input.GetAxisRaw("Run");
            _direction = transform.TransformDirection(_horizontal, 0f, _vertical).normalized;
            PlayAnimaton();
            Jump();
        }

        _direction.y -= _gravity * Time.deltaTime;
        float speed = _run * _runSpeed + _movementSpeed;
        Vector3 dir = _direction * speed * Time.deltaTime;
        dir.y = _direction.y;
        _characterController.Move(dir);
    }

    private void Attack()
    {
        if (_characterController.isGrounded && Input.GetButtonDown("Fire1") && !_isAttack)
        {
            _isAttack = true;
            StartCoroutine(AttackCoroutine());
        }
    }

    private IEnumerator AttackCoroutine()
    {
        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(1.5f);
        _isAttack = false;
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            _animator.SetTrigger("Jump");
            _direction.y += _jumpForce;
        }
    }

    private void PlayAnimaton()
    {
        float horizontal = _run * _horizontal + _horizontal;
        float vertical = _run * _vertical + _vertical;
        _animator.SetFloat("Vertical", vertical, 0.1f, Time.deltaTime);
        _animator.SetFloat("Horizontal", horizontal, 0.1f, Time.deltaTime);
    }
}
