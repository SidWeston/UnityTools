using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraPosition : MonoBehaviour
{
	//The target transform, whose position values will be copied and smoothed;
	public Transform target;
	Transform currentTransform;

	Vector3 currentPosition;

	//Speed that controls how fast the current position will be smoothed toward the target position when 'Lerp' is selected as smoothType;
	public float lerpSpeed = 20f;

	//Time that controls how fast the current position will be smoothed toward the target position when 'SmoothDamp' is selected as smoothType;
	public float smoothDampTime = 0.02f;

	//Local position offset at the start of the game;
	Vector3 localPositionOffset;

	Vector3 refVelocity;

	//Awake;
	void Awake()
	{

		//If no target has been selected, choose this transform's parent as the target;
		if (target == null)
        {
			target = this.transform.parent;
		}
			
		currentTransform = transform;
		currentPosition = transform.position;

		localPositionOffset = currentTransform.localPosition;
	}

	//OnEnable;
	void OnEnable()
	{
		//Reset current position when gameobject is re-enabled to prevent unwanted interpolation from last position;
		ResetCurrentPosition();
	}

	void Update()
	{
		SmoothUpdate();
	}

	//void LateUpdate()
	//{
	//	SmoothUpdate();
	//}

	void SmoothUpdate()
	{
		//Smooth current position;
		currentPosition = Smooth(currentPosition, target.position);

		//Set position;
		currentTransform.position = currentPosition;
	}

	Vector3 Smooth(Vector3 startPos, Vector3 targetPos)
	{
		//Convert local position offset to world coordinates;
		Vector3 offset = currentTransform.localToWorldMatrix * localPositionOffset;

		//Add local position offset to target;
		targetPos += offset;

		return Vector3.SmoothDamp(startPos, targetPos, ref refVelocity, smoothDampTime);
	}

	//Reset stored position and move this gameobject directly to the target's position;
	//Call this function if the target has just been moved a larger distance and no interpolation should take place (teleporting);
	public void ResetCurrentPosition()
	{
		//Convert local position offset to world coordinates;
		Vector3 offset = currentTransform.localToWorldMatrix * localPositionOffset;
		//Add position offset and set current position;
		currentPosition = target.position + offset;
	}
}
