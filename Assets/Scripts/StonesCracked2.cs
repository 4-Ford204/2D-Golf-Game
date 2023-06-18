using UnityEngine;

public class StonesCracked2 : MonoBehaviour
{
    #region Fields

    Rigidbody2D rb2d;
    Vector2 position;
    int changeDirection = 0;
    float speed = 15F;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        position = transform.position;
        if (transform.position.y > 7F || transform.position.y < -7F) changeDirection = (changeDirection + 1) % 2;
        if (changeDirection % 2 == 0) position.y += speed * Time.deltaTime;
        else position.y -= speed * Time.deltaTime;
        transform.position = position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            Destroy(gameObject);
        }
    }

    #endregion

}
