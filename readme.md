# Advantech Wise4012e Modbus Simulation

Application which simulates an Advantech Wise 4012e IO module. The application is written in .Net Core.

## Purpose

It runs a Modbus service at [127.0.0.1] (configurable):

* Switch 1 on Discrete coil 1 (read/write) 0 or 1
* Switch 2 on Discrete coil 2 (read/write) 0 or 1
* Relay 1 on Discrete coil 17 (read) 0 or 1
* Relay 2 on Discrete coil 18 (read) 0 or 1
* Knob 1 on Holding Register 40001 (read/write) 0 - 4500
* Knob 2 on Holding Register 40002 (read/write) 0 - 4500 

## Usage

Compile and run this application in [Visual studio Code](https://code.visualstudio.com/).

You can read and write Modbus using this simulation in Modbus clients like [CAS Modbus Scanner](http://store.chipkin.com/articles/modbus-scanner-what-is-the-cas-modbus-scanner) or [Azure IoT Edge](https://docs.microsoft.com/en-us/azure/iot-edge/deploy-modbus-gateway).

![alt tag](img/simulation.jpg)

## Alter behaviour

When running the application, you can alter the values of the simulation.

Just enter:

* 1 0 (switch to false)
* 1 1 (switch to true)
* 2 0 (switch to false)
* 2 2 (switch to true)
* 40001 0-4500 (pick a value between 0 and 4500)
* 40002 0-4500 (pick a value between 0 and 4500)

## NModbus4 library

This simulation is making use of the Nuget package [NModbus4](https://github.com/NModbus4/NModbus4). Great work!

## Contribute

This logic is licenced under the MIT license.

Want to contribute? Throw me a pull request....

Want to know more about me? Check out my (blog)[http://blog.vandevelde-online.com]

