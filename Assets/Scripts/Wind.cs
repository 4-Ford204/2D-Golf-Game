using UnityEngine;

public class Wind : MonoBehaviour
{
    #region Fields

    [SerializeField]
    Sprite wind;
    Rigidbody2D rb2d;
    float minSpeed = 1F;
    float maxSpeed = 5F;
    Vector3 width;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        if (gameObject.transform.position.x <= 0)
        {
            rb2d.velocity = new Vector2(Random.Range(minSpeed, maxSpeed), 0);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
            rb2d.velocity = new Vector2(-Random.Range(minSpeed, maxSpeed), 0);
        }
        width = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.transform.position.x < -width.x || gameObject.transform.position.x > width.x)
        {
            Destroy(gameObject);
        }
    }


    #endregion
}
