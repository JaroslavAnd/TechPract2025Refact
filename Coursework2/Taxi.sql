CREATE DATABASE TaxiService;

USE TaxiService;

CREATE TABLE Customers (
id INT IDENTITY(1,1) PRIMARY KEY,
name VARCHAR(100) NOT NULL,
phone VARCHAR(15) NOT NULL UNIQUE,
email VARCHAR(100),
address TEXT
);

CREATE TABLE Drivers (
id INT IDENTITY(1,1) PRIMARY KEY,
name VARCHAR(100) NOT NULL,
phone VARCHAR(15) NOT NULL UNIQUE,
license_number VARCHAR(50) NOT NULL UNIQUE,
rating DECIMAL(2,1) DEFAULT 5.0
);

CREATE TABLE Vehicles (
id INT IDENTITY(1,1) PRIMARY KEY,
driver_id INT NOT NULL,
make VARCHAR(50),
model VARCHAR(50),
license_plate VARCHAR(15) NOT NULL UNIQUE,
year INT,
FOREIGN KEY (driver_id) REFERENCES Drivers(id)
);

CREATE TABLE Rides (
id INT IDENTITY(1,1) PRIMARY KEY,
customer_id INT NOT NULL,
driver_id INT NOT NULL,
pickup_location TEXT NOT NULL,
dropoff_location TEXT NOT NULL,
fare DECIMAL(10,2) NOT NULL,
ride_date DATETIME DEFAULT GETDATE(),
FOREIGN KEY (customer_id) REFERENCES Customers(id),
FOREIGN KEY (driver_id) REFERENCES Drivers(id)
);

CREATE TABLE Rates (
id INT IDENTITY(1,1) PRIMARY KEY,
type VARCHAR(50) NOT NULL,
base_fare DECIMAL(10,2) NOT NULL,
per_km_rate DECIMAL(10,2) NOT NULL,
per_min_rate DECIMAL(10,2) NOT NULL
);

CREATE TABLE Payments (
id INT IDENTITY(1,1) PRIMARY KEY,
ride_id INT NOT NULL,
amount DECIMAL(10,2) NOT NULL,
payment_method VARCHAR(50) NOT NULL,
payment_date DATETIME DEFAULT GETDATE(),
FOREIGN KEY (ride_id) REFERENCES Rides(id)
);

CREATE TABLE AuditLog (
id INT IDENTITY(1,1) PRIMARY KEY,
event_type VARCHAR(50) NOT NULL,
table_name VARCHAR(50) NOT NULL,
record_id INT,
event_time DATETIME DEFAULT GETDATE(),
details TEXT
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    Role NVARCHAR(20) NOT NULL
);



ALTER TABLE Rides
ADD rate_id INT;

ALTER TABLE Rides
ADD CONSTRAINT FK_Rides_Rates
FOREIGN KEY (rate_id)
REFERENCES Rates(id)
ON DELETE SET NULL;

ALTER TABLE Customers
ADD user_id INT;

ALTER TABLE Customers
ADD CONSTRAINT FK_Customers_Users
FOREIGN KEY (user_id)
REFERENCES Users(Id)
ON DELETE SET NULL;

ALTER TABLE Rides
ALTER COLUMN customer_id INT NULL;

ALTER TABLE Rides
ADD CONSTRAINT FK_Rides_Customers
FOREIGN KEY (customer_id)
REFERENCES Customers(id)
ON DELETE SET NULL;

SELECT *
FROM Rides
WHERE customer_id IS NOT NULL
  AND customer_id NOT IN (SELECT id FROM Customers);



ALTER TABLE Rides
ADD CONSTRAINT FK_Rides_Customers FOREIGN KEY (customer_id)
REFERENCES Customers(id) ON DELETE CASCADE;

ALTER TABLE Payments
ADD CONSTRAINT FK_Payments_Rides FOREIGN KEY (ride_id)
REFERENCES Rides(id) ON DELETE CASCADE;

ALTER TABLE Vehicles
ADD CONSTRAINT FK_Vehicles_Rates
FOREIGN KEY (rate_id) REFERENCES Rates(id)
ON DELETE CASCADE;




ALTER TABLE Rides
DROP CONSTRAINT FK_Rides_Drivers;

ALTER TABLE Rides
ADD CONSTRAINT FK_Rides_Drivers
FOREIGN KEY (driver_id) REFERENCES Drivers(id)
ON DELETE NO ACTION;

ALTER TABLE Vehicles
DROP CONSTRAINT FK_Vehicles_Drivers;

ALTER TABLE Vehicles
ADD CONSTRAINT FK_Vehicles_Drivers
FOREIGN KEY (driver_id) REFERENCES Drivers(id)
ON DELETE NO ACTION;


INSERT INTO Customers (name, phone, email,address, user_id)
VALUES
('Олександр Іваненко', '380501234567','example1@gmail.com' ,'вул. Центральна, 10, Київ',1023),
('Марія Коваленко', '380671234568','example2@gmail.com' ,'вул. Грушевського, 15, Львів',1024),
('Іван Петренко', '380931234569','example3@gmail.com' ,'вул. Соборна, 20, Одеса',1025),
('Анна Зінченко', '380661234570','example4@gmail.com' ,'вул. Миру, 8, Харків',1026),
('Дмитро Гнатюк', '380991234571','example5@gmail.com' ,'вул. Перемоги, 4, Дніпро',1027),
('Олена Тарасова', '380671234572','example6@gmail.com' ,'вул. Шевченка, 5, Запоріжжя',1028),
('Вікторія Мельник', '380681234573','example7@gmail.com' ,'вул. Лесі Українки, 12, Черкаси',1029),
('Петро Михайлов', '380691234574','example8@gmail.com' ,'вул. Небесної Сотні, 7, Київ',1030),
('Катерина Гончарова', '380701234575','example9@gmail.com' ,'вул. Залізнична, 9, Харків',1031),
('Максим Сидоренко', '380711234576','example10@gmail.com' ,'вул. Мечникова, 11, Львів',1032),
('Вадим Шевченко', '380721234577','example11@gmail.com' ,'вул. Садова, 8, Одеса',1033),
('Ірина Бондаренко', '380731234578','example12@gmail.com' ,'вул. Петрова, 6, Дніпро',1034),
('Наталія Іванова', '380741234579','example13@gmail.com' ,'вул. Лугова, 14, Київ',1035),
('Андрій Ковальчук', '380751234580','example14@gmail.com' ,'вул. Центральна, 16, Львів',1036),
('Юлія Костенко', '380761234581','example15@gmail.com' ,'вул. Спортивна, 10, Черкаси',1037);


INSERT INTO Drivers (name, phone, license_number, rating)
VALUES
('Іван Іванов', '+380501244567', 'AR123456', 4.5),
('Петро Петренко', '+380505345678', 'BT234567', 5.0),
('Сергій Сергієнко', '+380506456789', 'CY345678', 4.8),
('Микола Миколайчук', '+380504767890', 'DU456789', 4.7),
('Олександр Олексієнко', '+380505878901', 'EI567890', 5.0);

INSERT INTO Vehicles (driver_id, make, model, license_plate, year, rate_id)
VALUES
(44, 'Toyota', 'Camry', 'ABC123456', 2019, 41),
(45, 'Hyundai', 'Elantra', 'DEF654321', 2020,42),
(46, 'Skoda', 'Octavia', 'GHI789123', 2021,43),
(47, 'Volkswagen', 'Passat', 'JKL456789', 2018,44),
(48, 'Nissan', 'Altima', 'MNO123456', 2022,45);

INSERT INTO Rates (type, base_fare, per_km_rate, per_min_rate)
VALUES
('Standard', 50.00, 10.00, 5.00),
('Premium', 100.00, 15.00, 7.50),
('Economy', 30.00, 8.00, 4.00),
('Luxury', 150.00, 20.00, 10.00),
('VIP', 200.00, 25.00, 12.00);


INSERT INTO Rides (customer_id, driver_id, pickup_location, dropoff_location, fare, ride_date, rate_id)
VALUES
(1203, 39, 'вул. Центральна, 10, Київ', 'вул. Грушевського, 15, Львів', 100.50, '2024-12-01 10:00:00',41),
(1204, 40, 'вул. Грушевського, 15, Львів', 'вул. Соборна, 20, Одеса', 150.75, '2024-12-01 11:30:00',42),
(1205, 41, 'вул. Соборна, 20, Одеса', 'вул. Миру, 8, Харків', 120.25, '2024-12-02 09:00:00',43),
(1206, 42, 'вул. Миру, 8, Харків', 'вул. Перемоги, 4, Дніпро', 140.00, '2024-12-02 12:00:00',44),
(1207, 43, 'вул. Перемоги, 4, Дніпро', 'вул. Шевченка, 5, Запоріжжя', 130.00, '2024-12-03 08:30:00',41),
(1208, 39, 'вул. Шевченка, 5, Запоріжжя', 'вул. Лесі Українки, 12, Черкаси', 125.50, '2024-12-03 15:45:00',42),
(1209, 40, 'вул. Лесі Українки, 12, Черкаси', 'вул. Небесної Сотні, 7, Київ', 160.00, '2024-12-04 10:00:00',43),
(1210, 41, 'вул. Небесної Сотні, 7, Київ', 'вул. Залізнична, 9, Харків', 110.00, '2024-12-04 13:30:00',44),
(1211, 42, 'вул. Залізнична, 9, Харків', 'вул. Мечникова, 11, Львів', 115.50, '2024-12-05 08:00:00',41),
(1212, 43, 'вул. Мечникова, 11, Львів', 'вул. Садова, 8, Одеса', 135.00, '2024-12-05 14:30:00',42),
(1213, 39, 'вул. Садова, 8, Одеса', 'вул. Петрова, 6, Дніпро', 140.00, '2024-12-06 09:30:00',43),
(1214, 40, 'вул. Петрова, 6, Дніпро', 'вул. Лугова, 14, Київ', 150.00, '2024-12-06 12:45:00',44),
(1215, 41, 'вул. Лугова, 14, Київ', 'вул. Центральна, 16, Львів', 160.00, '2024-12-07 11:00:00',41),
(1216, 42, 'вул. Центральна, 16, Львів', 'вул. Спортивна, 10, Черкаси', 130.00, '2024-12-07 16:30:00',42),
(1217, 43, 'вул. Спортивна, 10, Черкаси', 'вул. Шевченка, 5, Запоріжжя', 145.00, '2024-12-08 10:15:00',43);

INSERT INTO Payments (ride_id, amount, payment_method, payment_date)
VALUES
(1101, 150.50, 'Карта', '2023-12-01 14:30:00'),
(1102, 200.00, 'Готівка', '2023-12-01 15:00:00'),
(1103, 75.00, 'Карта', '2023-12-02 09:15:00'),
(1104, 120.75, 'Карта', '2023-12-02 11:45:00'),
(1105, 300.00, 'Готівка', '2023-12-03 18:10:00'),
(1106, 99.99, 'Карта', '2023-12-03 20:20:00'),
(1107, 180.20, 'Готівка', '2023-12-04 07:50:00'),
(1108, 250.00, 'Карта', '2023-12-04 12:30:00'),
(1109, 135.50, 'Карта', '2023-12-05 10:00:00'),
(1110, 400.00, 'Карта', '2023-12-05 19:00:00'),
(1111, 220.10, 'Готівка', '2023-12-06 08:45:00'),
(1112, 160.30, 'Карта', '2023-12-06 14:20:00'),
(1113, 310.00, 'Карта', '2023-12-07 16:00:00'),
(1114, 275.75, 'Готівка', '2023-12-08 09:35:00'),
(1115, 190.40, 'Готівка', '2023-12-09 11:15:00');


INSERT INTO Users (Username, PasswordHash, Role)
VALUES
('admin', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'adminPassword'), 2), 'Admin'),
('user1', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user1Password'), 2), 'User'),
('user2', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user2Password'), 2), 'User'),
('user3', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user3Password'), 2), 'User'),
('user4', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user4Password'), 2), 'User'),
('user5', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user5Password'), 2), 'User'),
('user6', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user6Password'), 2), 'User'),
('user7', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user7Password'), 2), 'User'),
('user8', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user8Password'), 2), 'User'),
('user9', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user9Password'), 2), 'User'),
('user10', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user10Password'), 2), 'User'),
('user11', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user11Password'), 2), 'User'),
('user12', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user12Password'), 2), 'User'),
('user13', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user13Password'), 2), 'User'),
('user14', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user14Password'), 2), 'User'),
('user15', CONVERT(VARCHAR(64), HASHBYTES('SHA2_256', 'user15Password'), 2),'User');




DELETE FROM Customers
DELETE FROM Drivers
DELETE FROM Rates
DELETE FROM Vehicles
DELETE FROM Payments

DELETE FROM AuditLog
DELETE FROM Users

DELETE FROM Rides
WHERE id = 1120;


EXEC sp_helpconstraint 'Customers';
EXEC sp_helpconstraint 'Rides';
EXEC sp_helpconstraint 'Users';
EXEC sp_columns users;

SELECT* FROM Customers
SELECT* FROM Users
SELECT* FROM Vehicles
SELECT* FROM Rides
SELECT* FROM Rates
SELECT* FROM Drivers	
SELECT* FROM Payments	

ALTER TABLE Customers
ADD User_id INT;


ALTER TABLE Customers
ADD CONSTRAINT FK_Customers_Users
FOREIGN KEY (User_id)
REFERENCES Users(Id);

UPDATE Customers
SET User_id = (SELECT Id FROM Users WHERE Users.phone_number = Customers.phone)
WHERE EXISTS (SELECT 1 FROM Users WHERE Users.phone_number = Customers.phone);


IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TR' AND name = 'trg_Customers_Log')
DROP TRIGGER trg_Customers_Log;

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TR' AND name = 'trg_Drivers_Log')
DROP TRIGGER trg_Drivers_Log;

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'TR' AND name = 'trg_Rides_Log')
DROP TRIGGER trg_Rides_Log;

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ClearTable')
DROP PROCEDURE ClearTable;

IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetAuditLog')
DROP PROCEDURE GetAuditLog;


IF OBJECT_ID('trg_Customers_Log', 'TR') IS NOT NULL
    DROP TRIGGER trg_Customers_Log;
GO

GO
CREATE OR ALTER TRIGGER trg_Customers_Log
ON Customers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EventType VARCHAR(50), @TableName VARCHAR(50) = 'Customers';

    IF EXISTS (SELECT * FROM inserted) AND NOT EXISTS (SELECT * FROM deleted)
    BEGIN
        SET @EventType = 'INSERT';
        INSERT INTO AuditLog (event_type, table_name, record_id, details)
        SELECT @EventType, @TableName, id, 
            CONCAT('Name: ', name, ', Phone: ', phone, ', Email: ', email, ', Address: ', address)
        FROM inserted;
    END

    IF EXISTS (SELECT * FROM deleted) AND NOT EXISTS (SELECT * FROM inserted)
    BEGIN
        SET @EventType = 'DELETE';
        INSERT INTO AuditLog (event_type, table_name, record_id, details)
        SELECT @EventType, @TableName, id, 
            CONCAT('Name: ', name, ', Phone: ', phone, ', Email: ', email, ', Address: ', address)
        FROM deleted;
    END

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        SET @EventType = 'UPDATE';
        INSERT INTO AuditLog (event_type, table_name, record_id, details)
        SELECT @EventType, @TableName, i.id,
            CONCAT('Old -> Name: ', d.name, ', New -> Name: ', i.name,
                   '; Old -> Phone: ', d.phone, ', New -> Phone: ', i.phone,
                   '; Old -> Email: ', d.email, ', New -> Email: ', i.email,
                   '; Old -> Address: ', d.address, ', New -> Address: ', i.address)
        FROM inserted i
        JOIN deleted d ON i.id = d.id;
    END
END;
GO


IF OBJECT_ID('trg_Drivers_Log', 'TR') IS NOT NULL
    DROP TRIGGER trg_Drivers_Log;
GO

CREATE TRIGGER trg_Drivers_Log
ON Drivers
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EventType VARCHAR(50);

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @EventType = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @EventType = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @EventType = 'DELETE';

    IF @EventType = 'INSERT' OR @EventType = 'UPDATE'
    BEGIN
        INSERT INTO AuditLog (event_type, table_name, record_id, details, event_time)
        SELECT 
            @EventType, 
            'Drivers', 
            id, 
            CONCAT('Name: ', ISNULL(name, 'NULL'),
                   ', Phone: ', ISNULL(phone, 'NULL'),
                   ', License: ', ISNULL(license_number, 'NULL'),
                   ', Rating: ', ISNULL(CAST(rating AS NVARCHAR), 'NULL')), 
            GETDATE()
        FROM inserted;
    END
    ELSE IF @EventType = 'DELETE'
    BEGIN
        INSERT INTO AuditLog (event_type, table_name, record_id, details, event_time)
        SELECT 
            'DELETE', 
            'Drivers', 
            id, 
            CONCAT('Name: ', ISNULL(name, 'NULL'),
                   ', Phone: ', ISNULL(phone, 'NULL'),
                   ', License: ', ISNULL(license_number, 'NULL'),
                   ', Rating: ', ISNULL(CAST(rating AS NVARCHAR), 'NULL')),
            GETDATE()
        FROM deleted;
    END
END;
GO

IF OBJECT_ID('trg_Rides_Log', 'TR') IS NOT NULL
    DROP TRIGGER trg_Rides_Log;
GO

CREATE TRIGGER trg_Rides_Log
ON Rides
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @EventType VARCHAR(50);

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @EventType = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @EventType = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @EventType = 'DELETE';

    IF @EventType = 'INSERT' OR @EventType = 'UPDATE'
    BEGIN
        INSERT INTO AuditLog (event_type, table_name, record_id, details, event_time)
        SELECT 
            @EventType, 
            'Rides', 
            id, 
            CONCAT('CustomerID: ', ISNULL(CAST(customer_id AS NVARCHAR), 'NULL'),
                   ', DriverID: ', ISNULL(CAST(driver_id AS NVARCHAR), 'NULL'),
                   ', Pickup: ', ISNULL(pickup_location, 'NULL'),
                   ', Dropoff: ', ISNULL(dropoff_location, 'NULL'),
                   ', Fare: ', ISNULL(CAST(fare AS NVARCHAR), 'NULL')),
            GETDATE()
        FROM inserted;
    END
    ELSE IF @EventType = 'DELETE'
    BEGIN
        INSERT INTO AuditLog (event_type, table_name, record_id, details, event_time)
        SELECT 
            'DELETE', 
            'Rides', 
            id, 
            CONCAT('CustomerID: ', ISNULL(CAST(customer_id AS NVARCHAR), 'NULL'),
                   ', DriverID: ', ISNULL(CAST(driver_id AS NVARCHAR), 'NULL'),
                   ', Pickup: ', ISNULL(pickup_location, 'NULL'),
                   ', Dropoff: ', ISNULL(dropoff_location, 'NULL'),
                   ', Fare: ', ISNULL(CAST(fare AS NVARCHAR), 'NULL')),
            GETDATE()
        FROM deleted;
    END
END;
GO

CREATE TRIGGER trg_Payments_Log
ON Payments
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @EventType VARCHAR(50), @RecordID INT, @Details VARCHAR(MAX);

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @EventType = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @EventType = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @EventType = 'DELETE';

    IF @EventType = 'INSERT' OR @EventType = 'UPDATE'
    BEGIN
        SELECT @RecordID = id, @Details = CONCAT('Amount: ', amount, ', Date: ', payment_date, ', Method: ', payment_method)
        FROM inserted;
    END

    ELSE IF @EventType = 'DELETE'
    BEGIN
        SELECT @RecordID = id, @Details = CONCAT('Amount: ', amount, ', Date: ', payment_date, ', Method: ', payment_method)
        FROM deleted;
    END

    INSERT INTO AuditLog (event_type, table_name, record_id, details)
    VALUES (@EventType, 'Payments', @RecordID, @Details);
END;
GO

CREATE TRIGGER trg_Vehicles_Log
ON Vehicles
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @EventType VARCHAR(50), @RecordID INT, @Details VARCHAR(MAX);

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @EventType = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM inserted)
        SET @EventType = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @EventType = 'DELETE';

    IF @EventType = 'INSERT' OR @EventType = 'UPDATE'
    BEGIN
        SELECT @RecordID = id, @Details = CONCAT('Driver ID: ', driver_id, ', Make: ', make, ', Model: ', model, ', License Plate: ', license_plate, ', Year: ', year)
        FROM inserted;
    END

    ELSE IF @EventType = 'DELETE'
    BEGIN
        SELECT @RecordID = id, @Details = CONCAT('Driver ID: ', driver_id, ', Make: ', make, ', Model: ', model, ', License Plate: ', license_plate, ', Year: ', year)
        FROM deleted;
    END

    INSERT INTO AuditLog (event_type, table_name, record_id, details)
    VALUES (@EventType, 'Vehicles', @RecordID, @Details);
END;
GO

CREATE TRIGGER trg_Rates_Log
ON Rates
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    DECLARE @EventType VARCHAR(50), @RecordID INT, @Details VARCHAR(MAX);

    
    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
        SET @EventType = 'UPDATE';
    ELSE IF EXISTS (SELECT * FROM	)
        SET @EventType = 'INSERT';
    ELSE IF EXISTS (SELECT * FROM deleted)
        SET @EventType = 'DELETE';

    IF @EventType = 'INSERT' OR @EventType = 'UPDATE'
    BEGIN
        SELECT @RecordID = id, @Details = CONCAT('Type: ', type, ', Base Fare: ', base_fare, ', Per Km Rate: ', per_km_rate, ', Per Min Rate: ', per_min_rate)
        FROM inserted;
    END
 
    ELSE IF @EventType = 'DELETE'
    BEGIN
        SELECT @RecordID = id, @Details = CONCAT('Type: ', type, ', Base Fare: ', base_fare, ', Per Km Rate: ', per_km_rate, ', Per Min Rate: ', per_min_rate)
        FROM deleted;
    END

    INSERT INTO AuditLog (event_type, table_name, record_id, details)
    VALUES (@EventType, 'Rates', @RecordID, @Details);
END;
GO

CREATE PROCEDURE GetDriverRides
    @driver_id INT
AS
BEGIN
    SELECT r.id AS RideID, c.name AS CustomerName, r.pickup_location, r.dropoff_location, r.fare
    FROM Rides r
    JOIN Customers c ON r.customer_id = c.id
    WHERE r.driver_id = @driver_id;
END;
GO

CREATE PROCEDURE GetPaymentsByDateRange
    @start_date DATE,
    @end_date DATE
AS
BEGIN
    SELECT p.id AS PaymentID, r.id AS RideID, c.name AS CustomerName, p.amount, p.payment_method, p.payment_date
    FROM Payments p
    JOIN Rides r ON p.ride_id = r.id
    JOIN Customers c ON r.customer_id = c.id
    WHERE p.payment_date BETWEEN @start_date AND @end_date;
END;
GO

CREATE PROCEDURE GetCustomerRideCount
AS
BEGIN
    SELECT c.id AS CustomerID, c.name AS CustomerName, COUNT(r.id) AS RideCount
    FROM Customers c
    LEFT JOIN Rides r ON c.id = r.customer_id
    GROUP BY c.id, c.name;
END;
GO


GO
CREATE PROCEDURE GetSystemActivityReport
    @start_date DATE = NULL,
    @end_date DATE = NULL,
    @event_type NVARCHAR(50) = NULL
AS
BEGIN
    SELECT 
        al.id AS LogID,
        al.event_type AS EventType,
        al.table_name AS TableName,
        al.record_id AS RecordID,
        al.event_time AS Timestamp
    FROM AuditLog al
    WHERE (@start_date IS NULL OR al.event_time >= @start_date)
      AND (@event_type IS NULL OR al.event_type = @event_type)
    ORDER BY al.event_time DESC;
END;
