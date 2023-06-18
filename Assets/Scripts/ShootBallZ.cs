using UnityEngine;
using UnityEngine.SceneManagement;

public class ShootBallZ : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject ballPrediction;
    [SerializeField]
    GameObject prefabsGoalFX;
    Rigidbody2D rb2d;
    LineRenderer lr;
    private Vector2 startPosition;
    private Vector2 endPosition;
    private float force = 50F;
    private Scene mainScene;
    private PhysicsScene2D mainPhysicsScene;
    private Scene predictionScene;
    private PhysicsScene2D predictionPhysicsScene;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        lr = gameObject.GetComponent<LineRenderer>();
        Physics2D.simulationMode = SimulationMode2D.Script;
        mainScene = SceneManager.CreateScene("MainScene");
        mainPhysicsScene = mainScene.GetPhysicsScene2D();
        CreateSceneParameters sceneParameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("PredictionScene", sceneParameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) DragStart();

        if (Input.GetMouseButton(0)) DragChange();

        if (Input.GetMouseButtonUp(0)) DragRelease();
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
        prediction.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Force);
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
        lr.positionCount = 0;
        endPosition = GetMousePosition();
        Vector2 direction = startPosition - endPosition;
        rb2d.AddForce(direction * force, ForceMode2D.Force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            Instantiate(prefabsGoalFX, collision.gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    #endregion

}
