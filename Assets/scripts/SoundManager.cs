using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SoundManager : MonoBehaviour {

    public static SoundManager soundSysManager;

//    public AudioClip firstStratumMusic;
//    public AudioClip secondStratumMusic;
	public AudioClip[] stratumsMusic;
	public float[] stratumsBgmModificators;
    public AudioClip firstStratumBeats;
    public AudioClip logMusic;

    public AudioClip unlockingSound;
    public AudioClip selectingSound;
    public AudioClip validationSound;
    public AudioClip lvlendSound;
    public AudioClip restartSound;
    public AudioClip changeSound;
    public AudioClip nochangeSound;
    public AudioClip wallhit;
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
    private int stratum;
    private int currenlyStratumMusic;

    private Button[] buttons;
    private bool levelSelectorOn; 

    void Awake()
    {
        levelSelectorOn = false;
        if (soundSysManager == null)
        {
            soundSysManager = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
        LevelNumberScript lvlnumb = (LevelNumberScript)(GameObject.FindObjectOfType(typeof(LevelNumberScript)));
        if (lvlnumb != null)
            stratum = lvlnumb.GetStratum();
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


         //buttons = (Button[])(GameObject.FindObjectsOfTypeAll(typeof(Button)));
         buttons = Resources.FindObjectsOfTypeAll<Button>();
         foreach (Button button in buttons)
         {
             EventTrigger.Entry entry = new EventTrigger.Entry();
             entry.eventID = EventTriggerType.Select;
             entry.callback.AddListener((eventData) => { PlaySelecting(); });
             EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
             if (trigger != null)
                 trigger.triggers.Add(entry);
         }

	}

    void OnLevelWasLoaded(int level)
    {
        LevelNumberScript lvlnumb = (LevelNumberScript)(GameObject.FindObjectOfType(typeof(LevelNumberScript)));
        if (lvlnumb != null)
        {
            stratum = lvlnumb.GetStratum();
            if (stratum != currenlyStratumMusic)
                PlayStratum();
        }
        //buttons = (Button[])(GameObject.FindObjectsOfType(typeof(Button)));
        buttons = Resources.FindObjectsOfTypeAll<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayValidation);
            EventTrigger.Entry entry = new EventTrigger.Entry();
            EventTrigger.Entry entrymouse = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.Select;
            entrymouse.eventID = EventTriggerType.PointerEnter;
            entry.callback.AddListener((eventData) => { PlaySelecting(); });
            entrymouse.callback.AddListener((eventData) => { PlaySelecting(); });
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            if (trigger != null)
            {
                trigger.triggers.Add(entry);
                trigger.triggers.Add(entrymouse);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
        
        player = (Shape)(GameObject.FindObjectOfType(typeof(Shape)));

            if (player != null)
            {
                LevelNumberScript lvlnumb = (LevelNumberScript)(GameObject.FindObjectOfType(typeof(LevelNumberScript)));
                if (lvlnumb != null)
                    stratum = lvlnumb.GetStratum();
                if (InGame == false)
                {
                    
                    InGame = true;
                    PlayStratum();
                }
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -10);
                gm = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
                if (gm != null)
                {
                    if (gm.getMaxTransfo() >= 3)
                        dangerlimit = gm.getMaxTransfo() / 4;
                    else
                        dangerlimit = -1;
                    if (gm.remainingTransformation < dangerlimit)
                    {
                        if (InDanger == false)
                        {
                            InDanger = true;
                            PlayFirstStratumBeats();
                            mainSource.Stop();
                            currenlyStratumMusic = 0;
                        }
                    }
                    else
                    {
                        if (InDanger == true)
                        {
                            InDanger = false;
                            PlayStratum();
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

    public void addBSound()
    {
        if (levelSelectorOn == false)
        {
            levelSelectorOn = true;
            Button[] other_buttons = (Button[])(GameObject.FindObjectsOfType(typeof(Button)));
            foreach (Button button in other_buttons)
            {
                button.onClick.AddListener(PlayValidation);
                EventTrigger.Entry entry = new EventTrigger.Entry();
                EventTrigger.Entry entrymouse = new EventTrigger.Entry();
                EventTrigger.Entry entrySubmit = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.Select;
                entrymouse.eventID = EventTriggerType.PointerEnter;
                entrySubmit.eventID = EventTriggerType.Submit;
                entry.callback.AddListener((eventData) => { PlaySelecting(); });
                entrymouse.callback.AddListener((eventData) => { PlaySelecting(); });
                entrySubmit.callback.AddListener((eventData) => { PlayValidation(); });
                EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
                if (trigger != null)
                {
                    trigger.triggers.Add(entry);
                    trigger.triggers.Add(entrymouse);
                    trigger.triggers.Add(entrySubmit);
                }
            }
        }
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
       PlayUnlocking();
   }

    void PlayLogMenu()
    {
        mainSource.Stop();
        bgmModificator = 1.0f;
        mainSource.volume = bgmModificator * bgmVolume;
        mainSource.loop = true;
        mainSource.clip = logMusic;
        mainSource.Play();
        currenlyStratumMusic = 0;
    }

    void PlayStratum()
    {
		mainSource.Stop ();
		mainSource.clip = stratumsMusic [stratum - 1];
		bgmModificator = stratumsBgmModificators[stratum - 1];
		mainSource.volume = bgmModificator * bgmVolume;
		mainSource.loop = true;
		mainSource.Play ();
		currenlyStratumMusic = stratum;
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

    public void PlayHitWall()
    {
        effectModificator = 1.0f;
        noisesSource.volume = effectsVolume;
        noisesSource.PlayOneShot(wallhit);
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

	//used to set the volume sliders properly in the in-game menus
	public float getBGMVolume() {
		return bgmVolume;
	}

	public float getEffectsVolume() {
		return effectsVolume;
	}
}
