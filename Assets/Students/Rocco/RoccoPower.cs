using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sounds_Rocco))]
public class RoccoPower : GenericPower {
	public float activeTime = 1.5f;
	public float speedMult = 2f;
	public float newTimeScale = 0.5f;
	public AudioClip noise;
	private Sounds_Rocco sounds;
	private bool active;
	private float timer;
	private float oSpeed;

	private void Start() {
		sounds = GetComponent<Sounds_Rocco>();
		oSpeed = Player.Speed;
		Time.timeScale = 1f;
	}

	public override void Activate() {
		if (active == false) {
			sounds.source.PlayOneShot(noise);
			Time.timeScale = newTimeScale;
			Player.Speed = oSpeed * speedMult;
			Player.Body.color = Color.yellow;
			active = true;
		}
	}

	void Update() {
		if (active == true) {
			timer += Time.unscaledDeltaTime;
		}

		if (timer >= activeTime) {
			Time.timeScale = 1f;
			timer = 0f;
			Player.Speed = oSpeed;
			Player.Body.color = Color.white;
			active = false;
		}
	}
}
