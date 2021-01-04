#!/bin/bash

sudo dotnet publish  -o /var/www/Algoromida_01
sudo cp app.db /var/www/Algoromida_01
sudo cp -R Views /var/www/Algoromida_01
sudo systemctl start Algoromida_01.service
sudo systemctl start nginx.service
