-- =======================================
-- 1. Addresses
-- =======================================
SET @addr1 = UUID(); SET @addr2 = UUID(); SET @addr3 = UUID(); SET @addr4 = UUID(); SET @addr5 = UUID();
SET @addr6 = UUID(); SET @addr7 = UUID(); SET @addr8 = UUID(); SET @addr9 = UUID(); SET @addr10 = UUID();

INSERT INTO addresses (id, street, number, neighborhood, city, state, zip_code, country)
VALUES
    (@addr1, 'Rua 1', '100', 'Centro', 'Cidade', 'Estado', '00000001', 'Brasil'),
    (@addr2, 'Rua 2', '200', 'Centro', 'Cidade', 'Estado', '00000002', 'Brasil'),
    (@addr3, 'Rua 3', '300', 'Centro', 'Cidade', 'Estado', '00000003', 'Brasil'),
    (@addr4, 'Rua 4', '400', 'Centro', 'Cidade', 'Estado', '00000004', 'Brasil'),
    (@addr5, 'Rua 5', '500', 'Centro', 'Cidade', 'Estado', '00000005', 'Brasil'),
    (@addr6, 'Rua 6', '600', 'Centro', 'Cidade', 'Estado', '00000006', 'Brasil'),
    (@addr7, 'Rua 7', '700', 'Centro', 'Cidade', 'Estado', '00000007', 'Brasil'),
    (@addr8, 'Rua 8', '800', 'Centro', 'Cidade', 'Estado', '00000008', 'Brasil'),
    (@addr9, 'Rua 9', '900', 'Centro', 'Cidade', 'Estado', '00000009', 'Brasil'),
    (@addr10,'Rua 10','1000','Centro', 'Cidade', 'Estado', '00000010', 'Brasil');

-- =======================================
-- 2. Yards
-- =======================================
SET @yard1 = UUID(); SET @yard2 = UUID(); SET @yard3 = UUID(); SET @yard4 = UUID(); SET @yard5 = UUID();
SET @yard6 = UUID(); SET @yard7 = UUID(); SET @yard8 = UUID(); SET @yard9 = UUID(); SET @yard10 = UUID();

INSERT INTO yards (id, name, address_id)
VALUES
    (@yard1,'Pátio 1',@addr1),
    (@yard2,'Pátio 2',@addr2),
    (@yard3,'Pátio 3',@addr3),
    (@yard4,'Pátio 4',@addr4),
    (@yard5,'Pátio 5',@addr5),
    (@yard6,'Pátio 6',@addr6),
    (@yard7,'Pátio 7',@addr7),
    (@yard8,'Pátio 8',@addr8),
    (@yard9,'Pátio 9',@addr9),
    (@yard10,'Pátio 10',@addr10);

-- =======================================
-- 2a. YardPoints (4 pontos por yard)
-- =======================================
INSERT INTO yard_points (Id, YardId, point_order, x, y)
VALUES
    (UUID(), @yard1,1,0,0), (UUID(), @yard1,2,0,50), (UUID(), @yard1,3,50,50), (UUID(), @yard1,4,50,0),
    (UUID(), @yard2,1,0,0), (UUID(), @yard2,2,0,60), (UUID(), @yard2,3,60,60), (UUID(), @yard2,4,60,0),
    (UUID(), @yard3,1,0,0), (UUID(), @yard3,2,0,40), (UUID(), @yard3,3,40,40), (UUID(), @yard3,4,40,0),
    (UUID(), @yard4,1,0,0), (UUID(), @yard4,2,0,45), (UUID(), @yard4,3,45,45), (UUID(), @yard4,4,45,0),
    (UUID(), @yard5,1,0,0), (UUID(), @yard5,2,0,55), (UUID(), @yard5,3,55,55), (UUID(), @yard5,4,55,0),
    (UUID(), @yard6,1,0,0), (UUID(), @yard6,2,0,35), (UUID(), @yard6,3,35,35), (UUID(), @yard6,4,35,0),
    (UUID(), @yard7,1,0,0), (UUID(), @yard7,2,0,65), (UUID(), @yard7,3,65,65), (UUID(), @yard7,4,65,0),
    (UUID(), @yard8,1,0,0), (UUID(), @yard8,2,0,50), (UUID(), @yard8,3,50,50), (UUID(), @yard8,4,50,0),
    (UUID(), @yard9,1,0,0), (UUID(), @yard9,2,0,60), (UUID(), @yard9,3,60,60), (UUID(), @yard9,4,60,0),
    (UUID(), @yard10,1,0,0), (UUID(), @yard10,2,0,70), (UUID(), @yard10,3,70,70), (UUID(), @yard10,4,70,0);

-- =======================================
-- 3. SectorTypes
-- =======================================
SET @stype1 = UUID(); SET @stype2 = UUID(); SET @stype3 = UUID(); SET @stype4 = UUID(); SET @stype5 = UUID();
SET @stype6 = UUID(); SET @stype7 = UUID(); SET @stype8 = UUID(); SET @stype9 = UUID(); SET @stype10 = UUID();

INSERT INTO sector_types (id,name)
VALUES
    (@stype1,'VIP'),(@stype2,'Normal'),(@stype3,'Moto'),(@stype4,'Carga'),(@stype5,'Visitante'),
    (@stype6,'Reserva'),(@stype7,'Rapido'),(@stype8,'Longo'),(@stype9,'Especial'),(@stype10,'Exclusivo');

-- =======================================
-- 4. Sectors
-- =======================================
SET @sec1 = UUID(); SET @sec2 = UUID(); SET @sec3 = UUID(); SET @sec4 = UUID(); SET @sec5 = UUID();
SET @sec6 = UUID(); SET @sec7 = UUID(); SET @sec8 = UUID(); SET @sec9 = UUID(); SET @sec10 = UUID();

INSERT INTO sectors (id, yard_id, sector_type_id)
VALUES
    (@sec1,@yard1,@stype1),(@sec2,@yard2,@stype2),(@sec3,@yard3,@stype3),(@sec4,@yard4,@stype4),
    (@sec5,@yard5,@stype5),(@sec6,@yard6,@stype6),(@sec7,@yard7,@stype7),(@sec8,@yard8,@stype8),
    (@sec9,@yard9,@stype9),(@sec10,@yard10,@stype10);

-- =======================================
-- 4a. SectorPoints (4 pontos por setor)
-- =======================================
INSERT INTO sector_points (Id,SectorId,point_order,x,y)
VALUES
    (UUID(),@sec1,1,5,5),(UUID(),@sec1,2,5,20),(UUID(),@sec1,3,20,20),(UUID(),@sec1,4,20,5),
    (UUID(),@sec2,1,5,5),(UUID(),@sec2,2,5,25),(UUID(),@sec2,3,25,25),(UUID(),@sec2,4,25,5),
    (UUID(),@sec3,1,3,3),(UUID(),@sec3,2,3,15),(UUID(),@sec3,3,15,15),(UUID(),@sec3,4,15,3),
    (UUID(),@sec4,1,4,4),(UUID(),@sec4,2,4,18),(UUID(),@sec4,3,18,18),(UUID(),@sec4,4,18,4),
    (UUID(),@sec5,1,5,5),(UUID(),@sec5,2,5,22),(UUID(),@sec5,3,22,22),(UUID(),@sec5,4,22,5),
    (UUID(),@sec6,1,3,3),(UUID(),@sec6,2,3,14),(UUID(),@sec6,3,14,14),(UUID(),@sec6,4,14,3),
    (UUID(),@sec7,1,6,6),(UUID(),@sec7,2,6,26),(UUID(),@sec7,3,26,26),(UUID(),@sec7,4,26,6),
    (UUID(),@sec8,1,5,5),(UUID(),@sec8,2,5,20),(UUID(),@sec8,3,20,20),(UUID(),@sec8,4,20,5),
    (UUID(),@sec9,1,6,6),(UUID(),@sec9,2,6,24),(UUID(),@sec9,3,24,24),(UUID(),@sec9,4,24,6),
    (UUID(),@sec10,1,7,7),(UUID(),@sec10,2,7,28),(UUID(),@sec10,3,28,28),(UUID(),@sec10,4,28,7);

-- =======================================
-- 5. Spots (5 por setor)
-- =======================================
SET @spot1 = UUID(); SET @spot2 = UUID(); SET @spot3 = UUID(); SET @spot4 = UUID(); SET @spot5 = UUID();
SET @spot6 = UUID(); SET @spot7 = UUID(); SET @spot8 = UUID(); SET @spot9 = UUID(); SET @spot10 = UUID();
SET @spot11 = UUID(); SET @spot12 = UUID(); SET @spot13 = UUID(); SET @spot14 = UUID(); SET @spot15 = UUID();
SET @spot16 = UUID(); SET @spot17 = UUID(); SET @spot18 = UUID(); SET @spot19 = UUID(); SET @spot20 = UUID();
SET @spot21 = UUID(); SET @spot22 = UUID(); SET @spot23 = UUID(); SET @spot24 = UUID(); SET @spot25 = UUID();
SET @spot26 = UUID(); SET @spot27 = UUID(); SET @spot28 = UUID(); SET @spot29 = UUID(); SET @spot30 = UUID();
SET @spot31 = UUID(); SET @spot32 = UUID(); SET @spot33 = UUID(); SET @spot34 = UUID(); SET @spot35 = UUID();
SET @spot36 = UUID(); SET @spot37 = UUID(); SET @spot38 = UUID(); SET @spot39 = UUID(); SET @spot40 = UUID();
SET @spot41 = UUID(); SET @spot42 = UUID(); SET @spot43 = UUID(); SET @spot44 = UUID(); SET @spot45 = UUID();
SET @spot46 = UUID(); SET @spot47 = UUID(); SET @spot48 = UUID(); SET @spot49 = UUID(); SET @spot50 = UUID();

INSERT INTO spots (spot_id,sector_id,x,y,status)
VALUES
    (@spot1,@sec1,6,6,'FREE'),(@spot2,@sec1,7,6,'FREE'),(@spot3,@sec1,6,7,'FREE'),(@spot4,@sec1,7,7,'FREE'),(@spot5,@sec1,6.5,6.5,'FREE'),
    (@spot6,@sec2,6,6,'FREE'),(@spot7,@sec2,7,6,'FREE'),(@spot8,@sec2,6,7,'FREE'),(@spot9,@sec2,7,7,'FREE'),(@spot10,@sec2,6.5,6.5,'FREE'),
    (@spot11,@sec3,4,4,'FREE'),(@spot12,@sec3,5,4,'FREE'),(@spot13,@sec3,4,5,'FREE'),(@spot14,@sec3,5,5,'FREE'),(@spot15,@sec3,4.5,4.5,'FREE'),
    (@spot16,@sec4,5,5,'FREE'),(@spot17,@sec4,6,5,'FREE'),(@spot18,@sec4,5,6,'FREE'),(@spot19,@sec4,6,6,'FREE'),(@spot20,@sec4,5.5,5.5,'FREE'),
    (@spot21,@sec5,6,6,'FREE'),(@spot22,@sec5,7,6,'FREE'),(@spot23,@sec5,6,7,'FREE'),(@spot24,@sec5,7,7,'FREE'),(@spot25,@sec5,6.5,6.5,'FREE'),
    (@spot26,@sec6,4,4,'FREE'),(@spot27,@sec6,5,4,'FREE'),(@spot28,@sec6,4,5,'FREE'),(@spot29,@sec6,5,5,'FREE'),(@spot30,@sec6,4.5,4.5,'FREE'),
    (@spot31,@sec7,6,6,'FREE'),(@spot32,@sec7,7,6,'FREE'),(@spot33,@sec7,6,7,'FREE'),(@spot34,@sec7,7,7,'FREE'),(@spot35,@sec7,6.5,6.5,'FREE'),
    (@spot36,@sec8,5,5,'FREE'),(@spot37,@sec8,6,5,'FREE'),(@spot38,@sec8,5,6,'FREE'),(@spot39,@sec8,6,6,'FREE'),(@spot40,@sec8,5.5,5.5,'FREE'),
    (@spot41,@sec9,6,6,'FREE'),(@spot42,@sec9,7,6,'FREE'),(@spot43,@sec9,6,7,'FREE'),(@spot44,@sec9,7,7,'FREE'),(@spot45,@sec9,6.5,6.5,'FREE'),
    (@spot46,@sec10,5,5,'FREE'),(@spot47,@sec10,6,5,'FREE'),(@spot48,@sec10,5,6,'FREE'),(@spot49,@sec10,6,6,'FREE'),(@spot50,@sec10,5.5,5.5,'FREE');

-- =======================================
-- 6. Motorcycles (2 por setor, engineType COMBUSTION)
-- =======================================
SET @moto1 = UUID(); SET @moto2 = UUID(); SET @moto3 = UUID(); SET @moto4 = UUID(); SET @moto5 = UUID();
SET @moto6 = UUID(); SET @moto7 = UUID(); SET @moto8 = UUID(); SET @moto9 = UUID(); SET @moto10 = UUID();

INSERT INTO Motorcycles (id,model,engineType,plate,lastRevisionDate)
VALUES
    (@moto1,'Yamaha XJ6','COMBUSTION','ABC1234',NOW()),(@moto2,'Honda CB500','COMBUSTION','DEF5678',NOW()),
    (@moto3,'Suzuki GSX','COMBUSTION','GHI9012',NOW()),(@moto4,'KTM Duke','COMBUSTION','JKL3456',NOW()),
    (@moto5,'BMW F800','COMBUSTION','MNO7890',NOW()),(@moto6,'Ducati Monster','COMBUSTION','PQR1234',NOW()),
    (@moto7,'Triumph Street','COMBUSTION','STU5678',NOW()),(@moto8,'Kawasaki Ninja','COMBUSTION','VWX9012',NOW()),
    (@moto9,'Honda CBR','COMBUSTION','YZA3456',NOW()),(@moto10,'Yamaha R1','COMBUSTION','BCD7890',NOW());

