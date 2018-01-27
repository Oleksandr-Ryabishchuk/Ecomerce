using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFThingRepository: IThingRepository
    {
        EFDbContext context = new EFDbContext();
        public IEnumerable<Thing> Things
        {
            get { return context.Things; }
        }

        public void SaveThing(Thing thing)
        {
            if(thing.ThingId == 0)
            {
                context.Things.Add(thing);
            }
            else
            {
                Thing dbEntry = context.Things.Find(thing.ThingId);
                if(dbEntry != null)
                {
                    dbEntry.Name = thing.Name;
                    dbEntry.Kind = thing.Kind;
                    dbEntry.Material = thing.Material;
                    dbEntry.Description = thing.Description;
                    dbEntry.Producer = thing.Producer;
                    dbEntry.Articul = thing.Articul;
                    dbEntry.Price = thing.Price;
                }

            }
            context.SaveChanges();
        }
    }
}
