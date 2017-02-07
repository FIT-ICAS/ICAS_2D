using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Digit : MonoBehaviour {

	public int number = 0;
	public bool displayZero = false;
	public GameObject a, b, c, d, e, f, g;
	private GameObject[] zero = new GameObject[6], // a b c d e f
	one= new GameObject[2], // e, d
	two = new GameObject[5], //{b, c, e, f, g};
	three= new GameObject[5],//{c, d, e, f, g};
	four= new GameObject[4],//{a, d, e, g};
	five= new GameObject[5],//{a, c, d, f, g};
	six= new GameObject[6],//{a, b, c, d, f, g};
	seven= new GameObject[3],// {d, e, f};
	eight= new GameObject[7],//{a, b, c, d, e, f, g};
	nine= new GameObject[5];//{a, d, e, f, g};
	private GameObject[][] numbers = new GameObject[10][];
	// Use this for initialization
	void Start(){
		zero [0] = a; zero [1] = b; zero [2] = c; zero [3] = d; zero [4] = e; zero[5] = f;
		one [0] = e; one [1] = d;
		two [0] = b; two [1] = c; two [2] = e; two [3] = f; two [4] = g;
		three [0] = c; three [1] = d; three [2] = e; three [3] = f; three [4] = g;
		four [0] = a; four [1] = d; four [2] = e; four [3] = g;
		five [0] = a; five [1] = c; five [2] = d; five [3] = f; five [4] = g;
		six [0] = a; six [1] = b; six [2] = c; six [3] = d; six [4] = f; six [5] = g;
		seven [0] = d; seven [1] = e; seven [2] = f;
		eight [0] = a; eight [1] = b; eight [2] = c; eight [3] = d; eight [4] = e; eight [5] = f; eight [6] = g;
		nine [0] = a; nine [1] = d; nine [2] = e; nine [3] = f; nine [4] = g;
		numbers [0] = zero; numbers [1] = one; numbers [2] = two; numbers [3] = three; numbers [4] = four; numbers [5] = five; numbers [6] = six; numbers [7] = seven; numbers [8] = eight; numbers [9] = nine;
	}

	void FixedUpdate(){
		displayNumber (number);
	}
	void displayNumber(int num){
		foreach (GameObject obj in eight) { // first disable ever segment
			obj.SetActive (false);
		}
		if(displayZero && num >= 0)
		foreach (GameObject obj in numbers[num]) {
			obj.SetActive (true);
		}
		if(!displayZero && num > 0)
			foreach (GameObject obj in numbers[num]) {
				obj.SetActive (true);
			}
	}
}
