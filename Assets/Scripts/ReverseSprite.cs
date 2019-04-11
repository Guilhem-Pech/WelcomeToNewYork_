using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseSprite : MonoBehaviour
{
    public SpriteRenderer spriteParent;

    private SpriteRenderer thisSprite;

    // Start is called before the first frame update
    void Start()
    {
        thisSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        thisSprite.sprite = spriteParent.sprite;
        thisSprite.flipX = spriteParent.flipX;
    }
}
