using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalFX : MonoBehaviour
{
    #region Fields

    ParticleSystem ps;
    float aliveTime;
    float destroyTime = 4F;

    #endregion

    #region Methods

    // Start is called before the first frame update
    void Start()
    {
        ps = gameObject.GetComponent<ParticleSystem>();
        aliveTime = 0F;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime >= destroyTime) Destroy(gameObject);
    }

    #endregion

}
