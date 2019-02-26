/*
Manages the spawning, destruction and timing of text
*/
using UnityEngine;

public class TextManager : MonoBehaviour {
    private TextMesh infoText;
    private bool inUse;

    public void Start() {
        infoText = GameObject.Find("InfoText").GetComponent(typeof(TextMesh)) as TextMesh;
        inUse = false;
    }

    public void Update() {

    }

    public void RequestText(string text)
    {
        infoText.text = text;
        inUse = true;
    }

    public void RequestText(string text, float duration)
    {
        RequestText(text);
        Invoke("RequestReset", duration);
    }

    public void RequestReset()
    {
        infoText.text = "";
    }

    public void RequestReset(float delay)
    {
        Invoke("RequestReset", delay);
    }
}