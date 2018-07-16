using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ToggleAR : MonoBehaviour
{
    public GameObject DepthMask;
    public GameObject DoorLeft;
    public GameObject DoorRight;
    public AudioClip DefaultClip;

    private Vuforia.VuforiaBehaviour Vuforia;
    private Camera CameraAR;
    private Renderer Renderer;
    private GameObject VuforiaClipping;
    private Animator DoorLeftAnim;
    private Animator DoorRightAnim;
    private bool Toggle;
    private bool DepthMaskCache;
    private bool DoorCache;
    private bool VuforiaClippingCache;
    private bool FirstUpdate = true;
    private AudioSource CameraARAudioSouce;

    public void Awake()
    {
        SceneManager.activeSceneChanged += ResetScene; // subscribe    
        Vuforia = GameObject.FindWithTag("Singleton").GetComponent<SingletonControl>().Camera.GetComponent<Vuforia.VuforiaBehaviour>();
    }

    public void Start()
    {
        CameraAR = GameObject.FindWithTag("Singleton").GetComponent<SingletonControl>().Camera.GetComponent<Camera>();
        CameraARAudioSouce = CameraAR.GetComponent<AudioSource>();

        if (Vuforia.enabled)
        {
            Renderer = GetComponent<Renderer>();
            VuforiaClipping = GameObject.Find("BackgroundPlane");
        }

        DoorLeftAnim = DoorLeft.GetComponent<Animator>();
        DoorRightAnim = DoorRight.GetComponent<Animator>();
    }

    public void OnDestroy()
    {
        SceneManager.activeSceneChanged -= ResetScene; // unsubscribe
    }

    public void ResetScene(Scene previousScene, Scene newScene)
    {
        Color newCol;

        if (ColorUtility.TryParseHtmlString("#80808080", out newCol))
        {
            if (RenderSettings.skybox.HasProperty("_Tint"))
            {
                RenderSettings.skybox.SetColor("_Tint", newCol);
            }
        }

        Time.timeScale = 1;
        Time.fixedDeltaTime = 1 * 0.02f;

        if (!FirstUpdate)
        {
            FullMode(false);

            if (CameraARAudioSouce.clip != DefaultClip)
            {
                CameraARAudioSouce.clip = DefaultClip;
                CameraARAudioSouce.Play();
            }

            if (newScene.name == "Menu")
            {
                if (!DoorLeftAnim.GetCurrentAnimatorStateInfo(0).IsName("Close"))
                {
                    DoorLeftAnim.SetTrigger("Close");
                }
                if (!DoorRightAnim.GetCurrentAnimatorStateInfo(0).IsName("Close"))
                {
                    DoorRightAnim.SetTrigger("Close");
                }
            }
            else if (newScene.name != "Menu")
            {
                if (!DoorLeftAnim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                {
                    DoorLeftAnim.SetTrigger("Open");
                }
                if (!DoorRightAnim.GetCurrentAnimatorStateInfo(0).IsName("Open"))
                {
                    DoorRightAnim.SetTrigger("Open");
                }
            }
        }
    }

    public void Update()
    {
        if (FirstUpdate)
        {
            if (Vuforia.enabled)
            {
                Toggle = !Renderer.enabled;
                DepthMaskCache = DepthMask.activeInHierarchy;
                var DoorLeftChild = DoorLeft.GetComponentsInChildren<Transform>(true);
                DoorCache = DoorLeftChild[0].gameObject.activeInHierarchy;
                VuforiaClippingCache = VuforiaClipping.activeInHierarchy;
                FullMode(false);
            }

            FirstUpdate = false;
        }

        if (Vuforia.enabled)
        {
            if (Renderer.enabled != Toggle)
            {
                RefreshComponents();
                Toggle = Renderer.enabled;
            }
        }
    }

    public void RefreshComponents()
    {
        if (!Renderer.enabled)
        {
            CameraAR.clearFlags = CameraClearFlags.SolidColor;
            DepthMask.SetActive(true);
            VuforiaClipping.SetActive(true);

            foreach (Transform child in DoorLeft.transform)
            {
                child.gameObject.SetActive(false);
            }

            foreach (Transform child in DoorRight.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
        else if (Renderer.enabled)
        {
            CameraAR.clearFlags = CameraClearFlags.Skybox;
            DepthMask.SetActive(DepthMaskCache);
            VuforiaClipping.SetActive(VuforiaClippingCache);

            foreach (Transform child in DoorLeft.transform)
            {
                child.gameObject.SetActive(DoorCache);
            }

            foreach (Transform child in DoorRight.transform)
            {
                child.gameObject.SetActive(DoorCache);
            }
        }
    }

    public void FullMode(bool On)
    {
        if (Vuforia.enabled)
        {
            if (On)
            {
                DepthMaskCache = false;
                DoorCache = false;
                VuforiaClippingCache = false;
            }
            else
            {
                DepthMaskCache = true;
                DoorCache = true;
                VuforiaClippingCache = true;
            }

            DepthMask.SetActive(DepthMaskCache);
            VuforiaClipping.SetActive(VuforiaClippingCache);

            foreach (Transform child in DoorLeft.transform)
            {
                child.gameObject.SetActive(DoorCache);
            }

            foreach (Transform child in DoorRight.transform)
            {
                child.gameObject.SetActive(DoorCache);
            }
        }
        else
        {
            if (On)
            {
                DepthMaskCache = false;
                DoorCache = false;
            }
            else
            {
                DepthMaskCache = true;
                DoorCache = true;
            }

            DepthMask.SetActive(DepthMaskCache);

            foreach (Transform child in DoorLeft.transform)
            {
                child.gameObject.SetActive(DoorCache);
            }

            foreach (Transform child in DoorRight.transform)
            {
                child.gameObject.SetActive(DoorCache);
            }
        }
    }
}
