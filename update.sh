#!/bin/sh

git pull

docker build . -t jenscski/fwsrv

docker stop fwsrv

docker rm fwsrv

docker run -d -p 127.0.0.1:5100:80 --name fwsrv --restart unless-stopped -v /home/jenscski/.fwsrv:/var/lib/fwsrv -e TZ=Europe/Oslo -e "FIRMWARESERVER__PASSWORD=123456" jenscski/fwsrv
