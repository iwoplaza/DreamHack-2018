using System.Collections.Generic;
using System.Xml.Linq;

namespace Game.Items
{
    public class ItemStorage
    {
        public List<ItemStack> ItemStacks { get; private set; }

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
    }
}