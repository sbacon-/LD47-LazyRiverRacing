using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharSelectScript : MonoBehaviour
{
    GameObject charNameText;
    Transform[] charSelect = new Transform[6];
    int selected = 0;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
        foreach(Transform t in GetComponentInChildren<Transform>()) {
            if (t.CompareTag("NPC")) {
                charSelect[i] = t;
                i++;
            }
        }
        foreach(Transform t in charSelect) {
            t.gameObject.SetActive(false);
        }
        charNameText = GameObject.Find("CHAR NAME");
        DisplayChar();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * 32);
        if (Input.GetKeyDown(KeyCode.Space)) {
            GameObject selection = new GameObject(selected.ToString());
            selection.tag = "select";
            Object.DontDestroyOnLoad(selection);
            StartGame();
        }
    }

    public void Left() {
        selected--;
        DisplayChar();
    }
    public void Right() {
        selected++;
        DisplayChar();
    }
    void DisplayChar() {
        selected %= 6;
        if (selected < 0) selected = 5;
        foreach (Transform t in charSelect) {
            t.gameObject.SetActive(false);
        }
        charSelect[selected].gameObject.SetActive(true);

        string name = "";
        switch (selected) {
            case 0: 
                name = "LOOP CAT";
                break;
            case 1:
                name = "KING SHARK";
                break;
            case 2:
                name = "TONY HAWK";
                break;
            case 3:
                name = "PIRATE";
                break;
            case 4:
                name = "SNOW DRAMA";
                break;
            case 5:
                name = "WATERPARK KID";
                break;
        }
        charNameText.GetComponent<UnityEngine.UI.Text>().text = name;
    }

    void StartGame() {
        //GET SELECTED CHAR
        GameObject ddolInject = new GameObject("inject");
        DontDestroyOnLoad(ddolInject);
        foreach (GameObject go in ddolInject.scene.GetRootGameObjects()) {
            if(go.CompareTag("titleMusic"))Destroy(go);
        }
        Destroy(ddolInject);
        SceneManager.LoadScene("SampleScene");
    }
}
