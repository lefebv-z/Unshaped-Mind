using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour {

    public static SoundManager soundSysManager;

    public AudioClip firstStratumMusic;
    public AudioClip firstStratumBeats;

    /*public AudioClip secondStratumMusic;
    public AudioClip secondStratumBeats;
    */

    public AudioClip logMusic;

    public AudioClip unlockingSound;
    public AudioClip selectingSound;
    public AudioClip validationSound;
    public AudioClip lvlendSound;
    public AudioClip restartSound;
    public AudioClip changeSound;
    public AudioClip nochangeSound;
    //wall hit
    public AudioClip helpSound;

    public AudioSource mainSource; //play main music
    public AudioSource secondarySource; //play beats of main music
    public AudioSource noisesSource; // play action sound

    private bool InGame;
    private bool InDanger;
    private int dangerlimit; //nbr of transfo before music change
    private Shape player;
    private GameManager gm;

    private float bgmVolume;
    private float effectsVolume;

    private float bgmModificator;
    private float beatsModificator;
    private float effectModificator;

    private Button[] buttons;

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
        bgmVolume = 0.5f;
        effectsVolume = 0.5f;
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
            buttons = (Button[])(GameObject.FindObjectsOfType(typeof(Button)));
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(PlayValidation);
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) => { PlaySelecting(); });
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
                if (trigger != null)
                    trigger.triggers.Add(entry);
            }
        
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
                            mainSource.Stop();
                        }
                    }
                    else
                    {
                        if (InDanger == true)
                        {
                            InDanger = false;
                            PlayFirstStratum();
                            secondarySource.Stop();
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
                    secondarySource.Stop();
                }
            }
            //secondarySource.timeSamples = mainSource.timeSamples;
	}

   public void UpdateVolume()
    {
        mainSource.volume = bgmVolume * bgmModificator;
        secondarySource.volume = bgmVolume * beatsModificator;
        noisesSource.volume = effectModificator * effectsVolume;
    }

   public void BGMVolumeChange(float value)
   {
       bgmVolume = value;
       UpdateVolume();
   }

   public void EffectsVolumeChange(float value)
   {
       effectsVolume = value;
       UpdateVolume();
   }

    void PlayLogMenu()
    {
        mainSource.Stop();
        bgmModificator = 1.0f;
        mainSource.volume = bgmModificator * bgmVolume;
        mainSource.loop = true;
        mainSource.clip = logMusic;
        mainSource.Play();  
    }

    void PlayFirstStratum()
    {
        mainSource.Stop();
        bgmModificator = 1.0f;
        mainSource.volume = bgmModificator * bgmVolume;
        mainSource.loop = true;
        mainSource.clip = firstStratumMusic;
        mainSource.Play();
    }

    void PlayFirstStratumBeats()
    {
        secondarySource.Stop();
        beatsModificator = 0.2f;
        secondarySource.volume = beatsModificator * bgmVolume;
        secondarySource.loop = true;
        secondarySource.clip = firstStratumBeats;
        secondarySource.Play();
    }
    /*
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
    */
    void StopBeats()
    {
        secondarySource.Stop();
    }

    void StopMusic()
    {
        mainSource.Stop();
    }

    public void PlayUnlocking()
    {
        effectModificator = 1.0f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(unlockingSound);
    }

    public void PlaySelecting()
    {
        effectModificator = 1.0f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(selectingSound);
    }

    public void PlayValidation()
    {
        effectModificator = 1.0f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(validationSound);
    }

    public void PlayRestart()
    {
        effectModificator = 0.4f;
        noisesSource.volume = effectModificator *  effectsVolume;
        noisesSource.PlayOneShot(restartSound);
    }

    public void PlayLvlEnd()
    {
        effectModificator = 0.2f;
        noisesSource.volume = effectModificator * effectsVolume;
        noisesSource.PlayOneShot(lvlendSound);
    }

    public void PlayChange()
    {
        effectModificator = 0.5f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(changeSound);
    }

    public void PlayNoChange()
    {
        effectModificator = 0.5f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(nochangeSound);
    }

    public void PlayHelp()
    {
        effectModificator = 1.0f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(helpSound);
    }

    /* Give him the position of the wall unlocked */
    void PlayUnlocking(Vector3 pos)
    {
        effectModificator = 1.0f;
        noisesSource.volume = effectsVolume;
        /* unity 5.3 +*/
        //noisesSource.PlayClipAtPoint(unlockingSound, pos);
    }


}
