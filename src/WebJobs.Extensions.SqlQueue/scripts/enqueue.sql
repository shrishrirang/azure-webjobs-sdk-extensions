-- Copyright (c) .NET Foundation. All rights reserved.
-- Licensed under the MIT License. See License.txt in the project root for license information.

USE [webjobs]
GO

BEGIN TRANSACTION

DECLARE @conversationID UNIQUEIDENTIFIER

BEGIN DIALOG @conversationID
              FROM SERVICE SENDERSERVICE
              TO SERVICE 'RECEIVERSERVICE1'
			  ON CONTRACT C
              WITH ENCRYPTION = OFF;

SEND ON CONVERSATION @conversationID MESSAGE TYPE [MT] ('Dummy Message1');

SEND ON CONVERSATION @conversationID MESSAGE TYPE [MT] ('Dummy Message2');

-- This is optional. This adds a NULL message
END CONVERSATION @conversationID

COMMIT TRANSACTION
