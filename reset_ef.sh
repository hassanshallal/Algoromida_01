#!/bin/bash

rm Migrations/*
dotnet ef database drop -f -v

sudo rm -r obj/Debug/netcoreapp3.1/Razor

dotnet ef migrations add Initial
dotnet ef database update
