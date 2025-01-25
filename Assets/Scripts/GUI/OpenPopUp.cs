using System.Collections.Generic;
using UnityEngine;

public class OpenPopUp : MonoBehaviour
{
    private static List<OpenPopUp> instance = new List<OpenPopUp>();

    public OpenPopUp parent;

    void Awake()
    {
        OpenPopUp.instance.Add(this);
    }

    public void Open()
    {
        for(int i = 0; i < OpenPopUp.instance.Count; i++)
        {
            instance[i].Close();
        }

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Close(bool openParent)
    {
        Close();
        if(openParent && parent)
        {
            parent.Open();
        }
    }

    void OnDestroy()
    {
        OpenPopUp.instance.Remove(this);
    }
}
