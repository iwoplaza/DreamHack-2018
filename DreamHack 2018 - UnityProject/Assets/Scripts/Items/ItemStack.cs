using System.Xml.Linq;

namespace Game.Items
{
    public class ItemStack
    {
        private int m_amount;

        public Item Item { get; private set; }
        public int Amount
        {
            get
            {
                return m_amount;
            }
            set
            {
                m_amount = value;
            }
        }

        public ItemStack(Item item, int amount)
        {
            Item = item;
            Amount = amount;
        }

        public void Populate(XElement element)
        {
            element.SetAttributeValue("itemId", Item.Identifier);
            element.SetAttributeValue("amount", Amount);
        }

        public static ItemStack CreateAndParse(XElement element)
        {
            XAttribute itemIdAttrib = element.Attribute("itemId");
            XAttribute amountAttrib = element.Attribute("amount");

            if (itemIdAttrib != null && amountAttrib != null)
            {
                Item item = Item.Get(int.Parse(itemIdAttrib.Value));
                int amount = int.Parse(amountAttrib.Value);
                ItemStack itemStack = new ItemStack(item, amount);
                return itemStack;
            }
            return null;
        }
    }
}