using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds_Rocco : MonoBehaviour {
	public PlayerController pc;
	public AudioClip jump;
	public AudioSource source;
	public AudioClip die;
	private bool ded;

	private void Update() {
		if (Input.GetKeyDown(KeyCode.Z) == true && ded == false && pc.Floors.Count > 0) {
			source.volume = 0.25f;
			source.PlayOneShot(jump);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.GetComponent<DangerController>() == true) {
			source.volume = 1;
			source.PlayOneShot(die);
			ded = true;
		}
	}
}

// ~ Rocco Russo