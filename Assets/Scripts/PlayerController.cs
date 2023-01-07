using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction PlayerControls;

	public InputAction PlayerClicked;

    public float MoveSpeed = 10.0f;

	public GameObject Scythe;

	public float ScytheTime = 0.2f;

    private Animator _animator;

    private int _facingLeftAnimId;
	private int _facingRightAnimId;
	private int _facingUpAnimId;
	private int _facingDownAnimId;

	private float _pressedThreshold = 0.5f;

	private float _scytheTimer;

	private Camera _mainCam;

	// Start is called before the first frame update
	void Start()
    {
		_animator = GetComponent<Animator>();
		_facingLeftAnimId = Animator.StringToHash("FacingLeft");
		_facingRightAnimId = Animator.StringToHash("FacingRight");
		_facingUpAnimId = Animator.StringToHash("FacingUp");
		_facingDownAnimId = Animator.StringToHash("FacingDown");

		_mainCam = FindObjectOfType<Camera>();

		Scythe.SetActive(false);
    }

	void SetFacing(Vector2 movement)
	{
		bool right = movement.x > _pressedThreshold;
		bool left = movement.x < -_pressedThreshold;
		bool up = movement.y > _pressedThreshold;
		bool down = movement.y < -_pressedThreshold;

		_animator.SetBool(_facingLeftAnimId, false);
		_animator.SetBool(_facingRightAnimId, false);
		_animator.SetBool(_facingUpAnimId, false);
		_animator.SetBool(_facingDownAnimId, false);

		if (right)
			_animator.SetBool(_facingRightAnimId, true);
		else if (up)
			_animator.SetBool(_facingUpAnimId, true);
		else if (left)
			_animator.SetBool(_facingLeftAnimId, true);
		else if (down)
			_animator.SetBool(_facingDownAnimId, true);

	}

	private void OnEnable()
	{
        PlayerControls.Enable();
		PlayerClicked.Enable();
	}

	private void OnDisable()
	{
        PlayerControls.Disable();
		PlayerClicked.Disable();
	}

	// Update is called once per frame
	void Update()
    {
        var movement = PlayerControls.ReadValue<Vector2>();
		var click = PlayerClicked.WasPressedThisFrame();

		SetFacing(movement);
		UpdateScythe(click);
		Move(movement);
        
	}

	private void UpdateScythe(bool click)
	{
		if (_scytheTimer > 0)
		{
			_scytheTimer -= Time.deltaTime;

			if (_scytheTimer <= 0)
				Scythe.SetActive(false);
		}
		else if (click)
		{
			_scytheTimer = ScytheTime;
			Scythe.SetActive(true);

			var mousePos = _mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			var charPos = transform.position;

			mousePos.z = 0;
			charPos.z = 0;

			var diff = mousePos - charPos;
			diff = diff.normalized;
			var theta = Mathf.Atan2(diff.y, diff.x) + Mathf.PI * 1.5f;
			Scythe.transform.rotation = Quaternion.EulerRotation(new Vector3(0, 0, theta));
		}
	}

	private void Move(Vector2 movement)
	{
		transform.position += new Vector3(movement.x, movement.y, 0) * MoveSpeed * Time.deltaTime;
	}
}
