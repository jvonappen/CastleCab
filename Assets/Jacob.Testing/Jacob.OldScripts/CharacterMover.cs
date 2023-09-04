using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{
    public float speed = 10;
    [SerializeField] private float boost = 0.1f;
    [SerializeField] private float speedMAX = 20;
    private float _originalSpeed;

   CharacterController cc;
    Vector2 moveInput = new Vector2();

    bool jumpInput;
    public Vector3 velocity;
    public float jumpVelocity;
    public bool isGrounded;

    public Transform cam;

    public Animator animator;

    public float mass = 200;

    //[SerializeField] private GameObject dustTrail;
    [SerializeField] private ParticleSystem[] _dustTrail;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CharacterController>();
        _originalSpeed = speed;
    }

    void Update()
    {
        moveInput.x = Input.GetAxis("Horizontal");
        moveInput.y = Input.GetAxis("Vertical");
        jumpInput = Input.GetButton("Jump");   /* Disabled jumping */
        DriftControls();

    }

    void FixedUpdate()
    {
        // find the horizontal unit vector facing forward from the camera
        Vector3 camForward = cam.forward;
        camForward.y = 0;
        camForward.Normalize();

        transform.forward = camForward;

        // use our camera's right vector, which is always horizontal
        Vector3 camRight = cam.right;

        // player movement using WASD or arrow keys
        Vector3 delta = (moveInput.x * camRight + moveInput.y * camForward) * speed * Time.fixedDeltaTime;
        if (isGrounded || moveInput.x != 0 || moveInput.y != 0)
        {
            velocity.x = delta.x;
            velocity.z = delta.z;

        }

        // check for jumping
        if (jumpInput && isGrounded)
            velocity.y = jumpVelocity;


        // check if we've hit ground from falling. If so, remove our velocity
        if (isGrounded && velocity.y < 0)
            velocity.y = 0;

        // apply gravity after zeroing velocity so we register as grounded still
        velocity += Physics.gravity * Time.fixedDeltaTime;

        // and apply this to our positional update this frame
        delta += velocity * Time.fixedDeltaTime;


        if (!isGrounded)
            hitDirection = Vector3.zero;

        // slide objects off surfaces they're hanging on to
        if (moveInput.x == 0 && moveInput.y == 0)
        {
            Vector3 horizontalHitDirection = hitDirection;
            horizontalHitDirection.y = 0;
            float displacement = horizontalHitDirection.magnitude;
            if (displacement > 0)
                velocity -= 0.2f * horizontalHitDirection / displacement;
        }

        cc.Move(delta);
        isGrounded = cc.isGrounded;

    }

    public Vector3 hitDirection;
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        hitDirection = hit.point - transform.position;
        if (hit.rigidbody)
        {
            hit.rigidbody.AddForceAtPosition(velocity * mass, hit.point);
        }
    }

    private void DriftControls()
    {
        if(moveInput.x != 0 && moveInput.y != 0)
        {
            PlayDustParticles();
            if(speed < speedMAX)
            {
                speed = speed + boost * Time.fixedDeltaTime;
            }
            else if(speed > speedMAX) { speed = speedMAX; }
            
        }
        else
        {
            StopDustParticles();
            if (speed > _originalSpeed)
            {
                speed = speed - boost * Time.fixedDeltaTime;
            }
            if(speed <= _originalSpeed) 
            {
                
                speed = _originalSpeed;
            }

        }
    }

    private void PlayDustParticles()
    {
        for (int i = 0; i < _dustTrail.Length; i++)
        {
            _dustTrail[i].Play();
        }
    }

    private void StopDustParticles()
    {
        for (int i = 0; i < _dustTrail.Length; i++)
        {
            _dustTrail[i].Stop();
        }
    }

}