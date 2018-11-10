using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseUI : MonoBehaviour
{
    public Text restartText;
    public Text resumeText;
    public Text quitText;

    public Color defaultColor = Color.white;
    public Color highlightColor = Color.yellow;

    public void SelectRestart()
    {
        restartText.color = highlightColor;
        DeselectResume();
        DeselectQuit();
    }

    public void SelectResume()
    {
        DeselectRestart();
        resumeText.color = highlightColor;
        DeselectQuit();
    }

    public void SelectQuit()
    {
        DeselectRestart();
        DeselectResume();
        quitText.color = highlightColor;
    }

    private void DeselectRestart()
    {
        restartText.color = defaultColor;
    }

    private void DeselectResume()
    {
        resumeText.color = defaultColor;
    }

    private void DeselectQuit()
    {
        quitText.color = defaultColor;
    }
}
