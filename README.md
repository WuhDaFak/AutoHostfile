# AutoHostfile
Automatic, lightweight UDP name discovery with automatic host file update service

AutoHostfile is a lightweight service, which can be installed on MS Windows computers. It requires almost no configuration and allows groups of computers to discover each other, updating their DNS host files automatically.

It's useful:

* If you don't control the DNS environment in your organisation and you want to be able to locate your machines by their own friendly names of your choosing.
* If you have DHCP given addresses and your organisation doesn't provide you a sensible way of naming them.
* If you have machines connected via different network interfaces on different networks and you want a coherent approach to being able to name them.

It features:

* Lightweight UDP discovery.
* Option to name computers
* End to end, AES traffic encryption
* Detects power events and the addition of new network interfaces to trigger discovery
* Simple Settings UI
* Optional expiry of old host records
