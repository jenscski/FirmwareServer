# Firmware server

#### Description

Serve firmware images to IOT devices

#### Supported hardware
 - ESP8266 [Arduino for ESP8266](https://github.com/esp8266/Arduino/)

## How to use with Docker


### Environment Variables

**TZ** the timezone to use. If not set, it defaults to Etc/UTC.
 
**FIRMWARESERVER__PASSWORD** set to the password used to get access to the web gui. If not set, the gui is not password proteted.

**FIRMWARESERVER__CULTURE** the culture to use in web gui (e.g. date format). If not set, invariant culture is used.

### Examples

#### Start server listening on port 5000
```
docker run -d -p 5000:80 jenscski/fwsrv
```

#### Start server with timezone Europe/Oslo and password 123456

```
docker run -d -p 5000:80 -e "TZ=Europe/Oslo" -e "FIRMWARESERVER__PASSWORD=123456" jenscski/fwsrv
```

### Persistent storage

When Firmware Server is running in a docker image, the data is stored in /var/lib/fwsrv.

To read how to make this folder persistent, visit [Manage data in Docker](https://docs.docker.com/storage/)
