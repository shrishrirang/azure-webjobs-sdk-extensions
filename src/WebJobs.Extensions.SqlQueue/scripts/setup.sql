-- Copyright (c) .NET Foundation. All rights reserved.
-- Licensed under the MIT License. See License.txt in the project root for license information.

USE [SqlQueue]
GO

ALTER DATABASE [SqlQueue] SET ENABLE_BROKER
GO

CREATE MESSAGE TYPE [MyMessageType] VALIDATION = NONE
GO

CREATE CONTRACT [MyContract] ([MyMessageType] SENT BY INITIATOR)
GO

CREATE QUEUE [SenderQueue] 
GO

-- Retention Off
CREATE QUEUE [MyQueue] WITH RETENTION = OFF
GO

-- Retention On
CREATE QUEUE [MyQueue2] WITH RETENTION = ON
GO

CREATE SERVICE [SenderService] ON QUEUE [SenderQueue] 
GO

CREATE SERVICE [ReceiverService] ON QUEUE [MyQueue] (MyContract)
GO

CREATE SERVICE [ReceiverService2] ON QUEUE [MyQueue2] (MyContract)
GO

-- GRANT RECEIVE ON [MyQueue] TO [sa]
-- GO

-- GRANT RECEIVE ON [MyQueue2] TO [sa]
-- GO

