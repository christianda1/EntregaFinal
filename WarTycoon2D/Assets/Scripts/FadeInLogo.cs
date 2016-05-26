using UnityEngine;
using System.Collections;

public class FadeInLogo : MonoBehaviour {

	public int numLogo;


	// Use this for initialization
	void Start () {
	
	
		StartCoroutine (realizasConteo());
	}
	
	// Update is called once per frame
	private IEnumerator realizasConteo(){


		yield return new WaitForSeconds (3);


		switch(numLogo){

		case 1:
			Application.LoadLevel ("logo1");
			break;

		case 2:
			Application.LoadLevel ("menu");
			break;

		case 3:
			Application.LoadLevel ("nivel00");
			break;

		default:
			Debug.LogError ("No se encuentran las escenas");
			break;
		}


	}
}
