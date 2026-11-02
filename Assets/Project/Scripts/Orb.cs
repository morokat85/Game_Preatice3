
using UnityEngine;
using UnityEngine.InputSystem;

public class Orb : MonoBehaviour
{
    public float speed = 3f;

    public Color clickColor = Color.red;
    public Color moveColor = Color.green;

    private Renderer orbRenderer;
    private bool isSelected = false;

    void Start()
    {
        orbRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        // Click to select this Orb
        if (Mouse.current != null &&
            Mouse.current.leftButton.wasPressedThisFrame)
        {
            Camera cam = Camera.main;

            if (cam != null)
            {
                Ray ray = cam.ScreenPointToRay(
                    Mouse.current.position.ReadValue()
                );

                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform == transform ||
                        hit.transform.IsChildOf(transform))
                    {
                        isSelected = true;
                        orbRenderer.material.color = clickColor;

                        Debug.Log(gameObject.name + " selected!");
                    }
                    else
                    {
                        isSelected = false;
                    }
                }
            }
        }

        // Move only the selected Orb
        if (isSelected && Keyboard.current != null)
        {
            float x = 0f;
            float z = 0f;

            if (Keyboard.current.aKey.isPressed ||
                Keyboard.current.leftArrowKey.isPressed)
                x = -1f;

            if (Keyboard.current.dKey.isPressed ||
                Keyboard.current.rightArrowKey.isPressed)
                x = 1f;

            if (Keyboard.current.sKey.isPressed ||
                Keyboard.current.downArrowKey.isPressed)
                z = -1f;

            if (Keyboard.current.wKey.isPressed ||
                Keyboard.current.upArrowKey.isPressed)
                z = 1f;

            Vector3 movement = new Vector3(x, 0f, z).normalized;

            if (movement.sqrMagnitude > 0f)
            {
                transform.Translate(
                    movement * speed * Time.deltaTime,
                    Space.World
                );

                orbRenderer.material.color = moveColor;
            }
        }
    }
}
