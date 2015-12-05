using UnityEngine;
using System.Collections;

public class BuilingInfo {

    public int price;
    public string male;
    public string female;
    public string resource;
    public string button;
    public string groupName;

    public BuilingInfo(int price, string male, string female, string resource, string button, string groupName)
    {
        this.price = price;
        this.male = male;
        this.female = female;
        this.resource = resource;
        this.button = button;
        this.groupName = groupName;
    }
}
