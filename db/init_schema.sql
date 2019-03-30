\c postgres

-- ##############################################
-- #                                            #
-- #  Create AspIdentity                              #
-- #                                            #
-- ##############################################

CREATE TABLE "AspNetRoles" ( 
  "Id" varchar(128) NOT NULL,
  "Name" varchar(256) NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUsers" (
  "Id" character varying(128) NOT NULL,
  "UserName" character varying(256) NOT NULL,
  "NormalizedUserName" character varying(256) NULL,
  "PasswordHash" character varying(256),
  "SecurityStamp" character varying(256),
  "ConcurrencyStamp" character varying(256) NULL,
  "Email" character varying(256) DEFAULT NULL::character varying,
  "NormalizedEmail" character varying(256) NULL,
  "EmailConfirmed" boolean NOT NULL DEFAULT false,
  "PhoneNumber" character varying(256),
  "PhoneNumberConfirmed" boolean NOT NULL DEFAULT false,
  "TwoFactorEnabled" boolean NOT NULL DEFAULT false,
  "LockoutEnd" timestamp,
  "LockoutEnabled" boolean NOT NULL DEFAULT false,
  "AccessFailedCount" int NOT NULL DEFAULT 0,

  PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUserClaims" ( 
  "Id" serial NOT NULL,
  "ClaimType" varchar(256) NULL,
  "ClaimValue" varchar(256) NULL,
  "UserId" varchar(128) NOT NULL,
  PRIMARY KEY ("Id")
);

CREATE TABLE "AspNetUserLogins" ( 
  "UserId" varchar(128) NOT NULL,
  "LoginProvider" varchar(128) NOT NULL,
  "ProviderKey" varchar(128) NOT NULL,
  PRIMARY KEY ("UserId", "LoginProvider", "ProviderKey")
);

CREATE TABLE "AspNetUserRoles" ( 
  "UserId" varchar(128) NOT NULL,
  "RoleId" varchar(128) NOT NULL,
  PRIMARY KEY ("UserId", "RoleId")
);

CREATE TABLE "AspNetUserTokens" (
	"UserId" varchar(128) NOT NULL,
	"LoginProvider" varchar(128) NOT NULL,
	"Name" varchar(128) NOT NULL,
	"Value" varchar(256) NULL,
	PRIMARY KEY("UserId", "LoginProvider", "Name")
);
	
CREATE INDEX "IX_AspNetUserClaims_UserId"	ON "AspNetUserClaims"	("UserId");
CREATE INDEX "IX_AspNetUserLogins_UserId"	ON "AspNetUserLogins"	("UserId");
CREATE INDEX "IX_AspNetUserRoles_RoleId"	ON "AspNetUserRoles"	("RoleId");
CREATE INDEX "IX_AspNetUserRoles_UserId"	ON "AspNetUserRoles"	("UserId");

ALTER TABLE "AspNetUserClaims"
  ADD CONSTRAINT "FK_AspNetUserClaims_AspNetUsers_User_Id" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
  ON DELETE CASCADE;

ALTER TABLE "AspNetUserLogins"
  ADD CONSTRAINT "FK_AspNetUserLogins_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
  ON DELETE CASCADE;

ALTER TABLE "AspNetUserRoles"
  ADD CONSTRAINT "FK_AspNetUserRoles_AspNetRoles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "AspNetRoles" ("Id")
  ON DELETE CASCADE;

ALTER TABLE "AspNetUserRoles"
  ADD CONSTRAINT "FK_AspNetUserRoles_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
  ON DELETE CASCADE;
  
ALTER TABLE "AspNetUserTokens"
  ADD CONSTRAINT "FK_AspNetUserTokens_AspNetUsers_UserId" FOREIGN KEY ("UserId") REFERENCES "AspNetUsers" ("Id")
  ON DELETE CASCADE;
 
-- ##############################################
-- #                                            #
-- #  Create Customers                              #
-- #                                            #
-- ############################################## 
  
CREATE TABLE "Customers" (
	"Id" serial NOT NULL,
	"CustomerId" character varying(64) NOT NULL,
	"Identity" character varying(32),
	"Name" character varying(128),
	"Surname" character varying(128),
	"Building" character varying(3),
	"Apartment" int NOT NULL,
	"Address" character varying(256),
	"City" character varying(32),
	"Age" int,
	"PersonsAtHome" int,
	"KeyReceived" timestamp,
	"ProjectName" character varying(64),
	"Constructor" character varying(64),
	"Email" character varying(128),
	"Subscribed" boolean NOT NULL DEFAULT false,
	PRIMARY KEY ("Id")
);

CREATE INDEX "IX_Customers_UserId"	ON "Customers"	("CustomerId");