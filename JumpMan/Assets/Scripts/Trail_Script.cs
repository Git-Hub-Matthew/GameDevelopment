using UnityEngine;

public class Trail_Script : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        Destroy(gameObject, 0.5f);
    }

    // Flips Sprite depending on Direction Facing
    public void SetDirection(string direction)
    {
        if (direction == "right")
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}
