using UnityEngine;
using System.Collections;

public class ControlCamara : MonoBehaviour {

	public Transform objetivoCamara;
	private Vector3 posicionObjetivo;

	private float limiteIzquierdo;
	private float limiteDerecho;
	public int nivel;



	void Start () {

		limiteIzquierdo = -22.3f;
		limiteDerecho = 21.5f;
	}


	void Update () {
		this.posicionObjetivo = this.objetivoCamara.position;
		this.posicionObjetivo.z = -10;
		this.posicionObjetivo.y = 0;

		limitarCamara ();

		this.transform.position = Vector3.Lerp (this.transform.position,this.posicionObjetivo,1);

	}

	void limitarCamara(){
		
		if(this.posicionObjetivo.x < limiteIzquierdo){
			this.posicionObjetivo.x=limiteIzquierdo;
		}

		if(this.posicionObjetivo.x > limiteDerecho){
			this.posicionObjetivo.x=limiteDerecho;
		}

	}
}
