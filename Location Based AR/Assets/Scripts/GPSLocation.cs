using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class GPSLocation : MonoBehaviour
{
    // [SerializeField]
    // private ARCameraManager arCameraManager;
    [SerializeField]
    private Text LongitudeValue;
    [SerializeField]
    private Text LatitudeValue;
    [SerializeField]
    private Text AltitudeValue;

    // Shiba prefab
    [SerializeField]
    private GameObject arCamera;
    [SerializeField]
    private GameObject Shiba;

    // private float count = 0;
    private Text AltitudeStored;
    private Text LongitudeStored;
    private Text LatitudeStored;
    [SerializeField]
    private Text VariableText;
    [SerializeField]
    private Text UnityPositionText;
    private float LongitudeFloat;
    private float LatitudeFloat;
    private float AltitudeFloat;
    private float point1;
    private float point2;
    private float distance;
    // private Vector3 UnityPosition;

    // Start is called before the first frame update
    private void Start()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            Permission.RequestUserPermission(Permission.CoarseLocation);
        }
        StartCoroutine(GetLocation());
    }
    private void Update()
    {
        if (Input.location.status == LocationServiceStatus.Running)
        {
            // Access granted and location value could be retrieved
            LongitudeValue.text = Input.location.lastData.longitude.ToString();
            LatitudeValue.text = Input.location.lastData.latitude.ToString();
            AltitudeValue.text = Input.location.lastData.altitude.ToString();
            // UnityPosition = GPSEncoder.GPSToUCS(Input.location.lastData.latitude, Input.location.lastData.longitude);
            UnityPositionText.text = "Unity Position : (" + GPSEncoder.GPSToUCS(Input.location.lastData.latitude, Input.location.lastData.longitude).ToString() + ")";
        }
        // count += 1;
    }
    public void drawObject()
    {
            GameObject newObject = Instantiate(Shiba, new Vector3(arCamera.transform.position.x, arCamera.transform.position.y, arCamera.transform.position.z), Quaternion.identity);
            Debug.Log("Object created at :" + newObject.transform.position);
            newObject.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            // newObject.transform.Rotate(0, 0, 50*Time.deltaTime);
            VariableText.GetComponent<Text>().text = ("Created at: " + GPSEncoder.GPSToUCS(Input.location.lastData.latitude, Input.location.lastData.longitude).ToString());
            newObject.transform.parent = GameObject.Find("Canvas").transform;  // Set the object as a child of the Canvas game object
            Debug.Log("Shiba created");
    }

    public void applicationQuit()
    {
        Application.Quit();
    }

    public void StoreLocation()
    {
        // Placeholder for storing location values
        // LatitudeStored = Input.location.lastData.latitude.ToString();
        // LongitudeStored = Input.location.lastData.longitude.ToString();
        // AltitudeStored = Input.location.lastData.altitude.ToString();
        LatitudeFloat = Input.location.lastData.latitude;
        LongitudeFloat = Input.location.lastData.longitude;
        AltitudeFloat = Input.location.lastData.altitude;
    }

    public void calculateDistance()
    {
        double latitude = (double)Input.location.lastData.latitude * 0.0174532925199433;
        double longitude = (double)Input.location.lastData.longitude * 0.0174532925199433;
        double num = (double)LatitudeFloat * 0.0174532925199433;
        double longitude1 = (double)LongitudeFloat * 0.0174532925199433;
        double num1 = longitude1 - longitude;
        double num2 = num - latitude;
        double num3 = Mathf.Pow(Mathf.Sin((float)num2 / 2), 2) + Mathf.Cos((float)latitude) * Mathf.Cos((float)num) * Mathf.Pow(Mathf.Sin((float)num1 / 2), 2);
        double num4 = 2 * Mathf.Atan2(Mathf.Sqrt((float)num3), Mathf.Sqrt(1 - (float)num3));
        double num5 = 6376500 * num4;
        distance = (float)num5;
        VariableText.GetComponent<Text>().text = "Distance : " + distance.ToString() + " m";
    }

    IEnumerator GetLocation()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("User has not enabled location");
            yield break;
        }
        Input.location.Start(5f, 5f);
        while(Input.location.status == LocationServiceStatus.Initializing)
        {
            yield return new WaitForSeconds(1);
        }
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("Unable to determine device location");
            yield break;
        }
        // else
        // {
        //     InvokeRepeating("UpdateGPSData", 0.5f, 1f);
        // }
        // Debug.Log ("Latitude : " + Input.location.lastData.latitude);
        // Debug.Log ("Longitude : " + Input.location.lastData.longitude);
        // Debug.Log ("Altitude : " + Input.location.lastData.altitude);
    }
}
