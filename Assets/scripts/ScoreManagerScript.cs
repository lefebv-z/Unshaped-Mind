using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreManagerScript : MonoBehaviour
{

    private int topScore;// best score of this level
    private int score;// final score
    private float scoreCal;// score being calculated
    private string keyScoreName;
    private string keyTimeName;
    private string keyTransfoName;

    private int beatTime;
    private int beatTransfo;

    public GameObject winStar;
    public GameObject timeStar;
    public GameObject transfoStar;

    // Use this for initialization
    void Start()
    {
        keyScoreName = "TopScore";
        keyTimeName = "TopTimeScore";
        keyTransfoName = "TopTransfoScore";
        topScore = PlayerPrefs.GetInt(keyScoreName + 1 + "-" + 1);
        beatTime = PlayerPrefs.GetInt(keyTimeName + 1 + "-" + 1);
        beatTransfo = PlayerPrefs.GetInt(keyTransfoName + 1 + "-" + 1);
    }

    void getLevelTopScore(int strate, int lvl)
    {
        if (PlayerPrefs.HasKey(keyScoreName + lvl) == false)
        {
            topScore = 0;
            beatTransfo = 0;
            beatTime = 0;
            PlayerPrefs.SetInt(keyScoreName + strate + "-" + lvl, 0);
            PlayerPrefs.SetInt(keyTimeName + strate + "-" + lvl, 0);
            PlayerPrefs.SetInt(keyTransfoName + strate + "-" + lvl, 0);
            Debug.Log("no" + topScore);
        }
        else
        {
            topScore = PlayerPrefs.GetInt(keyScoreName + strate + "-" + lvl);
            beatTime = PlayerPrefs.GetInt(keyTimeName + strate + "-" + lvl);
            beatTransfo = PlayerPrefs.GetInt(keyTransfoName + strate + "-" + lvl);
            Debug.Log(topScore);
        }
    }

    void setLevelTopScore(int strate, int lvl)
    {
        PlayerPrefs.SetInt(keyScoreName + strate + "-" + lvl, topScore);
        PlayerPrefs.SetInt(keyTimeName + strate + "-" + lvl, beatTime);
        PlayerPrefs.SetInt(keyTransfoName + strate + "-" + lvl, beatTransfo);
        PlayerPrefs.Save();
    }

    void updateStar()
    {
        winStar.SetActive(topScore >= 1);
        timeStar.SetActive(beatTime == 1);
        transfoStar.SetActive(beatTransfo == 1);
    }

    /*
    void calculateScore(int level, int maxTransfo, int remainingTransfo, float timeLimit, float timeUsed)
    {
        getLevelTopScore(level);
        if (timeUsed < timeLimit - timediff)
            scoreCal = 2.5f;
        else if (timeUsed > timeLimit + timediff)
            scoreCal = 1.5f;
        else
            scoreCal = 2.0f;

        if (remainingTransfo <= maxTransfo / 3)
            scoreCal -= 0.5f;
        else if (remainingTransfo >= maxTransfo / 2)
            scoreCal += 0.5f;

        score = (int)((scoreCal * 2.0f) - 1.0f);
        if (score > topScore)
        {
            topScore = score;
            setLevelTopScore(level);
        }
    }
    */
    public void starScore()
    {
        NbTransfoLevelSave scoreinfo = (NbTransfoLevelSave)(GameObject.FindObjectOfType(typeof(NbTransfoLevelSave)));
        GameManager transfoinfo = (GameManager)(GameObject.FindObjectOfType(typeof(GameManager)));
        LevelTimerScript timeinfo = (LevelTimerScript)(GameObject.FindObjectOfType(typeof(LevelTimerScript)));
        if (scoreinfo != null && transfoinfo != null && timeinfo != null)
        {
            getLevelTopScore(transfoinfo.getStratum(), transfoinfo.getLevel());
            if (timeinfo.getTime() <= scoreinfo.bestTime)
                beatTime = 1;

            if (transfoinfo.remainingTransformation >= scoreinfo.bestRemainingTransfo)
                beatTransfo = 1;
            score = 1 + beatTime + beatTransfo;
            if (score > topScore)
            {
                topScore = score;
                setLevelTopScore(transfoinfo.getStratum(), transfoinfo.getLevel());
            }
            updateStar();
        }
    }

    public void recapScore(int strate, int lvl)
    {
        Debug.Log("-----"+ strate+ lvl);
        getLevelTopScore(strate, lvl);
        updateStar();
    }
    public void initrecapScore()
    {
        Debug.Log("-----");
        getLevelTopScore(1, 1);
        updateStar();
    }


}
