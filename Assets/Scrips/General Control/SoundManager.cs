using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager> 
{
	AudioSource audioSource;

	[SerializeField] AudioClip arrowSFX, fireballSFX, rockShotSFX, rockSFX, towerBuiltSFX,
		newGameSFX, levelSFX, gameOverSFX, wonSFX, deathSFX;

	protected override void Awake()
	{
		base.Awake();
		audioSource = gameObject.GetComponent<AudioSource>();
		audioSource.clip = levelSFX;
		audioSource.loop = true;
	}

	public void PlayArrowSound()
	{
		audioSource.PlayOneShot(arrowSFX);
	}

	public void PlayFireballSound()
	{
		audioSource.PlayOneShot(fireballSFX);
	}

	public void PlayRockShotSound()
	{
		audioSource.PlayOneShot(rockShotSFX);
	}

	public void PlayRockSound()
	{
		audioSource.PlayOneShot(rockSFX);
	}

	public void PlayTowerBuiltSound()
	{
		audioSource.PlayOneShot(towerBuiltSFX);
	}

	public void PlaySoundsOnGameStart()
	{
		StartCoroutine(gameStartingSound());
	}

	public void StopPlayingLevelSound()
	{
		audioSource.Stop();
	}

	public void PlayWonSound()
	{
		audioSource.PlayOneShot(wonSFX);
	}
	
	public void PlayGameOverSound()
	{
		audioSource.PlayOneShot(gameOverSFX);
	}

	public void PlayDeathSound()
	{
		audioSource.PlayOneShot(deathSFX);
	}
	
	private IEnumerator gameStartingSound()
	{
		audioSource.PlayOneShot(newGameSFX);
		yield return GameManager.Instance.Sync(newGameSFX.length);
		audioSource.Play();
	}
}
