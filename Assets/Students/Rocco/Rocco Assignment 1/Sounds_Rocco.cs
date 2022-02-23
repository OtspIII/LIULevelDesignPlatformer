using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds_Rocco : MonoBehaviour {
	public PlayerController pc;
	public AudioSource source;
	public AudioClip die;
	private bool ded;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.GetComponent<DangerController>() == true && ded == false) {
			source.volume = 1;
			source.PlayOneShot(die);
			ded = true;
		}
	}
}

// ~ Rocco Russo