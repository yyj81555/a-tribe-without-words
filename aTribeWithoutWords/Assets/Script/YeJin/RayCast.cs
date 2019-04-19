using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class RayCast : MonoBehaviour
{
    //rayCast
    [SerializeField]
    private float rayLength;

    public string hitname = null;

    Variable variable;
    NPCMove npcmove;

    public GameObject prefab;

    public int ChooseCount = 2;

    // Start is called before the first frame update
    void Start()
    {
        variable = GameObject.Find("ItemList").GetComponent<Variable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, rayLength))
            {
                Debug.Log(hit.collider.name);
                hitname = hit.collider.name;

                if (hit.transform.gameObject.tag == "Worker")
                {
                    if (GameObject.Find(hitname + "Effect") == null && variable.EffectCount < ChooseCount)
                    {
                      GameObject obj = Instantiate(prefab, new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y + 2, hit.collider.gameObject.transform.position.z), Quaternion.identity);
                      obj.name = hitname + "Effect";
                      npcmove = GameObject.Find(hitname).GetComponent<NPCMove>();
                      npcmove.RePos();
                    }
                }

                else if(hit.transform.gameObject.tag == "FruitFarm")
                {
                    for (int i = 1; i <= 3; i++)
                    {

                        npcmove = GameObject.Find("Worker" + i).GetComponent<NPCMove>();

                        if (variable.chooseNPC[i] == 1 && npcmove.NPCState == 1)
                        {
                            npcmove.CommandState = 1;
                            npcmove.IndicationPos();
                        }
                    }
                }
                    
                else if (hit.transform.gameObject.tag == "Mine")
                {
                    if (variable.EffectCount >= 2)
                    {
                        Debug.Log("수행인원이 너무 많습니다.");

                        for (int i = 1; i <= 3; i++)
                        {
                            npcmove = GameObject.Find("Worker" + i).GetComponent<NPCMove>();
                            npcmove.standardMode();
                            Destroy(GameObject.Find("Worker" + i + "Effect"));
                        }
                    }
                    else
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            npcmove = GameObject.Find("Worker" + i).GetComponent<NPCMove>();

                            if (variable.chooseNPC[i] == 1 && npcmove.NPCState == 1)
                            {
                                npcmove.CommandState = 2;
                                npcmove.IndicationPos();
                            }
                        }
                    }
                }   
            } 
        }
    }
}
