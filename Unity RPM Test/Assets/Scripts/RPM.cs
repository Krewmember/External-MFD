using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using KRPC.Client;
using KRPC.Client.Services.KRPC;
using KRPC.Client.Services.SpaceCenter;

public class RPM : MonoBehaviour {

	public Text AltText;
	public Text RadAltText;
	public Text SRFSpeedText;
	public Text ORBVelText;
	public Text AccelText;
	public Text ModeText;
	public Text RollText;
	public Text PitchText;
	public Text HDGText;
	public Text HORSpeedText;
	public Text VERTSpeedText;
	public Text ThrottleText;
	public Text SASText;
	public Text RCSText;
	public Text GearText;
	public Text BrakesText;
	public Text LightsText;
	public RawImage HDG;
	public GameObject hdgobj;
	public Transform FDAI;

	//FDAI interpolation speed
	public float FDAISpeed = 0.1F;
	public float hdgSpeed;

	Connection conn;
	FDAI fdai;
	Quaternion rotation;

	float Heading;
	float Roll;
	float Pitch;
	float Throttle;

	Double SrfSpeed;
	Double MeanAltitude;
	Double RadarAltitude;
	Double OrbitalVelocity;
	Double HorizontalSpeed;
	Double VerticalSpeed;

	bool SAS;
	bool RCS;
	bool Gear;
	bool Brakes;
	bool Lights;

	void Start () 
	{
		ConnectToServer ();
	}

	void Update () 
	{
		//Create temporary variable
		Quaternion currentRot = FDAI.rotation;
		//Updates rotation and adds spherical interpolation to the FDAI
		FDAI.rotation = Quaternion.Slerp(currentRot, rotation, Time.deltaTime * FDAISpeed);

		//Update UI
		float throttlePercent = Throttle * 100;
		Rect uvRect = new Rect(0.5f + Heading / 360, 0, 1, 1);
		HDG.uvRect = uvRect;
		HDGText.text = Heading.ToString("000.0°");
		RollText.text = Roll.ToString("000.0°");
		PitchText.text = Pitch.ToString("000.0°");
		SRFSpeedText.text = SrfSpeed.ToString("0m/s");
		ORBVelText.text = OrbitalVelocity.ToString("0.0 m/s");
		if (MeanAltitude > 999.9) 
		{
			AltText.text = MeanAltitude / 1000.ToString ("0.0 Km");
		} 
		else 
		{
			AltText.text = MeanAltitude.ToString ("0.0 m");
		}

		if (RadarAltitude > 999.9) 
		{
			RadAltText.text = RadarAltitude / 1000.ToString("0.0 Km");
		} 
		else 
		{
			RadAltText.text = RadarAltitude.ToString("0.000 m");
		}
		HORSpeedText.text = HorizontalSpeed.ToString("0.00 m/s");
		VERTSpeedText.text = VerticalSpeed.ToString("0.00 m/s");
		ThrottleText.text = Throttle.ToString("0 %");

		if (SAS) 
		{
			SASText.text = "On";
			SASText.color = Color.green;
		} 
		else 
		{
			SASText.text = "Off";
			SASText.color = Color.white;
		}

		if (RCS) 
		{
			RCSText.text = "On";
			RCSText.color = Color.green;
		} 
		else 
		{
			RCSText.text = "Off";
			RCSText.color = Color.white;
		}

		if (Gear) 
		{
			GearText.text = "Down";
			GearText.color = Color.green;
		} 
		else 
		{
			GearText.text = "Up";
			GearText.color = Color.white;
		}

		if (Brakes) 
		{
			BrakesText.text = "On";
			BrakesText.color = Color.green;
		} 
		else 
		{
			BrakesText.text = "Off";
			BrakesText.color = Color.white;
		}

		if (Lights) 
		{
			LightsText.text = "On";
			LightsText.color = Color.green;
		} 
		else 
		{
			LightsText.text = "Off";
			LightsText.color = Color.white;
		}
	}

	//Connects to krpc server
	void ConnectToServer() 
	{
		conn = new Connection(
			name: "RasterPropMonitor",
			address: IPAddress.Parse("127.0.0.1"),
			rpcPort: 1000,
			streamPort: 1001);
		var krpc = conn.KRPC();
		Debug.Log(krpc.GetStatus().Version);

		//Start update coroutine
		StartCoroutine ("GetUpdate");
	}

	//Updates data
	void VesselUpdate ()
	{
		var spaceCenter = conn.SpaceCenter();
		var vessel = spaceCenter.ActiveVessel;
		var flightInfo = vessel.Flight();
		var srfFrame = vessel.SurfaceReferenceFrame;
		Heading = flightInfo.Heading;
		Roll = flightInfo.Roll;
		Pitch = flightInfo.Pitch;
		SrfSpeed = vessel.Flight (vessel.Orbit.Body.ReferenceFrame).Speed;
		MeanAltitude = vessel.Flight (vessel.Orbit.Body.ReferenceFrame).MeanAltitude;
		RadarAltitude = vessel.Flight (vessel.Orbit.Body.ReferenceFrame).SurfaceAltitude;
		OrbitalVelocity = vessel.Orbit.Speed;
		HorizontalSpeed = vessel.Flight (vessel.Orbit.Body.ReferenceFrame).HorizontalSpeed;
		VerticalSpeed = vessel.Flight (vessel.Orbit.Body.ReferenceFrame).VerticalSpeed;
		Throttle = vessel.Control.Throttle;
		SAS = vessel.Control.SAS;
		RCS = vessel.Control.RCS;
		Gear = vessel.Control.Gear;
		Brakes = vessel.Control.Brakes;
		Lights = vessel.Control.Lights;
		rotation = new Quaternion ((float)vessel.Rotation(srfFrame).Item1, (float)vessel.Rotation(srfFrame).Item3, (float)vessel.Rotation(srfFrame).Item2, (float)vessel.Rotation(srfFrame).Item4);
	}

	//Gets updates from krpc at a fixed rate
	IEnumerator GetUpdate()
	{
		//Update data
		VesselUpdate ();
		//kRPC update rate
		yield return new WaitForSeconds (0.1f);
		//Restart coroutine
		StartCoroutine ("GetUpdate");
	}

	void OnApplicationQuit()
	{
		//Stops connection when the application is closed
		conn.Dispose();
	}
}
