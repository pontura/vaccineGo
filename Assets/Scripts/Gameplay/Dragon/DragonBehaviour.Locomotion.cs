using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;

// kzlukos@gmail.com
// Dragon locomotion logic
public partial class DragonBehaviour : GameStateObserver
{
	
    [SerializeField] AudioSource landingAudioSource;
    bool landed = false;

	[Header("Locomotion")]
	[SerializeField]
	private float distanceMargin = 0.3f;
	private DragonController _character;
	private Vector3 _targetPositionVector;


	[Header("IK")]
	private DragonHeadIK _headIK;
	[SerializeField]
	private Transform airIKTarget;
	[SerializeField]
	private Transform landIKTarget;

	public bool InPlace {
		get 
		{
			return (new Vector2(_targetPositionVector.x, _targetPositionVector.z).magnitude < 0.2f);
		}
	}

	//
	void Awake()
	{
		_headIK = GetComponent<DragonHeadIK>();
		_character = GetComponent<iMalbersInputs>() as DragonController;
		_lookAtScript = GetComponent<VRLookAtDragon>();
		_character.Fly = true;
		_targetTransform = startTransfom;
	}


	//
	private void FixedUpdate()
	{
		if (_targetTransform != null)
		{
			_targetPositionVector =_targetTransform.position - transform.position;

			KeepHeight();
			Move();
			_headIK.IKWeight = InPlace ? 1f : 0f;

			_headIK.targetTransform = _character.Fly ? airIKTarget : landIKTarget;

			if(InPlace)
				AdjustPosition ();
		}

        if (!landingAudioSource.isPlaying && !_character.Fly && !landed)
        {
            landingAudioSource.Play();
            landed = true;
        }

        if (_character.Fly)
            landed = false;
    }


	//
	void Move()
	{

		Vector3 direction = _targetPositionVector;
		direction.y = 0f;

		if(InPlace)
        {
			Vector3 v1 = transform.forward;
			//v1.y = 0f;
			Vector3 v2 = _targetTransform.forward;
			//v2.y = 0f;

			float angle = Vector3.Angle(v2, v1);
            Vector3 cross = Vector3.Cross(v2, v1);
            if (cross.y < 0) angle = -angle;

			if (angle < 2f)
				angle = 0f;

            angle = angle / 180f;
            _character.Move(new Vector3(-angle, 0f, 0f), false);


        } else
        {
			direction /= 4f;
			_character.Move(direction, true);
        }
        
	}

	//
	private void AdjustPosition() 
	{
		transform.position = Vector3.Lerp(transform.position, _targetTransform.position, 0.01f);
		transform.rotation = Quaternion.Lerp(transform.rotation, _targetTransform.rotation, 0.01f);
	}

	//
	void KeepHeight()
	{

		RaycastHit hit;

		_character.Jump = (_targetTransform.position.y - transform.position.y > distanceMargin);
		_character.Down = (transform.position.y - _targetTransform.position.y > distanceMargin);

		//Land
		if (transform.position.y - _targetTransform.position.y > 0f && Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, 1 << 10) && _character.Fly)
		{
			_character.Fly = false;
			return;
		}

		//Take off
		if (_character.Jump && !_character.Fly)
			_character.Fly = true;
	}

}
