#!/bin/bash

sudo dotnet publish 
sudo systemctl start Algoromida_01.service
sudo systemctl start nginx.service
