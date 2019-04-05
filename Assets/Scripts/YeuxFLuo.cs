using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Gestion des yeux pou qu'ils soient toujours colorés dans le noir
public class YeuxFLuo : MonoBehaviour
{
    private SpriteRenderer spriteParent;

    private SpriteRenderer thisSprite;

    private Sprite[] sheetSprite;

    public string sheetName;

    // Start is called before the first frame update
    void Start()
    {
        thisSprite = GetComponent<SpriteRenderer>();
        spriteParent = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        sheetSprite = Resources.LoadAll<Sprite>(sheetName);
    }

    // Update is called once per frame
    void Update()
    {
        switch (spriteParent.sprite.name)
        {
            case "SmallEnnemy_Walk1": thisSprite.sprite = sheetSprite[0]; break;
            case "SmallEnnemy_Walk2": thisSprite.sprite = sheetSprite[1]; break;
            case "SmallEnnemy_Walk3": thisSprite.sprite = sheetSprite[2]; break;
            case "SmallEnnemy_Walk4": thisSprite.sprite = sheetSprite[3]; break;
            case "SmallEnnemy_Walk5": thisSprite.sprite = sheetSprite[4]; break;
            case "SmallEnnemy_Walk6": thisSprite.sprite = sheetSprite[5]; break;
                
            case "SmallEnnemy_Chase1": thisSprite.sprite = sheetSprite[6]; break;
            case "SmallEnnemy_Chase2": thisSprite.sprite = sheetSprite[7]; break;
            case "SmallEnnemy_Chase3": thisSprite.sprite = sheetSprite[8]; break;
            case "SmallEnnemy_Chase4": thisSprite.sprite = sheetSprite[9]; break;
            case "SmallEnnemy_Chase5": thisSprite.sprite = sheetSprite[10]; break;
            case "SmallEnnemy_Chase6": thisSprite.sprite = sheetSprite[11]; break;

            case "SmallEnnemy_Attack1": thisSprite.sprite = sheetSprite[12]; break;
            case "SmallEnnemy_Attack2": thisSprite.sprite = sheetSprite[13]; break;
            case "SmallEnnemy_Attack3": thisSprite.sprite = sheetSprite[14]; break;
            case "SmallEnnemy_Attack4": thisSprite.sprite = sheetSprite[15]; break;

            case "SmallEnnemy_Stun": thisSprite.sprite = sheetSprite[16]; break;

            case "RangedEnnemy_Walk1": thisSprite.sprite = sheetSprite[0]; break;
            case "RangedEnnemy_Walk2": thisSprite.sprite = sheetSprite[1]; break;
            case "RangedEnnemy_Walk3": thisSprite.sprite = sheetSprite[2]; break;
            case "RangedEnnemy_Walk4": thisSprite.sprite = sheetSprite[3]; break;
            case "RangedEnnemy_Walk5": thisSprite.sprite = sheetSprite[4]; break;
            case "RangedEnnemy_Walk6": thisSprite.sprite = sheetSprite[5]; break;

            case "RangedEnnemy_Attack1": thisSprite.sprite = sheetSprite[6]; break;
            case "RangedEnnemy_Attack2": thisSprite.sprite = sheetSprite[7]; break;
            case "RangedEnnemy_Attack3": thisSprite.sprite = sheetSprite[8]; break;
            case "RangedEnnemy_Attack4": thisSprite.sprite = sheetSprite[9]; break;
            case "RangedEnnemy_Attack5": thisSprite.sprite = sheetSprite[10]; break;
            case "RangedEnnemy_Attack6": thisSprite.sprite = sheetSprite[11]; break;
            case "RangedEnnemy_Attack7": thisSprite.sprite = sheetSprite[12]; break;
            case "RangedEnnemy_Attack8": thisSprite.sprite = sheetSprite[13]; break;

            case "RangedEnnemy_Stun": thisSprite.sprite = sheetSprite[14]; break;
         
            default: thisSprite.sprite = null; break;
        }
    }
}
