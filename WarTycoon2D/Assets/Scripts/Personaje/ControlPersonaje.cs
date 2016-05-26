using UnityEngine;
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

	public CircleCollider2D cuerpoFisico;

	private Animator animacionPersonajePrueba;
	private bool mirarLadoDerecho;



	// Use this for initialization
	void Start () {


		this.bodyPersonaje= this.GetComponent<Rigidbody2D>();
		this.movimiento = new Vector2 ();

		this.estoyEnElPiso = false;

		this.animacionPersonajePrueba= this.GetComponent<Animator>();
	
		this.mirarLadoDerecho = true;
	
	}

	// Update is called once per frame
	void Update () {

		if (estoyMuerto == true)
			return;
		
		this.horizontalInput = Input.GetAxis ("Horizontal");
		this.saltarInput = Input.GetKey(KeyCode.Space);

		this.animacionPersonajePrueba.SetFloat ("VelocidadHorizontal",Mathf.Abs(this.bodyPersonaje.velocity.x));
		this.animacionPersonajePrueba.SetFloat ("VelocidadVertical", Mathf.Abs (this.bodyPersonaje.velocity.y));


		if ((horizontalInput < 0.0f) && (this.mirarLadoDerecho)) {
			this.Doblar ();
			this.mirarLadoDerecho=false;
		}else if((horizontalInput>0.0f) && (!this.mirarLadoDerecho)){
			this.Doblar ();
			this.mirarLadoDerecho=true;
		}

		//Si estoy en el aire (No estoy tocando nada)
		if (Physics2D.OverlapCircle (this.puntoVerificarPiso.position, 0.02f, this.capaPiso)) {
			this.estoyEnElPiso = true;
			//Debug.Log("ESTOY TOCANDO EL PSIO");

		} else {
			this.estoyEnElPiso = false;

			//Debug.Log("ESTOY EN EL AIRE");
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
		if (col.gameObject.tag == "Peligro" && !estoyMuerto){
			Morir ();

		}
	}


	void Morir(){
		bodyPersonaje.velocity = Vector2.zero;
		this.horizontalInput = 0;
		this.impulsoSalto = 0;

		this.animacionPersonajePrueba.SetBool ("Muerte", true);
		bodyPersonaje.AddForce (new Vector2 (-60, 50));
		estoyMuerto = true;
	
	}



}
