using UnityEngine;
using UnityEngine.UI;

public class ShadowCaster : MonoBehaviour
{
    private GameObject buba;
    void Start()
    {
        buba = GameObject.Find("buba");
        MakeTransparent();
        transform.name = transform.name.Replace(name, "shadow");
        GoDown(Vector3.down);
    }
    private void GoDown(Vector3 direction)
    {
        transform.position += direction;
        foreach (Transform block in transform)
        {
            if (Mathf.Round(block.position.y * 2) / 2 < -9.5f)
            {
                transform.position -= direction;
                return;
            }
            foreach (Transform bubablock in buba.transform)
            {
                if (Mathf.Round(block.position.x * 2) / 2 == Mathf.Round(bubablock.position.x * 2) / 2 && Mathf.Round(block.position.y * 2) / 2 == Mathf.Round(bubablock.position.y * 2) / 2)
                {
                    transform.position -= direction;
                    return;
                }
            }
        }
        GoDown(Vector3.down);
        return;
    }

    private void MakeTransparent()
    {
        SpriteRenderer sprite = new SpriteRenderer();
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        foreach (var child in children)
        {
            sprite = child.GetComponent<SpriteRenderer>();
            if (sprite != null)
            {
                Color childColor = sprite.color;
                childColor.a = 0.2f;
                sprite.color = childColor;
            }
        }
    }
}