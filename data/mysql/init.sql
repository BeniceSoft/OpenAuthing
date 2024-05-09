ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `OAuthing_DepartmentMembers` (
                                              `DepartmentId` char(36) COLLATE ascii_general_ci NOT NULL COMMENT '部门ID',
                                              `UserId` char(36) COLLATE ascii_general_ci NOT NULL COMMENT '用户ID',
                                              `IsLeader` tinyint(1) NOT NULL DEFAULT FALSE COMMENT '是否负责人',
                                              `IsMain` tinyint(1) NOT NULL DEFAULT FALSE COMMENT '是否主部门',
                                              `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                              `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                              `CreationTime` datetime(6) NOT NULL,
                                              `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                              `LastModificationTime` datetime(6) NULL,
                                              `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                              CONSTRAINT `PK_OAuthing_DepartmentMembers` PRIMARY KEY (`DepartmentId`, `UserId`)
) CHARACTER SET=utf8mb4 COMMENT='部门成员';

CREATE TABLE `OAuthing_Departments` (
                                        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                        `Code` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '编码',
                                        `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '名称',
                                        `ParentId` char(36) COLLATE ascii_general_ci NULL COMMENT '父级Id',
                                        `Seq` int NOT NULL COMMENT '排序',
                                        `Path` varchar(1000) CHARACTER SET utf8mb4 NOT NULL COMMENT '路径',
                                        `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                        `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                        `CreationTime` datetime(6) NOT NULL,
                                        `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                        `LastModificationTime` datetime(6) NULL,
                                        `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                        `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                        `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                        `DeletionTime` datetime(6) NULL,
                                        CONSTRAINT `PK_OAuthing_Departments` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='部门';

CREATE TABLE `OAuthing_ExternalIdentityProviders` (
                                                      `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                                      `ProviderName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '提供者名称',
                                                      `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '名称（唯一）',
                                                      `DisplayName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '显示名称',
                                                      `Enabled` tinyint(1) NOT NULL COMMENT '是否启用',
                                                      `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                                      `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                                      `CreationTime` datetime(6) NOT NULL,
                                                      `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                                      `LastModificationTime` datetime(6) NULL,
                                                      `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                                      `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                                      `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                                      `DeletionTime` datetime(6) NULL,
                                                      CONSTRAINT `PK_OAuthing_ExternalIdentityProviders` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='外部身份提供者';

CREATE TABLE `OAuthing_ExternalIdentityProviderTemplates` (
                                                              `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                                              `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '名称（唯一）',
                                                              `Logo` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT 'LOGO',
                                                              `Title` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '标题',
                                                              `Description` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT '描述',
                                                              `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                                              `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                                              CONSTRAINT `PK_OAuthing_ExternalIdentityProviderTemplates` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='外部身份提供者模板';

CREATE TABLE `OAuthing_OpenIddict_Applications` (
                                                    `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                                    `ApplicationType` varchar(50) CHARACTER SET utf8mb4 NULL,
                                                    `ClientId` varchar(100) CHARACTER SET utf8mb4 NULL,
                                                    `ClientSecret` longtext CHARACTER SET utf8mb4 NULL,
                                                    `ClientType` varchar(50) CHARACTER SET utf8mb4 NULL,
                                                    `ConsentType` varchar(50) CHARACTER SET utf8mb4 NULL,
                                                    `DisplayName` longtext CHARACTER SET utf8mb4 NULL,
                                                    `DisplayNames` longtext CHARACTER SET utf8mb4 NULL,
                                                    `JsonWebKeySet` longtext CHARACTER SET utf8mb4 NULL,
                                                    `Permissions` longtext CHARACTER SET utf8mb4 NULL,
                                                    `PostLogoutRedirectUris` longtext CHARACTER SET utf8mb4 NULL,
                                                    `Properties` longtext CHARACTER SET utf8mb4 NULL,
                                                    `RedirectUris` longtext CHARACTER SET utf8mb4 NULL,
                                                    `Requirements` longtext CHARACTER SET utf8mb4 NULL,
                                                    `Settings` longtext CHARACTER SET utf8mb4 NULL,
                                                    `ClientUri` longtext CHARACTER SET utf8mb4 NULL,
                                                    `LogoUri` longtext CHARACTER SET utf8mb4 NULL,
                                                    `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                                    `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                                    `CreationTime` datetime(6) NOT NULL,
                                                    `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                                    `LastModificationTime` datetime(6) NULL,
                                                    `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                                    `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                                    `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                                    `DeletionTime` datetime(6) NULL,
                                                    CONSTRAINT `PK_OAuthing_OpenIddict_Applications` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_OpenIddict_Scopes` (
                                              `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                              `Description` longtext CHARACTER SET utf8mb4 NULL,
                                              `Descriptions` longtext CHARACTER SET utf8mb4 NULL,
                                              `DisplayName` longtext CHARACTER SET utf8mb4 NULL,
                                              `DisplayNames` longtext CHARACTER SET utf8mb4 NULL,
                                              `Name` varchar(200) CHARACTER SET utf8mb4 NULL,
                                              `Properties` longtext CHARACTER SET utf8mb4 NULL,
                                              `Resources` longtext CHARACTER SET utf8mb4 NULL,
                                              `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                              `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                              `CreationTime` datetime(6) NOT NULL,
                                              `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                              `LastModificationTime` datetime(6) NULL,
                                              `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                              `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                              `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                              `DeletionTime` datetime(6) NULL,
                                              CONSTRAINT `PK_OAuthing_OpenIddict_Scopes` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_PermissionGrants` (
                                             `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                             `SystemCode` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
                                             `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
                                             `ProviderName` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
                                             `ProviderKey` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
                                             CONSTRAINT `PK_OAuthing_PermissionGrants` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_Permissions` (
                                        `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                        `SystemId` char(36) COLLATE ascii_general_ci NOT NULL,
                                        `SystemCode` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
                                        `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
                                        `DisplayName` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
                                        `ParentName` varchar(128) CHARACTER SET utf8mb4 NULL,
                                        `IsEnabled` tinyint(1) NOT NULL,
                                        `Providers` longtext CHARACTER SET utf8mb4 NOT NULL,
                                        `StateCheckers` longtext CHARACTER SET utf8mb4 NOT NULL,
                                        `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                        `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                        CONSTRAINT `PK_OAuthing_Permissions` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_PermissionSpaces` (
                                             `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                             `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '名称',
                                             `NormalizedName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '标准化名称',
                                             `DisplayName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '现实名称',
                                             `Description` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT '描述',
                                             `IsSystemBuiltIn` tinyint(1) NOT NULL DEFAULT FALSE COMMENT '是否系统内置',
                                             `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                             `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                             `CreationTime` datetime(6) NOT NULL,
                                             `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                             `LastModificationTime` datetime(6) NULL,
                                             `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                             `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                             `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                             `DeletionTime` datetime(6) NULL,
                                             CONSTRAINT `PK_OAuthing_PermissionSpaces` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='权限空间';

CREATE TABLE `OAuthing_Positions` (
                                      `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                      `Name` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
                                      `Description` varchar(512) CHARACTER SET utf8mb4 NULL,
                                      `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                      `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                      CONSTRAINT `PK_OAuthing_Positions` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_Roles` (
                                  `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                  `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '角色名',
                                  `NormalizedName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '归一化后的角色名',
                                  `DisplayName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '显示名',
                                  `Description` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT '描述',
                                  `Enabled` tinyint(1) NOT NULL DEFAULT TRUE COMMENT '是否启用',
                                  `IsSystemBuiltIn` tinyint(1) NOT NULL DEFAULT FALSE COMMENT '是否系统内置',
                                  `PermissionSpaceId` char(36) COLLATE ascii_general_ci NOT NULL COMMENT '所属权限空间',
                                  `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                  `CreationTime` datetime(6) NOT NULL,
                                  `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                  `LastModificationTime` datetime(6) NULL,
                                  `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                  `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                  `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                  `DeletionTime` datetime(6) NULL,
                                  CONSTRAINT `PK_OAuthing_Roles` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='角色';

CREATE TABLE `OAuthing_SettingDefinitions` (
                                               `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                               `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
                                               `DisplayName` varchar(256) CHARACTER SET utf8mb4 NOT NULL,
                                               `Description` varchar(512) CHARACTER SET utf8mb4 NULL,
                                               `DefaultValue` varchar(2048) CHARACTER SET utf8mb4 NULL,
                                               `IsVisibleToClients` tinyint(1) NOT NULL,
                                               `Providers` varchar(1024) CHARACTER SET utf8mb4 NULL,
                                               `IsInherited` tinyint(1) NOT NULL,
                                               `IsEncrypted` tinyint(1) NOT NULL,
                                               `ExtraProperties` longtext CHARACTER SET utf8mb4 NULL,
                                               CONSTRAINT `PK_OAuthing_SettingDefinitions` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_Settings` (
                                     `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                     `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
                                     `Value` varchar(2048) CHARACTER SET utf8mb4 NOT NULL,
                                     `ProviderName` varchar(64) CHARACTER SET utf8mb4 NULL,
                                     `ProviderKey` varchar(64) CHARACTER SET utf8mb4 NULL,
                                     CONSTRAINT `PK_OAuthing_Settings` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_UserGroups` (
                                       `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                       `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '名称',
                                       `DisplayName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '显示名称',
                                       `Description` varchar(500) CHARACTER SET utf8mb4 NOT NULL COMMENT '描述',
                                       `Enabled` tinyint(1) NOT NULL COMMENT '是否启用',
                                       `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                       `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                       `CreationTime` datetime(6) NOT NULL,
                                       `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                       `LastModificationTime` datetime(6) NULL,
                                       `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                       `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                       `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                       `DeletionTime` datetime(6) NULL,
                                       CONSTRAINT `PK_OAuthing_UserGroups` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='用户组';

CREATE TABLE `OAuthing_Users` (
                                  `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                  `UserName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '用户名',
                                  `NormalizedUserName` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '归一化后的用户名',
                                  `Nickname` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '昵称',
                                  `PasswordHash` varchar(500) CHARACTER SET utf8mb4 NOT NULL COMMENT '哈希后的密码',
                                  `PhoneNumber` varchar(50) CHARACTER SET utf8mb4 NULL COMMENT '手机号码',
                                  `PhoneNumberConfirmed` tinyint(1) NOT NULL COMMENT '手机号码是否确认',
                                  `Email` varchar(50) CHARACTER SET utf8mb4 NULL COMMENT '邮箱',
                                  `NormalizedEmail` varchar(50) CHARACTER SET utf8mb4 NULL COMMENT '归一化后的邮箱',
                                  `EmailConfirmed` tinyint(1) NOT NULL COMMENT '邮箱是否确认',
                                  `Avatar` varchar(1000) CHARACTER SET utf8mb4 NULL COMMENT '头像',
                                  `Gender` varchar(10) CHARACTER SET utf8mb4 NULL COMMENT '性别',
                                  `JobTitle` varchar(200) CHARACTER SET utf8mb4 NULL COMMENT '职务',
                                  `Enabled` tinyint(1) NOT NULL,
                                  `LockoutEnd` datetime(6) NULL COMMENT '锁定结束时间',
                                  `LockoutEnabled` tinyint(1) NOT NULL COMMENT '锁定状态',
                                  `AccessFailedCount` int NOT NULL DEFAULT 0 COMMENT '访问错误次数',
                                  `SecurityStamp` varchar(1000) CHARACTER SET utf8mb4 NULL COMMENT '安全凭证',
                                  `TwoFactorEnabled` tinyint(1) NOT NULL DEFAULT FALSE COMMENT '是否启用2FA',
                                  `IsSystemBuiltIn` tinyint(1) NOT NULL DEFAULT FALSE COMMENT '是否系统内置',
                                  `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                  `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                  `CreationTime` datetime(6) NOT NULL,
                                  `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                  `LastModificationTime` datetime(6) NULL,
                                  `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                  `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                  `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                  `DeletionTime` datetime(6) NULL,
                                  CONSTRAINT `PK_OAuthing_Users` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='用户';

CREATE TABLE `OAuthing_ExternalIdentityProviderOptions` (
                                                            `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                                            `Key` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '键',
                                                            `Value` longtext CHARACTER SET utf8mb4 NULL COMMENT '值',
                                                            `ExternalIdentityProviderId` char(36) COLLATE ascii_general_ci NULL,
                                                            CONSTRAINT `PK_OAuthing_ExternalIdentityProviderOptions` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='外部身份提供者配置';

CREATE TABLE `OAuthing_ExternalIdentityProviderTemplateFields` (
                                                                   `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                                                   `Name` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '字段名',
                                                                   `Label` varchar(200) CHARACTER SET utf8mb4 NULL COMMENT '显示文本',
                                                                   `Placeholder` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT '站位文本',
                                                                   `Required` tinyint(1) NOT NULL COMMENT '是否必填',
                                                                   `Type` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '类型',
                                                                   `HelpText` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT '帮助文本',
                                                                   `ExtraData` longtext CHARACTER SET utf8mb4 NULL COMMENT '扩展数据',
                                                                   `Order` int NOT NULL COMMENT '排序（正序）',
                                                                   `ExternalIdentityProviderTemplateId` char(36) COLLATE ascii_general_ci NULL,
                                                                   CONSTRAINT `PK_OAuthing_ExternalIdentityProviderTemplateFields` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='外部身份提供者模板字段';

CREATE TABLE `OAuthing_OpenIddict_Authorizations` (
                                                      `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                                      `ApplicationId` char(36) COLLATE ascii_general_ci NULL,
                                                      `CreationDate` datetime(6) NULL,
                                                      `Properties` longtext CHARACTER SET utf8mb4 NULL,
                                                      `Scopes` longtext CHARACTER SET utf8mb4 NULL,
                                                      `Status` varchar(50) CHARACTER SET utf8mb4 NULL,
                                                      `Subject` varchar(400) CHARACTER SET utf8mb4 NULL,
                                                      `Type` varchar(50) CHARACTER SET utf8mb4 NULL,
                                                      `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                                      `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                                      `CreationTime` datetime(6) NOT NULL,
                                                      `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                                      `LastModificationTime` datetime(6) NULL,
                                                      `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                                      `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                                      `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                                      `DeletionTime` datetime(6) NULL,
                                                      CONSTRAINT `PK_OAuthing_OpenIddict_Authorizations` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `OAuthing_RoleSubjects` (
                                         `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                         `RoleId` char(36) COLLATE ascii_general_ci NOT NULL COMMENT '角色id',
                                         `SubjectType` int NOT NULL COMMENT '主体类型(0:用户 1:用户组)',
                                         `SubjectId` char(36) COLLATE ascii_general_ci NOT NULL COMMENT '主体id',
                                         `CreationTime` datetime(6) NOT NULL,
                                         `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                         CONSTRAINT `PK_OAuthing_RoleSubjects` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4 COMMENT='角色主体';

CREATE TABLE `OAuthing_UserGroupMembers` (
                                             `UserGroupId` char(36) COLLATE ascii_general_ci NOT NULL,
                                             `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
                                             `CreationTime` datetime(6) NOT NULL,
                                             `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                             CONSTRAINT `PK_OAuthing_UserGroupMembers` PRIMARY KEY (`UserGroupId`, `UserId`)
) CHARACTER SET=utf8mb4 COMMENT='用户组成员';

CREATE TABLE `OAuthing_UserLogins` (
                                       `UserId` char(36) COLLATE ascii_general_ci NOT NULL COMMENT '用户id',
                                       `LoginProvider` varchar(200) CHARACTER SET utf8mb4 NOT NULL COMMENT '登录提供程序名称',
                                       `ProviderKey` varchar(500) CHARACTER SET utf8mb4 NOT NULL COMMENT '此登录程序的唯一标识',
                                       `ProviderDisplayName` varchar(500) CHARACTER SET utf8mb4 NULL COMMENT '此登录程序的显示名',
                                       CONSTRAINT `PK_OAuthing_UserLogins` PRIMARY KEY (`UserId`, `LoginProvider`)
) CHARACTER SET=utf8mb4 COMMENT='用户第三方登录信息';

CREATE TABLE `OAuthing_UserTokens` (
                                       `UserId` char(36) COLLATE ascii_general_ci NOT NULL,
                                       `LoginProvider` varchar(64) CHARACTER SET utf8mb4 NOT NULL,
                                       `Name` varchar(128) CHARACTER SET utf8mb4 NOT NULL,
                                       `Value` longtext CHARACTER SET utf8mb4 NOT NULL,
                                       CONSTRAINT `PK_OAuthing_UserTokens` PRIMARY KEY (`UserId`, `LoginProvider`, `Name`)
) CHARACTER SET=utf8mb4 COMMENT='用户token';

CREATE TABLE `OAuthing_OpenIddict_Tokens` (
                                              `Id` char(36) COLLATE ascii_general_ci NOT NULL,
                                              `ApplicationId` char(36) COLLATE ascii_general_ci NULL,
                                              `AuthorizationId` char(36) COLLATE ascii_general_ci NULL,
                                              `CreationDate` datetime(6) NULL,
                                              `ExpirationDate` datetime(6) NULL,
                                              `Payload` longtext CHARACTER SET utf8mb4 NULL,
                                              `Properties` longtext CHARACTER SET utf8mb4 NULL,
                                              `RedemptionDate` datetime(6) NULL,
                                              `ReferenceId` varchar(100) CHARACTER SET utf8mb4 NULL,
                                              `Status` varchar(50) CHARACTER SET utf8mb4 NULL,
                                              `Subject` varchar(400) CHARACTER SET utf8mb4 NULL,
                                              `Type` varchar(50) CHARACTER SET utf8mb4 NULL,
                                              `ExtraProperties` longtext CHARACTER SET utf8mb4 NOT NULL,
                                              `ConcurrencyStamp` varchar(40) CHARACTER SET utf8mb4 NOT NULL,
                                              `CreationTime` datetime(6) NOT NULL,
                                              `CreatorId` char(36) COLLATE ascii_general_ci NULL,
                                              `LastModificationTime` datetime(6) NULL,
                                              `LastModifierId` char(36) COLLATE ascii_general_ci NULL,
                                              `IsDeleted` tinyint(1) NOT NULL DEFAULT FALSE,
                                              `DeleterId` char(36) COLLATE ascii_general_ci NULL,
                                              `DeletionTime` datetime(6) NULL,
                                              CONSTRAINT `PK_OAuthing_OpenIddict_Tokens` PRIMARY KEY (`Id`)
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_OAuthing_Departments_ParentId` ON `OAuthing_Departments` (`ParentId`);

CREATE INDEX `IX_OAuthing_ExternalIdentityProviderOptions_ExternalIdentityPro~` ON `OAuthing_ExternalIdentityProviderOptions` (`ExternalIdentityProviderId`);

CREATE INDEX `IX_OAuthing_ExternalIdentityProviderTemplateFields_ExternalIden~` ON `OAuthing_ExternalIdentityProviderTemplateFields` (`ExternalIdentityProviderTemplateId`);

CREATE UNIQUE INDEX `IDX_ExternalIdentityProviderTemplates_Name` ON `OAuthing_ExternalIdentityProviderTemplates` (`Name`);

CREATE INDEX `IX_OAuthing_OpenIddict_Applications_ClientId` ON `OAuthing_OpenIddict_Applications` (`ClientId`);

CREATE INDEX `IX_OAuthing_OpenIddict_Authorizations_ApplicationId_Status_Subj~` ON `OAuthing_OpenIddict_Authorizations` (`ApplicationId`, `Status`, `Subject`, `Type`);

CREATE INDEX `IX_OAuthing_OpenIddict_Scopes_Name` ON `OAuthing_OpenIddict_Scopes` (`Name`);

CREATE INDEX `IX_OAuthing_OpenIddict_Tokens_ApplicationId_Status_Subject_Type` ON `OAuthing_OpenIddict_Tokens` (`ApplicationId`, `Status`, `Subject`, `Type`);

CREATE INDEX `IX_OAuthing_OpenIddict_Tokens_AuthorizationId` ON `OAuthing_OpenIddict_Tokens` (`AuthorizationId`);

CREATE INDEX `IX_OAuthing_OpenIddict_Tokens_ReferenceId` ON `OAuthing_OpenIddict_Tokens` (`ReferenceId`);

CREATE UNIQUE INDEX `IX_OAuthing_PermissionGrants_SystemCode_Name_ProviderName_Provi~` ON `OAuthing_PermissionGrants` (`SystemCode`, `Name`, `ProviderName`, `ProviderKey`);

CREATE INDEX `IX_OAuthing_Permissions_ParentName` ON `OAuthing_Permissions` (`ParentName`);

CREATE UNIQUE INDEX `IX_OAuthing_Permissions_SystemId_SystemCode_Name` ON `OAuthing_Permissions` (`SystemId`, `SystemCode`, `Name`);

CREATE INDEX `IX_OAuthing_Roles_PermissionSpaceId` ON `OAuthing_Roles` (`PermissionSpaceId`);

CREATE INDEX `IX_OAuthing_RoleSubjects_RoleId` ON `OAuthing_RoleSubjects` (`RoleId`);

CREATE UNIQUE INDEX `IX_OAuthing_RoleSubjects_SubjectType_SubjectId_RoleId` ON `OAuthing_RoleSubjects` (`SubjectType`, `SubjectId`, `RoleId`);

CREATE UNIQUE INDEX `IX_OAuthing_SettingDefinitions_Name` ON `OAuthing_SettingDefinitions` (`Name`);

CREATE UNIQUE INDEX `IX_OAuthing_Settings_Name_ProviderName_ProviderKey` ON `OAuthing_Settings` (`Name`, `ProviderName`, `ProviderKey`);