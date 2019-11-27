using System.Threading.Tasks;
using BritishCarAuctions.DealerProApp.Api.Data;
using static BritishCarAuctions.DealerProApp.Api.IntegrationTests.Constants;

namespace Just4Fun.ConsoleApp1.Helpers
{
    public class ScenarioState
    {
        public ScenarioState()
        {
            DealerProApi = new DealerProApi();
        }

        public int NumberOfSyncTasks { get; set; }

        public string LastSearchedVrm { get; set; }

        public string LastAddedNote { get; set; }

        public string VehicleMileage { get; set; }

        public Credentials Credentials
        {
            get => DealerProApi.RegularUserCredentials;
            set => DealerProApi.RegularUserCredentials = value;
        }

        public DealerProApi DealerProApi { get; private set; }

        public async Task SetAppVersionsAsync(int latestVersion)
        {
            await DealerProApi.SetApplicationVersionsAsync(AppiumManager.IsOnIOS ? "ios" : "uwp", latestVersion);
        }

        public async Task<RecentlyAddedVehiclesList> FetchRecentlyAddedVehicleListAsync()
        {
            return await DealerProApi.FetchRecentlyAddedVehicleListAsync();
        }
    }
}
