using UnityEngine;
using System.Collections;

public class ControlCamara : MonoBehaviour {

	public Transform objetivoCamara;
	private Vector3 posicionObjetivo;


	void Start () {


	}


	void Update () {
		this.posicionObjetivo = this.objetivoCamara.position;
		this.posicionObjetivo.z = -10;
		this.posicionObjetivo.y = 0;
		if(this.posicionObjetivo.x < -8){
			this.posicionObjetivo.x=-8;
		}
		this.transform.position = Vector3.Lerp (this.transform.position,this.posicionObjetivo,1);

	}
}
