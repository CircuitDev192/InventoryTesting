﻿using UnityEngine;
using UnityEngine.UI;
using CircuitDev.CharacterStats;

public class Character : MonoBehaviour
{
    public CharacterStat Strength;
    public CharacterStat Agility;
    public CharacterStat Intelligence;
    public CharacterStat Vitality;

    [SerializeField] private Inventory inventory;
    [SerializeField] private EquipmentPanel equipmentPanel;
    [SerializeField] private StatPanel statPanel;
    [SerializeField] private ItemTooltip itemTooltip;
    [SerializeField] private Image draggableItem;

    private ItemSlot draggedSlot;

    private void OnValidate()
    {
        if (itemTooltip == null)
        {
            itemTooltip = FindObjectOfType<ItemTooltip>();
        }
    }

    private void Awake()
    {
        statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
        statPanel.UpdateStatValues();

        //Setup Events:
        //Right Click
        inventory.OnRightClickEvent += Equip;
        equipmentPanel.OnRightClickEvent += Unequip;
        //Pointer Enter
        inventory.OnPointerEnterEvent += ShowTooltip;
        equipmentPanel.OnPointerEnterEvent += ShowTooltip;
        //Pointer Exit
        inventory.OnPointerExitEvent += HideTooltip;
        equipmentPanel.OnPointerExitEvent += HideTooltip;
        //Begin Drag
        inventory.OnBeginDragEvent += BeginDrag;
        equipmentPanel.OnBeginDragEvent += BeginDrag;
        //End Drag
        inventory.OnEndDragEvent += EndDrag;
        equipmentPanel.OnEndDragEvent += EndDrag;
        //Drag
        inventory.OnDragEvent += Drag;
        equipmentPanel.OnDragEvent += Drag;
        //Drop
        inventory.OnDropEvent += Drop;
        equipmentPanel.OnDropEvent += Drop;
    }

    private void Equip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if(equippableItem != null)
        {
            Equip(equippableItem);
        }
    }

    private void Unequip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            Unequip(equippableItem);
        }
    }

    private void ShowTooltip(ItemSlot itemSlot)
    {
        EquippableItem equippableItem = itemSlot.Item as EquippableItem;
        if (equippableItem != null)
        {
            itemTooltip.ShowToolTip(equippableItem);
        }
    }

    private void HideTooltip(ItemSlot itemSlot)
    {
        itemTooltip.HideToolTip();
    }

    private void BeginDrag(ItemSlot itemSlot)
    {
        if(itemSlot.Item != null)
        {
            draggedSlot = itemSlot;
            draggableItem.sprite = itemSlot.Item.Icon;
            draggableItem.transform.position = Input.mousePosition;
            draggableItem.enabled = true;
        }
    }

    private void EndDrag(ItemSlot itemSlot)
    {
        draggedSlot = null;
        draggableItem.enabled = false;
    }

    private void Drag(ItemSlot itemSlot)
    {
        if (draggableItem.enabled)
        {
            draggableItem.transform.position = Input.mousePosition;
        }
    }

    private void Drop(ItemSlot dropItemSlot)
    {
        if (draggedSlot == null) return;

        if (dropItemSlot.CanReceiveItem(draggedSlot.Item) && draggedSlot.CanReceiveItem(dropItemSlot.Item))
        {
            EquippableItem dragItem = draggedSlot.Item as EquippableItem;
            EquippableItem dropItem = dropItemSlot.Item as EquippableItem;

            if (draggedSlot is EquipmentSlot)
            {
                if (dragItem != null) dragItem.Unequip(this);
                if (dropItem != null) dropItem.Equip(this);
            }
            if (dropItemSlot is EquipmentSlot)
            {
                if (dragItem != null) dragItem.Equip(this);
                if (dropItem != null) dropItem.Unequip(this);
            }

            statPanel.UpdateStatValues();

            Item draggedItem = draggedSlot.Item;
            draggedSlot.Item = dropItemSlot.Item;
            dropItemSlot.Item = draggedItem;
        }
    }

    public void Equip(EquippableItem item)
    {
        if (inventory.RemoveItem(item))
        {
            EquippableItem previousItem;
            if (equipmentPanel.AddItem(item, out previousItem))
            {
                if (previousItem != null)
                {
                    inventory.AddItem(previousItem);
                    previousItem.Unequip(this);
                    statPanel.UpdateStatValues();
                }
                item.Equip(this);
                statPanel.UpdateStatValues();
            }
            else
            {
                inventory.AddItem(item);
            }
        }
    }

    public void Unequip(EquippableItem item)
    {
        if (!inventory.IsFull() && equipmentPanel.RemoveItem(item))
        {
            item.Unequip(this);
            statPanel.UpdateStatValues();
            inventory.AddItem(item);
        }
    }
}
