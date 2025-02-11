using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GlobalManager : MonoBehaviour
{
  public GameObject pauseMenu;
  // public GameObject loseMenu;

  public GameObject winMenu;
  public GameObject gui;

  public GameObject nextLevelBtn;

  public GameObject fallBackButton;

  //用于储存所有的二级协程，便于调用
  // private Dictionary<GameObject, Coroutine> coroutines = new Dictionary<GameObject, Coroutine>();

  public float fadeSpeed = 0.1f;
  // public GameObject monsterPrefab;

  public GameObject JoyStickToggle;
  private static GlobalManager _instance;
  public static GlobalManager Instance
  {
    get { return _instance; }
  }
  private bool paused = false;

  private float currentTimeScale;

  private string token;

  public static int totalLevel;
  private bool isJoyStick;
  public bool getIsJoyStick()
  {
    return isJoyStick;
  }

  // public Dictionary<GameObject, Coroutine> getCoroutines() { return coroutines; }

  public void updateScore(int score)
  {
    GameObject.FindGameObjectWithTag("Score").GetComponent<Text>().text = "Score:" + score.ToString();
  }

  public bool getPaused()
  {
    return paused;
  }
  void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
    }
  }
  void Start()
  {
    // token = PlayerPrefs.GetString("token");
    // Debug.Log(token);
    Transform joyStick = gui.transform.Find("JoyStick");
    int haveJoyStick = PlayerPrefs.GetInt("joyStickToggle");
    //挂载函数
    pauseMenu.transform.Find("JoyStickToggle").GetComponent<Toggle>().onValueChanged.AddListener(OnTogglejoyStick);
    joyStick.Find("left").GetComponent<Button>().onClick.AddListener(Snake.Instance.onClickLeft);
    joyStick.Find("right").GetComponent<Button>().onClick.AddListener(Snake.Instance.onClickRight);
    joyStick.Find("up").GetComponent<Button>().onClick.AddListener(Snake.Instance.onClickUp);
    joyStick.Find("down").GetComponent<Button>().onClick.AddListener(Snake.Instance.onClickDown);
    fallBackButton.GetComponent<Button>().onClick.AddListener(Snake.Instance.FallBack);
    if (haveJoyStick == 0)
    {
      isJoyStick = false;
      JoyStickToggle.GetComponent<Toggle>().isOn = false;
      gui.transform.Find("JoyStick").gameObject.SetActive(false);
    }
    else
    {
      isJoyStick = true;
      JoyStickToggle.GetComponent<Toggle>().isOn = true;
      gui.transform.Find("JoyStick").gameObject.SetActive(true);
    }
    pauseMenu.SetActive(false);
    // loseMenu.SetActive(false);
    // InvokeRepeating("instantiateMonster", 1.0f, 90.0f);
  }
  void OnTogglejoyStick(bool isOn)
  {
    Debug.Log(isOn);
    PlayerPrefs.SetInt("joyStickToggle", Convert.ToInt32(isOn));
    PlayerPrefs.Save();
    gui.transform.Find("JoyStick").gameObject.SetActive(isOn);
    isJoyStick = isOn;
  }
  // public IEnumerator monsterEnterCanBeEatenStatus()
  // {
  //   StopAllCoroutines();
  //   coroutines.Clear();
  //   GameObject[] monsters = GameObject.FindGameObjectsWithTag("monster");
  //   foreach (GameObject monster in monsters)
  //   {
  //     monster.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePosition;
  //     monster.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.4f);
  //     monster.GetComponent<EnemyMove>().canBeEaten = true;
  //   }
  //   yield return new WaitForSecondsRealtime(4.0f);
  //   //考虑这四秒被吃掉的，那么就从那里面挑选剩下的
  //   monsters = GameObject.FindGameObjectsWithTag("monster");
  //   ArrayList monsterArray = new ArrayList();
  //   foreach (GameObject monster in monsters)
  //   {
  //     if (monster.GetComponent<EnemyMove>().canBeEaten)
  //     {
  //       monsterArray.Add(monster);
  //     }
  //   }
  //   monsters = (GameObject[])monsterArray.ToArray(typeof(GameObject));
  //   foreach (GameObject monster in monsters)
  //   {
  //     var coroutine = StartCoroutine(enterResumeStatus(monster));
  //     coroutines.Add(monster, coroutine);
  //   }
  // }

  // IEnumerator postScore(int score)
  // {
  //   var request = new UnityWebRequest("https://snake-api.nspyf.top/auth/game/record", "POST");
  //   string body = @"{""score"":""" + score.ToString() + @"""}";
  //   Debug.Log(body);
  //   byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
  //   request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
  //   request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
  //   request.SetRequestHeader("Content-Type", "application/json");
  //   request.SetRequestHeader("token", token);
  //   yield return request.SendWebRequest();
  //   if (request.result == UnityWebRequest.Result.ConnectionError)
  //   {
  //     Debug.Log("Error while sending: " + request.error);
  //   }
  //   Debug.Log("Received: " + request.downloadHandler.text);
  // }

  // IEnumerator enterResumeStatus(GameObject monster)
  // {
  //   var sprite = monster.GetComponent<SpriteRenderer>();
  //   int times = 5;
  //   while (--times > 0)
  //   {
  //     while (sprite.color.a < 1)
  //     {
  //       sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + fadeSpeed);
  //       yield return new WaitForSecondsRealtime(0.05f);
  //       if (monster == null) yield break;
  //     }
  //     while (sprite.color.a > 0.4f)
  //     {
  //       sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a - fadeSpeed);
  //       yield return new WaitForSecondsRealtime(0.05f);
  //       if (monster == null) yield break;
  //     }
  //   }
  //   if (monster == null) yield break;
  //   while (sprite.color.a < 1)
  //   {
  //     sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, sprite.color.a + fadeSpeed);
  //     yield return new WaitForSecondsRealtime(0.05f);
  //     if (monster == null) yield break;
  //   }
  //   if (monster == null) yield break;
  //   monster.GetComponent<EnemyMove>().canBeEaten = false;
  //   monster.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
  // }

  public void backToMenu()
  {
    SceneManager.LoadScene("Menu");
  }
  // public void stopResume(Coroutine coroutine)
  // {
  //   StopCoroutine(coroutine);
  // }

  // void instantiateMonster()
  // {
  //   Instantiate(monsterPrefab, new Vector2(-38, 20), Quaternion.identity);
  // }

  // public void dead(int score)
  // {
  //   gui.SetActive(false);
  //   StartCoroutine(postScore(score));
  //   RankResponse response = new RankResponse();
  //   StartCoroutine(RankManager.Instance.getRank(response, score));
  // }
  public void pause()
  {
    gui.SetActive(false);
    pauseMenu.SetActive(true);
    // currentTimeScale = Time.timeScale;
    paused = true;
    // Time.timeScale = 0;
  }
  public void resume()
  {
    pauseMenu.SetActive(false);
    gui.SetActive(true);
    paused = false;
    // Time.timeScale = currentTimeScale;
  }
  public void restart()
  {
    SceneManager.UnloadSceneAsync("MainScene");
    SceneManager.LoadScene("MainScene");
    // Time.timeScale = 1;
  }
  public void win()
  {
    winMenu.SetActive(true);
  }
  // Update is called once per frame
  void Update()
  {
    if (Convert.ToInt32(SpawnMap.level)>=totalLevel)
    {
      nextLevelBtn.SetActive(false);
    }
    if (GameObject.FindWithTag("food") == null)
    {
      GlobalManager.Instance.win();
    }
    if (!Snake.Instance.CanFallBack)
    {
      fallBackButton.GetComponent<Button>().interactable = false;
    }
    else
    {
      fallBackButton.GetComponent<Button>().interactable = true;
    }
  }
}
