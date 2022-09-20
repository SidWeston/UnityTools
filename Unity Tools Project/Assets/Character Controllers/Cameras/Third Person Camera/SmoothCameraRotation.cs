using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothCameraRotation : MonoBehaviour
{

	//The target transform, whose rotation values will be copied and smoothed;
	public Transform target;
	Transform currentTransform;

	Quaternion currentRotation;

	//Speed that controls how fast the current rotation will be smoothed toward the target rotation;
	public float smoothSpeed = 20f;

	//Awake;
	void Awake()
	{

		//If no target has been selected, choose this transform's parent as target;
		if (target == null)
			target = this.transform.parent;

		currentTransform = transform;
		currentRotation = transform.rotation;
	}

	//OnEnable;
	void OnEnable()
	{
		//Reset current rotation when gameobject is re-enabled to prevent unwanted interpolation from last rotation;
		ResetCurrentRotation();
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
		//Smooth current rotation;
		currentRotation = Smooth(currentRotation, target.rotation, smoothSpeed);

		//Set rotation;
		currentTransform.rotation = currentRotation;
	}

	//Smooth a rotation toward a target rotation based on 'smoothTime';
	Quaternion Smooth(Quaternion currentRot, Quaternion targetRot, float smoothSpeed)
	{
		//Slerp rotation and return;
		return Quaternion.Slerp(currentRot, targetRot, Time.deltaTime * smoothSpeed);
	}

	//Reset stored rotation and rotate this gameobject to macth the target's rotation;
	//Call this function if the target has just been rotatedand no interpolation should take place (instant rotation);
	public void ResetCurrentRotation()
	{
		currentRotation = target.rotation;
	}
}
