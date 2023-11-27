using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{

    public float rotationSpeed;

    bool wantsToRotateCCW;
    bool wantsToRotateCW;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wantsToRotateCCW = Input.GetMouseButton(0);
        wantsToRotateCW = Input.GetMouseButton(1);
    }

    void FixedUpdate()
    {
        if (wantsToRotateCCW)
        {
            transform.Rotate(0, 0, rotationSpeed * Time.fixedDeltaTime);
        }
        else if (wantsToRotateCW)
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.fixedDeltaTime);
        }
        
    }

}
