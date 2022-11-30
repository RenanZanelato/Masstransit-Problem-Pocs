# POC Claim Check Pattern

## Context
When we configured the MessageData, if the contract constructor it's not empty, will throw a exception in System.Text.Json
but, we are not using the System.Text.Json but Newtonsoft.Json configuration.

## How to Simulate

-> First configure in appsettings your azureServiceBus connection
-> Send a MessageDataEvent with a Contract that have a constructor not empty
-> When try to consume, will thrown a exception

## Exception Example
Deserialization of types without a parameterless constructor, a singular parameterized constructor, or a parameterized constructor annotated with 'JsonConstructorAttribute' is not supported. Type 'MessageDataContractEvent'. Path: $ | LineNumber: 0 | BytePositionInLine: 1. Deserialization of types without a parameterless constructor, a singular parameterized constructor, or a parameterized constructor annotated with 'JsonConstructorAttribute' is not supported. Type 'MessageDataContractEvent'. 

## Failed method	
MassTransit.MessageData.Converters.SystemTextJsonObjectMessageDataConverter`1+<Convert>d__2.MoveNext