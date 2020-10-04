using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour
{
    GameObject raceFinish,splash,timer,lap;
    bool canChange = true;
    float time = 0.001f;
    public int lapCount = 1;
    string pickup = "";
    // Start is called before the first frame update
    void Start()
    {
        raceFinish = GameObject.FindGameObjectWithTag("RaceFinish");
        raceFinish.SetActive(false);
        splash = GameObject.FindGameObjectWithTag("Splash");
        splash.SetActive(false);
        timer = GameObject.FindGameObjectWithTag("Timer");
        lap = GameObject.FindGameObjectWithTag("Lap");
    }

    // Update is called once per frame
    void Update() {
        timer.GetComponent<UnityEngine.UI.Text>().text = time.ToString().Substring(0, 4);
        lap.GetComponent<UnityEngine.UI.Text>().text = "LAP:" + lapCount + "/3";
    }

    public void Splash(string text) {
        splash.SetActive(true);
        splash.GetComponent<UnityEngine.UI.Text>().text = text;
        
    }



    public void HideSplash() {
        splash.SetActive(false);
    }

    public void RaceFinish() {
        CancelInvoke("timerUpdate");
        raceFinish.SetActive(true);
    }
    public void Win() {
        if (!canChange) return;
        raceFinish.GetComponentInChildren<UnityEngine.UI.Text>().text = "YOU WON THE RACE! CONGRATS \n TIME:"+time.ToString().Substring(0,4);
        canChange = false;
    }
    public void Lose() {
        if (!canChange) return;
        raceFinish.GetComponentInChildren<UnityEngine.UI.Text>().text = "BETTER LUCK NEXT TIME \n TIME:" + time.ToString().Substring(0, 4);
        canChange = false;
    }
    
    public void SecretEnding() {
        canChange = false;
        raceFinish.GetComponentInChildren<UnityEngine.UI.Text>().text = "YOU'VE ESCAPED THE LOOP, AND THERIN HAVE FOUND THE TRUE VICTORY";
        RaceFinish();
    }

    public void MainMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    public void StartTimer() {
        InvokeRepeating("timerUpdate", 0, 0.01f);
    }

    void timerUpdate() {
        time += 0.01f;
    }

    public string scoreString(int x) {

        splash.GetComponent<UnityEngine.UI.Text>().fontSize = 35;

        if (x == 15) {
            pickup = "YOU COLLECTED ALL POWERUPS! WELL DONE!";
        } else {
            pickup = "GREAT! BUT YOU MISSED " + (15 - x) + " POWERUPS...";
        }

        return pickup;

    }


}

