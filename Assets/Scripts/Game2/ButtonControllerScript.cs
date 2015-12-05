using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class ButtonControllerScript : MonoBehaviour
{
    public GameObject interfacePrefab;
    public Camera mainCamera;
    public Queue<BuilingInfo> farmInfo = new Queue<BuilingInfo>();
    public Transform mainPanel;

    void Awake()
    {
        var info1 = new BuilingInfo(100, "Rooster", "Chicken", "Eggs", "Buy Cow place\n(200g)", "ChickenCoop");
        farmInfo.Enqueue(info1);
        var info2 = new BuilingInfo(10, "Ox", "Cow", "Milk", "", "CowPlace");
        farmInfo.Enqueue(info2);
    }

    public void MoveMainCameraTo(CameraInfo info)
    {
        var camScript = mainCamera.GetComponent<CameraControllerScript>();
        camScript.minMapX = info.minMapX;
        camScript.maxMapX = info.maxMapX;
        camScript.minMapY = info.minMapY;
        camScript.maxMapY = info.maxMapY;
        camScript.camSpeed = info.camSpeed;
        camScript.minZoom = info.minZoom;
        camScript.maxZoom = info.maxZoom;
        mainCamera.transform.position = info.gameObject.transform.position;
    }

    public void ExpandFarm(RectTransform transform)
    {
        Debug.Log(farmInfo.Peek().price);
        if (farmInfo.Peek().price <= Game2ControllerScript.gold)
        {
            var info = farmInfo.Dequeue();
            Game2ControllerScript.gold -= info.price;
            Game2ControllerScript.updateGold();
            var UI = (GameObject)Instantiate(interfacePrefab, transform.localPosition, Quaternion.identity);
            UI.transform.SetParent(mainPanel, false);
            var children = UI.GetComponentsInChildren<Transform>();
            Destroy(transform.gameObject);
            var scripts = ((GameObject)Resources.Load("SpacesInfo/" + info.groupName)).GetComponents<ItemScript>();
            ItemScript maleScript = null, femaleScript = null, resourceScript = null;
            GameObject male = null, female = null;
            for (int i = 0; i < children.Length; i++)
            {
                switch (children[i].tag)
                {
                    case "Male":
                        var maleScriptType = scripts[0].GetType();
                        children[i].gameObject.AddComponent(maleScriptType);
                        maleScript = children[i].GetComponent<ItemScript>();
                        male = children[i].gameObject;
                        break;
                    case "Female":
                        var femaleScriptType = scripts[1].GetType();
                        children[i].gameObject.AddComponent(femaleScriptType);
                        femaleScript = children[i].GetComponent<ItemScript>();
                        female = children[i].gameObject;
                        femaleScript.parents.Add(male);
                        femaleScript.parents.Add(female);
                        break;
                    case "Resource":
                        var resourceScriptType = scripts[2].GetType();
                        children[i].gameObject.AddComponent(resourceScriptType);
                        resourceScript = children[i].GetComponent<ItemScript>();
                        resourceScript.parents.Add(female);
                        break;
                    case "Name":
                        var text = children[i].GetComponent<Text>();
                        if (children[i].parent.CompareTag("Male"))
                            text.text = info.male;
                        else if (children[i].parent.CompareTag("Female"))
                            text.text = info.female;
                        else if (children[i].parent.CompareTag("Resource"))
                            text.text = info.resource;
                        break;
                    case "BuyButton":
                        var buyButton = children[i].GetComponent<Button>();
                        if (children[i].parent.CompareTag("Male"))
                        {
                            buyButton.onClick.AddListener(() => maleScript.BuyItem());
                        }
                        else if (children[i].parent.CompareTag("Female"))
                        {
                            buyButton.onClick.AddListener(() => femaleScript.BuyItem());
                        }
                        else if (children[i].parent.CompareTag("Resource"))
                        {
                            buyButton.onClick.AddListener(() => resourceScript.BuyItem());
                        }
                        break;
                    case "SellButton":
                        var sellButton = children[i].GetComponent<Button>();
                        if (children[i].parent.CompareTag("Male"))
                        {
                            sellButton.onClick.AddListener(() => maleScript.SellItem());
                        }
                        else if (children[i].parent.CompareTag("Female"))
                        {
                            sellButton.onClick.AddListener(() => femaleScript.SellItem());
                        }
                        else if (children[i].parent.CompareTag("Resource"))
                        {
                            sellButton.onClick.AddListener(() => resourceScript.SellItem());
                        }
                        break;
                    case "BuildingPlace":
                        if (farmInfo.Count > 0)
                        {
                            var nextPlace = (RectTransform) children[i].transform;
                            var button = children[i].GetComponentInChildren<Button>();
                            button.onClick.AddListener(() => ExpandFarm(nextPlace));
                            var textButton = button.GetComponentInChildren<Text>();
                            textButton.text = info.button; 
                        }
                        else
                        {
                            Destroy(children[i].gameObject);
                        }
                        break;
                }
            }
        }
    }
}
