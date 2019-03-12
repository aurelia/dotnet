using System.Threading.Tasks;

namespace Aurelia.DotNet.DataAccess.Interfaces
{
    public interface IDatabaseInitializer
    {
        void Seed();
        Task SeedAsync();

    }
}