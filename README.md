# EverythingAPI
I built this API using Data Access Layers to deepen my understanding of SQL and to practice writing APIs. The goal was to create a well-structured, scalable API that interacts with a SQL database, allowing for efficient data management and manipulation. This project helped me improve my skills in database design, SQL queries, and API development.

## Endpoints

### Board
#### POST /api/Board
**Beschrijving**: Creates a new board
- **Param**:
  - `name` (string): Name of the board
  - `userId` (int): ID of the user that makes the board

#### DELETE /api/Board/{boardId}
**Beschrijving**: Delete board on boardId
- **Param**:
  - `boardId` (int): ID of the board that gets deleted

---

### Item
#### POST /api/Item
**Beschrijving**: Creates a new item on the board.
- **Param**:
  - `itemName` (string): Name of the item
  - `itemDescription` (string): description of the item
  - `statusId` (int): status id
  - `boardId` (int): board ID of the board that the item is assigned to

#### PUT /api/Item
**Beschrijving**: changes the current status od the item
- **Param**:
  - `itemId` (int): ID of the item
  - `itemStatusId` (int): New status-ID for item

#### DELETE /api/Item/{itemId}
**Beschrijving**: Deletes an item.
- **Param**:
  - `itemId` (int): ID of the deleted item

---

### User
#### GET /api/User
**Beschrijving**: Get all users

#### POST /api/User
**Beschrijving**: Create a new user.
- **Param**:
  - `userName` (string): Name of the user
  - `userEmail` (string): E-mail of the user

#### GET /api/User/{email}
**Beschrijving**: Get a user with sepcific email.
- **Param**:
  - `email` (string): Email of user

#### DELETE /api/User/{userId}
**Beschrijving**: Delete user.
- **Param**:
  - `userId` (int): ID of the user about to be deleted



## Database setup
![erd](https://github.com/user-attachments/assets/e0d8229f-9284-49c3-85ed-eee98088addc)

### Create tables
```sql
USE {DatabaseName};

CREATE TABLE Users (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(100) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL
);

ALTER TABLE Users
ADD CONSTRAINT EmailCheck CHECK (Email LIKE '_%@_%');

CREATE TABLE Board (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    BoardName NVARCHAR(100) NOT NULL,
    UserID INT NOT NULL
);

ALTER TABLE Board
ADD CONSTRAINT FK_Board_User FOREIGN KEY (UserID) REFERENCES Users(ID)
ON DELETE CASCADE;

CREATE TABLE StatusItem (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    StatusName NVARCHAR(100) NOT NULL
);

CREATE TABLE Items (
    ID INT IDENTITY(1,1) PRIMARY KEY,
    ItemName NVARCHAR(100) NOT NULL,
    ItemDescription NVARCHAR(500) NULL,
    StatusID INT NOT NULL,
    BoardID INT NOT NULL
);

ALTER TABLE Items
ADD CONSTRAINT FK_Item_Board FOREIGN KEY (BoardID) REFERENCES Board(ID)
ON DELETE CASCADE;

ALTER TABLE Items
ADD CONSTRAINT FK_Item_StatusItem FOREIGN KEY (StatusID) REFERENCES StatusItem(ID);
```

### Add data to tables
```sql
USE {DatabaseName};

INSERT INTO StatusItem (StatusName)
VALUES
('Backlog'),
('ToDo'),
('Working'),
('Done');


INSERT INTO Users (UserName, Email)
VALUES
('TestUser', 'testuser@testing.com');


INSERT INTO Board (BoardName, UserID)
VALUES
('BoardNr1', 1);


INSERT INTO Items (ItemName, ItemDescription, StatusID, BoardID)
VALUES
('FinishThisRND', 'FinishThisMess', 2, 1);
```
