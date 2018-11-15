USE CustomMVCIdentity;
GO

CREATE TABLE [dbo].[tbUser](
	[Id] [nvarchar](50) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NOT NULL,
	[Status] [int] NOT NULL DEFAULT(0),
	[CreatedOnDate] [datetime] NULL,
 CONSTRAINT [PK_tbUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[tbUser] ADD  CONSTRAINT [DF_tbUser_CreatedOnDate]  DEFAULT (getdate()) FOR [CreatedOnDate]
GO

CREATE TABLE [dbo].[tbRole](
	[Id] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[tbUserRole](
	[UserRoleID] [nvarchar](50) NOT NULL,
	[UserID] [nvarchar](50) NOT NULL,
	[RoleID] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_tbUserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE PROCEDURE [dbo].[spUserInsert]
	-- Add the parameters for the stored procedure here
	@ID nvarchar(50),
	@UserName nvarchar(50),
	@Email nvarchar(50),
	@Password nvarchar(50),
	@Status int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO tbUser(
		ID,
		UserName ,
		Email ,
		Password ,
		Status 
	)VALUES(
		@ID,
		@UserName ,
		@Email ,
		@Password ,
		@Status 
	)
END
GO

CREATE PROCEDURE [dbo].[spUserDeleteID]
	-- Add the parameters for the stored procedure here
	@ID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DELETE FROM tbUser
	WHERE ID = @ID
END
GO

CREATE PROCEDURE [dbo].[spGetUserByID]
	-- Add the parameters for the stored procedure here
	@ID nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbUser
	WHERE ID = @ID
END
GO

CREATE PROCEDURE [dbo].[spGetUserByUsername]
	-- Add the parameters for the stored procedure here
	@Username nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT * FROM tbUser
	WHERE Username = @Username
END
GO


CREATE PROCEDURE [dbo].[spUserUpdate]
	-- Add the parameters for the stored procedure here
	@UserName nvarchar(50),
	@Email nvarchar(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE tbUser
	SET Email = @Email
	WHERE UserName = @UserName
END
GO

CREATE PROCEDURE [dbo].[spUserRoleInsert]
	@UserID nvarchar(50),
	@RoleName nvarchar(50)
AS
BEGIN
	DECLARE @UserRoleID nvarchar(50)
	DECLARE @RoleID nvarchar(50)
	
	SELECT @RoleID = Id
	FROM tbRole
	WHERE Name = @RoleName
	
	IF @RoleID IS NULL
		BEGIN
			INSERT INTO tbRole(
				Id,
				Name
			)VALUES(
				NEWID(),
				@RoleName
			)
			
			SELECT @RoleID = Id
			FROM tbRole
			WHERE Name = @RoleName
		END
	
	SELECT @UserRoleID = UserRoleID
	FROM tbUserRole
	WHERE UserID = @UserID AND RoleID = @RoleID
	
	IF @UserRoleID IS NULL
		BEGIN
			INSERT INTO tbUserRole(
				UserRoleID,
				UserID,
				RoleID
			)VALUES(
				NEWID(),
				@UserID,
				@RoleID
			)
		END 

END
GO

CREATE PROCEDURE [dbo].[spUserRoleDelete]
	@UserID nvarchar(50),
	@RoleName nvarchar(50)
AS
BEGIN
	DECLARE @RoleID nvarchar(50)
	
	SELECT @RoleID = Id
	FROM tbRole
	WHERE Name = @RoleName
	
	IF @RoleID IS NULL
		BEGIN
			Delete FROM tbUserRole
			WHERE RoleID = @RoleID AND UserID = @UserID
		END

END
GO

CREATE PROCEDURE [dbo].[spGetUserRoleByID]
	@UserID nvarchar(50)
AS
BEGIN
	
	SELECT R.Name As RoleName
	FROM tbUserRole UR
	INNER JOIN tbRole R
	ON UR.RoleID = R.Id
	WHERE UR.UserID = @UserID
	
END
GO




DECLARE @RoleIDAdmin nvarchar(50),
		@RoleIDMember nvarchar(50),
		@UserIDAdmin nvarchar(50),
		@UserIDMember nvarchar(50)
			
SET @RoleIDAdmin = NewID()
SET @RoleIDMember = NewID()
SET @UserIDAdmin = NewID()
SET @UserIDMember = NewID()
		
INSERT INTO tbRole(
	ID,
	Name
)VALUES(
	@RoleIDAdmin,
	'Administrator'
)
	
INSERT INTO tbRole(
	ID,
	Name
)VALUES(
	@RoleIDMember,
	'Member'
)
		
INSERT INTO tbUser(
	ID,
	UserName ,
	Email ,
	Password ,
	Status 
)VALUES(
	@UserIDAdmin,'admin', 'admin@example.com', '1234567', 1
)
	
INSERT INTO tbUser(
	ID,
	UserName ,
	Email ,
	Password ,
	Status 
)VALUES(
	@UserIDMember,'member', 'member@example.com', '1234567', 1
)
	
INSERT INTO tbUserRole(
	UserRoleID,
	UserID,
	RoleID
)VALUES(
	NewID(), @UserIDAdmin, @RoleIDAdmin
)
	
INSERT INTO tbUserRole(
	UserRoleID,
	UserID,
	RoleID
)VALUES(
	NewID(), @UserIDMember, @RoleIDMember
)
GO