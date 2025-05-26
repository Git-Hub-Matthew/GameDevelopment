using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class Player_Script : MonoBehaviour
{
    // [SerializeField] -> Makes it editable inside Unity
    [SerializeField] private float _xYSpeed;        // Left and Right Speed
    [SerializeField] private float _jumpStrength;   // Jump Height

    [SerializeField] private float _dashDistance;   // Dash Distance
    [SerializeField] private float _dashDuration;   // Dash Duration
    [SerializeField] private float _dashCooldown;   // Dash Cooldown

    [SerializeField] private float _poofPower;      // Poof Power
    [SerializeField] private float _poofDuration;   // Poof Duration
    [SerializeField] private float _poofCooldown;   // Poof Cooldown

    [SerializeField] private GameObject _dlb_Jump_Unlock;
    [SerializeField] private GameObject _dashUnlock;
    [SerializeField] private GameObject _poofUnlock;

    private Rigidbody2D _body;
    private SpriteRenderer _spriteRenderer;
    private UI_Countdown_Script _UICountdown;

    public Animator Animator;
    public GameObject Trail;
    public GameObject Poof;

    private bool _isDashing = false;
    private bool _canDash = true;
    private bool _isPoofing = false;
    private bool _poofCreated = false;
    private bool _hasDblJump;
    private bool _hasDash;
    private bool _hasPoof;
    private bool _isFirstJump;
    private bool _gameComplete;

    private string _direction = "right";

    private float _dashCountdown = 0;
    private float _poofCountdown;

    private Vector3 _mousePos;
    private Vector2 _poofTranslation;
    private Vector2 _poofPosition;


    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if(!_gameComplete){

            //Left and Right Mechanics
            if (!_isDashing && !_isPoofing)
            {
                _body.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * _xYSpeed, _body.linearVelocity.y);
                Animator.SetFloat("Speed", Mathf.Abs(_body.linearVelocity.x));
            }

            // Jumping Mechanics
            if (_body.linearVelocity.y == 0)
            {
                Animator.SetBool("isJumping", false); // Animation Reset
                
                if (Input.GetKeyDown(KeyCode.W))
                {
                    _body.linearVelocity = new Vector2(_body.linearVelocity.x, _jumpStrength);
                    Animator.SetBool("isJumping", true); // Play jump animation
                    _isFirstJump = true;
                }

            } else if (_isFirstJump)
            {
                if (Input.GetKeyDown(KeyCode.W) && _hasDblJump)
                {
                    _body.linearVelocity = new Vector2(_body.linearVelocity.x, _jumpStrength);
                    Animator.SetBool("isJumping", true); // Play jump animation
                    _isFirstJump = false;
                }
            }


            // Direction Control
            if (Input.GetKey(KeyCode.A)){
                _direction = "left";
                _spriteRenderer.flipX = true;   // flips sprite
                
            }
            else if (Input.GetKey(KeyCode.D)){
                _direction = "right";
                _spriteRenderer.flipX = false;
            }

            // Dash Cooldown
            if (_dashCountdown > 0)
            {
                _dashCountdown -= Time.deltaTime;
                //_UICountdown.UiCountDown(_dashCountdown);
            }
            else
            {
                _canDash = true;
            }

            // Dash Mechanics
            if (Input.GetKeyDown(KeyCode.LeftShift) && !_isDashing && _canDash && _hasDash)
            {
                StartCoroutine(Dash()); 
            }

            // Poof Cooldown
            if (_poofCountdown > 0)
            {
                _poofCountdown -= Time.deltaTime;
            }
            else
            {
                _poofCreated = false;
            }

            // Poof Mechanics
            if (Input.GetMouseButton(0) && !_isPoofing && !_poofCreated && _hasPoof)
            {
                StartCoroutine(PoofEnum());
            }
        }
    }

    // Coroutine for dash movement
    private IEnumerator Dash()
    {
        _isDashing = true;

        if (_direction == "right")
        {
            _body.linearVelocity = new Vector2(_dashDistance, _body.linearVelocity.y);
            _body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        }
        else if (_direction == "left")
        {
            _body.linearVelocity = new Vector2(-_dashDistance, _body.linearVelocity.y);
            _body.constraints = RigidbodyConstraints2D.FreezePositionY;
            _body.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;

        }

        SpawnSprite(_body.position);

        yield return new WaitForSeconds(_dashDuration);

        _body.constraints = RigidbodyConstraints2D.FreezeRotation;
        _isDashing = false; // End dash, return to normal movement
        _canDash = false;
        _dashCountdown = _dashCooldown;
    }

    public void SpawnSprite(Vector3 spawnPosition)
    {
        spawnPosition.y -= 0.1f;    // Grounds Sprite

        GameObject trailInstance = Instantiate(Trail, spawnPosition, Quaternion.identity);  // Creates instance of Trail

        // Pass direction to Trail.cs
        Trail_Script trailScript = trailInstance.GetComponent<Trail_Script>();

        if (trailScript != null)
        {
            trailScript.SetDirection(_direction);
        }
    }

    // Coroutine for dash movement
    private IEnumerator PoofEnum()
    {
        _isPoofing = true;

        // Calculate Mouse Position
        _mousePos = Input.mousePosition;    // Position on monitor 
        _mousePos = Camera.main.ScreenToWorldPoint(_mousePos);

        // Normalize realtive to Player
        _poofTranslation = new Vector2(_mousePos.x - _body.position.x, _mousePos.y - _body.position.y);
        _poofTranslation.Normalize();
        _poofPosition = _body.position + _poofTranslation * 2;

        GameObject poofInstance = Instantiate(Poof, _poofPosition, Quaternion.identity);  // Creates instance of Trail

        float angle = Mathf.Atan2(_poofTranslation.y, _poofTranslation.x) * Mathf.Rad2Deg;  // Get the angle in degrees using Mathf.Atan2

        poofInstance.transform.rotation = Quaternion.Euler(0, 0, angle + 90);

        _body.linearVelocity = new Vector2(_body.linearVelocity.x + -_poofTranslation.x * _poofPower, -_poofTranslation.y * _poofPower);    // Opposite direction of mouse click with consideration of current X velocity

        yield return new WaitForSeconds(_poofDuration);

        _isPoofing = false;
        _poofCountdown = _poofCooldown;
        _poofCreated = true;
        _poofCountdown = 3;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Dbl_jump_unlock")
        {
            _hasDblJump = true;
            Destroy(_dlb_Jump_Unlock);
        }
        if (collision.gameObject.name == "Dash_unlock")
        {
            _hasDash = true;
            Destroy(_dashUnlock);
        }
        if (collision.gameObject.name == "Poof_unlock")
        {
            _hasPoof = true;
            Destroy(_poofUnlock);
        }

        if(collision.gameObject.tag == "Finish"){
            Finish_Win();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.name == "BigVegas"){
            Debug.Log("!Game Over!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void Finish_Win(){
        _gameComplete = true;
        Animator.SetBool("isJumping", false);
        _body.linearVelocity = Vector2.right * _xYSpeed / 2;
        Camera.main.transform.parent.GetComponent<CameraTarget>().enabled = false;
    }
}
