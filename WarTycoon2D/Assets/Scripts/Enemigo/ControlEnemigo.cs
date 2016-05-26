using UnityEngine;
using System.Collections;

public class ControlEnemigo : MonoBehaviour {

	public float velocidadCaminar;

	public Transform puntoVerificarPiso;
	public LayerMask capaPiso;


	private Rigidbody2D bodyPersonaje;
	private Vector2 movimiento;

	public float horizontalInput;
	public bool estoyEnElPiso;
	public bool estoyMuerto;
	//Variable para implementar Inteligencia artificial

	public Transform etiqueta;
	public CircleCollider2D cuerpoFisico;

	private Animator animacionEnemigo;
	private bool mirarLadoDerecho;

	public AudioSource sonidoAtacar;


	// Use this for initialization
	void Start () {


		this.bodyPersonaje= this.GetComponent<Rigidbody2D>();
		this.movimiento = new Vector2 ();

		this.estoyEnElPiso = false;

		this.animacionEnemigo= this.GetComponent<Animator>();
	
		this.mirarLadoDerecho = true;

	
		//sonidoAtacar = GameObject.FindWithTag ("SonidoAtacar");
	
	}

	// Update is called once per frame
	void Update () {
		PerseguirMason ();
		
		this.animacionEnemigo.SetFloat ("VelocidadHorizontal",Mathf.Abs(this.bodyPersonaje.velocity.x));
		this.animacionEnemigo.SetFloat ("VelocidadVertical", Mathf.Abs (this.bodyPersonaje.velocity.y));

		if ((horizontalInput > 0.0f) && (this.mirarLadoDerecho)) {
			this.Doblar ();
			this.mirarLadoDerecho=false;
		}else if((horizontalInput < 0.0f) && (!this.mirarLadoDerecho)){
			this.Doblar ();
			this.mirarLadoDerecho=true;
		}
		//Si estoy en el aire (No estoy tocando nada)
		if (Physics2D.OverlapCircle (this.puntoVerificarPiso.position, 0.02f, this.capaPiso)) {
			this.estoyEnElPiso = true;
			//Debug.Log("ESTOY TOCANDO EL PISO");

		} else {
			this.estoyEnElPiso = false;

			//Debug.Log("ESTOY EN EL AIRE");
		}

		//FixedUpdate ();
	}

	void FixedUpdate(){
		this.movimiento = this.bodyPersonaje.velocity;

		this.movimiento.x = horizontalInput * velocidadCaminar;

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
		if (col.gameObject.tag == "Player"){
			Atacar ();

		}
	}
		
	void Atacar(){

		bodyPersonaje.AddForce (new Vector2 (0, 0));
		sonidoAtacar.Play();

		this.animacionEnemigo.SetBool ("Muerte", true);
	
	}
	//Metodo para perseguir al personaje
	void PerseguirMason(){

		if (this.transform.position.x < this.etiqueta.position.x) {
			this.horizontalInput = 1;
		} else if (this.transform.position.x > this.etiqueta.position.x) {
			this.horizontalInput = -1;	
	}
}
}