-- Copyright (c) .NET Foundation. All rights reserved.
-- Licensed under the MIT License. See License.txt in the project root for license information.

USE [SqlQueue]
GO

BEGIN TRANSACTION

WAITFOR (RECEIVE TOP(1) CONVERT(VarChar(MAX), message_body) AS MyContract FROM [MyQueue]), TIMEOUT 2000

COMMIT TRANSACTION

-- Read all messages. This will block if a transaction is in progress
-- SELECT CONVERT(VarChar(MAX), message_body), status from MyQueue

-- WAITFOR (RECEIVE TOP(1) CONVERT(VarChar(Max), message_body) AS MyContract FROM MyQueue), TIMEOUT 10000


