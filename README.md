# Project to solve the task ==insert later==.

## solved tasks:
- [x] Database developed
- [x] Read tree from .json and save it to DB
- [x] Read from DB as tree
- [x] Bonus 1
- [x] Bonus 2
- [ ] Bonus 3

## using the next database structure (mssql):
```
CREATE TABLE [dbo].[Objects] (
    [Id]        INT           IDENTITY (1, 1) NOT NULL,
    [Key]       VARCHAR (120) NULL,
    [Value]     VARCHAR (255) NULL,
    [Separator] VARCHAR (7)   NOT NULL,
    [Parent_id] INT           NULL,
    [Tree_id]   INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [Selfrefference_FK] FOREIGN KEY ([Parent_id]) REFERENCES [dbo].[Objects] ([Id]),
    CONSTRAINT [FK_ToRootTable] FOREIGN KEY ([Tree_id]) REFERENCES [dbo].[Trees] ([Id]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Trees] (
    [Id]      INT           IDENTITY (1, 1) NOT NULL,
    [Name]    VARCHAR (100) NOT NULL,
    [Root_id] INT           NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Trees_ToObjects] FOREIGN KEY ([Root_id]) REFERENCES [dbo].[Objects] ([Id])
);
```
### Data looks in DB:
![data](/Images-for-github/tree-data-in-db.png)![data](/Images-for-github/data-looks-in-db.png) 

## some examples
