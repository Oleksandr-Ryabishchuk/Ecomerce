using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines { get { return lineCollection; } }

        public void AddItem(Thing thing, int quantity)
        {
            CartLine line = lineCollection
                .Where(b => b.Thing.ThingId == thing.ThingId)
                .FirstOrDefault();
            if(line == null)
            {
                lineCollection.Add(new CartLine { Thing = thing, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }

        public void RemoveLine(Thing thing)
        {
            lineCollection.RemoveAll(l => l.Thing.ThingId == thing.ThingId);
        }
        public decimal ComputeTotalValue()
        {
            return lineCollection.Sum(e => e.Thing.Price * e.Quantity);
        }
        public void Clear()
        {
            lineCollection.Clear();
        }

    }

    public class CartLine
    {
        public Thing Thing { get; set; }
        public int Quantity { get; set; }
    }

}

