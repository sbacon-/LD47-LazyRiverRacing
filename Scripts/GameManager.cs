using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    GameObject player,racers,startPos,trackPoints, ui;
    Transform[] position = new Transform[6];
    Transform[] startPosList = new Transform[6];
    Transform[] trackPointsList;

    bool raceStarted=false;
    float countDown = 4;
    int trackPointCount=0;
    // Start is called before the first frame update
    void Start() {
        //GET SELECTED CHAR
        GameObject ddolInject = new GameObject("inject");
        DontDestroyOnLoad(ddolInject);
        int selectedChar=0;
        foreach(GameObject go in ddolInject.scene.GetRootGameObjects()) {
            if (go.CompareTag("select")) {
                selectedChar = int.Parse(go.name);
                Destroy(go);
            }
        }
        Destroy(ddolInject);


        //LIST START POSITIONS
        startPos = GameObject.Find("StartPos");
        int i = 0;
        foreach (Transform go in startPos.GetComponentsInChildren<Transform>()) {
            if (go.CompareTag("StartPos")) {
                startPosList[i++] = go;
            }
        }
        //LIST RACERS AND ASSIGN START POSITIONS
        racers = GameObject.Find("Racers");
        i = 0;
        foreach (Transform go in racers.GetComponentsInChildren<Transform>()) {
            if (go.CompareTag("NPC")) {
                go.position = startPosList[i].position;
                position[i++] = go;

            }
        }
        player = position[selectedChar].gameObject;
        player.name = "PLAYER";
        player.tag = "Player";
        Destroy(player.GetComponent<NPCMovement>());
        player.AddComponent<PlayerMovement>();
        player = position[selectedChar].gameObject;
        Camera.main.transform.parent = player.transform;
        Camera.main.transform.localPosition = new Vector3(0,7,-7);
        

        //LIST TRACK POINTS and CREATE DISTRIBUT LIST TO PLAYER AND NPCS
        trackPoints = GameObject.FindGameObjectWithTag("Track");
        i = 0;
        int count = -1;
        foreach (Transform c in trackPoints.GetComponentsInChildren<Transform>()) count++;
        trackPointCount = count * 3;
        trackPointsList = new Transform[trackPointCount];
        for (int lapPoint = 0; lapPoint < 3; lapPoint++) {
            foreach (Transform tp in trackPoints.GetComponentsInChildren<Transform>()) {
                if (tp.CompareTag("TrackPoint")) {
                    trackPointsList[i] = tp;
                    i++;
                }
            }
        }
        player.GetComponent<PlayerMovement>().trackPointsList = trackPointsList;
        player.GetComponent<PlayerMovement>().CalculateDir();
        foreach (Transform go in racers.GetComponentsInChildren<Transform>()) {
            if (go.CompareTag("NPC")) {
                go.GetComponent<NPCMovement>().trackPointsList = trackPointsList;
                go.GetComponent<NPCMovement>().SetTargetPos();
            }
        }
        InvokeRepeating("Countdown", 2, 1);

        ui = GameObject.Find("UI");
    }
    // Update is called once per frame
    void Update() {
        foreach (Transform go in racers.GetComponentsInChildren<Transform>()) {
            if (go.CompareTag("NPC")) {
                if(go.GetComponent<NPCMovement>().trackPoint > trackPointCount - 1) {
                    ui.GetComponent<CanvasScript>().Lose();
                }
            }
        }
        if (player.GetComponent<PlayerMovement>().trackPoint > trackPointCount - 1) {
            ui.GetComponent<CanvasScript>().Win();
            ui.GetComponent<CanvasScript>().RaceFinish();
        }

        if (player.GetComponent<PlayerMovement>().trackPoint > trackPointCount - 3) {
            ui.GetComponent<CanvasScript>().Splash(ui.GetComponent<CanvasScript>().scoreString(player.GetComponent<PlayerMovement>().score));
        }

        ui.GetComponent<CanvasScript>().lapCount = (player.GetComponent<PlayerMovement>().trackPoint / (trackPointCount/3) )+1;
    }

    void Countdown() {
        countDown -= 1;
        ui.GetComponent<CanvasScript>().Splash(countDown.ToString());
        if (countDown == 0) {
            foreach (Transform go in racers.GetComponentsInChildren<Transform>()) {
                if (go.CompareTag("NPC")) {
                    go.GetComponent<NPCMovement>().started = true;
                }
            }
            player.GetComponent<PlayerMovement>().started = true;

            ui.GetComponent<CanvasScript>().Splash("GO!");
            ui.GetComponent<CanvasScript>().StartTimer();
            Invoke("HideSplash", 2);
            CancelInvoke("Countdown");
        }
    }

    void HideSplash() {
        ui.GetComponent<CanvasScript>().HideSplash();
    }
}
