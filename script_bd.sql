-- =============================================================================
-- SCRIPT DDL - SISTEMA MOTTU CHALLENGE
-- Sistema de Gestão de Pátio e Setores para Motos
-- =============================================================================

-- -----------------------------------------------------------------------------
-- TABELA: addresses
-- Descrição: Armazena informações de endereços dos pátios
-- -----------------------------------------------------------------------------
CREATE TABLE addresses (
    id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do endereço',
    street VARCHAR(150) NOT NULL COMMENT 'Nome da rua/logradouro',
    number INT NOT NULL COMMENT 'Número do endereço',
    neighborhood VARCHAR(100) NOT NULL COMMENT 'Nome do bairro',
    city VARCHAR(100) NOT NULL COMMENT 'Nome da cidade',
    state VARCHAR(50) NOT NULL COMMENT 'Estado/UF',
    zip_code VARCHAR(8) NOT NULL COMMENT 'CEP (8 dígitos)',
    country VARCHAR(100) NOT NULL COMMENT 'País',
    PRIMARY KEY (id)
) COMMENT='Tabela de endereços dos pátios';

-- -----------------------------------------------------------------------------
-- TABELA: yards  
-- Descrição: Representa os pátios físicos das filiais
-- -----------------------------------------------------------------------------
CREATE TABLE yards (
    id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do pátio',
    name VARCHAR(150) NOT NULL COMMENT 'Nome do pátio',
    address_id CHAR(36) NOT NULL COMMENT 'Chave estrangeira para endereço',
    PRIMARY KEY (id),
    FOREIGN KEY (address_id) REFERENCES addresses(id) ON DELETE CASCADE
) COMMENT='Tabela de pátios - espaços físicos das filiais';

-- -----------------------------------------------------------------------------
-- TABELA: yard_points
-- Descrição: Pontos que definem o polígono do pátio
-- -----------------------------------------------------------------------------
CREATE TABLE yard_points (
    Id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do ponto',
    YardId CHAR(36) NOT NULL COMMENT 'Chave estrangeira para o pátio',
    point_order INT NOT NULL COMMENT 'Ordem do ponto no polígono',
    x DOUBLE NOT NULL COMMENT 'Coordenada X do ponto',
    y DOUBLE NOT NULL COMMENT 'Coordenada Y do ponto',
    PRIMARY KEY (Id),
    FOREIGN KEY (YardId) REFERENCES yards(id) ON DELETE CASCADE
) COMMENT='Pontos que definem os limites geográficos dos pátios';

-- -----------------------------------------------------------------------------
-- TABELA: sector_types
-- Descrição: Tipos de setores disponíveis
-- -----------------------------------------------------------------------------
CREATE TABLE sector_types (
    id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do tipo de setor',
    name VARCHAR(100) NOT NULL COMMENT 'Nome do tipo de setor',
    PRIMARY KEY (id)
) COMMENT='Tipos de setores (ex: Estacionamento, Manutenção, Revisão)';

-- -----------------------------------------------------------------------------
-- TABELA: sectors
-- Descrição: Setores dentro dos pátios
-- -----------------------------------------------------------------------------
CREATE TABLE sectors (
    id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do setor',
    yard_id CHAR(36) NOT NULL COMMENT 'Chave estrangeira para o pátio',
    sector_type_id CHAR(36) NOT NULL COMMENT 'Chave estrangeira para tipo de setor',
    PRIMARY KEY (id),
    FOREIGN KEY (yard_id) REFERENCES yards(id) ON DELETE CASCADE,
    FOREIGN KEY (sector_type_id) REFERENCES sector_types(id) ON DELETE RESTRICT
) COMMENT='Setores - áreas específicas dentro dos pátios';

-- -----------------------------------------------------------------------------
-- TABELA: sector_points
-- Descrição: Pontos que definem o polígono do setor
-- -----------------------------------------------------------------------------
CREATE TABLE sector_points (
    Id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do ponto',
    SectorId CHAR(36) NOT NULL COMMENT 'Chave estrangeira para o setor',
    point_order INT NOT NULL COMMENT 'Ordem do ponto no polígono',
    x DOUBLE NOT NULL COMMENT 'Coordenada X do ponto',
    y DOUBLE NOT NULL COMMENT 'Coordenada Y do ponto',
    PRIMARY KEY (Id),
    FOREIGN KEY (SectorId) REFERENCES sectors(id) ON DELETE CASCADE
) COMMENT='Pontos que definem os limites geográficos dos setores';

-- -----------------------------------------------------------------------------
-- TABELA: Motorcycles
-- Descrição: Cadastro das motocicletas
-- -----------------------------------------------------------------------------
CREATE TABLE Motorcycles (
    Id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID da motocicleta',
    Model VARCHAR(100) NOT NULL COMMENT 'Modelo da motocicleta',
    EngineType VARCHAR(50) NOT NULL COMMENT 'Tipo de motor (ELECTRIC, COMBUSTION)',
    Plate VARCHAR(8) NOT NULL COMMENT 'Placa da motocicleta',
    LastRevisionDate DATETIME NOT NULL COMMENT 'Data da última revisão',
    SpotId CHAR(36) NULL COMMENT 'Chave estrangeira para vaga atual',
    PRIMARY KEY (Id)
) COMMENT='Cadastro das motocicletas do sistema';

-- -----------------------------------------------------------------------------
-- TABELA: spots
-- Descrição: Vagas individuais dentro dos setores
-- -----------------------------------------------------------------------------
CREATE TABLE spots (
    spot_id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID da vaga',
    sector_id CHAR(36) NOT NULL COMMENT 'Chave estrangeira para o setor',
    x DOUBLE NOT NULL COMMENT 'Coordenada X da vaga',
    y DOUBLE NOT NULL COMMENT 'Coordenada Y da vaga',
    status VARCHAR(50) NOT NULL COMMENT 'Status da vaga (FREE, OCCUPIED, RESERVED)',
    motorcycle_id CHAR(36) NULL COMMENT 'Chave estrangeira para moto ocupante',
    PRIMARY KEY (spot_id),
    FOREIGN KEY (sector_id) REFERENCES sectors(id) ON DELETE CASCADE,
    FOREIGN KEY (motorcycle_id) REFERENCES Motorcycles(Id) ON DELETE SET NULL
) COMMENT='Vagas individuais para estacionamento de motos';

-- -----------------------------------------------------------------------------
-- TABELA: logs
-- Descrição: Log de movimentações das motocicletas
-- -----------------------------------------------------------------------------
CREATE TABLE logs (
    id CHAR(36) NOT NULL COMMENT 'Chave primária - UUID do log',
    message VARCHAR(150) NOT NULL COMMENT 'Mensagem descritiva do movimento',
    created_at DATETIME NOT NULL COMMENT 'Data e hora do movimento',
    motorcycle_id CHAR(36) NOT NULL COMMENT 'Chave estrangeira para motocicleta',
    previous_spot_id CHAR(36) NOT NULL COMMENT 'Vaga de origem',
    destination_spot_id CHAR(36) NOT NULL COMMENT 'Vaga de destino',
    PRIMARY KEY (id),
    FOREIGN KEY (motorcycle_id) REFERENCES Motorcycles(Id) ON DELETE RESTRICT,
    FOREIGN KEY (previous_spot_id) REFERENCES spots(spot_id) ON DELETE RESTRICT,
    FOREIGN KEY (destination_spot_id) REFERENCES spots(spot_id) ON DELETE RESTRICT
) COMMENT='Histórico de movimentações das motocicletas entre vagas';

-- -----------------------------------------------------------------------------
-- ADICIONANDO CHAVE ESTRANGEIRA SPOT PARA MOTORCYCLE (RELAÇÃO CIRCULAR)
-- -----------------------------------------------------------------------------
ALTER TABLE Motorcycles 
ADD CONSTRAINT FK_Motorcycles_Spots 
FOREIGN KEY (SpotId) REFERENCES spots(spot_id) ON DELETE RESTRICT;

-- -----------------------------------------------------------------------------
-- ÍNDICES PARA MELHOR PERFORMANCE
-- -----------------------------------------------------------------------------
CREATE INDEX IDX_yards_address_id ON yards(address_id);
CREATE INDEX IDX_yard_points_YardId ON yard_points(YardId);
CREATE INDEX IDX_sectors_yard_id ON sectors(yard_id);
CREATE INDEX IDX_sectors_sector_type_id ON sectors(sector_type_id);
CREATE INDEX IDX_sector_points_SectorId ON sector_points(SectorId);
CREATE INDEX IDX_spots_sector_id ON spots(sector_id);
CREATE INDEX IDX_spots_motorcycle_id ON spots(motorcycle_id);
CREATE INDEX IDX_motorcycles_SpotId ON Motorcycles(SpotId);
CREATE INDEX IDX_logs_motorcycle_id ON logs(motorcycle_id);
CREATE INDEX IDX_logs_created_at ON logs(created_at);

-- =============================================================================
-- RESUMO DA ESTRUTURA:
-- 
-- 1. addresses: Endereços dos pátios
-- 2. yards: Pátios das filiais
-- 3. yard_points: Coordenadas geográficas dos pátios
-- 4. sector_types: Tipos de setores
-- 5. sectors: Setores dentro dos pátios
-- 6. sector_points: Coordenadas geográficas dos setores  
-- 7. Motorcycles: Cadastro das motocicletas
-- 8. spots: Vagas individuais para motos
-- 9. logs: Histórico de movimentações
--
-- RELACIONAMENTOS PRINCIPAIS:
-- - Yard 1:1 Address
-- - Yard 1:N Sectors
-- - Sector 1:N Spots
-- - Spot 1:1 Motorcycle (opcional)
-- - Motorcycle 1:N Logs
-- =============================================================================