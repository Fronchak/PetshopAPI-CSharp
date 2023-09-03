using PetShopAPIV2.Exceptions;

namespace PetShopAPIV2.Utils
{
    public class ParseUtils
    {
        public static int ParsePathParam(string idStr)
        {
            try
            {
                return int.Parse(idStr);
            }
            catch (Exception)
            {
                throw new BadRequestException("Id must be a number");
            }
        }
    }
}
