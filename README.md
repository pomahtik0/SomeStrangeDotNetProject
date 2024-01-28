# Project to solve the task ==insert later==.

Program connects to **local database** by user-entered path to .mdf(mssql). Than upload new trees to database using .json or .txt files as well as browse them in a custom-made treeview.

Installed packeges:
- System.Data.SqlClient by Microsoft

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
![img](/Images-for-github/tree-data-in-db.png)![img](/Images-for-github/data-looks-in-db.png) 

## some examples
all the examples show navigation through tree as in this [JSON](/Json-examples/someComplexJson.json)

### The treelooks like:
![img](/Images-for-github/loading-tree-by-name.png)

### Navigation through tree:
/*tree name*/*tree keys*
![1](/Images-for-github/navigating-to-tree-leaf.png)
![2](/Images-for-github/navigating-to-tree-subroote.png)

### Navigation through array:
![img](/Images-for-github/navigation-through-array.png)

### Navigation in short variant:
![img](/Images-for-github/example-of-short-variant-tree-navigation.png)
