#!/bin/bash

rm app.db
sudo rm /home/hshallal/Algoromida_01/Algoromida_01/wwwroot/uploads/*
sudo rm -r obj/Debug/netcoreapp3.1/Razor
rm Migrations/*

dotnet ef database drop -f -v
dotnet ef migrations add Initial
dotnet ef database update


