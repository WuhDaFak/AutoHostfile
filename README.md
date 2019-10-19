# AutoHostfile
An automatic, lightweight UDP name discovery service which updates the MS Windows hosts file

AutoHostfile is a lightweight service, which can be installed on MS Windows computers. It requires almost no configuration and allows groups of computers to discover each other, updating their DNS host files automatically.

It's useful:

* If you don't control the DNS environment in your organisation and you want to be able to locate your machines by their own friendly names of your choosing.
* If you have DHCP given addresses and your organisation doesn't provide you a sensible way of naming them.
* If you have machines connected via different network interfaces on different networks and you want a coherent approach to being able to name them.

# Latest Release

https://github.com/benstaniford/AutoHostfile/raw/master/Releases/AutoHostfile-1.08.msi

# Installation Instructions

1. Install using MSI on each machine you want to manage
2. Check the settings box after installation to add a friendly name (defaults to hostname) and a shared encryption password.
3. Check your hosts file to watch it update.
4. That's it!

# Features

* Lightweight UDP discovery
* Option to name computers
* End to end, AES traffic encryption
* Detects power events and the addition of new network interfaces to trigger discovery
* Simple Settings UI
* MSI Installer
* Optional expiry of old host records

# Building

* Visual Studio 2019
* Wix 3.11.2 Toolset
* Wix 3.11.2 Visual Studio Extension

# Logging

* The MSI installs a command line tool which will be added to the path, "autohosts -log" from the command line will allow you to watch the service real time.
