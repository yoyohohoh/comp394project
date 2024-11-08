using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Scripting.APIUpdating;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Subject
{
    #region Private Fields
    COMP397LABS _inputs;
    Vector2 _move;
    #endregion

    #region Serialized Fields
    [Header("Character Controller")]
    [SerializeField] CharacterController _controller;

    [Header("Joystick")]
    [SerializeField] bool isUsingJoystick = true;
    [SerializeField] private Joystick _joystick;
    
    [Header("Movements")]
    [SerializeField] float _speed;
    [SerializeField] float _gravity = -30.0f;
    [SerializeField] float _jumpHeight = 3.0f;
    [SerializeField] Vector3 _velocity;
    [SerializeField] Vector3 initialPosition;

    [Header("Attacks")]
    [SerializeField] public GameObject projectilePrefab;
    [SerializeField] public Transform spawnPoint;
    [SerializeField] public float projectileSpeed = 10f;

    [Header("Ground Detection")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundRadius = 0.5f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] bool _isGrounded;

    [Header("HUD")]
    [SerializeField] public int life = 3;
    [SerializeField] public float health = 100f;
    [SerializeField] public GameObject levelupCharacter;
    [SerializeField] public GameObject characterEyes;

    #endregion

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new COMP397LABS();
        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += context => _move = Vector2.zero;

    }

    void OnEnable() => _inputs.Enable();

    void OnDisable() => _inputs.Disable();

    void Start()
    {
        levelupCharacter.SetActive(false);
        InitiatePlayerPosition();
    }

    private void Update()
    {
        Levelup();
    }

    void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundMask);
        if (_isGrounded && _velocity.y < 0.0f)
        {
            _velocity.y = -2.0f;
        }

        if (isUsingJoystick)
        {
            _move = _joystick.Direction;

        }
        else
        {
            _inputs.Player.Jump.performed += context => Jump();
            _inputs.Player.Fire.performed += context => Attack();
        }
        
        if (!_controller.enabled) { return; }
        Vector3 movement = new Vector3(_move.x, 0.0f, _move.y) * _speed * Time.fixedDeltaTime;
        _controller.Move(movement);
        _velocity.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    public void InitiatePlayerPosition()
    {
        _controller.enabled = false;
        transform.position = initialPosition;
        _controller.enabled = true;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
    }
    public void Jump()
    {
;        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
            NotifyObservers(PlayerEnums.Jump);
        }
    }

    public void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = Vector2.right * projectileSpeed;
        }

        Destroy(projectile, 2f);
    }
    public void MovePlayer(Vector3 position)
    {
        _controller.enabled = false;
        transform.position = position;
        _controller.enabled = true;
    }

    public void Levelup()
    {
        GameObject[] levelupItems = GameObject.FindGameObjectsWithTag("Levelup");
        int levelupItemAmount = levelupItems.Length;

        if(levelupItemAmount <= 0)
        {
            levelupCharacter.SetActive(true);
            this.GetComponent<MeshRenderer>().enabled = false;
            characterEyes.SetActive(false);
            _controller.enabled = false;
            Invoke("WinGame", 5f);
        }
    }

    void WinGame()
    {
        Debug.Log("You Win!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(3);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Levelup"))
        {
            Destroy(other.gameObject);
        }
    }
}