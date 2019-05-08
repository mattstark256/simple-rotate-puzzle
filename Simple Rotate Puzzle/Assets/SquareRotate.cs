using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRotate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Rotate());
        }
    }

    private IEnumerator Rotate()
    {
        float initialAngle = GetComponent<MeshRenderer>().material.GetFloat("_RotateAngle");

        float f = 0;
        while (f < 1)
        {
            f += Time.deltaTime / 0.3f;
            if (f > 1) f = 1;

            GetComponent<MeshRenderer>().material.SetFloat("_RotateAngle", initialAngle + Mathf.SmoothStep(0, 90, f));

            yield return null;
        }
    }
}
