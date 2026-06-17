using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    public void GenerateDeadDrop(bool _isPositionGrounded)
    {
        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0,100) < possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for (int i = 0;i < possibleItemDrop; i++)
        {
            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];

            int dropDir = Random.Range(0, 1) * 2 - 1;
            dropList.Remove(randomItem);

            randomItem = Instantiate(randomItem);

            randomItem = SetupEquipmentEffect(randomItem);

            DropItem(randomItem, dropDir, _isPositionGrounded);
        }
    }

    public ItemData SetupEquipmentEffect(ItemData _item)
    {
        if (_item.itemType != ItemType.Equipment)
            return _item;

        ItemData_Equipment equipment = _item as ItemData_Equipment;
        QualityType qualityType = equipment.quality;

        if (qualityType == QualityType.White)
            return _item;

        int effectCount = 0;
        switch (qualityType)
        {
            case QualityType.Blue:  effectCount = 1; break;
            case QualityType.Purple:effectCount = 2; break;
            case QualityType.Gold:  effectCount = 3; break;
        }

        List<EffectType> effectTypes = Inventory.instance.GetEffectTypes();

        for (int i = 0; i < effectTypes.Count; i++) //使用Fisher-Yates方法进行词条类型的随机排列,保证每位至少进行一次重新排列
        {
            int rand = Random.Range(i, effectTypes.Count);
            (effectTypes[i], effectTypes[rand]) = (effectTypes[rand], effectTypes[i]);  
        }

        for (int i = 0; i < effectCount; i++)
        {
            ItemEffect_Equipment originalEffect = Inventory.instance.GetRandomEffect(effectTypes[i]);
            ItemEffect_Equipment newEffect = Instantiate(originalEffect); //重新实例化(克隆)，防止污染原词条数值

            newEffect.InitEffectValue(qualityType);     //根据品质设置词条效果的数值 

            equipment.itemEffects.Add(newEffect);  //!在装备中显示词条，后续需要删除
        }

        return equipment;
    }   //在掉落物品前，设置装备的词条

    public void DropItem(ItemData _itemData, int _dropDir, bool _isPositionGrounded)
    {
        Vector2 newPosition = new Vector2(transform.position.x, transform.position.y + 1f);

        GameObject newDrop = Instantiate(dropPrefab, newPosition, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(2,4) * _dropDir, Random.Range(7,9));

        ItemObject dropItemObject = newDrop.GetComponent<ItemObject>();
        dropItemObject.SetupItem(_itemData, randomVelocity);
        dropItemObject.StartCoroutine(dropItemObject.CheckGrounded(transform.position, _isPositionGrounded, 3f));
    }
}
