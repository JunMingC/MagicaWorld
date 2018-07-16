using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangeDoorButton : MonoBehaviour
{
    public SceneControl Scene;
    public string NextSceneName;
    public bool OpenDoor = true;
    public List<Renderer> RendererList;
    public List<Collider> ColliderList;

    private Animator DoorLeft;
    private Animator DoorRight;
    private bool ClickOnce;

    public void Start()
    {
        DoorLeft = GameObject.FindWithTag("DoorLeft").GetComponent<Animator>();
        DoorRight = GameObject.FindWithTag("DoorRight").GetComponent<Animator>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerBullet") && !ClickOnce)
        {
            ClickOnce = true;
            StartCoroutine(OnClick());
        }
    }

    public IEnumerator OnClick()
    {
        if(OpenDoor)
        {
            GetComponent<Collider>().enabled = false;

            foreach (Collider Coll in ColliderList)
            {
                Coll.enabled = false;
            }

            GetComponent<Renderer>().enabled = false;

            foreach (Renderer Rend in RendererList)
            {
                Rend.enabled = false;
            }

            DoorLeft.Play("Open");
            DoorRight.Play("Open");
            yield return new WaitForSeconds(0.001f);
            yield return new WaitForSeconds(DoorLeft.GetCurrentAnimatorStateInfo(0).length - 0.001f);
        }

        Scene.LoadScene(NextSceneName);
    }
}
