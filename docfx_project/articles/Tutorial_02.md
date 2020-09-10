# Tutorial 02

Pre-requisites:

* Complete Tutorial 01 first.

## Getting Started

- Download the source code from [Github](https://github.com/GuidanceAutomation/FleetClients)
- Open the solution ```FleetClients.sln``` in VS 2019

> [!TIP]
> Ensure your fleet manager service is running and exposing endpoints on localhost.

> [!IMPORTANT]
> Check the http endpoint [fleetManager.svc](http://127.0.0.1:41916/fleetManager.svc) is active.

### Run the Console application

Select ```Tutorial 02``` as the startup project, build and run the solution.

A simple demo will run where a virtual vehicle is created with a pose of 0,0,0. Upon subscribing to fleet updates the state of the fleet will be continuously flushed to the console window.

## Code Snippets

### ICallbackClient

The ```IFleetManagerClient``` implements the ```ICallbackClient``` interface, in that it will automatically connect to the server and subscribe to fleet state update messages. When a new fleet state message is received, the ```FleetState``` property will be updated and the event ```FleetStateUpdated``` fired.

The ```ICallbackClient``` interface defines two events:

```
event Action<DateTime> Connected;
event Action<DateTime> Disconnected;
```

Which will fire whenever the client is connected or disconnected from the server. In addition the ```IsConnected``` property can be used to determine the state of the connection.

With the console app running and displaying the fleet state, stopping the Fleet Manager Service will result in the last received fleet state being displayed, e.g.

```
Fleet size:1
IP Address:192.168.0.1 Tick:140, X:1, Y:1, Heading:1.57
```

Re-starting the Fleet Manager Service will result in:

```
Fleet size:0
```
being displayed as the fleet manager is re-initialized with no virtual vehicles.
