using System.Collections;
using System.Collections.Generic;
using Photon.Pun; // 유니티용 포톤 컴포넌트들
using UnityEngine;

public class Inventory : MonoBehaviourPunCallbacks
{
    public List<Item> inventory = new List<Item>();
    // 인벤토리를 리스트로 만듭니다.
    private itemDatabase db;
    // 아이템 데이터베이스는 db로 축약해서 사용합니다.

    public int slotX, slotY;    // 인벤토리 가로 세로 속성 설정 위한 변수
    public List<Item> slots = new List<Item>(); // 인벤토리 속성 변수

    public GUISkin skin;

    private bool showTooltip;
    private string tooltip;
    // 툴팁 추가를 위한 부울 변수와 스트링 변수

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < slotX * slotY; i++)
        {
            slots.Add(new Item());
            // 아이템 슬롯칸에 빈 오브젝트 추가하기
            inventory.Add(new Item());
            // 인벤토리에 추가
        }
        db = GameObject.FindGameObjectWithTag("Item Database").GetComponent<itemDatabase>();
        // 디비 변수에 "Item Database" 태그를 가진 오브젝트를 연결합니다.
        // 그리고 그 중 가져오는 컴포넌트는 "itemDatabse"라는 속성입니다.

        AddItem(1001);
    }

    void Update()
    {

    }
    void OnGUI()
    {
        tooltip = "";
        GUI.skin = skin;
        DrawInventory();
        if (showTooltip)
        {
            GUI.Box(new Rect(Event.current.mousePosition.x + 5, Event.current.mousePosition.y + 2, 400, 50), tooltip, skin.GetStyle("tooltip"));
            // 아이템 설명창을 마우스의 좌표에 컨트롤 되게 설정하였으며, GUI skin을 응용하여 설정하였음
        }

    }

    void DrawInventory()
    {
        int k = 0;
        for (int j = 0; j < slotY; j++)
        {

            for (int i = 0; i < slotX; i++)
            {
                Rect slotRect = new Rect(i * 52 + 100, j * 52 + 1000, 50, 50);
                // 박스 분할하기
                GUI.Box(slotRect, "", skin.GetStyle("slot background"));
                // 각 박스의 생성 위치를 설정해주는 곳입니다. skin.GetStyle은 이전에 만들었던 skin을 불러오는 것임

                // 기능 추가하기
                slots[k] = inventory[k];
                if (slots[k].itemName != null)
                {
                    GUI.DrawTexture(slotRect, slots[k].itemIcon);
                    if (slotRect.Contains(Event.current.mousePosition))
                    // 만약 마우스가 해당 인벤토리 창-버튼-위로 올라온다면,
                    {
                        tooltip = CreateTooltip(slots[i]);
                        // 툴팁 만드는 함수를 실행하며,
                        // 보내는 속성은 i번째 슬롯입니다.
                        showTooltip = true;
                        // 툴팁을 만들고, showTooltip을 true로 만들어서 활성화 시켜줍니다.
                    }
                    //if(Event.current.type == EventType.KeyDown)
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        useItem(0);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        useItem(1);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha3))
                    {
                        useItem(2);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha4))
                    {
                        useItem(3);
                    }
                    else if (Input.GetKeyDown(KeyCode.Alpha5))
                    {
                        useItem(4);
                    }
                }
                if (tooltip == "")
                {
                    showTooltip = false;
                }

                k++;
                // 갯수 증가
            }
        }
    }

    void useItem(int k)
    {
        switch (inventory[k].itemID)
        // 인벤토리의 아이템ID가 각각의 상태라면,
        {
            case 1001:
                // 1000~1009대은 HP 포션
                Debug.Log("heal +50");
                break;
            case 1011:
                // 1010~1019는 SP 포션
                Debug.Log("increase sight + 1");
                break;
            default:
                // 그 밖의 use 아이템은 아직 활성화가 안됨
                Debug.Log("I don't know, what is use this?");
                break;
        }
        inventory[k] = new Item();
    }

    string CreateTooltip(Item item)
    {
        tooltip = "Item name: <color=#a10000><b>" + item.itemName + "</b></color>\nItem description: <color=#a10000>" + item.itemDes+ "</color>";
        /* html 태그가 어느정도 먹힘
         * <color=#000000> 말 </color>
         * <b> 두껍게 </b>
         * ... emdemdemd
         */

        return tooltip;
    }

    public void AddItem(int id)
    // 본 함수에서 id를 받아서
    {
        for (int i = 0; i < inventory.Count; i++)
        // 전체 인벤토리를 모두 찾습니다
        {

            if (inventory[i].itemName == null)
            // 인벤토리가 빈자리면 
            {
                for (int j = 0; j < db.items.Count; j++)
                // 추가한 값까지 모두 찾은 다음에
                {
                    if (db.items[j].itemID == id)
                    {
                        // 디비의 아이템의 ID와 입력한 ID가 같다면,
                        inventory[i] = db.items[j];
                        // 빈 인벤토리에 db에 저장된 아이템을 적용하고
                        return;
                        // 함수를 마무리합니다.
                    }
                }
            }
        }
    }

    //bool inventoryContains(int id)
    //{
    //    for (int i = 0; i < inventory.Count; i++)
    //    {
    //        return (inventory[i].itemID == id);
    //    }
    //    return false;
    //}
}