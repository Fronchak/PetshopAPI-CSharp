using PetShopAPIV2.DTOs.Animals;

namespace PetShopAPIV2.Interfaces
{
    public interface IAnimalService
    {
        Task<AnimalDTO> SaveAsync(AnimalInsertDTO animalInsertDTO);

        Task<ICollection<AnimalDTO>> FindAllAsync();

        Task<AnimalDTO> FindByIdAsync(int id);

        Task<AnimalDTO> UpdateAsync(AnimalUpdateDTO animalUpdateDTO, int id);

        Task DeleteByIdAsync(int id);
    }
}
