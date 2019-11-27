using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using BritishCarAuctions.ApiClient;
using BritishCarAuctions.ApiClient.OAuth;
using Just4Fun.ConsoleApp1.Helpers.Requests;
using BritishCarAuctions.DealerProApp.Api.Data;
using BritishCarAuctions.DealerProApp.Api.Data.Appraisal;
using BritishCarAuctions.DealerProApp.Api.Data.User;
using BritishCarAuctions.DealerProApp.Api.Data.Vehicle;
using BritishCarAuctions.DealerProApp.Api.Data.VehicleEquipmentData;
using BritishCarAuctions.DealerProApp.Api.Extensions;
using BritishCarAuctions.DealerProApp.Api.IntegrationTests;
using BritishCarAuctions.DealerProApp.Api.Requests;
using Just4Fun.ConsoleApp1.Tests;
using static BritishCarAuctions.DealerProApp.Api.IntegrationTests.Constants;

namespace Just4Fun.ConsoleApp1.Helpers
{
    public class DealerProApi
    {
        private IApiClient _reqularUserApiClient;
        private IApiClient _adminUserApiClient;

        private UserInfo _regularUserInfo;

        private IOAuthTokenRepository _regularUserTokenRepository = new InMemoryOAuthTokenRepository();
        private IOAuthTokenRepository _adminUserTokenRepository = new InMemoryOAuthTokenRepository();

        public DealerProApi()
        {
            _reqularUserApiClient = GivenAnApiClient(_regularUserTokenRepository);
            _adminUserApiClient = GivenAnApiClient(_adminUserTokenRepository);
        }

        public Credentials RegularUserCredentials { get; set; }

        public async Task<bool> IsVrmNumberValid(string vrmNumber)
        {
            await CreateRegularUserTokenIfNeededAsync();

            try
            {
                var response = await _reqularUserApiClient.ExecuteAsync(new VehicleSummaryRequest(vrmNumber, Constants.LanguageCode));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<VehicleSummary> GetVehicleSummary(string vrm)
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new VehicleSummaryRequest(vrm, Constants.LanguageCode));

            return response.DeserializedResponse;
        }

        public async Task<VehicleDetail> GetVehicleDetails(string vrm)
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new VehicleDetailRequest(await GetVehicleSummary(vrm)));

            return response.DeserializedResponse;
        }

        public async Task<VehicleAppraisal> GetVehicleAppraisal(string vrm)
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new VehicleAppraisalRequest(await GetVehicleSummary(vrm)));

            return response.DeserializedResponse;
        }

        public async Task<VehicleDetail> GetVehicleDetails(VehicleSummary vehicleSummary)
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new VehicleDetailRequest(vehicleSummary));

            return response.DeserializedResponse;
        }

        public async Task<VehicleAppraisal> GetVehicleAppraisal(VehicleSummary vehicleSummary)
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new VehicleAppraisalRequest(vehicleSummary));

            return response.DeserializedResponse;
        }

        public async Task<VehicleEquipment> GetVehicleEquipment(VehicleSummary vehicleSummary)
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new VehicleEquipmentRequest(vehicleSummary));

            return response.DeserializedResponse;
        }

        public async Task<bool> VerifyVehicleExistanceAndUpdateItsNote(string vrm, string generalComments)
        {
            await CreateRegularUserTokenIfNeededAsync();

            VehicleSummary vehicleSummary;

            try
            {
                vehicleSummary = await GetVehicleSummary(vrm);
            }
            catch (ApiResponseException exception) when (exception.HttpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                await CreateVehicle(vrm);
                vehicleSummary = await GetVehicleSummary(vrm);
            }

            vehicleSummary.OdometerReading = string.IsNullOrEmpty(vehicleSummary.OdometerReading) ? Constants.ValidMileage : vehicleSummary.OdometerReading;
            var vehicleDetails = await GetVehicleDetails(vehicleSummary);

            VehicleAppraisal vehicleAppraisal = null;

            try
            {
                vehicleAppraisal = await GetVehicleAppraisal(vehicleSummary);
            }
            catch (ApiResponseException exception)
            {
                if (!exception.IsNotFoundException())
                {
                    throw exception;
                }
            }

            if (vehicleAppraisal == null)
            {
                vehicleAppraisal = GetAValidAppraisal();
            }

            VehicleEquipment vehicleEquipment = null;

            try
            {
                vehicleEquipment = await GetVehicleEquipment(vehicleSummary);
            }
            catch (ApiResponseException exception)
            {
                if (!exception.IsNotFoundException())
                {
                    throw exception;
                }
            }

            vehicleDetails.GeneralComments = generalComments;

            var response = await _reqularUserApiClient.ExecuteAsync(new UpdateVehicleRequest(vehicleSummary, vehicleDetails, vehicleAppraisal, vehicleEquipment));

            return response.DeserializedResponse;
        }

        public async Task DeleteVehicleIfExists(string vrm)
        {
            await CreateRegularUserTokenIfNeededAsync();

            try
            {
                var vehicleSummary = await GetVehicleSummary(vrm);
                await _reqularUserApiClient.ExecuteAsync(new DeleteVehicleRequest(Constants.ApiBaseUri, vehicleSummary.VehicleId));
            }
            catch (ApiResponseException exception) when (exception.HttpResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
            }
        }

        public async Task CreateVehicle(string vrm, string vehicleMotStatus = null)
        {
            await CreateRegularUserTokenIfNeededAsync();

            VehicleMot vehicleMot = new VehicleMot();

            switch (vehicleMotStatus)
            {
                case "Expired":
                    vehicleMot.MotStatus = MotExpiredId;
                    vehicleMot.MotDate = "Jan 23, 2019";
                    break;
                case "Expires on":
                    vehicleMot.MotStatus = MotExpiresOnId;
                    vehicleMot.MotDate = "Jan 23, 2019";
                    break;
                case "Unavailable":
                    vehicleMot.MotStatus = MotUnspecifiedId;
                    break;
                default:
                    vehicleMot = null;
                    break;
            }

            var vehicleDerivativesResponse = await _reqularUserApiClient.ExecuteAsync(new VehicleDerivativesRequest(vrm));
            var vehicleDetivativesList = vehicleDerivativesResponse.DeserializedResponse;
            var firstDerivative = vehicleDetivativesList.VehicleDerivatives.First();
            var vehicleToBeCreated = new VehicleToBeCreated(firstDerivative, vehicleMot, _regularUserInfo.DefaultLocation.Id);
            var createdVehicleResponse = await _reqularUserApiClient.ExecuteAsync(new CreateVehicleRequest(vehicleToBeCreated));
            var createdVehicle = createdVehicleResponse.DeserializedResponse;

            if (!createdVehicle.Success)
            {
                throw new CreateVehicleException();
            }
        }

        public async Task SetApplicationVersionsAsync(string platform, int latestVersion)
        {
            await CreateAdminDealerTokenIfNeededAsync();

            var requestBody = new AppVersions
            {
                Platform = platform,
                Latest = new AppVersion
                {
                    VersionName = "Any version name",
                    VersionCode = latestVersion,
                    RealeaseNotes = "Any release notes"
                },
                Minimum = new AppVersion
                {
                    VersionName = "Any version name",
                    VersionCode = 1,
                    RealeaseNotes = "Any release notes"
                }
            };

            await _adminUserApiClient.ExecuteAsync(new UpdateApplicationVersionsRequest(requestBody));
        }

        public async Task CreateVehicleIfDoesNotExists(string vrm)
        {
            if (!await IsVrmNumberValid(vrm))
            {
                await CreateVehicle(vrm);
            }
        }

        public async Task<RecentlyAddedVehiclesList> FetchRecentlyAddedVehicleListAsync()
        {
            await CreateRegularUserTokenIfNeededAsync();

            var response = await _reqularUserApiClient.ExecuteAsync(new RecentlyAddedVehiclesRequest());

            return response.DeserializedResponse;
        }

        private async Task CreateRegularUserTokenIfNeededAsync()
        {
            if (_regularUserTokenRepository.Token == null)
            {
                _regularUserTokenRepository.Token = await GivenAnAuthenticatedToken(RegularUserCredentials);
                _regularUserInfo = await GetUserInfoAsync();
            }
        }

        private async Task<UserInfo> GetUserInfoAsync()
        {
            var resonse = await _reqularUserApiClient.ExecuteAsync(new UserInfoRequest());

            return resonse.DeserializedResponse;
        }

        private IApiClient GivenAnApiClient(IOAuthTokenRepository tokenRepository)
        {
            var uriResolver = new UriResolver(new Dictionary<string, string>()
            {
                ["dealerPro"] = Constants.DiscoveryEndpoint
            });

            var httpClient = new HttpClient(new ConsoleLoggingHandler(new HttpClientHandler()))
            {
                Timeout = TimeSpan.FromSeconds(210)
            };

            var apiClient = new HttpApiClient(httpClient, uriResolver, tokenRepository);

            apiClient.SessionExpiredEvent += (sender, e) => apiClient.CancelUnauthorizedRequests();

            return apiClient;
        }

        private async Task<OAuthToken> GivenAnAuthenticatedToken(Credentials credentials = null)
        {
            credentials = credentials ?? Users.AutomationUser1;

            var oauthClient = GetGivenAnOAuthClient();
            var tokenReponse = await oauthClient.LoginAsync(credentials.Account, credentials.Password, SSOEndpoint.Scope);

            return tokenReponse.Token;
        }

        private OAuthClient GetGivenAnOAuthClient()
        {
            var discoveryClient = new OAuthDiscoveryClient(SSOEndpoint.Uri, () => new ConsoleLoggingHandler(new HttpClientHandler()));
            var tokenClient = new OAuthTokenClient(SSOEndpoint.ClientId, SSOEndpoint.ClientSecret, () => new ConsoleLoggingHandler(new HttpClientHandler()));
            var userInfoClient = new OAuthUserInfoClient(() => new ConsoleLoggingHandler(new HttpClientHandler()));

            return new OAuthClient(discoveryClient, tokenClient, userInfoClient);
        }

        private VehicleAppraisal GetAValidAppraisal()
        {
            return new VehicleAppraisal
            {
                Appraisal = new AppraisalModel
                {
                    XmlReport = new XmlReportModel
                    {
                        Inspection = new InspectionModel
                        {
                            Grade = "1"
                        }
                    }
                }
            };
        }

        private async Task CreateAdminDealerTokenIfNeededAsync()
        {
            if (_adminUserTokenRepository.Token == null)
            {
                var tokenResponse = await _adminUserApiClient.ExecuteAsync(new TokenRequest(AdminTokenConstants.Uri, AdminTokenConstants.ClientId, AdminTokenConstants.ClientSecret, AdminTokenConstants.Scope, AdminTokenConstants.GrantType, AdminTokenConstants.Username, AdminTokenConstants.Password));
                _adminUserTokenRepository.Token = tokenResponse.DeserializedResponse.Token;
            }
        }
    }

    public class CreateVehicleException : Exception { }
}
