using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandsPresence : MonoBehaviour
{
    public bool showController = false;
    public InputDeviceCharacteristics controllerCharacteristics;
    public List<GameObject> controllerPrefabs;
    public GameObject handModelPrefab;
    
    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;


    // Start is called before the first frame update
    void Start()
    {
        List<InputDevice> devices = new List<InputDevice>();
  
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                Debug.LogError("Did not find a corresponding controller");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);

        }



    }

    // Update is called once per frame
    void Update()
    {

        targetDevice.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
        if (primaryButtonValue)
        {
            Debug.Log("The button to swap controllers was pressed");
            if (showController)
            {
                Debug.Log("Showing Controller, Not hand model");
                spawnedHandModel.SetActive(false);
                spawnedController.SetActive(true);
            }
            else
            {
                Debug.Log("Showing Hand Model, Not Controller Model");
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
            }
        }


        targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripButtonValue);
        if (gripButtonValue > 0.1f)
            showController = false;
        

    }
}
