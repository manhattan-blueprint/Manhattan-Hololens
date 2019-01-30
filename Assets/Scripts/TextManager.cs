/*
Manages the spawning, destruction and timing of text
*/
using UnityEngine;

public class TextManager : MonoBehaviour {
    public TextMesh infoText;

    private string[] infoTexts = {"Hello Mr Blueprint!", "Tap to collect", "Follow the pointer", ""};
    private int infoTextCounter = 0;

    public void Start() {

    }

    public void Update() {
        
    }

    public void Reset() {
        Debug.Log("Holointeractive object data reset");
        infoText.transform.position = new Vector3(0.0f, 0.3f, 3.5f);
        infoText.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        infoTextCounter = 0;
        infoText.text = infoTexts[infoTextCounter];
        Invoke("UpdateInfoText", 3);
        Invoke("UpdateInfoText", 6);
        Invoke("UpdateInfoText", 9);
    }

    public void SetText(string inpText) {
        infoText.text = inpText;
    }

    public void Next()
    {
        infoTextCounter++;
        infoText.text = infoTexts[infoTextCounter];
    }

    public void Blank()
    {
        infoText.text = "";
    }
}