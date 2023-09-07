using PetShopAPIV2.DTOs.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetShopAPIV2.Tests.Builders
{
    public class AnimalUpdateDTOBuilder
    {
        private AnimalUpdateDTO AnimalInsertDTO;
        private AnimalUpdateDTOBuilder()
        {
            AnimalInsertDTO = new AnimalUpdateDTO();
        }

        public static AnimalUpdateDTOBuilder Create()
        {
            return new AnimalUpdateDTOBuilder();
        }

        public AnimalUpdateDTOBuilder WithName(string name)
        {
            AnimalInsertDTO.Name = name;
            return this;
        }

        public AnimalUpdateDTO Get()
        {
            return AnimalInsertDTO;
        }
    }
}
