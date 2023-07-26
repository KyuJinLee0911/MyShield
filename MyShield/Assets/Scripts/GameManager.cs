using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    private GameObject boxPrefab;

    Queue<Square> poolingObjectQueue = new Queue<Square>();

    public Text timeText;
    public Text scoreText;
    public Text bestScoreText;
    float alive = 0.0f;
    public GameObject endPanel;
    bool isAlive = true;
    public Animator anim;

    private void Awake()
    {
        if (!instance)
            instance = this;

        if (instance != this)
            Destroy(gameObject);

        Initialize(10);
    }

    private void Start()
    {
        Time.timeScale = 1.0f;
        InvokeRepeating("makeSquare", 0.0f, 0.5f);
    }

    private void Update()
    {
        if (isAlive)
        {
            alive += Time.deltaTime;
            timeText.text = alive.ToString("N2");
        }
    }

    public void GameOver()
    {
        anim.SetBool("isDie", true);
        isAlive = false;
        
        Invoke("TimeStop", 0.5f);

        scoreText.text = alive.ToString("N2");
        endPanel.SetActive(true);

        if(!PlayerPrefs.HasKey("Best"))
        {
            PlayerPrefs.SetFloat("Best", alive);
        }
        else
        {
            if(PlayerPrefs.GetFloat("Best") < alive)
                PlayerPrefs.SetFloat("Best", alive);
        }

        bestScoreText.text = PlayerPrefs.GetFloat("Best").ToString("N2");
    }

    void TimeStop()
    {
        Time.timeScale = 0.0f;
    }

    public void Retry()
    {
        SceneManager.LoadScene("MainScene");
    }

    void makeSquare()
    {
        GetObject();
    }

    private void Initialize(int initCount)
    {
        for (int i = 0; i < initCount; i++)
        {
            poolingObjectQueue.Enqueue(CreateNewObject());
        }
    }

    private Square CreateNewObject()
    {
        var newObj = Instantiate(boxPrefab).GetComponent<Square>();
        newObj.gameObject.SetActive(false);
        newObj.transform.SetParent(transform);
        return newObj;
    }

    public static Square GetObject()
    {
        if (instance.poolingObjectQueue.Count > 0)
        {
            var obj = instance.poolingObjectQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);

            var rb = obj.GetComponent<Rigidbody2D>();

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            rb.rotation = 0.0f;

            float x = Random.Range(-3.0f, 3.0f);
            float y = Random.Range(3.0f, 5.0f);
            obj.transform.position = new Vector3(x, y, 0);

            float size = Random.Range(0.5f, 1.5f);
            obj.transform.localScale = new Vector3(size, size, 1);

            return obj;
        }
        else
        {
            var newObj = instance.CreateNewObject();
            newObj.gameObject.SetActive(true);
            newObj.transform.SetParent(null);

            var rb = newObj.GetComponent<Rigidbody2D>();

            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0.0f;
            rb.rotation = 0.0f;

            float x = Random.Range(-3.0f, 3.0f);
            float y = Random.Range(3.0f, 5.0f);
            newObj.transform.position = new Vector3(x, y, 0);

            float size = Random.Range(0.5f, 1.5f);
            newObj.transform.localScale = new Vector3(size, size, 1);

            return newObj;
        }
    }

    public static void ReturnObject(Square obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(instance.transform);
        instance.poolingObjectQueue.Enqueue(obj);
    }

}
