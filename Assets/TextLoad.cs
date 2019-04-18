using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextLoad : MonoBehaviour
{
    public string loadingText;
    public string waitingForPlayersText;

    private bool doneLoading;
    private int it;
    // Start is called before the first frame update
    void Start()
    {
        doneLoading = false;

        it = 3;
        gameObject.GetComponent<Text>().text = addPoints(loadingText, it);
    }

    // Update is called once per frame
    void Update()
    {
        if (!doneLoading)
        {
            if (it == 3)
                it = 1;
            else
                it++;

            gameObject.GetComponent<Text>().text = addPoints(loadingText, it);
        }
    }

    private string addPoints(string s,int n)
    {
        string sToModify = s + " ";

        for (int i = 0; i > n; i++)
            sToModify += ".";

        return sToModify;
    }
}
