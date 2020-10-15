using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stack : MonoBehaviour
{
    public ChangeColors changeColorsScript;
    public TileMovement tileMovementScript;
    public Material stackMaterial;
    public GameManager gm;

    private const float BOUNDS_SIZE = 3.5f;
    private const float STACK_MOVING_SPEED = 5f;
    private const float ERROR_MARGIN = 0.1f;
    private const float STACK_BOUNDS_GAIN = 0.25f;
    private const float COMBO_START_GAIN = 3f;

    private GameObject[] theStack;
    private Vector2 stackBounds = new Vector2(BOUNDS_SIZE, BOUNDS_SIZE);
    private int stackIndex;
    private int scoreCount = 0;
    private int combo = 0;


    private float secondaryPosition;
    private bool isMovingX = true;
    private bool GameOver = false;

    private Vector3 desiredPosition;
    private Vector3 lastTilePosition;



    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        theStack = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            theStack[i] = transform.GetChild(i).gameObject;
            changeColorsScript.ColorMesh(theStack[i].GetComponent<MeshFilter>().mesh, scoreCount);

        }
        stackIndex = transform.childCount - 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
            //call game manager restart
            gm.restartGame();

        }

        if (Input.GetMouseButtonDown(0))
        {
            if (PlaceTile())
            {
                SpawnTile();
                scoreCount++;
            }
            else
            {
                //endgame
                GameOver = true;
            }
        }
        tileMovementScript.MoveTile(isMovingX, theStack[stackIndex], scoreCount, secondaryPosition);

        //move stack
        transform.position = Vector3.Lerp(transform.position, desiredPosition, STACK_MOVING_SPEED * Time.deltaTime);
    }


    bool PlaceTile()
    {
        //get the current stack
        Transform t = theStack[stackIndex].transform;
        if (isMovingX)
        {
            float deltaX = lastTilePosition.x - t.position.x;
            if (Mathf.Abs(deltaX) > ERROR_MARGIN)
            {
                combo = 0;
                stackBounds.x -= Mathf.Abs(deltaX);
                if (stackBounds.x <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.x + t.localPosition.x / 2;
                t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);

                //create the part that is fall down
                CreateRubble(
                new Vector3(
                    (t.position.x > 0) ? t.position.x + (t.localScale.x / 2f) : t.position.x - (t.localScale.x / 2f)
                    , t.position.y,
                    t.position.z),
                new Vector3(Mathf.Abs(deltaX), 1f, t.localScale.z));

                t.localPosition = new Vector3(middle - (lastTilePosition.x / 2f), scoreCount, lastTilePosition.z);

            }
            else
            {
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.x += STACK_BOUNDS_GAIN;
                    if (stackBounds.x > BOUNDS_SIZE)
                    {
                        stackBounds.x = BOUNDS_SIZE;
                    }

                    float middle = lastTilePosition.x + t.localPosition.x / 2f;
                    t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);
                    t.localPosition = new Vector3(middle - (lastTilePosition.x / 2f), scoreCount, lastTilePosition.z);

                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }
        else
        {
            float deltaZ = lastTilePosition.z - t.position.z;
            if (Mathf.Abs(deltaZ) > ERROR_MARGIN)
            {
                combo = 0;
                stackBounds.y -= Mathf.Abs(deltaZ);
                if (stackBounds.y <= 0)
                {
                    return false;
                }

                float middle = lastTilePosition.z + t.localPosition.z / 2f;
                t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);

                CreateRubble(new Vector3(
                    t.position.x,
                    t.position.y,
                    (t.position.z > 0) ? t.position.z + (t.localScale.z / 2f) : t.position.z - (t.localScale.z / 2f)),
                    new Vector3(t.localScale.x, 1f, Mathf.Abs(deltaZ))
                    );

                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, middle - (lastTilePosition.z / 2f));
            }
            else
            {
                if (combo > COMBO_START_GAIN)
                {
                    stackBounds.y += COMBO_START_GAIN;

                    if (stackBounds.y > BOUNDS_SIZE)
                    {
                        stackBounds.y = BOUNDS_SIZE;
                    }

                    float middle = lastTilePosition.z + t.localPosition.z / 2f;
                    t.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);
                    t.localPosition = new Vector3(lastTilePosition.x / 2f, scoreCount, middle - (lastTilePosition.z / 2f));
                }
                combo++;
                t.localPosition = new Vector3(lastTilePosition.x, scoreCount, lastTilePosition.z);
            }
        }//else moving on z
        secondaryPosition = (isMovingX) ? t.localPosition.x : t.localPosition.z;
        isMovingX = !isMovingX;
        return true;
    }//place tile

    void CreateRubble(Vector3 pos, Vector3 scale)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localPosition = pos;
        go.transform.localScale = scale;
        go.AddComponent<Rigidbody>();

        go.GetComponent<MeshRenderer>().material = stackMaterial;
        changeColorsScript.ColorMesh(go.GetComponent<MeshFilter>().mesh, scoreCount);
    }//create rubble

    void SpawnTile()
    {
        lastTilePosition = theStack[stackIndex].transform.localPosition;
        stackIndex--;

        if (stackIndex < 0)
        {
            stackIndex = transform.childCount - 1;
        }
        desiredPosition = Vector3.down * scoreCount;
        theStack[stackIndex].transform.localPosition = new Vector3(0f, scoreCount, 0f);
        theStack[stackIndex].transform.localScale = new Vector3(stackBounds.x, 1f, stackBounds.y);

        changeColorsScript.ColorMesh(theStack[stackIndex].GetComponent<MeshFilter>().mesh, scoreCount);
    }

    public int getScoreCount()
    {
        return scoreCount;
    }
}//end of main class
