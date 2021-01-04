#!/bin/bash

sudo systemctl stop Algoromida_01.service
sudo systemctl stop nginx.service
sudo rm -r /var/www/Algoromida_01
sudo mkdir /var/www/Algoromida_01
