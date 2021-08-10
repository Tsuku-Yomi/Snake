using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using static System.Math;
using UnityEngine;

public class Snake : MonoBehaviour
{
  Vector2 dir = Vector2.right;
  List<Transform> tail = new List<Transform>();

  private bool ate = false, dead = false, moved = false;

  private int score = 0;
  private float runningTime = 1;

  public AudioClip eatClip;

  public double boostScale = 1.1f;

  public GameObject tailPrefab;
  // Start is called before the first frame update
  void Start()
  {
    for (int i = 1; i < 5; i++)
    {
      GameObject g = (GameObject)Instantiate(tailPrefab, new Vector2(this.transform.position.x - i, this.transform.position.y), Quaternion.identity);

      tail.Add(g.transform);
    }
    InvokeRepeating("Move", 0.3f, 0.3f);
  }

  // Update is called once per frame
  void Update()
  {
    if (!dead && !GlobalManager.Instance.getPaused())
    {
      //一次输入后，在移动之前，使输入无效化，防止出现同时按下两个键，在移动判断之前导致dir变换两个方向，导致自己撞自己的bug产生而死亡
      if (!moved)
      {
        if (Input.GetKey(KeyCode.RightArrow) && (dir == Vector2.up || dir == -Vector2.up))
          dir = Vector2.right;
        else if (Input.GetKey(KeyCode.DownArrow) && (dir == Vector2.right || dir == -Vector2.right))
          dir = -Vector2.up;
        else if (Input.GetKey(KeyCode.LeftArrow) && (dir == Vector2.up || dir == -Vector2.up))
          dir = -Vector2.right;
        else if (Input.GetKey(KeyCode.UpArrow) && (dir == Vector2.right || dir == -Vector2.right))
          dir = Vector2.up;
        moved = true;
      }
      runningTime += Time.deltaTime;
      Time.timeScale = (float)Pow(boostScale, Log(runningTime));
    }
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.name.StartsWith("FoodPrefab") || other.name.StartsWith("RewardPrefab"))
    {
      ate = true;

      Destroy(other.gameObject);
      if (other.name.StartsWith("RewardPrefab"))
      {
        score += 50;
        Time.timeScale -= 0.9f;
      }
      else
      {
        score += 10;
      }
      GlobalManager.Instance.updateScore(score);
    }
  }
  void Move()
  {
    if (dead)
    {
      return;
    }
    if (isDead(dir) && !dead)
    {
      dead = true;
      GlobalManager.Instance.dead();
      return;
    }
    Vector2 v = transform.position;

    transform.Translate(dir);

    if (ate)
    {
      AudioSource.PlayClipAtPoint(eatClip, new Vector3(0, 0, -10));
      GameObject g = (GameObject)Instantiate(tailPrefab, v, Quaternion.identity);

      tail.Insert(0, g.transform);

      ate = false;
    }
    else if (tail.Count > 0)
    {
      tail.Last().position = v;

      tail.Insert(0, tail.Last());
      tail.RemoveAt(tail.Count - 1);
    }
    moved = false;
  }

  private bool isDead(Vector2 dir)
  {
    Vector2 pos = transform.position;
    //从pos+dir向pos发射一条射线
    RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
    if (hit.collider.name.StartsWith("TailPrefab") || hit.collider.name.StartsWith("MonsterPrefab")) return true;
    if (hit.collider.transform.parent == null) return false;
    return hit.collider.transform.parent.name == "Wall";
  }
}
