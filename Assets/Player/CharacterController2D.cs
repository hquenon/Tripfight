using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
	[Header("Movements")]
	[Space]
	[SerializeField] private float m_JumpForce = 400f;                          // Amount of force added when the player jumps.
	[SerializeField] private float m_WallJumpForce = 400f;                      // Amount of force added when the player jumps.
	[SerializeField] private int m_MaxJumps = 2;								// max number of jumps resets when the player touch a wall or the ground	

	[SerializeField] private float m_DashSpeed = 400f;
	[SerializeField] private float m_DashTime = 0.5f;
	[SerializeField] private int m_MaxDashes = 2;

	[SerializeField] private float m_defaultGravity = 5;
	[SerializeField] private float m_wallGravity = 1;

	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;  // How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;                         // Whether or not a player can steer while jumping;

	[Header("Wall checks")]
	[Space]
	[SerializeField] private LayerMask m_WhatIsGround;                          // A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Transform m_LeftCheck;                           // A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_RightCheck;                          // A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;                // A collider that will be disabled when crouching


	private Rigidbody2D m_Rigidbody2D;


	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	/*state*/
	private bool touchWallRight;
	private bool touchGround;
	private bool touchCeiling;

	private bool canMove = true;

	private bool wasTouchingWall;


	private int jumps;
	private int dashes;

	/*Timers*/
	private float dashTimer = -1;
	private float minJumpTime = 0.5f;
	private float jumpTimer = -1;
	private float disableTimer = 0;


	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
	}




	public void Dash()

	{
		if (!canMove) { return; }
		if (dashes>0) {
			dashTimer = 0;
			if (touchWallRight)
			{
				Flip();
			}

			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			touchWallRight = false;
			canMove = false;
			dashes--;
		}

	}
	public void Jump()
	{
		if (!canMove) { return; }
		if ( jumps > 0)
		{
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);
			if (!touchWallRight)// | (touchWallRight && touchGround))
			{
				m_Rigidbody2D.AddForce(new Vector2(0, m_JumpForce));
			} else
			{
				Vector2 forceVector;
				if (m_FacingRight)
				{
					forceVector = new Vector2(-Mathf.Sqrt(2) * m_WallJumpForce, Mathf.Sqrt(2) * m_WallJumpForce);
				}
				else
				{
					forceVector = new Vector2(Mathf.Sqrt(2) * m_WallJumpForce, Mathf.Sqrt(2) * m_WallJumpForce);
				}
				m_Rigidbody2D.AddForce(forceVector);
			}
			
			jumps--;
			jumpTimer = 0;
		}

	}
	public void Move(float move)
	{
		if (!canMove) { return; }
		//only control the player if grounded or airControl is turned on
		if ( touchGround || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
		}
	}
	public void SetDisableTimer(float time)
	{
		disableTimer = time;
	}
	private void checkDisableTimer()
	{
		if (disableTimer > 0) {
			canMove = false;
			disableTimer -= Time.deltaTime;
		} else
		{
			canMove = true;
		}
	}

	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void Update()
	{
		updateWallChecks();
		checkDisableTimer();
		if (!canMove) { return; }

		if (!wasTouchingWall && touchWallRight)
		{
			m_Rigidbody2D.velocity = Vector3.zero;
			m_Rigidbody2D.gravityScale = m_wallGravity;
			wasTouchingWall = true;
		}
		if (!touchWallRight)
		{
			m_Rigidbody2D.gravityScale = m_defaultGravity;
			wasTouchingWall = false;
		}



		/*Debug.Log("Touch ground: " + touchGround);
		Debug.Log("Touch right: " + touchWallRight);
		Debug.Log("Touch ceiling: " + touchCeiling);
		Debug.Log("Jumps: " + jumps);*/


		//Jump and dash reset
		if ((jumpTimer == -1) && (touchGround || touchWallRight))
		{
			jumps = m_MaxJumps;
			dashes = m_MaxDashes;
		}
		else if (0 <= jumpTimer && jumpTimer < minJumpTime)
		{
			jumpTimer += Time.deltaTime;
		}
		else
		{
			jumpTimer = -1;
		}

		//Dash update
		if (dashTimer >= 0 && dashTimer < m_DashTime)
		{
			Vector2 speedDirection;
			if (m_FacingRight)
			{
				speedDirection = Vector2.right;
			}
			else
			{
				speedDirection = Vector2.left;
			}
			m_Rigidbody2D.velocity = speedDirection * m_DashSpeed;
			dashTimer += Time.deltaTime;
		}
		else if (dashTimer >= m_DashTime)
		{
			dashTimer = -1;
			m_Rigidbody2D.velocity = Vector2.zero;
			canMove = true;
			m_Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

	}

	private void FixedUpdate()
	{
	}

	private bool doesTouchGround(Transform check, float checkRadius)
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(check.position, checkRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				return true;
			}
		}
		return false;
	}

	private void updateWallChecks()
	{
		float radius = .1f;
		touchGround = doesTouchGround(m_GroundCheck, radius);
		touchWallRight = doesTouchGround(m_RightCheck, radius);
		touchCeiling = doesTouchGround(m_CeilingCheck, radius);
	}

}