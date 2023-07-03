using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class ShootBallZ : MonoBehaviour
{
    #region Fields

    private MainCamera mainCamera;
    [SerializeField]
    private GameObject gameOverScreen;
    [SerializeField]
    private GameObject ballPrediction;
    [SerializeField]
    private GameObject prefabsGoalFX;
    private GameObject start;
    private Rigidbody2D rb2d;
    private LineRenderer lr;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float minPower = 50F;
    private float maxPower = 800F;
    private Scene mainScene;
    private PhysicsScene2D mainPhysicsScene;
    private Scene predictionScene;
    private PhysicsScene2D predictionPhysicsScene;
    private AudioSource[] audioSources;
    private AudioSource goalSource;
    private AudioSource shootSource;
    [SerializeField]
    private TextMeshProUGUI textScore;
    private int score = 0;
    [SerializeField]
    private TextMeshProUGUI textGameOver;
    [SerializeField]
    private TextMeshProUGUI textTurn;
    private int maxTurn = 10;
    private int turn;
    [SerializeField]
    private TextMeshProUGUI textTime;
    private float maxTime = 60F;
    private float time;
    private bool isWaiting = false;
    private float waitTime = 0F;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<MainCamera>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        lr = gameObject.GetComponent<LineRenderer>();
        start = GameObject.Find("Start");
        Physics2D.simulationMode = SimulationMode2D.Script;
        mainScene = SceneManager.CreateScene("MainScene");
        mainPhysicsScene = mainScene.GetPhysicsScene2D();
        CreateSceneParameters sceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("PredictionScene", sceneParameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();
        audioSources = GameObject.FindWithTag("MainCamera").GetComponents<AudioSource>();
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.clip.name == "Goal")
            {
                goalSource = audioSource;
            }
            else if (audioSource.clip.name == "Shoot")
            {
                shootSource = audioSource;
            }
        }
        textScore.text = "Score: " + score;
        turn = maxTurn;
        textTurn.text = "Turn: " + turn;
        time = maxTime;
        textTime.text = "Time: " + time;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            CountDownTime();

            if (!ReadyShoot()) { return; }

            if ((ReadyShoot() && turn == 0) || time <= 0F) GameOver();

            if (Input.GetMouseButtonDown(0)) DragStart();

            if (Input.GetMouseButton(0)) DragChange();

            if (Input.GetMouseButtonUp(0)) DragRelease();

        }
        else Waiting();
    }

    private bool ReadyShoot()
    {
        return rb2d.velocity.magnitude <= 0F;
    }

    private Vector2 GetMousePosition() => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    private void FixedUpdate()
    {
        if (!mainPhysicsScene.IsValid()) return;
        else
        {
            mainPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    private void DragStart()
    {
        startPosition = GetMousePosition();
    }

    private void DragChange()
    {
        Vector2 dragPosition = GetMousePosition();
        Vector2 direction = startPosition - dragPosition;
        GameObject prediction = GameObject.Instantiate(ballPrediction);
        SceneManager.MoveGameObjectToScene(prediction, predictionScene);
        prediction.transform.position = gameObject.transform.position;
        prediction.GetComponent<Rigidbody2D>().AddForce(Vector2.ClampMagnitude(direction * minPower, maxPower), ForceMode2D.Force);
        lr.positionCount = 50;
        for (int i = 0; i < 50; i++)
        {
            predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
            lr.SetPosition(i, new Vector2(prediction.transform.position.x, prediction.transform.position.y));
        }
        Destroy(prediction);
    }

    private void DragRelease()
    {
        rb2d.velocity = new Vector2(0.01F, 0.01F);
        lr.positionCount = 0;
        endPosition = GetMousePosition();
        Vector2 direction = startPosition - endPosition;
        rb2d.AddForce(Vector2.ClampMagnitude(direction * minPower, maxPower), ForceMode2D.Force);
        shootSource.Play();
        SetTurn(--turn);
    }

    private void SetScore(int score) => textScore.text = "Score: " + score.ToString();

    private void SetTurn(int turn) => textTurn.text = "Turn: " + turn.ToString();

    private void SetTime(int time) => textTime.text = "Time: " + time.ToString();

    private void CountDownTime()
    {
        time -= Time.deltaTime;
        if (time <= Mathf.Floor(time) + 0.01) SetTime((int)time);
    }

    private void Waiting()
    {
        waitTime += Time.deltaTime;

        if (waitTime >= 5F)
        {
            waitTime = 0F;
            isWaiting = false;
            rb2d.isKinematic = false;
            gameOverScreen.SetActive(false);
            Destroy(GameObject.FindWithTag("Hole"));
            score = 0;
            SetScore(score);
            turn = maxTurn;
            SetTurn(turn);
            time = maxTime;
            SetTime((int)maxTime);
            mainCamera.GenerateStage();
        }
    }

    private void GameOver()
    {
        textGameOver.text = "End Game\r\nYour Score: " + score.ToString();
        gameOverScreen.SetActive(true);
        rb2d.velocity = new Vector2(0F, 0F);
        rb2d.isKinematic = true;
        isWaiting = true;
    }

    private void Rain()
    {
        Destroy(GameObject.FindWithTag("Rain"));
        if (Random.Range(0, 5) < 1) mainCamera.GenerateRain();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            Instantiate(prefabsGoalFX, collision.gameObject.transform.position, Quaternion.identity);
            goalSource.Play();
            SetScore(++score);
            Destroy(collision.gameObject);
            mainCamera.GenerateHole();
            rb2d.velocity = new Vector2(0F, 0F);
            gameObject.transform.position = start.transform.position;
            maxPower -= 50F;
            Rain();
        }
        else if (collision.gameObject.tag == "MainCamera")
        {
            GameOver();
        }
    }

    #endregion

}
