﻿using Pusharp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Pusharp.Entities;
using Pusharp.RequestParameters;

namespace Pusharp
{
    public class PushBulletClient
    {
        private readonly Requests _requests;

        public AuthenticationResult Authentication { get; }

        private PushBulletClient(Requests requests, AuthenticationModel model)
        {
            _requests = requests;
            Authentication = new AuthenticationResult(model);
        }

        public static async Task<PushBulletClient> CreateClientAsync(string accessToken)
        {
            var requests = new Requests(accessToken);
            
            var authentication = await requests.SendAsync<AuthenticationModel>("/v2/users/me", RequestType.GET).ConfigureAwait(false);

            return new PushBulletClient(requests, authentication);
        }

        public async Task<IEnumerable<Device>> GetDevicesAsync()
        {
            var devicesModel = await _requests.SendAsync<DevicesModel>("/v2/devices", RequestType.GET);
            return devicesModel.Models.ToDevices();
        }

        public async Task<Device> CreateDeviceAsync(DeviceCreationParameters parameters)
        {
            var deviceModel = await _requests.SendAsync<DeviceModel>("/v2/devices", RequestType.POST, parameters);
            return new Device(deviceModel);
        }
    }
}
