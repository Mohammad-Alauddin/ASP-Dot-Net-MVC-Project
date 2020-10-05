USE master;
GO
CREATE DATABASE SuperShopMVC;
--DROP DATABASE SuperShopMVC;
GO
USE SuperShopMVC;
GO
CREATE TABLE Customer (
CustomeID INT IDENTITY PRIMARY KEY, -- PK
CustomerName NVARCHAR(50),
City NVARCHAR(50),
Country NVARCHAR(50),
Phone NVARCHAR(50));
GO
INSERT INTO Customer VALUES
('John','Dhaka','Bangladesh','12541231'),
('Adham','Tokyo','Japan','12145126'),
('Kits','Tokyo','Japan','12415269'),
('Rick','Delhi','India','124512691'),
('Tom','Kolkata','India','12412563'),
('Aric','Comilla','Bangladesh','12145128');
GO
CREATE TABLE Category(
CategoryID INT IDENTITY PRIMARY KEY,  -- PK
CategoryName NVARCHAR(50)
);

GO
INSERT INTO Category VALUES
('Baby Food & Care'),
('Chocolates & Candies'),
('Pet Care'),
('Fruits & Vagetables'),
('Meat & Fish'),
('Breads, Biscuits & Cakes'),
('Milk & Dairy'),
('Snacks & Instant Foods'),
('Drinks'),
('Cooking Essentials'),
('Bags'),
('Fashon / Lifestyle');
GO
GO

CREATE TABLE Product(
ProductID INT IDENTITY PRIMARY KEY, -- PK
CategoryID INT REFERENCES Category(CategoryID),  -- FK
ProductName NVARCHAR(50),
UnitPrice NVARCHAR(50),
imageUrl    NVARCHAR(MAX)
);

GO
INSERT INTO Product VALUES
(1,'Baby Food','200','~/imagePro/img_1.jpg'),
(1,'Baby Wipes','300','~/imagePro/img_2.jpg'),
(2,'Snickers Single','100','~/imagePro/img_3.jpg'),
(2,'Fresh Stick','200','~/imagePro/img_4.jpg'),
(2,'Pran Mango Bar','200','~/imagePro/img_5.jpg'),
(3,'Pet Food','100','~/imagePro/img_6.jpg'),
(4,'Fruits','400','~/imagePro/img_7.jpg'),
(4,'Vagetables','200','~/imagePro/img_8.jpg'),
(5,'Meat','500','~/imagePro/img_9.jpg'),
(5,'Fish','200','~/imagePro/img_10.jpg'),
(5,'Egg','100','~/imagePro/img_11.jpg'),
(6,'Breads','120','~/imagePro/img_12.jpg'),
(6,'Biscuits','200','~/imagePro/img_13.jpg'),
(6,'Cakes','100','~/imagePro/img_14.jpg'),
(7,'Cheese','210','~/imagePro/img_15.jpg'),
(7,'Butter & Ghee','500','~/imagePro/img_16.jpg'),
(7,'Milk','200','~/imagePro/img_17.jpg'),
(7,'Yogurt/Curd','240','~/imagePro/img_18.jpg'),
(8,'Pastas','250','~/imagePro/img_19.jpg'),
(8,'Soups','70','~/imagePro/img_20.jpg'),
(8,'Chips','50','~/imagePro/img_21.jpg'),
(8,'Noodles','150','~/imagePro/img_22.jpg'),
(8,'Frozen Food','250','~/imagePro/img_23.jpg'),
(8,'Cereals','200','~/imagePro/img_24.jpg'),
(9,'Soft Drink','140','~/imagePro/img_25.jpg'),
(9,'Tea & Coffee','200','~/imagePro/img_26.jpg'),
(9,'Juice','180','~/imagePro/img_27.jpg'),
(9,'Energy & Malted Drinks','100','~/imagePro/img_28.jpg'),
(10,'Rice','90','~/imagePro/img_29.jpg'),
(10,'Oil','200','~/imagePro/img_30.jpg'),
(10,'Daal','120','~/imagePro/img_31.jpg'),
(10,'Sugar','80','~/imagePro/img_32.jpg'),
(10,'Flour','85','~/imagePro/img_33.jpg'),
(11,'School Bag','800','~/imagePro/img_34.jpg'),
(11,'Travell Bag','900','~/imagePro/img_35.jpg'),
(12,'Women','200','~/imagePro/img_36.jpg'),
(12,'Men','200','~/imagePro/img_37.jpg');
GO

CREATE TABLE Orders(
OrderID INT IDENTITY PRIMARY KEY, -- PK
CustomerID INT REFERENCES Customer(CustomeID),  -- FK
ProductID INT REFERENCES Product(ProductID),  -- FK
OrderNumber NVARCHAR(50),
OrderDate DATETIME,
TotalAmount NVARCHAR(50) ,
);
GO
INSERT INTO Orders VALUES
(1,5,'2145', 2020-01-01, 25120),
(2,4,'2146', 2020-01-03, 21451),
(3,3,'2147', 2020-01-09, 21456),
(4,9,'2148', 2020-01-13, 25462),
(5,6,'2149', 2020-01-14, 24512),
(6,7,'2150', 2020-01-18, 23152);

GO


SELECT * FROM Customer;

SELECT * FROM Orders;

SELECT * FROM Product;

SELECT * FROM Category;



