using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelControl : MonoBehaviour
{
    public List<GameObject> SummonerList;
    public List<GameObject> MinionList;
    public GameObject MinionPrefab;
    public GameObject BossPrespawn;
    public SceneControl SceneControl;
    public PlayerHit PlayerHit;
    public VictoryDefeat Victory;
    public VictoryDefeat Defeat;
    public Animator Warning;
    public GameObject VictoryMenu;
    public GameObject DefeatMenu;
    public GameObject PauseMenu;
    public GameObject PauseButton;
    public Text ProgressText;
    public Image ProgressFill;
    public AudioClip BossStageClip;
    public AudioClip DefaultClip;
    public int TotalSummonCount;

    private ToggleAR ToggleAR;
    private Animator DoorLeft;
    private Animator DoorRight;
    private int AliveCount;
    private bool PlayerWin;
    private bool PlayerLose;
    private int MinionKilledCount;
    private bool EnterBossStage = false;
    private AudioSource AudioSourceLevelControl;
    private AudioSource AudioSourceCameraAR;

    public void Start()
    {
        AudioSourceLevelControl = GetComponent<AudioSource>();
        AudioSourceCameraAR = Camera.main.GetComponent<AudioSource>();
        ToggleAR = GameObject.FindWithTag("Toggle").GetComponent<ToggleAR>();
        DoorLeft = GameObject.FindWithTag("DoorLeft").GetComponent<Animator>();
        DoorRight = GameObject.FindWithTag("DoorRight").GetComponent<Animator>();
    }

    public void Update()
    {
        if (BossPrespawn.GetComponent<Boss>().IsDied && !PlayerWin && !PlayerLose)
        {
            AudioSourceLevelControl.PlayOneShot(AudioSourceLevelControl.clip);
            AudioSourceCameraAR.clip = DefaultClip;
            AudioSourceCameraAR.Play();
            StartCoroutine(RequestWinScene());
        }

        if (PlayerHit.HealthNow <= 0 && !PlayerLose && !PlayerWin)
        {
            StartCoroutine(RequestLoseScene());
        }

        if (!PlayerLose && !PlayerWin)
        {
            AliveCount = 0;

            foreach (GameObject Minion in MinionList)
            {
                if (Minion.activeInHierarchy)
                {
                    AliveCount++;
                }
            }

            if (AliveCount <= 0 && TotalSummonCount > MinionList.Count)
            {
                float IntervalLeft = Mathf.Infinity;

                foreach (GameObject Summoner in SummonerList)
                {
                    IntervalLeft = Mathf.Min(IntervalLeft, Summoner.GetComponent<Summoner>().GetIntervalLeft());
                }

                foreach (GameObject Summoner in SummonerList)
                {
                    Summoner.GetComponent<Summoner>().SpawnFaster(IntervalLeft);
                }
            }
            else if (AliveCount <= 0 && TotalSummonCount >= MinionList.Count && !EnterBossStage)
            {
                EnterBossStage = true;
                StartCoroutine(BossStage());
            }
        }
    }

    public IEnumerator BossStage()
    {
        AudioSourceCameraAR.clip = BossStageClip;
        AudioSourceCameraAR.Play();
        DoorLeft.SetTrigger("Close");
        DoorRight.SetTrigger("Close");
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(DoorLeft.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        ProgressFill.transform.parent.gameObject.SetActive(false);
        Warning.Play("Animation");
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(Warning.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        DoorLeft.SetTrigger("Open");
        DoorRight.SetTrigger("Open");
        BossPrespawn.SetActive(true);
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(DoorLeft.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        ToggleAR.FullMode(true);
    }

    public void MinionKilled()
    {
        MinionKilledCount++;
        AudioSourceLevelControl.PlayOneShot(AudioSourceLevelControl.clip);
        ProgressText.text = MinionKilledCount.ToString();
        ProgressFill.fillAmount = (float)MinionKilledCount / TotalSummonCount;
    }

    public void PauseGame()
    {
        Time.timeScale = 0.005f;
        Time.fixedDeltaTime = 0.005f * 0.02f;
        PauseButton.SetActive(false);
        PauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = 1 * 0.02f;
        PauseButton.SetActive(true);
        PauseMenu.SetActive(false);
    }

    public IEnumerator RequestWinScene()
    {
        PlayerWin = true;
        Victory.gameObject.SetActive(true);
        Victory.PlayAnimation();
        foreach (GameObject Summoner in SummonerList)
        {
            if (Summoner.activeInHierarchy)
            {
                Summoner.SetActive(false);
            }
        }
        foreach (GameObject Minion in MinionList)
        {
            if (Minion.activeInHierarchy)
            {
                Minion.SetActive(false);
            }
        }
        BossPrespawn.SetActive(false);
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(Victory.Animator.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        ToggleAR.FullMode(false);
        DoorLeft.SetTrigger("Close");
        DoorRight.SetTrigger("Close");
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(DoorLeft.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        VictoryMenu.SetActive(true);
    }

    public IEnumerator RequestLoseScene()
    {
        PlayerLose = true;
        Defeat.gameObject.SetActive(true);
        Defeat.PlayAnimation();
        foreach (GameObject Summoner in SummonerList)
        {
            if (Summoner.activeInHierarchy)
            {
                Summoner.SetActive(false);
            }
        }
        foreach (GameObject Minion in MinionList)
        {
            if (Minion.activeInHierarchy)
            {
                Minion.SetActive(false);
            }
        }
        BossPrespawn.SetActive(false);
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(Defeat.Animator.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        ToggleAR.FullMode(false);
        DoorLeft.SetTrigger("Close");
        DoorRight.SetTrigger("Close");
        yield return new WaitForSeconds(0.001f);
        yield return new WaitForSeconds(DoorLeft.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        DefeatMenu.SetActive(true);
    }
}
