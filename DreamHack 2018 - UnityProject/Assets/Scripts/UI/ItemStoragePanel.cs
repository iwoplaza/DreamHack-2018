using Game.Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Game.UI
{
    public class ItemStoragePanel : MonoBehaviour
    {
        [SerializeField] Text m_metalAmountText;

        /// <summary>
        /// Called by <see cref="GameHUD"/>
        /// </summary>
        public void Setup()
        {
            ItemStorage storage = WorldController.Instance.MainState.ItemStorage;
            storage.RegisterOnChangedHandler(OnStorageChanged);

            ItemStack stackOfMetal = storage.StackOf(Item.METAL);
            if(stackOfMetal != null)
                m_metalAmountText.text = "" + stackOfMetal.Amount;
        }

        public void OnStorageChanged(ItemStorage storage, ItemStack stack)
        {
            m_metalAmountText.text = "" + stack.Amount;
        }
    }
}