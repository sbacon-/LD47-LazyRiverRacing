using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float maxSpeed = 10, turnSpeed = 45, acceleration = 0.5f, bobbingSpeed = 2;
    float yPosMax = 3.6f, yPosMin = 3.2f;
    public float fovRate = 0.25f, maxFOVChange=25;
    public Transform[] trackPointsList;
    public int trackPoint = 0;
    float yDir = 1, fieldOfView, speed=0;
    public bool started = false;
    public int score = 0;

    // Start is called before the first frame update
    void Start() {
        fieldOfView = Camera.main.fieldOfView;
    }

    // Update is called once per frame
    void Update()
    {
        //TURNING
        transform.Rotate(Vector3.up*((Input.GetKey(KeyCode.LeftShift) || (Input.GetKey(KeyCode.RightShift))?2:1) * Input.GetAxisRaw("Horizontal")*turnSpeed*Time.deltaTime));

        //ACCEL
        if (!started) return;
        speed += (Input.GetAxisRaw("Vertical") > 0) ? acceleration : -acceleration;
        speed = Mathf.Clamp(speed, 0, maxSpeed);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);


        Vector3 pos = transform.position;
        if (pos.y > yPosMax || pos.y < yPosMin) {
            yDir *= -1;
            transform.position = new Vector3(pos.x, Mathf.Clamp(pos.y, yPosMin, yPosMax), pos.z);
        }
        transform.Translate(Vector3.up * yDir * Time.deltaTime * bobbingSpeed);

        //FOV
        Camera.main.fieldOfView += ((speed==0)?-10:speed) * fovRate * Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, fieldOfView, fieldOfView + maxFOVChange);
    }

    private void OnTriggerEnter(Collider other) {
        try {
            if (other.CompareTag("TrackPoint") && other.transform == trackPointsList[trackPoint]) trackPoint++;
            
        } catch {

        }
        if (other.CompareTag("Ocean")) {
            GameObject.Find("UI").GetComponent<CanvasScript>().SecretEnding();
        }

        if (other.CompareTag("Item")) {
            Destroy(other.gameObject);
            score++;
        }

        if (other.CompareTag("Boost")) {
            maxSpeed = 15;
            speed += 5;
            Invoke("Unboost", 2);
        }
    }

    public void CalculateDir (){
        Vector3 direction = trackPointsList [0].position- transform.position;
        float theta = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        while (Mathf.Abs(theta) > 360) theta %= 360;
        transform.localRotation = Quaternion.Euler(Vector3.up* theta);
    }
    void Unboost() {
        maxSpeed = 10;
    }
}
