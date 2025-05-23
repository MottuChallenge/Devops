#🚀 Azure Infrastructure Setup & Docker Installation on Linux VM

This guide explains how to provision a Linux virtual machine on Azure, install Docker, and run a containerized application.

---

## 📦 Variables

```bash
RESOURCE_GROUP=rg-vm-challenge
LOCATION=eastus
VM_NAME=vm-challenge
IMAGE=Canonical:ubuntu-24_04-lts:minimal:24.04.202505020
SIZE=Standard_B2s
USERNAME=admin_fiap
PASSWORD='admin_fiap@123'

1 - az group create -l localization -n name-goup
 
2 - az vm create --resource-group name-goup --name name-machine --image image-choise --size size --admin-username username --admin-password password
 
3 - Instalação do Docker na máquina Linux

sudo apt-get update
sudo apt-get install ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update

sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

5 - az network nsg rule create --resource-group name-goup --nsg-name vm-challengeNSG --name port_8080 --protocol tcp --priority 1010 --destination-port-range 8080

6 - az network nsg rule create --resource-group name-goup --nsg-name vm-challengeNSG --name port_80 --protocol tcp --priority 1020 --destination-port-range 80

===============================================================================================================================================================================================================

# How it will look



1 - az group create -l eastus -n rg-vm-challenge
 
2 - az vm create --resource-group rg-vm-challenge --name vm-challenge --image Canonical:ubuntu-24_04-lts:minimal:24.04.202505020 --size Standard_B2s --admin-username admin_fiap --admin-password admin_fiap@123

4 - az network nsg rule create --resource-group rg-vm-challenge --nsg-name vm-challengeNSG --name port_8080 --protocol tcp --priority 1010 --destination-port-range 8080
 
5 - az network nsg rule create --resource-group rg-vm-challenge --nsg-name vm-challengeNSG --name port_80 --protocol tcp --priority 1020 --destination-port-range 80
 
6 - Instalação do Docker na máquina Linux

sudo apt-get update
sudo apt-get install ca-certificates curl
sudo install -m 0755 -d /etc/apt/keyrings
sudo curl -fsSL https://download.docker.com/linux/ubuntu/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc

echo \
  "deb [arch=$(dpkg --print-architecture) signed-by=/etc/apt/keyrings/docker.asc] https://download.docker.com/linux/ubuntu \
  $(. /etc/os-release && echo "${UBUNTU_CODENAME:-$VERSION_CODENAME}") stable" | \
  sudo tee /etc/apt/sources.list.d/docker.list > /dev/null

sudo apt-get update

sudo apt-get install docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

sudo usermod -aG docker $USER

newgrp docker

7 - rodar o projeto
docker run --name mottu -d -p 8080:80 pedrohenrique32/mottu-grid-dotnet
 

