using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

    public static SoundManager soundSysManager;

    public AudioClip firstStratumMusic;
    public AudioClip firstStratumBeats;

    public AudioClip secondStratumMusic;
    public AudioClip secondStratumBeats;
    
    public AudioClip logMusic;

    public AudioClip unlockingSound;
    public AudioClip selectingSound;
    public AudioClip validationSound;
    public AudioClip lvlendSound;
    public AudioClip restartSound;
    public AudioClip helpSound;

    public AudioSource mainSource; //play main music
    public AudioSource secondarySource; //play beats of main music
    public AudioSource noisesSource; // play action sound

    private bool InGame;
    private bool InDanger;
    public int dangerlimit; //nbr of transfo before music change
    private Shape player;
    private GameManager gm;

    void Awake()
    {
        if (soundSysManager == null)
        {
            soundSysManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    // Use this for initialization
	void Start () {
        dangerlimit = -1;
        InDanger = false;
        player = (Shape)(GameObject.FindObjectOfType(typeof(Shape)));
        if (player == null)
        {
            InGame = false;
            PlayLogMenu();
        }
        else
            InGame = true;
	}

    void OnLevelWasLoaded(int level)
    {
       //get all button and add play song event
    }
	
	// Update is called once per frame
	void Update () {
        
        player = (Shape)(GameObject.FindObjectOfType(typeof(Shape)));

            if (player != null)
            {
                if (InGame == false)
                {
                    InGame = true;
                    PlayFirstStratum();
                }
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
                
                gm = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
                if (gm != null)
                {
                    if (gm.getMaxTransfo() >= 3)
                        dangerlimit = gm.getMaxTransfo() / 4;
                    /* need to get max transfo for this. until then */
                    /*
                    if (gm.remainingTransformation >= dangerlimit && InDanger == true)
                    {
                        dangerlimit = gm.remainingTransformation / 3;
                        InDanger = false;
                    }
                    */

                    if (gm.remainingTransformation < dangerlimit)
                    {
                        if (InDanger == false)
                        {
                            InDanger = true;
                            PlayFirstStratumBeats();
                            StopMusic();
                        }
                    }
                    else
                    {
                        if (InDanger == true)
                        {
                            InDanger = false;
                            PlayFirstStratum();
                            StopBeats();
                        }
                    }
                }
            }
            else
            {
                if (InGame == true)
                {
                    PlayLogMenu();
                    InGame = false;
                }
            }
            //secondarySource.timeSamples = mainSource.timeSamples;
	}

    void PlayLogMenu()
    {
        mainSource.volume = 0.2f;
        StopBeats();
        mainSource.Stop();
        mainSource.loop = true;
        mainSource.clip = logMusic;
        mainSource.Play();  
    }

    void PlayFirstStratum()
    {
        mainSource.volume = 0.05f;
        mainSource.Stop();
        mainSource.loop = true;
        mainSource.clip = firstStratumMusic;
        mainSource.Play();
    }

    void PlayFirstStratumBeats()
    {
        secondarySource.volume = 0.2f;
        secondarySource.Stop();
        secondarySource.loop = true;
        secondarySource.clip = firstStratumBeats;
        secondarySource.Play();
    }

    void PlaySecondStratum()
    {
        mainSource.volume = 0.2f;
        mainSource.Stop();
        mainSource.loop = false;
        mainSource.clip = secondStratumMusic;
        mainSource.Play();
    }

    void PlaySecondStratumBeats()
    {
        secondarySource.volume = 1.0f;
        secondarySource.Stop();
        secondarySource.loop = true;
        secondarySource.clip = secondStratumBeats;
        secondarySource.Play();
    }

    void StopBeats()
    {
        secondarySource.volume = 0.0f;
        secondarySource.Stop();
    }

    void StopMusic()
    {
        mainSource.volume = 0.0f;
        mainSource.Stop();
    }

    public void PlayUnlocking()
    {
        noisesSource.volume = 1.0f;
        noisesSource.PlayOneShot(unlockingSound);
    }

    public void PlaySelecting()
    {
        noisesSource.volume = 1.0f;
        noisesSource.PlayOneShot(selectingSound);
    }

    public void PlayValidation()
    {
        noisesSource.volume = 1.0f;
        noisesSource.PlayOneShot(validationSound);
    }

    public void PlayRestart()
    {
        noisesSource.volume = 1.0f;
        noisesSource.PlayOneShot(restartSound);
    }

    public void PlayLvlEnd()
    {
        noisesSource.volume = 0.2f;
        noisesSource.PlayOneShot(lvlendSound);
    }

    public void PlayHelp()
    {
        noisesSource.PlayOneShot(helpSound);
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

    public void PlayLvlEnd(Vector3 pos)
    {
        Vector3 oldpos = noisesSource.transform.position;
        noisesSource.transform.position = pos;
        noisesSource.PlayOneShot(lvlendSound);
        noisesSource.transform.position = oldpos;
    }

    public void PlayHelp(Vector3 pos)
    {
        Vector3 oldpos = noisesSource.transform.position;
        noisesSource.transform.position = pos;
        noisesSource.PlayOneShot(helpSound);
        noisesSource.transform.position = oldpos;
    }
}
