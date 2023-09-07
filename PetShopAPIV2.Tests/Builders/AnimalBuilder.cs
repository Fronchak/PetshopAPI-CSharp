using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Builders
{
    public class AnimalBuilder
    {
        private Animal Animal;
        private AnimalBuilder() 
        {
            Animal = new Animal();
        }

        public static AnimalBuilder Create()
        {
            return new AnimalBuilder();
        }

        public AnimalBuilder WithId(int id)
        {
            Animal.Id = id;
            return this;
        }

        public AnimalBuilder WithName(string name)
        {
            Animal.Name = name;
            return this;
        }

        public Animal Get()
        {
            return Animal;
        }
    }
}
