# 🚀 Guia de Configuração da Infraestrutura Azure

Este guia explica como configurar e implantar a aplicação MottuGrid no Azure usando Azure Container Registry (ACR) e Azure Container Instances (ACI).

---

## 📋 Índice
1. [Configuração do Azure Container Registry (ACR)](#configuração-do-azure-container-registry-acr)
2. [Configuração do Banco de Dados com ACI](#configuração-do-banco-de-dados-com-aci)
3. [Implantação da API com ACI](#implantação-da-api-com-aci)
4. [Configuração de VM (Alternativa)](#configuração-de-vm-alternativa)

---

## 🏗️ Configuração do Azure Container Registry (ACR)

### 1. Criar Resource Group
```bash
az group create --name MottuGrid --location eastus
```

### 2. Criar Azure Container Registry
```bash
az acr create \
    --resource-group MottuGrid \
    --name <Nome ACR> \
    --sku Basic \
    --admin-enabled true
```
> **Nota:** Substitua `<Nome ACR>` por um nome único para seu Container Registry

### 3. Fazer Login no ACR
```bash
az acr login --name <Nome ACR>
```

### 4. Construir Imagens Localmente
```bash
docker-compose up -d
docker images
```

### 5. Marcar e Enviar Imagem MySQL
```bash
# Marcar imagem MySQL
docker tag mysql:8.0 <Nome ACR>.azurecr.io/mysql:8.0

# Enviar para ACR
docker push <Nome ACR>.azurecr.io/mysql:8.0
```

### 6. Marcar e Enviar Imagem da API
```bash
# Marcar imagem da API
docker tag mottu-grid:1.0 <Nome ACR>.azurecr.io/mottu-grid:1.0

# Enviar para ACR
docker push <Nome ACR>.azurecr.io/mottu-grid:1.0
```

---

## 🗄️ Configuração do Banco de Dados com ACI

### 1. Configurar o arquivo aci-mysql.yaml
Antes de implantar, você deve configurar o arquivo `aci-mysql.yaml` com os dados do seu ACR:

```yaml
# No arquivo aci-mysql.yaml, substitua:
image: <Seu ACR>.azurecr.io/mysql:8.0
server: <Seu ACR>.azurecr.io
username: <Seu User>
password: <Sua Senha>
```

> **Importante:** Para obter as credenciais do ACR, use:
```bash
az acr credential show --name <Nome ACR>
```

### 2. Implantar Container MySQL
```bash
az container create --resource-group MottuGrid --file aci-mysql.yaml
```

### 3. Obter IP Público do MySQL
```bash
az container show --name mysql-aci --resource-group MottuGrid
```
Procure pelo endereço IP na resposta:
```json
"ipAddress": {
    "ip": "SEU-IP-PUBLICO-MYSQL"
}
```

### 4. Conectar ao MySQL (Teste Opcional)
```bash
mysql -h <IP-DO-BANCO> -P 3306 -u user_test -p
```

---

## 🚀 Implantação da API com ACI

### 1. Configurar o arquivo aci-api.yaml
Antes de implantar a API, você deve configurar o arquivo `aci-api.yaml` com:

**a) Dados do ACR:**
```yaml
# No arquivo aci-api.yaml, substitua:
image: <Seu ACR>.azurecr.io/mottu-grid:1.0
server: <Seu ACR>.azurecr.io
username: <Seu User>
password: <Sua Senha>
```

**b) String de Conexão com IP do MySQL:**
```yaml
# Atualize a variável de ambiente DB_CONNECTION com o IP público do MySQL:
value: server=<IP-DO-BANCO>;uid=user_test;pwd=user_password;database=MottuGridDb;port=3306
```

> **Dica:** Use o IP obtido na etapa anterior da configuração do MySQL

### 2. Implantar Container da API
```bash
az container create --resource-group MottuGrid --file aci-api.yaml
```

---

## 📝 Notas Importantes

- **Nome do ACR:** Deve ser globalmente único no Azure
- **IP do MySQL:** Salve o IP público do container MySQL para configuração da API
- **String de Conexão:** Atualize a string de conexão da API com o IP do MySQL antes da implantação
- **Resource Group:** Todos os recursos são criados no resource group `MottuGrid`
- **Configuração dos YAMLs:** É essencial configurar os arquivos `aci-mysql.yaml` e `aci-api.yaml` com os dados corretos do ACR antes da implantação

---

## ⚙️ Configuração dos Arquivos YAML

### Dados necessários do ACR:
1. **Nome do ACR:** `<Seu ACR>.azurecr.io`
2. **Username:** Obtido com `az acr credential show --name <Nome ACR>`
3. **Password:** Obtido com `az acr credential show --name <Nome ACR>`

### Locais para configurar nos YAMLs:
- **aci-mysql.yaml:** Seção `image` e `imageRegistryCredentials`
- **aci-api.yaml:** Seção `image`, `imageRegistryCredentials` e variável `DB_CONNECTION`

---

## 🔧 Solução de Problemas

### Problemas Comuns
- **Falha no Login do ACR:** Verifique se você tem as permissões adequadas e se o nome do ACR está correto
- **Falha na Criação do Container:** Verifique se os arquivos YAML existem e estão configurados corretamente
- **Problemas de Conexão com o Banco:** Verifique se o container MySQL está executando e se o IP está configurado corretamente na API

### Comandos Úteis
```bash
# Verificar status dos containers
az container show --name mysql-aci --resource-group MottuGrid
az container show --name api-aci --resource-group MottuGrid

# Visualizar logs dos containers
az container logs --name mysql-aci --resource-group MottuGrid
az container logs --name api-aci --resource-group MottuGrid

# Listar todos os containers no resource group
az container list --resource-group MottuGrid --output table

# Obter credenciais do ACR
az acr credential show --name <Nome ACR>
```
