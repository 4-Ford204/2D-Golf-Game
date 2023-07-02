using UnityEngine;

public class MainCamera : MonoBehaviour
{
    #region Fields

    [SerializeField]
    private GameObject prefabsWind;
    [SerializeField]
    private GameObject prefabsHole;
    private float elapseTime = 0.0F;
    private float spawnTime = 2.0F;
    private Vector2 holePosition;
    private Vector2[] holePositionArray = {
        new Vector2(-5.75F, -2.85F),
        new Vector2(2.75F, 1.15F),
        new Vector2(10.75F, -1.85F),
        new Vector2(9F, 4.15F),
        new Vector2(-8.5F, 4.15F)
    };

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        holePosition = holePositionArray[0];
        Instantiate(prefabsHole, holePosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        GenerateWind();
    }

    public void GenerateStage()
    {
        GameObject.Find("BallZ").transform.position = GameObject.Find("Start").transform.position;
        holePosition = holePositionArray[0];
        Instantiate(prefabsHole, holePosition, Quaternion.identity);
    }

    private void GenerateWind()
    {
        elapseTime += Time.deltaTime;

        if (elapseTime >= spawnTime)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(
                Random.Range(1, 3) < 1.5 ? 0 : Screen.width,
                Random.Range(Screen.height / 3, Screen.height - 1), 20));
            Instantiate(prefabsWind, position, Quaternion.identity);
            elapseTime = 0;
        }
    }

    public void GenerateHole()
    {
        Vector2 newHolePosition = holePositionArray[Random.Range(1, holePositionArray.Length)];

        while (newHolePosition == holePosition) newHolePosition = holePositionArray[Random.Range(1, holePositionArray.Length)];

        holePosition = newHolePosition;
        Instantiate(prefabsHole, holePosition, Quaternion.identity);
    }

    #endregion

}
