How To USE:

SoundManager.Instance.CreateSound()
	.WithSoundData(soundData) //If has soundData
	.WithRandomPitch() //When wants to Use it
	.WithPosition(prefab.transform.position)  //When its attached to a especific position
	.Play();