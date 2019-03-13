using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCommands : MonoBehaviour
{
    public GameObject Ennemy;

    public GameObject CurrentCharacter;

    public GameObject AeCharacter;
    public GameObject JCharacter;

    public bool Ae = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Vector2 mousePosition = Input.mousePosition;
            Ray castPoint = Camera.main.ScreenPointToRay(mousePosition);

            RaycastHit hit;
            if (Physics.Raycast(castPoint, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                Instantiate(Ennemy, hit.point, Quaternion.Euler(0, 0, 0));
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 Position = CurrentCharacter.transform.position;
            Quaternion Rotation = CurrentCharacter.transform.rotation;

            Destroy(CurrentCharacter);

            if (!Ae)
            {
                CurrentCharacter = Instantiate(AeCharacter, Position, Rotation);
            }
            else
            {
                CurrentCharacter = Instantiate(JCharacter, Position, Rotation);
            }
            Ae = !Ae;
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            CurrentCharacter.transform.position = new Vector3(CurrentCharacter.transform.position.x, 1.8f, CurrentCharacter.transform.position.z);
        }
    }
}
