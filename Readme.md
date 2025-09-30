## 👥 INTEGRANTES DO GRUPO

- RM559064 - Pedro Henrique dos Santos
- RM556182 - Vinícius de Oliveira Coutinho
- RM557992 - Thiago Thomaz Sales Conceição

---

## 🎯 PROBLEMA A SER RESOLVIDO

A Mottu enfrenta dificuldades para localizar e gerenciar com precisão as motos estacionadas em seus pátios. O processo atual é manual, sujeito a erros e impacta negativamente a eficiência operacional e o controle de ativos.

---

# 🏍️ Mottu Challenge - Gestão de Pátio e Setores

Este projeto implementa um sistema de **gestão de pátio (Yard)**, **setores (Sector)** e **vagas (Spots)** para organização e alocação de motos.  
O objetivo é permitir que filiais da Mottu consigam estruturar seus pátios em setores e, automaticamente, gerar as vagas disponíveis para as motos.

Desenvolveremos uma API RESTful para registrar, atualizar e consultar a localização das motos em tempo real nos pátios da Mottu. O sistema permitirá:

- Cadastro e atualização de motos, pátios, seções e filiais.
- Consulta rápida da localização de cada moto.
- Integração com cameras e IA para verificar se um setor esta cheio e com base nisso aconselhar a criar outros setores ou mudar de patio as motos que chegaram com esse modelo especifico, tambem para localizar uma moto especifica
- Facilidade de integração com outros sistemas internos da Mottu.
- Tera um sistema alerta onde quando uma moto estiver perto de sua revisão avisara a um prestador de serviço da mottu para adicionar essa moto a um setor de revisão

Essa solução trará mais agilidade, precisão e controle para a operação, reduzindo erros e otimizando o uso dos recursos.

---

## 📌 Domínio

- **Yard (Pátio)**  
  Representa um espaço físico de uma filial, que pode conter múltiplos setores.  
  Cada pátio possui dimensões e restrições de coordenadas.

- **Sector (Setor)**  
  Representa uma área dentro de um pátio.  
  É definido por pontos (polígono), e a partir dele são geradas vagas (spots).  
  O sistema valida se o setor:
  - Está contido dentro do pátio.  
  - Não se sobrepõe a outros setores do mesmo pátio.  

- **Spot (Vaga)**  
  Representa uma vaga de moto dentro de um setor.  
  Por padrão, cada vaga ocupa um espaço de **2m x 2m**.
  Exemplo: um setor de 10m x 10m comporta 25 vagas.
  
- **Motorcycle (Motocicleta)**
  A motocicleta é a principal entidade do negócio, pois é o objeto que precisa ser cadastrado, alocado e movimentado dentro dos setores e pátios. Todas as operações de gestão convergem para ela.

---

# 🚀 Guia de Configuração da Infraestrutura Azure

Este guia explica como configurar e implantar a aplicação MottuGrid no Azure usando Azure Container Registry (ACR) e Azure Container Instances (ACI).

---

## 📋 Pré-requisitos

### 1. Obter o Projeto
```bash
git clone https://github.com/MottuChallenge/Devops.git
cd /Devops
```

### 2. Instalar Ferramentas Entity Framework (para migrations)
```bash
dotnet tool install --global dotnet-ef
dotnet ef --version
```

---

## 🏗️ Configuração do Azure Container Registry (ACR)

### 1. Criar Resource Group e ACR
```bash
az group create --name MottuGrid --location eastus

az acr create \
    --resource-group MottuGrid \
    --name <Nome ACR> \
    --sku Basic \
    --admin-enabled true
```
> **Nota:** Substitua `<Nome ACR>` por um nome único globalmente

### 2. Construir e Enviar Imagens
```bash
# Login no ACR
az acr login --name <Nome ACR>

# Construir imagens localmente
docker-compose up -d

# Marcar e enviar imagem MySQL
docker tag mysql:8.0 <Nome ACR>.azurecr.io/mysql:8.0
docker push <Nome ACR>.azurecr.io/mysql:8.0

# Marcar e enviar imagem da API
docker tag mottu-grid:1.0 <Nome ACR>.azurecr.io/mottu-grid:1.0
docker push <Nome ACR>.azurecr.io/mottu-grid:1.0
```

### 3. Obter Credenciais do ACR
```bash
az acr credential show --name <Nome ACR>
```
Guarde o **username** e **password** para configurar os arquivos YAML.

---

## 🗄️ Configuração dos Arquivos YAML e Deploy

### 1. Configurar aci-mysql.yaml
Edite o arquivo `aci-mysql.yaml` substituindo as seguintes informações:
```yaml
image: <Seu ACR>.azurecr.io/mysql:8.0
server: <Seu ACR>.azurecr.io
username: <Username do ACR>
password: <Password do ACR>
```

### 2. Implantar Container MySQL
```bash
az container create --resource-group MottuGrid --file aci-mysql.yaml

# Obter IP público do MySQL
az container show --name mysql-aci --resource-group MottuGrid
```

### 3. Configurar aci-api.yaml
Edite o arquivo `aci-api.yaml` substituindo:
```yaml
# Dados do ACR (mesmo do MySQL):
image: <Seu ACR>.azurecr.io/mottu-grid:1.0
server: <Seu ACR>.azurecr.io
username: <Username do ACR>
password: <Password do ACR>

# String de conexão com IP do MySQL:
value: server=<IP-DO-MYSQL>;uid=user_test;pwd=user_password;database=MottuGridDb;port=3306
```

### 4. Implantar Container da API
```bash
az container create --resource-group MottuGrid --file aci-api.yaml
```

### 5. Executar Migrations do Entity Framework
**⚠️ OBRIGATÓRIO:** Execute as migrations para criar as tabelas antes de testar a API:

```bash
# Navegue para a pasta raiz da solution
cd /Devops

# Configure a string de conexão no appsettings.json:
# "MySqlConnection": "server=<IP-PUBLICO-DO-MYSQL>;uid=user_test;pwd=user_password;database=MottuGridDb;port=3306"

# Execute as migrations
dotnet ef database update --startup-project MottuChallenge.Api --project MottuChallenge.Infrastructure
```

> **Explicação:** Como as migrations estão no projeto `MottuChallenge.Infrastructure` mas a string de conexão está no `MottuChallenge.Api`, especificamos ambos os projetos.

---

## � Comandos Úteis para Troubleshooting
### Verificar Status dos Containers
```bash
# Status dos containers
az container show --name mysql-aci --resource-group MottuGrid
az container show --name api-aci --resource-group MottuGrid

# Listar todos os containers
az container list --resource-group MottuGrid --output table
```

### Visualizar Logs
```bash
# Logs do MySQL
az container logs --name mysql-aci --resource-group MottuGrid

# Logs da API
az container logs --name api-aci --resource-group MottuGrid
```

### Obter URL da API
```bash
# Obter IP da API para acessar o Swagger
az container show --name api-aci --resource-group MottuGrid
# URL: http://<ip-do-api-aci>:8080/swagger/index.html
```

### Teste de Conexão MySQL (Opcional)
```bash
mysql -h <IP-DO-MYSQL> -P 3306 -u user_test -p
# Dentro do MySQL: USE MottuGridDb; SHOW TABLES;
```
---

## 🧪 Testando a Aplicação

### Acessar o Swagger
1. Obtenha o IP da API:
   ```bash
   az container show --name api-aci --resource-group MottuGrid
   ```
2. Acesse: `http://<ip-do-api-aci>:8080/swagger/index.html`

### Exemplos de Requisições

**POST /api/yards** - Criar Pátio
```json
{
  "name": "Pátio Central",
  "cep": "01311300",
  "number": "100",
  "points": [
    { "pointOrder": 1, "x": 0, "y": 0 },
    { "pointOrder": 2, "x": 0, "y": 50 },
    { "pointOrder": 3, "x": 50, "y": 50 },
    { "pointOrder": 4, "x": 50, "y": 0 }
  ]
}
```

**PUT /api/yards/{id}** - Atualizar Pátio
```json
{
  "name": "Pátio Central Renovado"
}
```
