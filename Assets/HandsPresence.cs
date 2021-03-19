using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;


public class HandsPresence : MonoBehaviour
{
    public bool showController = true;
    public InputDeviceCharacteristics controllerCharacteristics;
    public GameObject controllerPrefab;
    public GameObject handModelPrefab;
    public GameObject drumStickPrefab;
    private Quaternion controllerOrientation = Quaternion.Euler(180, 0, 180);

    private InputDevice targetDevice;
    private GameObject spawnedController;
    private GameObject spawnedHandModel;
    private Animator handAnimator;
    

    //Spawn the controllers into the scene
    void spawnControllers()
    {

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        /* -- Function to find all the items associated with the VR stuff
        foreach (var item in devices)
        {
            Debug.Log(item.name + item.characteristics);
        }*/

        //Check if the device exists
        if (devices.Count > 0)
        {
            targetDevice = devices[0];

            //Orient the controller for the player
            

            //Spawn the controller in the world for selected prefab
            spawnedController = Instantiate(controllerPrefab, transform);
            spawnedController.transform.rotation = controllerOrientation;
            spawnedController.SetActive(true);

            //Spawn hand object in the world and then deactivate it instantly
            spawnedHandModel = Instantiate(handModelPrefab, transform);
            spawnedHandModel.SetActive(false);

            handAnimator = spawnedHandModel.GetComponent<Animator>();

        }
    }

    void UpdateHandAnimation() {

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        spawnControllers();
    }

    // Update is called once per frame
    void Update()
        {

            //Buttons and values for the controllers
            targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripPressedValue);
            targetDevice.TryGetFeatureValue(CommonUsages.primaryTouch, out bool primaryTouchValue);
        targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);

        if (showController)
        {
            spawnedController.SetActive(true);
            spawnedHandModel.SetActive(false);
        }
        else
        {
            spawnedController.SetActive(false);
            spawnedHandModel.SetActive(true);
            UpdateHandAnimation();
        }


        //Figure out when the controller is grabbed by the player
        if (showController && primaryTouchValue)
        {
            Debug.Log("Controller: " + targetDevice.name + "has held the controller");
            ChangeControllerState(showController);
        }

        if (!showController && secondaryButtonValue)
        {
            SpawnDrumstick();
        }

    }
    //This function controls the state of the controller
    void ChangeControllerState(bool controllerState)
    {

        if (controllerState)
        {
            showController = false;
        }
        else
        {
            showController = true;
        }
    }

    void SpawnDrumstick()
    {
        Vector3 handPosition = spawnedHandModel.transform.position;
        GameObject spawnedDrumStick = Instantiate(drumStickPrefab);
        spawnedDrumStick.transform.position = handPosition;
        spawnedDrumStick.transform.rotation = controllerOrientation;
    }
}
