using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poof_Script : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    // Start is called before the first frame update
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Destroy(gameObject, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
