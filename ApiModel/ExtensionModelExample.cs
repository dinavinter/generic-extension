using System.Linq;
using Swashbuckle.AspNetCore.Filters;

namespace GenericExtesion
{
    public class ExtensionModelExample : IExamplesProvider<ExtensionModel>
    {
        public ExtensionModel GetExamples()
        {
            return DataSeed.GetSeed().FirstOrDefault();
        }
    }
}