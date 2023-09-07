using PetShopAPIV2.DTOs.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Builders
{
    public class AnimalInsertDTOBuilder
    {
        private AnimalInsertDTO AnimalInsertDTO;
        private AnimalInsertDTOBuilder() 
        {
            AnimalInsertDTO = new AnimalInsertDTO();
        }

        public static AnimalInsertDTOBuilder Create()
        {
            return new AnimalInsertDTOBuilder();
        }

        public AnimalInsertDTOBuilder WithName(string name)
        {
            AnimalInsertDTO.Name = name;
            return this;
        }

        public AnimalInsertDTO Get()
        {
            return AnimalInsertDTO;
        }
    }
}
