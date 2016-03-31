using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public AudioClip firstStratumMusic;
    public AudioClip firstStratumBeats;

    public AudioClip secondStratumMusic;
    public AudioClip secondStratumBeats;
    
    public AudioClip logMusic;

    public AudioClip unlockingSound;
    public AudioClip selectingSound;
    public AudioClip validationSound;
    public AudioClip restartSound;

    public AudioSource mainSource; //play main music
    public AudioSource secondarySource; //play beats of main music
    public AudioSource noisesSource; // play action sound

    // Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        secondarySource.timeSamples = mainSource.timeSamples; 
	}

    void PlayLogMenu()
    {
        mainSource.Stop();
        mainSource.loop = true;
        mainSource.clip = logMusic;
        mainSource.Play();

        secondarySource.Stop();   
    }

    void PlayFirstStratum()
    {
        mainSource.Stop();
        mainSource.loop = true;
        mainSource.clip = firstStratumMusic;
        mainSource.Play();

        secondarySource.Stop();
        secondarySource.loop = true;
        secondarySource.clip = firstStratumBeats;
        secondarySource.Play();
    }

    void PlaySecondStratum()
    {
        mainSource.Stop();
        mainSource.loop = true;
        mainSource.clip = secondStratumMusic;
        mainSource.Play();

        secondarySource.Stop();
        secondarySource.loop = true;
        secondarySource.clip = secondStratumBeats;
        secondarySource.Play();
    }

    public void PlayUnlocking()
    {
        noisesSource.PlayOneShot(unlockingSound);
    }

    public void PlaySelecting()
    {
        noisesSource.PlayOneShot(selectingSound);
    }

    public void PlayValidation()
    {
        noisesSource.PlayOneShot(validationSound);
    }

    public void PlayRestart()
    {
        noisesSource.PlayOneShot(restartSound);
    }

    void PlayUnlocking(Vector3 pos)
    {
        Vector3 oldpos = noisesSource.transform.position;
        noisesSource.transform.position = pos;
        noisesSource.PlayOneShot(unlockingSound);
        noisesSource.transform.position = oldpos;
    }

    void PlaySelecting(Vector3 pos)
    {
        Vector3 oldpos = noisesSource.transform.position;
        noisesSource.transform.position = pos;
        noisesSource.PlayOneShot(selectingSound);
        noisesSource.transform.position = oldpos;
    }

    void PlayValidation(Vector3 pos)
    {
        Vector3 oldpos = noisesSource.transform.position;
        noisesSource.transform.position = pos;
        noisesSource.PlayOneShot(validationSound);
        noisesSource.transform.position = oldpos;
    }

    void PlayRestart(Vector3 pos)
    {
        Vector3 oldpos = noisesSource.transform.position;
        noisesSource.transform.position = pos;
        noisesSource.PlayOneShot(restartSound);
        noisesSource.transform.position = oldpos;
    }
}
