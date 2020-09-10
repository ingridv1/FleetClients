# Overview

The **FleetClients** package allows easy manipulation of the fleet manager component of the scheduler and provides a small collection UI elements for use in bespoke applications.

## Key Functionality

The ```IFleetManagerClient```  (c)reated via the client factory) provides:

* Real-time fleet state updates.
  * Fleet size.
  * Vehicle poses.
  * Vehicle states.
  * Vehicle battery charge.
* Ability to create virtual vehicles.
* Ability to set the pose of vehicles.
* Enable / disabling vehicles.
* Freezing / unfreezing the entire fleet.
