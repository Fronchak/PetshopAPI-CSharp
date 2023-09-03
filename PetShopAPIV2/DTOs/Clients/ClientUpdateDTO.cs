using PetShopAPIV2.Entities;
using PetShopAPIV2.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PetShopAPIV2.DTOs.Clients
{
    public class ClientUpdateDTO : ClientInputDTO
    {
        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            IEnumerable<ValidationResult> result = new List<ValidationResult>();
            IClientRepository clientRepository = validationContext.GetRequiredService<IClientRepository>();
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
            Client? client = clientRepository.FindByEmail(Email);
            if (client != null && !client.Id.Equals(id))
            {
                ValidationResult validationResult = new ValidationResult("Email is already been used", new[] { nameof(Email) });
                yield return validationResult;
            }
        }
    }
}
