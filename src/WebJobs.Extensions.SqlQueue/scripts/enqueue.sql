-- Copyright (c) .NET Foundation. All rights reserved.
-- Licensed under the MIT License. See License.txt in the project root for license information.

USE [SqlQueue]
GO

BEGIN TRANSACTION

DECLARE @conversationID UNIQUEIDENTIFIER

BEGIN DIALOG @conversationID
              FROM SERVICE [SenderService]
              TO SERVICE 'ReceiverService'
			  ON CONTRACT MyContract
              WITH ENCRYPTION = OFF;

SEND ON CONVERSATION @conversationID MESSAGE TYPE [MyMessageType] ('Message #1');
SEND ON CONVERSATION @conversationID MESSAGE TYPE [MyMessageType] ('Message #2');
SEND ON CONVERSATION @conversationID MESSAGE TYPE [MyMessageType] ('Message #3');
SEND ON CONVERSATION @conversationID MESSAGE TYPE [MyMessageType] ('Message #4');
SEND ON CONVERSATION @conversationID MESSAGE TYPE [MyMessageType] ('Message #5');

-- This is optional. This adds a NULL message
END CONVERSATION @conversationID

COMMIT TRANSACTION
