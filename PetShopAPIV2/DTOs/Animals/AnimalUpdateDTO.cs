using Microsoft.AspNetCore.Http;
using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Animals
{
    public class AnimalUpdateDTO : IValidatableObject
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required")]
        public string Name { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();
            IAnimalRepository animalRepository = validationContext.GetRequiredService<IAnimalRepository>();
            IHttpContextAccessor accessor = validationContext.GetRequiredService<IHttpContextAccessor>();
            HttpContext context = accessor.HttpContext;
            RouteData routeData = context.GetRouteData();
            RouteValueDictionary routeValues = routeData.Values;
            object idObj = routeValues.GetValueOrDefault("id");
            int? id = null;
            try
            {
                id = int.Parse(idObj.ToString());
            }
            catch (Exception) { }

            Animal animal = animalRepository.FindByName(Name);
            if (animal != null && !animal.Id.Equals(id))
            {
                ValidationResult validationResult = new ValidationResult("Name is already been used", new[] { nameof(Name) });
                yield return validationResult;
            };
        }
    }
}
