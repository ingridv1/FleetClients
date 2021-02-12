```
______ _           _   _____ _ _            _
|  ___| |         | | /  __ \ (_)          | |
| |_  | | ___  ___| |_| /  \/ |_  ___ _ __ | |_ ___
|  _| | |/ _ \/ _ \ __| |   | | |/ _ \ '_ \| __/ __|
| |   | |  __/  __/ |_| \__/\ | |  __/ | | | |_\__ \
\_|   |_|\___|\___|\__|\____/_|_|\___|_| |_|\__|___/
```

* https://github.com/GuidanceAutomation/FleetClients
* https://www.guidanceautomation.com/

# Release Notes

## 3.1.3 (12th Feb 21)

* Updates service reference to handle MTOM explicit support. 

## 3.1.2 (2nd Oct 20)

* Updates package references to flush through GAAPICommon. 

## 3.1.1 (1st Oct 20)

* Updates package references and dependencies for bug fixes. No code changes. 

## 3.1.0 (16th Sept 20)

* Updates BaseClients package to enable named pipe support. 

## v3.0.0 (4th Sep 20)

* Major refactor to ```IServiceResult``` and ```IServiceResult<T>``` implementation.
* Removes ```ResetKingpin``` functionality.
* Consistent suffix renaming of all data transfer objects to ```dto```.
* Uses ```GAAPICommon``` package for .NET Standard enumerator definitions.

## v2.0.0 (20th Feb 20)

* Major refactor to MVVM.
* Removes FleetClients.Controls.
* Adds FleetClients.UI for MVVM implementation.
* Adds FleetClients.DemoApp to demonstrate MVVM controls.
* FleetTemplateManger for quickly creating virtual fleets from a file.

## v1.4.0 (14th Oct 19)

* Adds fleet template support.
  * Serialize fleet templates to json.
  * Create fleet templates from json.

## v1.3.2 (16th Sept 19)

* Minor refactor to use GenericMailbox from GACore.

## v1.3.1 (13th Sept 19)

* Moved the IsConnectedControl out to BaseClients.Controls.
* All assemblies .net 4.7.2 Framework.

## v1.3.0 (11th Sept 19)

* Updates demo app to configurable IP address.
* Added descriptions for demo app windows.

## v1.2.1 (21st Aug 19)

* Missing dependencies in nuspec.

## v1.2.0 (21st Aug 19)

* Refactored base client support.
* Fleet manager client control.
* v7.x.x scheduler support.

## v1.1.2 (3rd Jul 19)

* Ex2 waypoint support for v6.x.x schedulers.

## v1.1.1 (17th Apr 19)

* Scheduler v5.8.x support.

## v1.1.0 (14th Nov 18)

* Enable /disable individual AGVs.

## v1.0.0 (17th Aug 18)

- Initial release.
* Base functionality.
