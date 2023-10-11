using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject targetDoor;
    public MeshRenderer screen;
    
    private List<TeleportableUnit> teleportableUnits;
    private RenderTexture viewTexture;
    private Camera mainCam;
    private Camera targetCam;

    private void Awake() {
        teleportableUnits = new List<TeleportableUnit>();
        mainCam = Camera.main;
        targetCam = GetComponentInChildren<Camera>();
    }

    private void Start() {
        
    }

    private void Update() {
        Render();
        for(int i = 0; i < teleportableUnits.Count; i++) {
            TeleportableUnit unit = teleportableUnits[i];
            Vector3 offset = unit.transform.position - transform.position;

            int portalSide = System.Math.Sign(Vector3.Dot(offset, transform.forward));
            int oldPortalSide = System.Math.Sign(Vector3.Dot(unit.oldOffset, transform.forward));
            
            if(portalSide != oldPortalSide) {
                unit.gameObject.transform.position = targetDoor.transform.position;
                teleportableUnits.Remove(unit);
                continue;
            }

            unit.oldOffset = offset;
        }
    }

    private void OnTriggerEnter(Collider other) {
        TeleportableUnit unit = other.GetComponent<TeleportableUnit>();
        if(unit) {
            unit.oldOffset = unit.transform.position - transform.position;
            teleportableUnits.Add(unit);
        }
        
    }

    private void OnTriggerExit(Collider other) {
        TeleportableUnit unit = other.GetComponent<TeleportableUnit>();
        if(unit) {
            teleportableUnits.Remove(unit);
        }
    }

    public void CreatePortalTexture() {
        if(viewTexture == null || viewTexture.width != Screen.width || viewTexture.height != Screen.height) {
            if(viewTexture != null) {
                viewTexture.Release();
            }

            viewTexture = new RenderTexture(Screen.width, Screen.height, 0);
            targetCam.targetTexture = viewTexture;

            targetDoor.GetComponent<Door>().screen.material.SetTexture("_MainTex", viewTexture);
        }
    }

    public void Render() {
        screen.enabled = false;
        CreatePortalTexture();

        var m = transform.localToWorldMatrix * targetDoor.transform.worldToLocalMatrix * mainCam.transform.localToWorldMatrix;
        targetCam.transform.SetPositionAndRotation(m.GetColumn(3), m.rotation);

        targetCam.Render();

        screen.enabled = true;
    }
}
