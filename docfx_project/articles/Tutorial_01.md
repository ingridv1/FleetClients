# Tutorial 01 - Creating a Virtual Vehicle

Pre-requisites:

* An active fleet manager service installation.
  * Hosting HTTP endpoints on 41916.
  * Hosting Net.TCP endpoint on 41917.
* Visual Studio 2019.
* Basic C# knowledge.

## Getting Started

- Download the source code from [Github](https://github.com/GuidanceAutomation/FleetClients)
- Open the solution ```FleetClients.sln``` in VS 2019

> [!TIP]
> Ensure your fleet manager service is running and exposing endpoints on localhost.

> [!IMPORTANT]
> Check the http endpoint [fleetManager.svc](http://127.0.0.1:41916/fleetManager.svc) is active.

### Run the Console application

Select ```Tutorial 01``` as the startup project, build and run the solution.

A simple demo will run where a virtual vehicle is created with a pose of 0,0,0 and then has its pose updated to 1,1,1.57.

## Code Snippets

Check the code in ```Tutorial01\Program.cs``` for a commented walkthrough.

### EndpointSettings

```EndpointSettings``` is a lightweight object that tightly couples:

* IP Address
* Http port (41916 default)
* TCP port (41917 default)
* UDP port (41918 default)

of the host (server). Use this in conjunction with the client factory to create a client.

### IFleetManagerClient

The fleet manager client is created via the static factory class ```ClientFactory```

```
IFleetManagerClient fleetManagerClient = ClientFactory.CreateTcpFleetManagerClient(endpointSettings);
```
> [!WARNING]
> The client has its own background thread for auto-subscribing to fleet state updates. Dispose of the client when it is no longer required: ```fleetManagerClient.Dispose()```

### CreateVirtualVehicle & SetPose

A virtual vehicle is created via the client with:

```
IPAddress virtualVehicle = IPAddress.Parse("192.168.0.1");
IServiceCallResult result = fleetManagerClient.CreateVirtualVehicle(virtualVehicle, 0, 0, 0);
```

and its pose set via:

```
result = fleetManagerClient.SetPose(virtualVehicle, 1, 1, 1.57);
```

> [!WARNING]
> Remember to check the success of your ```IServiceCallResult``` for client or server errors.
