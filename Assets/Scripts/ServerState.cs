/*
Manages all
Attach this script to a GameObject. Create a Text GameObject (Create>UI>Text)
and attach it to the My Text field in the Inspector of your GameObject. Press
the space bar in Play Mode to see the Text change.
*/
using System.Net.Sockets;
using System;
using UnityEngine;

public class ServerState {
    private enum StateHolder {
        IDLE = 0,               // - Device socket not set yet, display IP on screen
        IDLE_IP = 1             // - Connected, waiting for instructions
    }

    private StateHolder stateHolder;
    private String lastContent;
    private String content;
    private Socket handler;

    public ServerState() {
        this.stateHolder = new StateHolder();
        this.stateHolder = StateHolder.IDLE;
        content = "";
        lastContent = "";
    }

    public void SetSocket(Socket handler) {
        this.handler = handler;
    }

    public Socket GetSocket() {
        return this.handler;
    }

    public void SetContent(String content) {
        this.content = content;
    }

    public String GetContent() {
        return this.content;
    }

    public void ToggleState() {
        if (this.stateHolder == StateHolder.IDLE) {
            this.stateHolder = StateHolder.IDLE_IP;
        }
        else {
            this.stateHolder = StateHolder.IDLE;
        }
    }

    public int GetState() {
        if (this.stateHolder == StateHolder.IDLE) {
            return 0;
        }
        else {
            return 1;
        }
    }

    public void RecycleContent() {
        if (!string.Equals("", content)) {
            lastContent = content;
            content = "";
            // Debug.Log("Recycling '" + content + "'. LastContent now '" + lastContent + "' and content '" + content + "'");
        }
    }
}
