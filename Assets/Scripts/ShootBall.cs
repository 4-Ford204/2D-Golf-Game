using UnityEngine;

public class ShootBall : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject prefabsBall;
    [SerializeField]
    GameObject prefabsGoalFX;
    CircleCollider2D cc2d;
    Rigidbody2D rb2d;
    LineRenderer lr;
    float minPower = 10F;
    float maxPower = 40F;
    bool isDragging;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        cc2d = gameObject.GetComponent<CircleCollider2D>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        lr = gameObject.GetComponent<LineRenderer>();
        isDragging = false;
    }

    // Update is called once per frame
    void Update()
    {
        Shoot();
    }

    private void Shoot()
    {
        if (!IsStop()) return;

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float distance = Vector2.Distance(transform.position, mousePosition);

        if (Input.GetMouseButtonDown(0) && distance <= cc2d.radius && !isDragging) Ready();
        if (Input.GetMouseButton(0) && isDragging) Adjust(mousePosition);
        if (Input.GetMouseButtonUp(0) && isDragging) Shoot(mousePosition);
    }

    private bool IsStop()
    {
        return rb2d.velocity.magnitude <= 1F;
    }

    private void Ready()
    {
        isDragging = true;
        lr.positionCount = 2;
    }

    private void Adjust(Vector2 mousePosition)
    {
        Vector2 direction = (Vector2)transform.position - mousePosition;

        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, (Vector2)transform.position + Vector2.ClampMagnitude((direction * minPower) / 2, maxPower / 2));
    }

    private void Shoot(Vector2 mousePosition)
    {
        float distance = Vector2.Distance(transform.position, mousePosition);

        if (distance <= cc2d.radius) return;
        else
        {
            Vector2 direction = (Vector2)transform.position - mousePosition;
            rb2d.velocity = Vector2.ClampMagnitude(direction * minPower, maxPower);
        }

        lr.positionCount = 0;
        isDragging = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            Instantiate(prefabsGoalFX, collision.gameObject.transform.position, Quaternion.identity);
            Instantiate(prefabsBall, new Vector2(-10, 0), Quaternion.identity);
            Destroy(gameObject);
        }
    }

    #endregion

}
