﻿using UnityEngine;
using System.Collections;

public class ControlPersonaje : MonoBehaviour {

	public float velocidadCaminar;
	public float impulsoSalto;

	public Transform puntoVerificarPiso;
	public LayerMask capaPiso;

	private Rigidbody2D bodyPersonaje;
	private Vector2 movimiento;

	private bool saltarInput;
	public float horizontalInput;
	public bool estoyEnElPiso;
	public bool estoyMuerto;
	public bool mover;


	public CircleCollider2D cuerpoFisico;

	private Animator animacionPersonajePrueba;
	private bool mirarLadoDerecho;

	public AudioSource sonidoMorirPorCaida;

	// Use this for initialization
	void Start () {


		this.bodyPersonaje= this.GetComponent<Rigidbody2D>();
		this.movimiento = new Vector2 ();

		this.estoyEnElPiso = false;

		this.animacionPersonajePrueba= this.GetComponent<Animator>();
	
		this.mirarLadoDerecho = true;

		this.mover = true;
	
	}

	// Update is called once per frame
	void Update () {

		if (estoyMuerto == true) {


			Application.LoadLevel ("nivel00");

			return;
		}
		if (this.mover) {
			
			this.horizontalInput = Input.GetAxis ("Horizontal");
			this.saltarInput = Input.GetKey (KeyCode.Space);

			this.animacionPersonajePrueba.SetFloat ("VelocidadHorizontal", Mathf.Abs (this.bodyPersonaje.velocity.x));
			this.animacionPersonajePrueba.SetFloat ("VelocidadVertical", Mathf.Abs (this.bodyPersonaje.velocity.y));


			if ((horizontalInput < 0.0f) && (this.mirarLadoDerecho)) {
				this.Doblar ();
				this.mirarLadoDerecho = false;
			} else if ((horizontalInput > 0.0f) && (!this.mirarLadoDerecho)) {
				this.Doblar ();
				this.mirarLadoDerecho = true;
			}

			//Si estoy en el aire (No estoy tocando nada)
			if (Physics2D.OverlapCircle (this.puntoVerificarPiso.position, 0.02f, this.capaPiso)) {
				this.estoyEnElPiso = true;
				//Debug.Log("ESTOY TOCANDO EL PSIO");

			} else {
				this.estoyEnElPiso = false;

				//Debug.Log("ESTOY EN EL AIRE");
			}
		}
		//FixedUpdate ();
	}

	void FixedUpdate(){
		this.movimiento = this.bodyPersonaje.velocity;

		this.movimiento.x = horizontalInput * velocidadCaminar;

		if(this.saltarInput==true && this.estoyEnElPiso==true){
			this.movimiento.y=impulsoSalto;
		}
		//Para caer con Velocidad Constante = 8.0 y no acelerar en caidas Vmax=8.0
		if(!estoyEnElPiso){
			if(this.movimiento.y < -8.0f){
				this.movimiento.y=-8.0f;
			}
		}

		this.bodyPersonaje.velocity = this.movimiento;

	}

	void Doblar(){
		//Vector3 escala = this.transform.localScale;
		//escala.x *= (-1);
		//this.transform.localScale = escala;
		this.transform.Rotate(Vector3.up, 180);

	}


	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag == "Enemigo" && !estoyMuerto){
			Morir ();
		}
	}

	void OnCollisionStay2D (Collision2D col){
		if (col.gameObject.tag == ("Enemigo") && !estoyMuerto){
			Morir ();
		}
	}
		

	void OnTriggerStay2D (Collider2D other){
		if (other.tag == "Peligro" && !estoyMuerto) {
			
			MorirPorCaida ();
		}

		if (other.tag == "Portal" && !estoyMuerto) {
			Debug.Log ("Estoy tocando el portal");
			StartCoroutine (cambiarNivel());
			//cambiarNivel ();
		}
	
	}


	void Morir(){
		this.mover = false;
		this.horizontalInput = 0;
		this.impulsoSalto = 0;
		bodyPersonaje.velocity = Vector2.zero;
		bodyPersonaje.AddForce (new Vector2 (-60, 50));
		this.animacionPersonajePrueba.SetBool ("Muerte", true);
		this.bodyPersonaje.freezeRotation = false;
		this.bodyPersonaje.MoveRotation (-90);

	
		StartCoroutine (Fallecer(1.5f));
	
	}


	void MorirPorCaida(){
		//this.mover = false;
		bodyPersonaje.AddForce (new Vector2 (0, 300));
		bodyPersonaje.velocity = Vector2.zero;
		this.animacionPersonajePrueba.SetBool ("Muerte", true);
		bodyPersonaje.Sleep ();

		//sonidoMorirPorCaida.Play ();

		bodyPersonaje.WakeUp ();
		this.bodyPersonaje.freezeRotation = false;
		this.bodyPersonaje.MoveRotation (45);
		bodyPersonaje.AddForce (new Vector2 (0, -5));


		StartCoroutine (Fallecer(1.8f));
		//estoyMuerto = true;

	}

	IEnumerator Fallecer(float time){
		// Le decimos que espere 1/2 de segundo antes de caerse

		yield return new WaitForSeconds (time);
		this.estoyMuerto = true;
		this.bodyPersonaje.gravityScale = 0;
		// inica la Corrutina de devolver el bloque a su posici{on inicial

	}
	IEnumerator cambiarNivel(){


		yield return new WaitForSeconds (0.6f);
		float fadeTime = GameObject.Find ("_GM").GetComponent<Fade>().BeginFade(1);
		yield return new WaitForSeconds (fadeTime);
		Application.LoadLevel ("nivel01");

	}


	} 