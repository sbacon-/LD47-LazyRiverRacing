using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    float logoGrowthIncrement = 0.00004f;
    RectTransform logoTransform;

    private void Start() {
        GameObject logo = GameObject.Find("Logo");
        if(logo != null) logoTransform = logo.GetComponent<RectTransform>();
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("titleMusic"));
    }


    void Update()
    {
        if (logoTransform != null) {
            logoTransform.localScale = new Vector3(logoTransform.localScale.x + logoGrowthIncrement, 1, 1);
            if (Mathf.Abs(1 - logoTransform.localScale.x) > 0.25f) logoGrowthIncrement *= -1;
        }
    }

    public void Play() {
        SceneManager.LoadScene("CharSelect");
    }

    public void Quit() {
        SceneManager.LoadScene("MainMenu");
    }

}
