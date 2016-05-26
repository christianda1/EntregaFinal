using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Arma : MonoBehaviour {
	//Variables
	//Variable publica para definir con que choca el rayo del arma de Mason
	public LayerMask objetoChocante;
	public GameObject balaPrefab;
	public float frecuenciaDisparo;
	public float dano;
	public int municion;
	//Creacion de elementos UI para mostrar la municion
	public Text cantidadMunicion;
	private float time;
	private float alcanceRayo;

	public Transform posicionTiro;
	// Use this for initialization
	void Start () {
		this.time = 0;
		this.actualizarMunicion();
	}
	
	// Update is called once per frame
	void Update () {

		this.time += Time.deltaTime;

		if(Input.GetButton("Fire1") && (this.time > this.frecuenciaDisparo)){
		this.disparar ();
			this.time = 0;
			this.municion--;
			this.actualizarMunicion ();
			this.alcanceRayo = 1.5f;
		}
	}

	//Metodo para disparar
	void disparar(){
		//Rayo invisible para informar si el disparo la posicion y la distancia a la que llega
		RaycastHit2D contacto = Physics2D.Raycast (this.posicionTiro.position, this.transform.right, alcanceRayo, objetoChocante);

		//Instanciar el objeto de tipo Gameobject
		GameObject balaTMP = Instantiate(this.balaPrefab) as GameObject;
		LineRenderer bala = balaTMP.GetComponent<LineRenderer>();
		//Si hay contacto del rayo con un objeto
		if (contacto.collider) {
			bala.SetPosition (0, this.posicionTiro.position);
			bala.SetPosition (1, contacto.point);
			Debug.Log ("Hay contacto");
		} else {
			bala.SetPosition (0, this.posicionTiro.position);
			bala.SetPosition (1, this.posicionTiro.position + (this.transform.right*alcanceRayo));
			Debug.Log ("No hay contacto");
		}
		Destroy (balaTMP, 0.05f);
	}

	//Metodo para mostrar la munición
	void actualizarMunicion (){
		this.cantidadMunicion.text = "Munición:" + this.municion;
	}
}
