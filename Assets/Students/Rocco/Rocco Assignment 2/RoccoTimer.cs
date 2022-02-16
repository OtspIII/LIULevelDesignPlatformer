using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RoccoTimer : MonoBehaviour {
	public TextMeshProUGUI text;
	private float timer;
	public float time;
	public GameObject lose;
	private bool lost;

	private void Start() {
		timer = time;
	}

	private void Update() {
		timer -= Time.deltaTime;
		text.text = Mathf.RoundToInt(timer).ToString();

		if (timer <= 0) {
			lose.SetActive(true);
			PlayerController.PC.gameObject.SetActive(false);
			lost = true;
		}

		if (timer <= -8 || lost == true && Input.GetKeyDown(KeyCode.X) == true) {
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}
	}
}

// ~ Rocco Russo