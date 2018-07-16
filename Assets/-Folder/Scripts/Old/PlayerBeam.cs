using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBeam : MonoBehaviour
{
    public GameObject hitParticle;
    public float weaponRange;

    private Camera fpsCam;
    private LineRenderer laserLine;
    private WaitForSeconds shotDuration = new WaitForSeconds(0.25f);
    private float startWidth = 0.5f;
    private float endWidth = 6f;
    float lerp = 0f, duration = 0.25f;

    public void Start()
    {
        fpsCam = Camera.main;
        laserLine = GetComponent<LineRenderer>();
    }

    public void Update()
    {
        Vector3 CameraCenter = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
        Debug.DrawRay(CameraCenter, fpsCam.transform.forward * weaponRange, Color.green);

        if (Input.GetButtonDown("Fire2"))
        {
            lerp = 0f;
            StartCoroutine(ShotEffect());

            Vector3 startPos = transform.position;
            laserLine.SetPosition(0, startPos);

            Vector3 endPos = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));
            RaycastHit hit;

            if (Physics.Raycast(startPos, fpsCam.transform.forward, out hit, weaponRange))
            {
                laserLine.SetPosition(1, hit.point);
                StartCoroutine(HitEffect(hit.point));
            }
            else
            {
                laserLine.SetPosition(1, endPos + (fpsCam.transform.forward * weaponRange));
            }
        }

        if(laserLine.enabled)
        {
            lerp += Time.deltaTime / duration;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, Mathf.Lerp(startWidth, 0, lerp));
            curve.AddKey(1, Mathf.Lerp(endWidth, 0, lerp));

            laserLine.widthCurve = curve;

        }
    }

    private IEnumerator ShotEffect()
    {
        laserLine.enabled = true;
        yield return shotDuration;
        laserLine.enabled = false;
    }

    private IEnumerator HitEffect(Vector3 pos)
    {
        hitParticle.transform.position = pos;
        hitParticle.SetActive(true);
        yield return shotDuration;
        hitParticle.SetActive(false);
    }
}