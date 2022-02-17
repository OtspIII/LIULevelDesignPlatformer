using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Sounds_Rocco))]
public class RoccoPower : GenericPower {
	public float activeTime = 1.5f;
	public float speedMult = 2f;
	public float newTimeScale = 0.5f;
	public AudioClip noise;
	public AudioClip coolNoise;
	private Sounds_Rocco sounds;
	private bool active;
	private float timer;
	private float oSpeed;

	private float cooldown;
	private bool cooling;

	private void Start() {
		sounds = GetComponent<Sounds_Rocco>();
		oSpeed = Player.Speed;
		Time.timeScale = 1f;
	}

	public override void Activate() {
		if (active == false) {
			if (cooling == false) {
				sounds.source.volume = 0.25f;
				sounds.source.PlayOneShot(noise);
				Time.timeScale = newTimeScale;
				Player.Speed = oSpeed * speedMult;
				Player.Body.color = Color.yellow;
				active = true;
			} else {
				if (sounds.source.isPlaying == false) {
					sounds.source.volume = 1f;
					sounds.source.clip = coolNoise;
					sounds.source.Play();
				}
			}
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
			cooling = true;
		}

		if (cooling == true) {
			cooldown += Time.deltaTime;
		}

		if (cooldown >= activeTime) {
			cooling = false;
			cooldown = 0f;
		}
	}
}
