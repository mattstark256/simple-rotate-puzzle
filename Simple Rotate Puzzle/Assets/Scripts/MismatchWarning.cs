using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MismatchWarning : MonoBehaviour
{
    [SerializeField]
    private float animationDuration = 0.2f;

    private bool warningEnabled = false;
    public void SetWarningEnabled(bool enabled) { warningEnabled = enabled; }

    private float size = 1;
    private Vector3 initialScale;


    private void Awake()
    {
        initialScale = transform.localScale;
    }


    void Update()
    {
        if (warningEnabled)
        {
            if (size < 1)
            {
                size += Time.deltaTime / animationDuration;
                size = Mathf.Clamp01(size);
                transform.localScale = Mathf.SmoothStep(0, 1, size) * initialScale;
            }
        }
        else
        {
            if (size > 0)
            {
                size -= Time.deltaTime / animationDuration;
                size = Mathf.Clamp01(size);
                transform.localScale = Mathf.SmoothStep(0, 1, size) * initialScale;
            }
        }
    }
}