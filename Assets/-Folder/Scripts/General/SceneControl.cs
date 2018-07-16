using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    public List<GameObject> CameraObject;
    public List<GameObject> SceneObject;
    private SingletonControl Singleton;
    private GameObject MainCamera;
    private GameObject MarkerEnvironment;
    private Renderer ToogleRenderer;

    public void Awake()
    {
        Singleton = GameObject.FindWithTag("Singleton").GetComponent<SingletonControl>();
        MainCamera = Singleton.Camera.GetComponent<Camera>().gameObject;
        MarkerEnvironment = Singleton.Environment;

        foreach (GameObject Object in CameraObject)
        {
            Object.transform.SetParent(MainCamera.transform);
            Object.transform.localPosition = Vector3.zero;
            Object.transform.localRotation = Quaternion.identity;
        }
        foreach (GameObject Object in SceneObject)
        {
            Object.transform.SetParent(Singleton.Environment.transform);
        }
    }

    public void Start()
    {
        ToogleRenderer = GameObject.FindWithTag("Toggle").GetComponent<Renderer>();

        if (ToogleRenderer.enabled)
        {
            OnTrackingFound();
        }
        else
        {
            OnTrackingLost();
        }
    }

    public void LoadScene(string SceneName)
    {
        foreach (GameObject Object in CameraObject)
        {
            Object.transform.SetParent(transform);
        }
        foreach (GameObject Object in SceneObject)
        {
            Object.transform.SetParent(transform);
        }
        SceneManager.LoadScene(SceneName);
    }

    public void LoadScene(int SceneIndex)
    {
        foreach (GameObject Object in CameraObject)
        {
            Object.transform.SetParent(transform);
        }
        foreach (GameObject Object in SceneObject)
        {
            Object.transform.SetParent(transform);
        }
        SceneManager.LoadScene(SceneIndex);
    }

    private void OnTrackingFound()
    {
        var rendererComponents = MarkerEnvironment.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = MarkerEnvironment.GetComponentsInChildren<Collider>(true);
        var canvasComponents = MarkerEnvironment.GetComponentsInChildren<Canvas>(true);

        // Enable rendering:
        foreach (var component in rendererComponents)
            component.enabled = true;

        // Enable colliders:
        foreach (var component in colliderComponents)
            component.enabled = true;

        // Enable canvas':
        foreach (var component in canvasComponents)
            component.enabled = true;
    }

    private void OnTrackingLost()
    {
        var rendererComponents = MarkerEnvironment.GetComponentsInChildren<Renderer>(true);
        var colliderComponents = MarkerEnvironment.GetComponentsInChildren<Collider>(true);
        var canvasComponents = MarkerEnvironment.GetComponentsInChildren<Canvas>(true);

        // Disable rendering:
        foreach (var component in rendererComponents)
            component.enabled = false;

        // Disable colliders:
        foreach (var component in colliderComponents)
            component.enabled = false;

        // Disable canvas':
        foreach (var component in canvasComponents)
            component.enabled = false;
    }
}
