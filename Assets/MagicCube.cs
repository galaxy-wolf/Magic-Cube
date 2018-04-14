using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class MagicCube : MonoBehaviour
{
    public float speed = 1.0f;
    public Button viewButton;
    public Button resetButton;
    public Button [] buttons;

    public GameObject opPlane;

    bool isView = false;

    GameObject[,,] cubes;

    // 每个面不同颜色， 所以使用6个plan 组成
    GameObject CreateCube()
    {
        GameObject ret = new GameObject();

        GameObject up = GameObject.CreatePrimitive(PrimitiveType.Plane);
        up.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        up.transform.localPosition = new Vector3(0, 0.5f, 0);
        up.transform.localRotation = Quaternion.Euler(0, 0, 0);
        up.transform.name = "up";
        up.transform.parent = ret.transform;
        up.GetComponent<MeshRenderer>().material.color = Color.red;


        GameObject down = GameObject.CreatePrimitive(PrimitiveType.Plane);
        down.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        down.transform.localPosition = new Vector3(0, -0.5f, 0);
        down.transform.localRotation = Quaternion.Euler(0, 0, 180);
        down.transform.name = "down";
        down.transform.parent = ret.transform;
        down.GetComponent<MeshRenderer>().material.color = Color.green;

        GameObject left = GameObject.CreatePrimitive(PrimitiveType.Plane);
        left.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        left.transform.localPosition = new Vector3(-0.5f, 0, 0);
        left.transform.localRotation = Quaternion.Euler(0, 0, 90);
        left.transform.name = "left";
        left.transform.parent = ret.transform;
        left.GetComponent<MeshRenderer>().material.color = Color.blue;

        GameObject right = GameObject.CreatePrimitive(PrimitiveType.Plane);
        right.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        right.transform.localPosition = new Vector3(0.5f, 0, 0);
        right.transform.localRotation = Quaternion.Euler(0, 0, -90);
        right.transform.name = "right";
        right.transform.parent = ret.transform;
        right.GetComponent<MeshRenderer>().material.color = Color.yellow;

        GameObject front = GameObject.CreatePrimitive(PrimitiveType.Plane);
        front.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        front.transform.localPosition = new Vector3(0, 0, -0.5f);
        front.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        front.transform.name = "front";
        front.transform.parent = ret.transform;
        front.GetComponent<MeshRenderer>().material.color = Color.white;

        GameObject back = GameObject.CreatePrimitive(PrimitiveType.Plane);
        back.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        back.transform.localPosition = new Vector3(0, 0, 0.5f);
        back.transform.localRotation = Quaternion.Euler(90, 0, 0);
        back.transform.name = "back";
        back.transform.parent = ret.transform;
        back.GetComponent<MeshRenderer>().material.color = Color.black;

        return ret;
    }

    void Rotate(int face, bool ccw=false)
    {
        int[,,] next = {
            { {0, 0 }, {0, 2} }, // cw : 0,0-> 0,2 ccw : 0,2->0,1
            { {0, 2 }, {2, 2} },
            { {2, 2 }, {2, 0} },
            { {2, 0 }, {0, 0} },
            { {0, 1 }, {1, 2} },
            { {1, 2 }, {2, 1} },
            { {2, 1 }, {1, 0} },
            { {1, 0 }, {0, 1} },
        };
        GameObject[,,] old = new GameObject[3, 3, 3];
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                for (int k = 0; k < 3; ++k)
                    old[i, j, k] = cubes[i, j, k];
        int src = 0;
        int dst = 1;
        int degree = 90;

        if (ccw)
        {
            src = 1;
            dst = 0;
            degree = -90;
        }

        switch (face)
        {
            case 1:// up
                {
                    int y = 2;
                    for (int i = 0; i < 8; ++i)
                    {
                        cubes[next[i, dst, 0], y, next[i, dst, 1]] = old[next[i, src, 0], y, next[i, src, 1]];
                        cubes[next[i, dst, 0], y, next[i, dst, 1]].transform.parent = cubes[1, y, 1].transform;
                    }
                    cubes[1, y, 1].transform.localRotation *= Quaternion.Euler(0, degree, 0);

                    for (int i=0; i<8; ++i)
                    {
                        cubes[next[i, dst, 0], y, next[i, dst, 1]].transform.parent = gameObject.transform;
                    }
                }
                break;
            case 2://down
                {
                    int y = 0;
                    for (int i = 0; i < 8; ++i)
                    {
                        cubes[next[i, dst, 0], y, next[i, dst, 1]] = old[next[i, src, 0], y, next[i, src, 1]];
                        cubes[next[i, dst, 0], y, next[i, dst, 1]].transform.parent = cubes[1, y, 1].transform;
                    }
                    cubes[1, y, 1].transform.localRotation *= Quaternion.Euler(0, degree, 0);

                    for (int i=0; i<8; ++i)
                    {
                        cubes[next[i, dst, 0], y, next[i, dst, 1]].transform.parent = gameObject.transform;
                    }
                }
                break;
            case 3://left
                {
                    int x = 0;
                    for (int i=0; i<8; ++i)
                    {
                        cubes[x, next[i, dst, 1], next[i, dst, 0]] = old[x, next[i, src, 1], next[i, src, 0]];
                        cubes[x, next[i, dst, 1], next[i, dst, 0]].transform.parent = cubes[x, 1, 1].transform;
                    }
                    cubes[x, 1, 1].transform.localRotation *= Quaternion.Euler(degree, 0, 0);
                    for (int i = 0; i < 8; ++i)
                        cubes[x, next[i, dst, 1], next[i, dst, 0]].transform.parent = gameObject.transform;
                }
                break;
            case 4:
                {
                    int x = 2;
                    for (int i=0; i<8; ++i)
                    {
                        cubes[x, next[i, dst, 1], next[i, dst, 0]] = old[x, next[i, src, 1], next[i, src, 0]];
                        cubes[x, next[i, dst, 1], next[i, dst, 0]].transform.parent = cubes[x, 1, 1].transform;
                    }
                    cubes[x, 1, 1].transform.localRotation *= Quaternion.Euler(degree, 0, 0);
                    for (int i = 0; i < 8; ++i)
                        cubes[x, next[i, dst, 1], next[i, dst, 0]].transform.parent = gameObject.transform;
                }
                break;
            case 5:// front
                {
                    int z = 0;
                    degree = -degree;

                    for (int i=0; i<8; ++i)
                    {
                        cubes[next[i, dst, 0], next[i, dst, 1], z] = old[next[i, src, 0], next[i, src, 1], z];
                        cubes[next[i, dst, 0], next[i, dst, 1], z].transform.parent = cubes[1, 1, z].transform;
                    }
                    cubes[1, 1, z].transform.localRotation *= Quaternion.Euler(0, 0, degree);
                    for (int i = 0; i < 8; ++i)
                        cubes[next[i, dst, 0], next[i, dst, 1], z].transform.parent = gameObject.transform;
                }
                break;
            case 6:// back
                {
                    int z = 2;
                    degree = -degree;

                    for (int i=0; i<8; ++i)
                    {
                        cubes[next[i, dst, 0], next[i, dst, 1], z] = old[next[i, src, 0], next[i, src, 1], z];
                        cubes[next[i, dst, 0], next[i, dst, 1], z].transform.parent = cubes[1, 1, z].transform;
                    }
                    cubes[1, 1, z].transform.localRotation *= Quaternion.Euler(0, 0, degree);
                    for (int i = 0; i < 8; ++i)
                        cubes[next[i, dst, 0], next[i, dst, 1], z].transform.parent = gameObject.transform;
                }
                break;
        }


    }

    void OnClickUp()
    {
        Rotate(1);
    }
    void OnClickDown()
    {
        Rotate(2);
    }
    void OnClickLeft()
    {
        Rotate(3);
    }
    void OnClickRight()
    {
        Rotate(4);
    }
    void OnClickFront()
    {
        Rotate(5);
    }
    void OnClickBack()
    {
        Rotate(6);
    }

    void ChangeToView()
    {
        isView = !isView;
        if (isView)
        {
            resetButton.gameObject.SetActive(true);
            viewButton.gameObject.SetActive(true);
            opPlane.SetActive(false);
        }
        else
        {
            resetButton.gameObject.SetActive(false);
            viewButton.gameObject.SetActive(false);
            opPlane.SetActive(true);
            transform.localEulerAngles = new Vector3(0, 0, 0);
        }
    }

    void ResetView()
    {
        resetButton.gameObject.SetActive(false);
        transform.Rotate(Vector3.right, 45, Space.World);

    }
    // Use this for initialization
    void Start()
    {
        buttons[0].GetComponent<Button>().onClick.AddListener(OnClickUp);
        buttons[1].GetComponent<Button>().onClick.AddListener(OnClickDown);
        buttons[2].GetComponent<Button>().onClick.AddListener(OnClickLeft);
        buttons[3].GetComponent<Button>().onClick.AddListener(OnClickRight);
        buttons[4].GetComponent<Button>().onClick.AddListener(OnClickFront);
        buttons[5].GetComponent<Button>().onClick.AddListener(OnClickBack);

        viewButton.GetComponent<Button>().onClick.AddListener(ChangeToView);
        resetButton.GetComponent<Button>().onClick.AddListener(ResetView);

        transform.localPosition = new Vector3(0, 0, 0);
        cubes = new GameObject[3, 3, 3];
        for (int i = 0; i < 3; ++i)
            for (int j = 0; j < 3; ++j)
                for (int k = 0; k < 3; ++k)
                {
                    cubes[i, j, k] = CreateCube();
                    cubes[i, j, k].transform.parent = gameObject.transform;
                    cubes[i, j, k].transform.localPosition = new Vector3(i - 1, j - 1, k - 1);
                    cubes[i, j, k].name = i.ToString() + j.ToString() + k.ToString();
                }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            Rotate(1);
        else if (Input.GetKeyDown(KeyCode.S))
            Rotate(2);
        else if (Input.GetKeyDown(KeyCode.A))
            Rotate(3);
        else if (Input.GetKeyDown(KeyCode.D))
            Rotate(4);
        else if (Input.GetKeyDown(KeyCode.Q))
            Rotate(5);
        else if (Input.GetKeyDown(KeyCode.E))
            Rotate(6);

        Input.simulateMouseWithTouches = true;
        if (isView && Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
        {
            float x = Input.GetAxis("Mouse X");
            float y = Input.GetAxis("Mouse Y");

            Vector3 old = transform.localEulerAngles;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                resetButton.gameObject.SetActive(true);
                transform.localEulerAngles = new Vector3(0, old.y - x, 0);
            }
            else
                transform.Rotate(Vector3.right, y, Space.World);
        }

        if (!isView && (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0)))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit casthit;
            if (Physics.Raycast(ray, out casthit))
            {
                ChangeToView();
            }
        }

    }
}
