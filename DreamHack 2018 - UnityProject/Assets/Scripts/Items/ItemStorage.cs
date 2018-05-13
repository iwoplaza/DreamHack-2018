using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Game.Items
{
    public class ItemStorage
    {
        public delegate void OnChangedHandler(ItemStorage itemStorage, ItemStack stack);
        OnChangedHandler m_onChangedHandlers;
        public List<ItemStack> ItemStacks { get; private set; }

        public ItemStorage()
        {
            ItemStacks = new List<ItemStack>();
        }

        public void Parse(XElement element)
        {
            ItemStacks = new List<ItemStack>();

            IEnumerable<XElement> itemStackElements = element.Elements("ItemStack");
            foreach(XElement itemStackElement in itemStackElements)
            {
                ItemStack itemStack = ItemStack.CreateAndParse(itemStackElement);
                if(itemStack != null)
                    ItemStacks.Add(itemStack);
            }
        }

        public void Populate(XElement element)
        {
            foreach (ItemStack itemStack in ItemStacks)
            {
                XElement itemStackElement = new XElement("ItemStack");
                itemStack.Populate(itemStackElement);
                element.Add(itemStackElement);
            }
        }

        public ItemStack StackOf(Item item)
        {
            foreach (ItemStack itemStack in ItemStacks)
            {
                if (itemStack.Item == item)
                    return itemStack;
            }
            return null;
        }

        public bool Has(int amount, Item item)
        {
            ItemStack itemStack = StackOf(item);
            if(itemStack != null)
            {
                if (itemStack.Amount >= amount)
                    return true;
            }
            return false;
        }

        public bool Consume(int amount, Item item)
        {
            if(Has(amount, item))
            {
                ItemStack itemStack = StackOf(item);
                if (itemStack != null)
                {
                    itemStack.Amount -= amount;
                    if (m_onChangedHandlers != null)
                        m_onChangedHandlers(this, itemStack);
                    return true;
                }
            }
            return false;
        }

        public void Add(int amount, Item item)
        {
            ItemStack itemStack = StackOf(item);
            if (itemStack != null)
            {
                itemStack.Amount += amount;
                if (m_onChangedHandlers != null)
                    m_onChangedHandlers(this, itemStack);
            }
        }

        public void RegisterOnChangedHandler(OnChangedHandler handler)
        {
            m_onChangedHandlers += handler;
        }
    }
}