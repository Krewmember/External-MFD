using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FDAI : MonoBehaviour {

	Vector3 v;
	public RectTransform rollPointer;
	public RectTransform yawRateArrow;
	public RectTransform pitchRateArrow;
	public RectTransform rollRateArrow;

	Vector3 rot;
	Vector3 angularVelocity;
	Rigidbody rb;

	public float Roll;
	public float Pitch;
	public float Heading;

	public Transform ballAxis1;
	public Transform ballAxis2;
	public Transform ballAxis3;

	Vector3 Torque;

	void Start () 
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		float roll = Input.GetAxis ("Roll");
		float yaw = Input.GetAxis ("Yaw");
		float pitch = Input.GetAxis ("Pitch");
		Roll += roll;
		Heading += yaw;
		Pitch += pitch;
		Torque = new Vector3 (pitch * 3, yaw * 3, roll * 3);

		pitchRateArrow.localPosition = new Vector3 (pitchRateArrow.localPosition.x, rb.angularVelocity.x * -28.8f, pitchRateArrow.localPosition.z);
		yawRateArrow.localPosition = new Vector3 (rb.angularVelocity.y * 28.8f, yawRateArrow.localPosition.y, yawRateArrow.localPosition.z); 
		rollRateArrow.localPosition = new Vector3 (rb.angularVelocity.z * 28.8f, rollRateArrow.localPosition.y, rollRateArrow.localPosition.z);

		Debug.Log ("Pitch Rate Degs Per Sec: " + rb.angularVelocity.x);
	}

	void FixedUpdate () 
	{
		rb.AddTorque (Torque);
	}
}
