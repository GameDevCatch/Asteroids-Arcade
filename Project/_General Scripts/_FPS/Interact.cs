using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public virtual void OnInteract() { }
}

public class Interact : MonoBehaviour
{

    [SerializeField]
    private GameObject HUD_Text;
    [SerializeField]
    private GameObject HUD_Dot;
    [SerializeField]
    private Camera PlayerCamera;
    [SerializeField]
    private float MaxInteractionRange = 1000f;

    [Space]

    [SerializeField]
    private KeyCode interactKey;

    private bool _canInteract = true;

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
            Gizmos.DrawRay(PlayerCamera.transform.position, PlayerCamera.transform.forward.normalized * MaxInteractionRange);
    }

    private void Update()
    {
        if (!_canInteract) return;

        Ray camRay = new Ray(PlayerCamera.transform.position, PlayerCamera.transform.forward.normalized);
        RaycastHit rayHit;

        if (Physics.Raycast(camRay, out rayHit, MaxInteractionRange))
        {
            if (rayHit.transform.TryGetComponent(out IInteractable interactable))
            {
                HUD_Text.SetActive(true);

                if (Input.GetKeyDown(interactKey))
                    interactable.OnInteract();
            }
            else
                HUD_Text.SetActive(false);
        }
        else
            HUD_Text.SetActive(false);
    }

    public void CanInteract(bool value)
    {
        _canInteract = value;

        if (!_canInteract)
            HideHUD();
        else
            ShowHUD();
    }

    private void ShowHUD()
    {
        HUD_Text.SetActive(true);
        HUD_Dot.SetActive(true);
    }

    private void HideHUD()
    {
        HUD_Text.SetActive(false);
        HUD_Dot.SetActive(false);
    }
}