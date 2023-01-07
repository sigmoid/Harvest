using Mono.Cecil.Cil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public InputAction PlayerControls;

	public InputAction PlayerClicked;

	public InputAction PlayerRightClicked;

    public float MoveSpeed = 10.0f;

	public GameObject Scythe;

	public float ScytheTime = 0.2f;

    private Animator _animator;

	public float HoeCoolDown = 0.2f;

    private int _facingLeftAnimId;
	private int _facingRightAnimId;
	private int _facingUpAnimId;
	private int _facingDownAnimId;

	private float _pressedThreshold = 0.5f;

	private float _scytheTimer;

	private Camera _mainCam;

	private float _hoeCoolDownTimer;

	private TileMapManager _tileMap;

	private Toolbelt _toolbelt;

	// Start is called before the first frame update
	void Start()
    {
		_animator = GetComponent<Animator>();
		_facingLeftAnimId = Animator.StringToHash("FacingLeft");
		_facingRightAnimId = Animator.StringToHash("FacingRight");
		_facingUpAnimId = Animator.StringToHash("FacingUp");
		_facingDownAnimId = Animator.StringToHash("FacingDown");

		_mainCam = FindObjectOfType<Camera>();

		_tileMap = FindObjectOfType<TileMapManager>();

		_toolbelt = GetComponent<Toolbelt>();

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
		PlayerRightClicked.Enable();
	}

	private void OnDisable()
	{
        PlayerControls.Disable();
		PlayerClicked.Disable();
		PlayerRightClicked.Disable();
	}

	// Update is called once per frame
	void Update()
    {
        var movement = PlayerControls.ReadValue<Vector2>();
		var click = PlayerClicked.WasPressedThisFrame();
		var rightClick = PlayerRightClicked.WasPerformedThisFrame();
		string tool = _toolbelt.GetCurrentTool().Name;

		SetFacing(movement);
		UpdateScythe(click);
		if (tool == "Hoe")
		{ 
			UpdateHoe(rightClick);
		}
		if (tool == "Seed")
		{
			UpdateSeed(rightClick);
		}


	
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
			Scythe.transform.localPosition = Scythe.transform.up;
		}
	}

	private void UpdateHoe(bool rightClicked)
	{
		if (_hoeCoolDownTimer > 0)
		{
			_hoeCoolDownTimer -= Time.deltaTime;
		}
		else if(rightClicked)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			_tileMap.Hoe(mousePosition);
		}
	}

	private void UpdateSeed(bool rightClicked)
	{
		if (rightClicked)
		{
			Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
			_tileMap.Plant(mousePosition);
		}
	}

	private void Move(Vector2 movement)
	{
		transform.position += new Vector3(movement.x, movement.y, 0) * MoveSpeed * Time.deltaTime;
	}
}
