using PetShopAPIV2.DTOs.Animals;
using PetShopAPIV2.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Builders
{
    public class AnimalDTOBuilder
    {
        private AnimalDTO Animal;
        private AnimalDTOBuilder()
        {
            Animal = new AnimalDTO();
        }

        public static AnimalDTOBuilder Create()
        {
            return new AnimalDTOBuilder();
        }

        public AnimalDTOBuilder WithId(int id)
        {
            Animal.Id = id;
            return this;
        }

        public AnimalDTOBuilder WithName(string name)
        {
            Animal.Name = name;
            return this;
        }

        public AnimalDTO Get()
        {
            return Animal;
        }
    }
}
