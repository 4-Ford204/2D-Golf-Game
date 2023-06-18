using UnityEngine;

public class Obstacle : MonoBehaviour
{
    #region Fields

    [SerializeField]
    GameObject prefabs3Stones;
    [SerializeField]
    GameObject prefabs3StonesCracked2;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(prefabs3Stones, new Vector2(4, 0), Quaternion.Euler(0, 0, 90));
        Instantiate(prefabs3StonesCracked2, new Vector2(0, 0), Quaternion.Euler(0, 0, 90));
    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion

}
