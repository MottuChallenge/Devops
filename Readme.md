## INTEGRANTES DO GRUPO

- RM559064 - Pedro Henrique dos Santos
- RM556182 - Vinícius de Oliveira Coutinho
- RM557992 - Thiago Thomaz Sales Conceição

---

## 🎯 PROBLEMA A SER RESOLVIDO

A Mottu enfrenta dificuldades para localizar e gerenciar com precisão as motos estacionadas em seus pátios. O processo atual é manual, sujeito a erros e impacta negativamente a eficiência operacional e o controle de ativos.

---

#  Mottu Challenge - Gestão de Pátio e Setores

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

##  Domínio

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

Este guia explica como configurar e implantar a aplicação MottuGrid no Azure usando App Service Web app, Azure Container Registry (ACR) e Azure Container Instances (ACI).
Faremos isso usando uma pipeline que ativará assim que dispararmos um push no github criando um artefato e uma imagem docker e a subindo para o registry e assim caindo em uma pipeline de release que ira subir a imagem docker do registry para o web app. Antes de darmos um push devemos enviar uma imagem docker do mysql para o ACR e assim subirmos ela para o ACI tendo assim uma instancia do banco na nuvem.
Apos o ACI ser criado devemos pegar o IP e configurar ele nas variaveis da pipeline de build. 

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

### 3. Configurar a variavel da pipeline
edite a variavel databaseConnectionString da pipeline
coloque os dados do seu banco

# String de conexão com IP do MySQL:
```bash
value: server=<IP-DO-MYSQL>;uid=user_test;pwd=user_password;database=MottuGridDb;port=3306
```

### 4. Criar o App services
```bash
  az appservice plan create \
  --name MottuGridPlan \
  --resource-group MottuGrid \
  --sku B1 \
  --is-linux
```
### 5. Criar o Web App quickstart:
```bash
  az webapp create \
  --resource-group MottuGrid \
  --plan MottuGridPlan \
  --name MottuGridAPI \
  --container-image-name nginx
```

### 6. Configurar as variaveis do ACR no Web App:
```bash
  az webapp config container set \
  --name MottuGridAPI \
  --resource-group MottuGrid \
  --container-image-name rm559064.azurecr.io/teste \
  --container-registry-url https://rm559064.azurecr.io \
  --container-registry-user $(az acr credential show --name rm559064 --query "username" -o tsv) \
  --container-registry-password $(az acr credential show --name rm559064 --query "passwords[0].value" -o tsv)
```



## � Comandos Úteis para Troubleshooting
### Verificar Status dos Containers
```bash
# Status dos containers
az container show --name mysql-aci --resource-group MottuGrid

# Listar todos os containers
az container list --resource-group MottuGrid --output table
```

### Visualizar Logs
```bash
# Logs do MySQL
az container logs --name mysql-aci --resource-group MottuGrid


### Teste de Conexão MySQL (Opcional)
```bash
mysql -h <IP-DO-MYSQL> -P 3306 -u user_test -p
# Dentro do MySQL: USE MottuGridDb; SHOW TABLES;
```
---

## 🧪 Testando a Aplicação

### Acessar o Swagger
1. Acesse: `http://<ip-da-api>:8080/swagger/index.html`

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
