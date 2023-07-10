using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EF
{
    internal class EntityManager: QueryBuilder
    {
        public QueryBuilder find(Entity entity)
        {
            Console.WriteLine(entity);

            return this;
        }

        public QueryBuilder set(Entity entity, Dictionary<string, string> values)
        {
            Console.WriteLine($"{entity} {values}");

            return this;
        }

        public QueryBuilder where(Dictionary<string, string> values)
        {
            Console.WriteLine($"{values}");

            return this;
        }
    }
}
