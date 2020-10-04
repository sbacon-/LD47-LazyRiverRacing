using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour {

    float maxSpeed = 10, acceleration = 0.5f;
    float speed=0;
    public Transform[] trackPointsList;
    public int trackPoint = 0;
    public Vector3 targetPos;
    public bool started = false;
    public string charName = "";

    // Start is called before the first frame update
    void Start() {
        maxSpeed = Random.Range(8,maxSpeed);
        acceleration = Random.Range(0.3f, acceleration);
    }

    // Update is called once per frame
    void Update() {

        //I DON'T KNOW HOW THIS CODE WORKS BUT SOMEHOW IT DOES SO DON'T FUCK WITH IT
        Vector3 direction = targetPos - transform.position;
        float theta = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg;
        while(Mathf.Abs(theta)>360)theta %= 360;
        transform.localRotation = Quaternion.Euler(Vector3.up * theta);

        if (!started) return;

        speed += acceleration;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
    private void OnTriggerEnter(Collider other) {
        try {
            if (other.CompareTag("TrackPoint") && other.transform == trackPointsList[trackPoint]) {
                trackPoint++;
                SetTargetPos();
            }
        } catch {
        }
        if (other.CompareTag("Boost")) {
            maxSpeed = 15;
            speed += 5;
            Invoke("Unboost", 2);
        }
    }
    
    public void SetTargetPos() {

        targetPos = trackPointsList[trackPoint].position;

        //Make the target Pos a little more natural
        targetPos = new Vector3(targetPos.x + Random.Range(-2.5f, 2.5f), transform.position.y, targetPos.z + Random.Range(-2.5f, 2.5f));
    }


    void Unboost() {
        maxSpeed = 10;
    }
}


