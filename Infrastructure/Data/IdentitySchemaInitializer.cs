using Microsoft.EntityFrameworkCore;

namespace Hotel.Infrastructure.Data;

public static class IdentitySchemaInitializer
{
    public static async Task EnsureIdentitySchemaAsync(HotelDbContext dbContext)
    {
        const string sql = """
if object_id(N'[dbo].[AspNetUsers]', N'U') is null
begin
    create table [dbo].[AspNetRoles] (
        [Id] nvarchar(450) not null,
        [Name] nvarchar(256) null,
        [NormalizedName] nvarchar(256) null,
        [ConcurrencyStamp] nvarchar(max) null,
        constraint [PK_AspNetRoles] primary key ([Id])
    );

    create table [dbo].[AspNetUsers] (
        [Id] nvarchar(450) not null,
        [UserName] nvarchar(256) null,
        [NormalizedUserName] nvarchar(256) null,
        [Email] nvarchar(256) null,
        [NormalizedEmail] nvarchar(256) null,
        [EmailConfirmed] bit not null,
        [PasswordHash] nvarchar(max) null,
        [SecurityStamp] nvarchar(max) null,
        [ConcurrencyStamp] nvarchar(max) null,
        [PhoneNumber] nvarchar(max) null,
        [PhoneNumberConfirmed] bit not null,
        [TwoFactorEnabled] bit not null,
        [LockoutEnd] datetimeoffset null,
        [LockoutEnabled] bit not null,
        [AccessFailedCount] int not null,
        [IsAdmin] bit not null constraint [DF_AspNetUsers_IsAdmin] default cast(0 as bit),
        constraint [PK_AspNetUsers] primary key ([Id])
    );

    create table [dbo].[AspNetRoleClaims] (
        [Id] int not null identity,
        [RoleId] nvarchar(450) not null,
        [ClaimType] nvarchar(max) null,
        [ClaimValue] nvarchar(max) null,
        constraint [PK_AspNetRoleClaims] primary key ([Id]),
        constraint [FK_AspNetRoleClaims_AspNetRoles_RoleId] foreign key ([RoleId]) references [dbo].[AspNetRoles] ([Id]) on delete cascade
    );

    create table [dbo].[AspNetUserClaims] (
        [Id] int not null identity,
        [UserId] nvarchar(450) not null,
        [ClaimType] nvarchar(max) null,
        [ClaimValue] nvarchar(max) null,
        constraint [PK_AspNetUserClaims] primary key ([Id]),
        constraint [FK_AspNetUserClaims_AspNetUsers_UserId] foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id]) on delete cascade
    );

    create table [dbo].[AspNetUserLogins] (
        [LoginProvider] nvarchar(450) not null,
        [ProviderKey] nvarchar(450) not null,
        [ProviderDisplayName] nvarchar(max) null,
        [UserId] nvarchar(450) not null,
        constraint [PK_AspNetUserLogins] primary key ([LoginProvider], [ProviderKey]),
        constraint [FK_AspNetUserLogins_AspNetUsers_UserId] foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id]) on delete cascade
    );

    create table [dbo].[AspNetUserRoles] (
        [UserId] nvarchar(450) not null,
        [RoleId] nvarchar(450) not null,
        constraint [PK_AspNetUserRoles] primary key ([UserId], [RoleId]),
        constraint [FK_AspNetUserRoles_AspNetRoles_RoleId] foreign key ([RoleId]) references [dbo].[AspNetRoles] ([Id]) on delete cascade,
        constraint [FK_AspNetUserRoles_AspNetUsers_UserId] foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id]) on delete cascade
    );

    create table [dbo].[AspNetUserTokens] (
        [UserId] nvarchar(450) not null,
        [LoginProvider] nvarchar(450) not null,
        [Name] nvarchar(450) not null,
        [Value] nvarchar(max) null,
        constraint [PK_AspNetUserTokens] primary key ([UserId], [LoginProvider], [Name]),
        constraint [FK_AspNetUserTokens_AspNetUsers_UserId] foreign key ([UserId]) references [dbo].[AspNetUsers] ([Id]) on delete cascade
    );

    create index [IX_AspNetRoleClaims_RoleId] on [dbo].[AspNetRoleClaims] ([RoleId]);
    create unique index [RoleNameIndex] on [dbo].[AspNetRoles] ([NormalizedName]) where [NormalizedName] is not null;
    create index [IX_AspNetUserClaims_UserId] on [dbo].[AspNetUserClaims] ([UserId]);
    create index [IX_AspNetUserLogins_UserId] on [dbo].[AspNetUserLogins] ([UserId]);
    create index [IX_AspNetUserRoles_RoleId] on [dbo].[AspNetUserRoles] ([RoleId]);
    create index [EmailIndex] on [dbo].[AspNetUsers] ([NormalizedEmail]);
    create unique index [UserNameIndex] on [dbo].[AspNetUsers] ([NormalizedUserName]) where [NormalizedUserName] is not null;

    if object_id(N'[dbo].[__EFMigrationsHistory]', N'U') is not null
    and not exists (select 1 from [dbo].[__EFMigrationsHistory] where [MigrationId] = N'20260321210500_AddIdentitySchema')
    begin
        insert into [dbo].[__EFMigrationsHistory] ([MigrationId], [ProductVersion])
        values (N'20260321210500_AddIdentitySchema', N'10.0.0');
    end
end

if object_id(N'[dbo].[AspNetUsers]', N'U') is not null
and col_length(N'[dbo].[AspNetUsers]', N'IsAdmin') is null
begin
    alter table [dbo].[AspNetUsers]
    add [IsAdmin] bit not null constraint [DF_AspNetUsers_IsAdmin] default cast(0 as bit);
end
""";

        await dbContext.Database.ExecuteSqlRawAsync(sql);
    }
}
