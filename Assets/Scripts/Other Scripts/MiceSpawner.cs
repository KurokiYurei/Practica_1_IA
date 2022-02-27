using UnityEngine;
using UnityEngine.UI;
//using FSM;

public class MiceSpawner : MonoBehaviour
{

    private GameObject sample;
    //private MICE_GLOBAL_Blackboard globalBlackboard;

    public int numInstances = 10;
    public float interval = 5f; // one ant every interval seconds
    public bool spyOne = false;

    private int generated = 0;
    private float elapsedTime = 0f; // time elapsed since last generation

    /*
    void Start()
    {
        sample = Resources.Load<GameObject>("MOUSE");
        if (sample == null)
            Debug.LogError("No MOUSE prefab found as a resource");

        //globalBlackboard = GetComponent<MICE_GLOBAL_Blackboard>();
        //if (globalBlackboard == null)
        //    globalBlackboard = gameObject.AddComponent<MICE_GLOBAL_Blackboard>();

    }

    // Update is called once per frame
    void Update()
    {

        GameObject clone;

        // spawn on click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 click = Input.mousePosition;
            Vector3 position = Camera.main.ScreenToWorldPoint(new Vector3(click.x, click.y, 1f));
            position.z = 0f;
            clone = Instantiate(sample);
            clone.transform.position = position;

            // give the global blackboard to the clone
            FSM_MOUSE_FEED fsm = clone.GetComponent<FSM_MOUSE_FEED>();
            if (fsm != null)
            {
                fsm.globalBlackboard = globalBlackboard;
            }
            else
            {
                Debug.Log("spawned mouse on click has no FSM_MOUSE_FEED");
            }
        }


        if (generated == numInstances)
            return;


        if (elapsedTime >= interval)
        {
            // spawn creating an instance...
            clone = Instantiate(sample);
            clone.transform.position = this.transform.position;

            // give the global blackboard to the clone
            FSM_MOUSE_FEED fsm = clone.GetComponent<FSM_MOUSE_FEED>();
            if (fsm != null)
            {
                fsm.globalBlackboard = globalBlackboard;
            }
            else
            {
                Debug.Log("spawned mouse has no FSM_MOUSE_FEED");
            }

            generated++;
            elapsedTime = 0;


            // first mouse is "special"
            if (generated == 1 && spyOne)
            {
                StateSpy sp = clone.AddComponent<StateSpy>();
                sp.fsm = fsm;
                sp.caption = "FSM Feed state";

                Text[] texts = Object.FindObjectsOfType<Text>();
                foreach (Text t in texts)
                {
                    if (t.name == "FSMFeedState")
                    {
                        sp.text = t;
                        break;
                    }
                }

                FSM_MOUSE_AWARE fsmA = clone.GetComponent<FSM_MOUSE_AWARE>();
                sp = clone.AddComponent<StateSpy>();
                sp.fsm = fsmA;
                sp.caption = "FSM Aware state";

                texts = Object.FindObjectsOfType<Text>();
                foreach (Text t in texts)
                {
                    if (t.name == "FSMAwareState")
                    {
                        sp.text = t;
                        break;
                    }
                }

                clone.AddComponent<TrailRenderer>();
            }


        }
        else
        {
            elapsedTime += Time.deltaTime;
        }

    }
    */
}

