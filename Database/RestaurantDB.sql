/*
  Restaurant Management System - database script (SQL Server)
  Creates the ResturantDB database, tables, constraints, and demo data.

  Demo accounts (local use only):
    admin     / Admin@123   (Admin role)
    employee  / Admin@123   (Employee role)

  If dbo.Users already exists, the script skips the setup.
*/

IF DB_ID(N'ResturantDB') IS NULL
    CREATE DATABASE [ResturantDB];
GO
USE [ResturantDB]
GO
IF OBJECT_ID(N'dbo.Users', N'U') IS NOT NULL
BEGIN
    PRINT 'ResturantDB already contains the schema. Nothing was changed.';
    SET NOEXEC ON;
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Bills](
	[BillId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[SubTotal] [decimal](18, 2) NOT NULL,
	[TaxAmount] [decimal](18, 2) NOT NULL,
	[Discount] [decimal](18, 2) NOT NULL,
	[FinalAmount] [decimal](18, 2) NOT NULL,
	[PaymentMethod] [int] NOT NULL,
	[PaymentStatus] [int] NOT NULL,
	[IssuedAt] [datetime] NOT NULL,
	[CreatedBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[BillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryId] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](70) NOT NULL,
 CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
(
	[CategoryId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customers](
	[CustomerId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](max) NULL,
	[Phone] [nvarchar](20) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CustomerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Employees](
	[EmployeeId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[ManagerId] [int] NULL,
	[Salary] [decimal](18, 2) NULL,
	[StartTime] [time](7) NULL,
	[EndTime] [time](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[EmployeeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderItems](
	[OrderItemId] [int] IDENTITY(1,1) NOT NULL,
	[OrderId] [int] NOT NULL,
	[ProductId] [int] NOT NULL,
	[UnitPrice] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderId] [int] IDENTITY(1,1) NOT NULL,
	[CustomerId] [int] NULL,
	[OrderType] [int] NOT NULL,
	[Status] [int] NOT NULL,
	[CreatedBy] [int] NULL,
	[OrderDate] [datetime] NULL,
	[TotalAmount] [decimal](18, 2) NULL,
	[TableNumber] [int] NULL,
PRIMARY KEY CLUSTERED 
(
	[OrderId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PersonId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[NationalNo] [nvarchar](20) NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Gender] [int] NOT NULL,
	[DateOfBirth] [date] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[PersonId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](100) NOT NULL,
	[Price] [decimal](18, 2) NOT NULL,
	[Quantity] [int] NOT NULL,
	[CategoryId] [int] NOT NULL,
	[PreparationTime] [int] NULL,
	[IsAvailable] [bit] NULL,
	[CreatedAt] [datetime] NULL,
	[ImageUrl] [nvarchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RefreshTokens](
	[RefreshTokenId] [int] IDENTITY(1,1) NOT NULL,
	[RefreshTokenHash] [nvarchar](70) NOT NULL,
	[RefreshTokenExpiresAt] [datetime2](7) NOT NULL,
	[RefreshTokenRevokedAt] [datetime2](7) NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_RefreshTokens] PRIMARY KEY CLUSTERED 
(
	[RefreshTokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Roles](
	[RoleId] [int] IDENTITY(1,1) NOT NULL,
	[RoleName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoles](
	[UserRolesId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[UserRolesId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[PersonId] [int] NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[HashPassword] [nvarchar](max) NOT NULL,
	[IsActive] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[Roles] ON
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (1, N'Admin')
INSERT [dbo].[Roles] ([RoleId], [RoleName]) VALUES (2, N'Employee')
SET IDENTITY_INSERT [dbo].[Roles] OFF
GO
SET IDENTITY_INSERT [dbo].[Categories] ON
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (1, N'Food')
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (2, N'Drinks')
INSERT [dbo].[Categories] ([CategoryId], [CategoryName]) VALUES (3, N'Sweets')
SET IDENTITY_INSERT [dbo].[Categories] OFF
GO
SET IDENTITY_INSERT [dbo].[Products] ON
INSERT [dbo].[Products] ([ProductId], [Name], [Price], [Quantity], [CategoryId], [PreparationTime], [IsAvailable], [CreatedAt], [ImageUrl]) VALUES (1, N'Pizza', CAST(10.00 AS Decimal(18, 2)), 5, 1, 30, 1, CAST(N'2026-05-10T22:07:09.340' AS DateTime), N'/images/products/c6a75b58-7de0-42b9-8835-b4fcc707f93d.jpg')
INSERT [dbo].[Products] ([ProductId], [Name], [Price], [Quantity], [CategoryId], [PreparationTime], [IsAvailable], [CreatedAt], [ImageUrl]) VALUES (2, N'coca-cola', CAST(2.00 AS Decimal(18, 2)), 13, 2, 0, 1, CAST(N'2026-05-11T13:14:19.107' AS DateTime), N'/images/products/e6786f41-b1b4-487c-94c3-f447550038ff.jpg')
INSERT [dbo].[Products] ([ProductId], [Name], [Price], [Quantity], [CategoryId], [PreparationTime], [IsAvailable], [CreatedAt], [ImageUrl]) VALUES (3, N'burger', CAST(45.00 AS Decimal(18, 2)), 9, 1, 45, 1, CAST(N'2026-08-05T16:52:42.047' AS DateTime), N'/images/products/8e3605bc-6b29-4a43-a48e-f5e51a5a38f6.jpg')
INSERT [dbo].[Products] ([ProductId], [Name], [Price], [Quantity], [CategoryId], [PreparationTime], [IsAvailable], [CreatedAt], [ImageUrl]) VALUES (1003, N'cake', CAST(10.00 AS Decimal(18, 2)), 3, 3, 50, 1, CAST(N'2026-09-04T15:54:49.247' AS DateTime), N'/images/products/9694779c-0f6c-4730-98bf-cd2d97df407d.jfif')
INSERT [dbo].[Products] ([ProductId], [Name], [Price], [Quantity], [CategoryId], [PreparationTime], [IsAvailable], [CreatedAt], [ImageUrl]) VALUES (1004, N'orange-juice', CAST(2.00 AS Decimal(18, 2)), 5, 2, 10, 1, CAST(N'2026-09-04T15:57:46.423' AS DateTime), N'/images/products/7d5ab444-1c42-464c-80a0-330472167d14.jfif')
SET IDENTITY_INSERT [dbo].[Products] OFF
GO
SET IDENTITY_INSERT [dbo].[People] ON
INSERT [dbo].[People] ([PersonId], [FirstName], [LastName], [NationalNo], [Phone], [Gender], [DateOfBirth]) VALUES (1, N'Demo', N'Admin', N'DEMO-001', N'0000000001', 0, CAST(N'1990-01-01' AS Date))
INSERT [dbo].[People] ([PersonId], [FirstName], [LastName], [NationalNo], [Phone], [Gender], [DateOfBirth]) VALUES (2, N'Demo', N'Employee', N'DEMO-002', N'0000000002', 1, CAST(N'1995-01-01' AS Date))
SET IDENTITY_INSERT [dbo].[People] OFF
GO
SET IDENTITY_INSERT [dbo].[Users] ON
INSERT [dbo].[Users] ([UserId], [PersonId], [Username], [HashPassword], [IsActive]) VALUES (1, 1, N'admin', N'$2a$11$t8jlf4lMM//JRZbiAL0Xt.ZXfxBYLlMn.FV2toSwtnJnNXKm9NE4u', 1)
INSERT [dbo].[Users] ([UserId], [PersonId], [Username], [HashPassword], [IsActive]) VALUES (2, 2, N'employee', N'$2a$11$t8jlf4lMM//JRZbiAL0Xt.ZXfxBYLlMn.FV2toSwtnJnNXKm9NE4u', 1)
SET IDENTITY_INSERT [dbo].[Users] OFF
GO
SET IDENTITY_INSERT [dbo].[UserRoles] ON
INSERT [dbo].[UserRoles] ([UserRolesId], [UserId], [RoleId]) VALUES (1, 1, 1)
INSERT [dbo].[UserRoles] ([UserRolesId], [UserId], [RoleId]) VALUES (2, 2, 2)
SET IDENTITY_INSERT [dbo].[UserRoles] OFF
GO
SET IDENTITY_INSERT [dbo].[Employees] ON
INSERT [dbo].[Employees] ([EmployeeId], [UserId], [ManagerId], [Salary], [StartTime], [EndTime]) VALUES (1, 1, NULL, CAST(3000.00 AS Decimal(18, 2)), CAST(N'08:00:00' AS Time), CAST(N'16:00:00' AS Time))
INSERT [dbo].[Employees] ([EmployeeId], [UserId], [ManagerId], [Salary], [StartTime], [EndTime]) VALUES (2, 2, 1, CAST(2000.00 AS Decimal(18, 2)), CAST(N'10:00:00' AS Time), CAST(N'18:00:00' AS Time))
SET IDENTITY_INSERT [dbo].[Employees] OFF
GO
SET IDENTITY_INSERT [dbo].[Customers] ON
INSERT [dbo].[Customers] ([CustomerId], [Name], [Address], [Phone]) VALUES (1, N'Omar Khalil', N'Nablus', N'0590000001')
INSERT [dbo].[Customers] ([CustomerId], [Name], [Address], [Phone]) VALUES (2, N'Lina Hassan', N'Ramallah', N'0590000002')
INSERT [dbo].[Customers] ([CustomerId], [Name], [Address], [Phone]) VALUES (3, N'Yousef Nasser', N'Hebron', N'0590000003')
SET IDENTITY_INSERT [dbo].[Customers] OFF
GO
SET IDENTITY_INSERT [dbo].[Orders] ON
INSERT [dbo].[Orders] ([OrderId], [CustomerId], [OrderType], [Status], [CreatedBy], [OrderDate], [TotalAmount], [TableNumber]) VALUES (1, NULL, 2, 1, 1, CAST(N'2026-10-20T13:10:00.000' AS DateTime), CAST(24.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[Orders] ([OrderId], [CustomerId], [OrderType], [Status], [CreatedBy], [OrderDate], [TotalAmount], [TableNumber]) VALUES (2, 1, 0, 1, 2, CAST(N'2026-10-25T19:30:00.000' AS DateTime), CAST(49.00 AS Decimal(18, 2)), 0)
INSERT [dbo].[Orders] ([OrderId], [CustomerId], [OrderType], [Status], [CreatedBy], [OrderDate], [TotalAmount], [TableNumber]) VALUES (3, 2, 1, 3, 1, CAST(N'2026-10-30T20:15:00.000' AS DateTime), CAST(12.00 AS Decimal(18, 2)), 0)
SET IDENTITY_INSERT [dbo].[Orders] OFF
GO
SET IDENTITY_INSERT [dbo].[OrderItems] ON
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (1, 1, 1, CAST(10.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (2, 1, 2, CAST(2.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (3, 2, 3, CAST(45.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (4, 2, 1004, CAST(2.00 AS Decimal(18, 2)), 2)
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (5, 3, 1003, CAST(10.00 AS Decimal(18, 2)), 1)
INSERT [dbo].[OrderItems] ([OrderItemId], [OrderId], [ProductId], [UnitPrice], [Quantity]) VALUES (6, 3, 2, CAST(2.00 AS Decimal(18, 2)), 1)
SET IDENTITY_INSERT [dbo].[OrderItems] OFF
GO
SET IDENTITY_INSERT [dbo].[Bills] ON
INSERT [dbo].[Bills] ([BillId], [OrderId], [SubTotal], [TaxAmount], [Discount], [FinalAmount], [PaymentMethod], [PaymentStatus], [IssuedAt], [CreatedBy]) VALUES (1, 1, CAST(24.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(24.00 AS Decimal(18, 2)), 1, 1, CAST(N'2026-10-20T13:50:00.000' AS DateTime), 1)
INSERT [dbo].[Bills] ([BillId], [OrderId], [SubTotal], [TaxAmount], [Discount], [FinalAmount], [PaymentMethod], [PaymentStatus], [IssuedAt], [CreatedBy]) VALUES (2, 2, CAST(49.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(10.00 AS Decimal(18, 2)), CAST(44.10 AS Decimal(18, 2)), 0, 0, CAST(N'2026-10-25T20:10:00.000' AS DateTime), 2)
SET IDENTITY_INSERT [dbo].[Bills] OFF
GO

ALTER TABLE [dbo].[People] ADD UNIQUE NONCLUSTERED 
(
	[NationalNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[UserRoles] ADD  CONSTRAINT [UQ_UserRoles_UserId_RoleId] UNIQUE NONCLUSTERED 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[Users] ADD UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Bills] ADD  CONSTRAINT [DF_Bills_TaxAmount]  DEFAULT ((0.00)) FOR [TaxAmount]
GO
ALTER TABLE [dbo].[Bills] ADD  CONSTRAINT [DF_Bills_Discount]  DEFAULT ((0.00)) FOR [Discount]
GO
ALTER TABLE [dbo].[Bills] ADD  DEFAULT (getdate()) FOR [IssuedAt]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT (getdate()) FOR [OrderDate]
GO
ALTER TABLE [dbo].[Orders] ADD  DEFAULT ((0)) FOR [TotalAmount]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT ((1)) FOR [IsAvailable]
GO
ALTER TABLE [dbo].[Products] ADD  DEFAULT (getdate()) FOR [CreatedAt]
GO
ALTER TABLE [dbo].[Users] ADD  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employees] ([EmployeeId])
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([ManagerId])
REFERENCES [dbo].[Employees] ([EmployeeId])
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([OrderId])
REFERENCES [dbo].[Orders] ([OrderId])
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD FOREIGN KEY([ProductId])
REFERENCES [dbo].[Products] ([ProductId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD FOREIGN KEY([CreatedBy])
REFERENCES [dbo].[Employees] ([EmployeeId])
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Customers] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customers] ([CustomerId])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Customers]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [FK_Products_Categories] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[Categories] ([CategoryId])
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [FK_Products_Categories]
GO
ALTER TABLE [dbo].[RefreshTokens]  WITH CHECK ADD  CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO
ALTER TABLE [dbo].[RefreshTokens] CHECK CONSTRAINT [FK_RefreshTokens_Users]
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD FOREIGN KEY([RoleId])
REFERENCES [dbo].[Roles] ([RoleId])
GO
ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD  CONSTRAINT [FK__UserRoles__UserI__5629CD9C] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserRoles] CHECK CONSTRAINT [FK__UserRoles__UserI__5629CD9C]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK__Users__PersonId__52593CB8] FOREIGN KEY([PersonId])
REFERENCES [dbo].[People] ([PersonId])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK__Users__PersonId__52593CB8]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD  CONSTRAINT [Ck_Bills_Discount] CHECK  (([Discount]>=(0)))
GO
ALTER TABLE [dbo].[Bills] CHECK CONSTRAINT [Ck_Bills_Discount]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD  CONSTRAINT [Ck_Bills_FinalAmount] CHECK  (([FinalAmount]>=(0)))
GO
ALTER TABLE [dbo].[Bills] CHECK CONSTRAINT [Ck_Bills_FinalAmount]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD  CONSTRAINT [Ck_Bills_PaymentMethod] CHECK  (([PaymentMethod]=(2) OR [PaymentMethod]=(1) OR [PaymentMethod]=(0)))
GO
ALTER TABLE [dbo].[Bills] CHECK CONSTRAINT [Ck_Bills_PaymentMethod]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD  CONSTRAINT [Ck_Bills_PaymentStatus] CHECK  (([PaymentStatus]=(1) OR [PaymentStatus]=(0)))
GO
ALTER TABLE [dbo].[Bills] CHECK CONSTRAINT [Ck_Bills_PaymentStatus]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD  CONSTRAINT [Ck_Bills_SubTotal] CHECK  (([SubTotal]>=(0)))
GO
ALTER TABLE [dbo].[Bills] CHECK CONSTRAINT [Ck_Bills_SubTotal]
GO
ALTER TABLE [dbo].[Bills]  WITH CHECK ADD  CONSTRAINT [Ck_Bills_TaxAmount] CHECK  (([TaxAmount]>=(0)))
GO
ALTER TABLE [dbo].[Bills] CHECK CONSTRAINT [Ck_Bills_TaxAmount]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [Ck_Employees_Salary] CHECK  (([Salary]>(0)))
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [Ck_Employees_Salary]
GO
ALTER TABLE [dbo].[Employees]  WITH CHECK ADD  CONSTRAINT [Ck_Employees_Time] CHECK  (([EndTime]>[StartTime]))
GO
ALTER TABLE [dbo].[Employees] CHECK CONSTRAINT [Ck_Employees_Time]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [Ck_OrderItems_Quantity] CHECK  (([Quantity]>=(0)))
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [Ck_OrderItems_Quantity]
GO
ALTER TABLE [dbo].[OrderItems]  WITH CHECK ADD  CONSTRAINT [Ck_OrderItems_UnitPrice] CHECK  (([UnitPrice]>=(0)))
GO
ALTER TABLE [dbo].[OrderItems] CHECK CONSTRAINT [Ck_OrderItems_UnitPrice]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Ck_Orders_OrderType] CHECK  (([OrderType]=(2) OR [OrderType]=(1) OR [OrderType]=(0)))
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Ck_Orders_OrderType]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Ck_Orders_Status] CHECK  (([Status]=(3) OR [Status]=(2) OR [Status]=(1) OR [Status]=(0)))
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Ck_Orders_Status]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [Ck_Orders_TotalAmount] CHECK  (([TotalAmount]>=(0)))
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [Ck_Orders_TotalAmount]
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [Ck_People_DateOfBirth] CHECK  ((datediff(year,[DateOfBirth],getdate())>=(18)))
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [Ck_People_DateOfBirth]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [Ck_Products_Price] CHECK  (([Price]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [Ck_Products_Price]
GO
ALTER TABLE [dbo].[Products]  WITH CHECK ADD  CONSTRAINT [Ck_Products_Quantity] CHECK  (([Quantity]>=(0)))
GO
ALTER TABLE [dbo].[Products] CHECK CONSTRAINT [Ck_Products_Quantity]
GO
GO
SET NOEXEC OFF
GO
