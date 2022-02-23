using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoccoTrigger : MonoBehaviour {
	public enum actions {
		activate, deactivate
	}
	public actions action;
	public GameObject target;

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.CompareTag("Player") == true) {
			DoTheThing();
		}
	}

	private void DoTheThing() {
		if (action == actions.activate) {
			target.SetActive(true);
		} else if (action == actions.deactivate) {
			target.SetActive(false);
		}
	}
}

// ~ Rocco Russo