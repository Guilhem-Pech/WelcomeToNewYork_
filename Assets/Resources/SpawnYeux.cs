using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnYeux : MonoBehaviour
{
    public SpriteRenderer yeux;
    private Sprite[] sheetSprite;

    public string sheetName;
    private int random;


    private bool inFirst = false;
    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log("done");
        sheetSprite = Resources.LoadAll<Sprite>(sheetName);
        random = Random.Range(6, 10);
    }
    
    // Update is called once per frame
    void Update()
    {

        if (Input.GetKey(KeyCode.Y))
        {
            if (yeux.sprite == null)
            {
                StartCoroutine(FinishFirst(Random.Range(0.0f, 3.0f)));
                StartCoroutine(DoLast());
            }
        }

        if (Input.GetKey(KeyCode.U))
            yeux.sprite = null;
    }

    IEnumerator FinishFirst(float waitTime)
    {
        inFirst = true;
        yield return new WaitForSeconds(waitTime);
        inFirst = false;
    }

    IEnumerator DoLast()
    {

        while (inFirst)
            yield return new WaitForSeconds(0.1f);
        yeux.sprite = sheetSprite[random];

    }
}
