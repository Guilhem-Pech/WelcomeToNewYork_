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

        TextUpdate();
    }

    public void StartPrinting()
    {
        StartCoroutine("TextUpdateRoutine");
    }

    private string AddPoints(string s,int n)
    {
        string sToModify = s + " ";

        for (int i = 0; i < n; i++)
            sToModify += ".";

        return sToModify;
    }

    private void TextUpdate()
    {
        string toPrint;
        if (!doneLoading)
            toPrint = loadingText;
        else
            toPrint = waitingForPlayersText;

        if (it == 3)
            it = 1;
        else
            it++;

        gameObject.GetComponent<Text>().text = AddPoints(toPrint, it);
    }

    IEnumerator TextUpdateRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            TextUpdate();
        }
    }
}
