using Schema.Protobuf;
using Schema.Protobuf.Message.Authentication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownEvent : MonoBehaviour
{
    public Dropdown dropdown;

    void Awake()
    {
    }

    public static long SelectedIdx { get; set; } = 0;

    public void OnValueChanged(Dropdown dropdown)
    {
        Debug.Log(dropdown.options[dropdown.value].text);
        SelectedIdx = long.Parse(dropdown.options[dropdown.value].text);
    }

    public void AddCharacter(long idx)
    {
        if (idxList.Contains(idx) == true)
        {
            return;
        }

        Dropdown.OptionData option = new Dropdown.OptionData();
        option.text = idx.ToString();
        dropdown.options.Add(option);
        idxList.Add(idx);
        
        if (SelectedIdx == 0)
        {
            SelectedIdx = idx;
        }
    }

    public List<long> idxList = new List<long>();

    public static DropdownEvent GetDropdownComponent()
    {
        return GameObject.Find("Dropdown").GetComponent<DropdownEvent>();
    }
}
