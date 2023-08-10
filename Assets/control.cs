using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class control : MonoBehaviour

{
    [SerializeField] private GameObject buba;

    private float deltaT;
    private float fr = 0.1f;
    private bool acc;
    private float init;
    private int posa = 1;
    private fall fallscript;
    private Dictionary<string, List<KeyCode>> keyBindings = new Dictionary<string, List<KeyCode>>()
    {
        {"MoveLeft", new List<KeyCode> {KeyCode.LeftArrow ,KeyCode.A }},
        {"MoveRight", new List<KeyCode> {KeyCode.RightArrow, KeyCode.D }},
        {"Rotate",new List<KeyCode> {KeyCode.Space, KeyCode.UpArrow, KeyCode.W }},
        {"Drop", new List<KeyCode> {KeyCode.DownArrow, KeyCode.S }},
    };

    void Start()
    {
        buba = buba = GameObject.Find("buba");
        fallscript = GetComponent<fall>();
        deltaT = Time.time;
        init = fallscript.dropInterval;
    }
    void Update()
    {
        if (keyBindings["MoveLeft"].Any(key => Input.GetKey(key)) && Time.time - deltaT > fr && IsMoveValid(Vector3.left))
            deltaT = Time.time;
        if (keyBindings["MoveRight"].Any(key => Input.GetKey(key)) && Time.time - deltaT > fr && IsMoveValid(Vector3.right))
            deltaT = Time.time;
        if (keyBindings["Rotate"].Any(key => Input.GetKeyDown(key)) && IsRotateValid() && transform.name != "O")
        {
            posa++;
            if (posa == 3 && transform.name == "I")
                posa = 1;
            else if (posa == 5)
                posa = 1;
        }
        if (keyBindings["Drop"].Any(key => Input.GetKeyDown(key)))
            acc = true;
        if (keyBindings["Drop"].Any(key => Input.GetKeyUp(key)))
            acc = false;
        if (acc)
            fallscript.dropInterval = 0.01f;
        else
            fallscript.dropInterval = init;
    }

    private bool IsMoveValid(Vector3 direction)
    {
        transform.position += direction;
        foreach (Transform block in transform)
        {
            if (Mathf.Round(block.transform.position.x * 2) / 2 < -4.5f || Mathf.Round(block.transform.position.x * 2) / 2 > 4.5f)
            {
                transform.position -= direction;
                return false;
            }
            foreach (Transform bubablock in buba.transform)
            {
                if (Mathf.Round(block.position.x * 2) / 2 == Mathf.Round(bubablock.position.x * 2) / 2 && Mathf.Round(block.position.y * 2) / 2 == Mathf.Round(bubablock.position.y * 2) / 2)
                {
                    transform.position -= direction;
                    return false;
                }
            }
        }
        fallscript.ShadowCast();
        return true;
    }

    private bool IsRotateValid(int shift = 0)
    {
        int maximumShifts = transform.name == "I" ? 3 : 2;
        int rotateDirection = 90;
        Vector3 direction = Vector3.zero;
        if (posa == 1 && (transform.name == "Z" || transform.name == "S"))
            direction += Vector3.right;
        else if (posa == 2)
        {
            direction += new Vector3(0, 1, 0);
            if (transform.name == "S" || transform.name == "Z")
                direction += Vector3.left;
        }
        else if (posa == 3)
        {
            direction += new Vector3(0, -1, 0);
            if (transform.name == "T")
                direction += Vector3.left;
        }
        else if (posa == 4 && transform.name == "T")
            direction += Vector3.right;
        if (transform.name == "I")
        {
            direction = Vector3.zero;
            if (shift == 1 || shift == 2)
                direction += Vector3.left * shift;
            else if (shift == 3)
                direction += Vector3.right;
            if (posa == 1)
                rotateDirection = 90;
            else
                rotateDirection = -90;
        }
        else
        {
            if (shift == 2)
                direction += Vector3.left;
            else if (shift == 1)
                direction += Vector3.right;
        }
        transform.Rotate(new Vector3(0, 0, rotateDirection), Space.Self);
        transform.position += direction;
        foreach (Transform block in transform)
        {
            if (Mathf.Round(block.transform.position.x * 2) / 2 < -4.5f || Mathf.Round(block.transform.position.x * 2) / 2 > 4.5f)
            {
                transform.Rotate(new Vector3(0, 0, -rotateDirection), Space.Self);
                transform.position -= direction;
                if (shift < maximumShifts)
                    return IsRotateValid(++shift);
                else
                    return false;
            }
            foreach (Transform bubablock in buba.transform)
            {
                if (Mathf.Round(block.position.x * 2) / 2 == Mathf.Round(bubablock.position.x * 2) / 2 && Mathf.Round(block.position.y * 2) / 2 == Mathf.Round(bubablock.position.y * 2) / 2)
                {
                    transform.Rotate(new Vector3(0, 0, -rotateDirection), Space.Self);
                    transform.position -= direction;
                    if (shift < maximumShifts)
                        return IsRotateValid(++shift);
                    else
                        return false;
                }
            }
        }
        fallscript.ShadowCast();
        return true;
    }
    private void PreRotate()
    {
        transform.position += new Vector3(0, -1.5f, 0);
        if (transform.name == "I")
        {
            transform.position += new Vector3(0.5f, 0.4f, 0);
            int R = Random.Range(0, 2);
            transform.RotateAround(transform.position - new Vector3(0.5f, 0, 0), Vector3.forward, 90f * R);
            posa++;
        }
        else if (transform.name != "O")
        {
            int R = Random.Range(0, 4);
            transform.RotateAround(transform.position + new Vector3(0, 0.5f, 0), Vector3.forward, 90f * R);
            posa += 1 * R;
        }
        else
            transform.position += new Vector3(0, 0.4f, 0);
    }
}
