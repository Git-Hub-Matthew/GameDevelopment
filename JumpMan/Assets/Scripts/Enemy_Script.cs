using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    [SerializeField] private float _xSpeed;

    private Rigidbody2D _body;

    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        _body.linearVelocity = new Vector2(_xSpeed, 0);
    }
}
