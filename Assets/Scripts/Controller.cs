using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Controller : NetworkBehaviour {
	public float EngineForce;
	public float drag_coeff;
	public float rr_coeff;
	public float steering;
	public float braking_coeff;

	private float v;
	private float h;
	private float cur_velocity;
	private float deceleration;
	private float braking;

	//steering variables
	private float degree_rot_per_second;
	private float degree_rot_per_frame;
	private float turn_radius; 
	private float turn_distance; //180 degree turn distance
	private float friction_coeff;
	private float rotation;

	//debugging
	private float cur_velocity_mph;
	private float speed;

	public Vector2 curspeed;
	Rigidbody2D rigidbody2D;

	// Use this for initialization
	void Start () {
		rigidbody2D = GetComponent<Rigidbody2D>();
		friction_coeff = 0.9f;
	}

	// Update is called once per frame
	void FixedUpdate () {
		if (isLocalPlayer) {

			v = Input.GetAxis ("Vertical");
			h = Input.GetAxis ("Horizontal");
			cur_velocity = rigidbody2D.velocity.magnitude;

			//Acceleration
			deceleration = (rr_coeff * cur_velocity * cur_velocity + drag_coeff * cur_velocity) * Time.deltaTime;

			//Braking
			//Will be removed and replace with braking_coeff*braking*Time.deltaTime*Right Trigger Pressure for Xbox Controller
			if (Input.GetKey ("space")) {
				braking = 1;
			} else {
				braking = 0;
			}

			float velocity1 = cur_velocity + EngineForce * Time.deltaTime * v - deceleration - braking_coeff * braking * Time.deltaTime;
			rigidbody2D.velocity = transform.up * velocity1; // Drew I changed this to forward ----------

			if (v == 0 && braking == 1 && cur_velocity < 0.5) { //Stops vehicle completely
				rigidbody2D.velocity = new Vector2 (0f, 0f);
			}

			//Steering

			//Old Steering Script
			/*rotation = transform.localEulerAngles.z - steering * h; //Creating Z value of rotation
		float reverse_rotation = transform.localEulerAngles.z + steering * h;

		if (velocity1 > 0) { //prevents car from turning when not moving
			transform.localEulerAngles = new Vector3 (0.0f, 0.0f, rotation);//sets new angle to direction
		}
		else if(velocity1 < 0){
			transform.localEulerAngles = new Vector3 (0.0f, 0.0f, reverse_rotation);
		}*/

			if (cur_velocity > 6.7f) {//When vehicle is moving >6.7 m/s turn radius is minimum safe turn radius (v^2)/(mu * gravity) = r
				turn_radius = (cur_velocity * cur_velocity) / (friction_coeff * 9.8f); //Maximum SAFE turn radius
				turn_distance = turn_radius * 3.14f;
			} else if (cur_velocity <= 6.7f) {
				turn_distance = 16.02f;//Camry 2007 turn_radius = 5m
			}

			degree_rot_per_second = 180f / (turn_distance / cur_velocity);
			degree_rot_per_frame = -degree_rot_per_second * Time.deltaTime * h;
			rotation = transform.localEulerAngles.z + degree_rot_per_frame;
			transform.localEulerAngles = new Vector3 (0.0f, 0.0f, rotation);

			rigidbody2D.angularVelocity = 0.0f;//Prevents floaty movement

			//Debugging
			cur_velocity_mph = cur_velocity / 0.44704f;
			speed = cur_velocity_mph;
		}
	}
	void OnGUI(){
		if(isLocalPlayer)
			GUI.Label (new Rect (10, 10, 100, 90), speed + " mph");
	}

}
