using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour {

    public float[] allowedX = { -12, 12 };
    public float[] allowedY = { 1, 5 };
    //time between changing new path (seconds)
    public float pathUnitTime = 1f;

    private float pathTimer = 0;
    private Vector2 nextPoint;
    private Vector2 oldPoint;
    private float distance;


    private void Start() {
        transform.position = new Vector2(allowedX[0], Random.Range(allowedY[0], allowedY[1]));
        pathTimer = 0;
        nextPoint = NewPoint();
    }


    private void Update() {
        pathTimer += Time.deltaTime;
        transform.position = Vector2.Lerp(oldPoint, nextPoint, pathTimer / (distance * pathUnitTime));

        //new path
        if (pathTimer / (distance * pathUnitTime) >= 1) {
            pathTimer = 0;
            nextPoint = NewPoint();
        }
    }


    private Vector2 NewPoint() {
        float x = Random.Range(Mathf.Max(allowedX[0], transform.position.x - 5), 
                               Mathf.Min(allowedX[1], transform.position.x + 5));
        float y = Random.Range(Mathf.Max(allowedY[0], transform.position.y - 2),
                               Mathf.Min(allowedY[1], transform.position.y + 2));
        Vector2 newPoint = new Vector2(x, y);

        oldPoint = transform.position;
        distance = Vector2.Distance(oldPoint, newPoint);
        Vector2 vector = newPoint - oldPoint;
        Vector2 baseVector = new Vector2(1, 0);

        //TODO fix angle
        float angleBetweenVectors = Mathf.Acos(Vector2.Dot(vector, baseVector) / (vector.magnitude * baseVector.magnitude));
        float rotationDegreesZ = angleBetweenVectors * 180 / Mathf.PI;
        transform.eulerAngles = new Vector3(0, 0, rotationDegreesZ);

        return newPoint;
    }
}
