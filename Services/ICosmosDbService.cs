using System.Threading.Tasks;

namespace starter_dotnet_core.Services{
    using System.Collections.Generic;
    using starter_dotnet_core.Models;
    using starter_dotnet_core.Wheels;
    public interface ICosmosDbService{
        Task<IEnumerable<Model>> GetItemsAsync(string query);
        Task<IEnumerable<CarWheel>> GetWheelItemsAsync(string query);
        Task<Model> AddItemASync(Model data);
        
        Task<CarWheel> AddWheelItemASync(CarWheel data);
    }



}