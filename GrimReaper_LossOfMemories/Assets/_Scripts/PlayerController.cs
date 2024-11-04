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

    [Header("Ground Detection")]
    [SerializeField] Transform _groundCheck;
    [SerializeField] float _groundRadius = 0.5f;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] bool _isGrounded;

    [Header("HUD")]
    [SerializeField] public int life = 3;
    [SerializeField] public float health = 100f;
    [SerializeField] public GameObject LevelupCharacter;

    #endregion

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new COMP397LABS();
        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += context => _move = Vector2.zero;
        _inputs.Player.Jump.performed += context => Jump();
    }

    void OnEnable() => _inputs.Enable();

    void OnDisable() => _inputs.Disable();

    void Start()
    {
        LevelupCharacter.SetActive(false);
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
        
        if (!_controller.enabled) { return; }
        Vector3 movement = new Vector3(_move.x, 0.0f, _move.y) * _speed * Time.fixedDeltaTime;
        _controller.Move(movement);
        _velocity.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
    }
    public void Jump()
    {
        Debug.Log("Jump Button Pressed")
;        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
            NotifyObservers(PlayerEnums.Jump);
        }
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
            LevelupCharacter.SetActive(true);
            this.GetComponent<MeshRenderer>().enabled = false;
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