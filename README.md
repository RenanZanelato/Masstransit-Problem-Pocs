# Example the problem Happening on MassTransit 8.0.7
- Before this, we was using MassTransit 7.3.1
- Its not a error, because this queues not receive any message or any messages go to deadletter. But, the problem is that ALL queues will receive a different configuration. So these queues cannot be created automatically

# What Happens
- using Azure Service Bus
- We only configured the Topics (not the queues) but, when the application starts will create QUEUES that was not configured

# How To Configure
- Open the file appsettings.json, edit the json AzureServiceBus with your connection


# Example that queues that can't be configured
- SampleCreatedEvent, SampleDeletedEvent, SampleUpdatedEvent
Only could be configured/create the topics with the names

- sbt-example-sample-SampleCreatedEvent
- sbt-example-sample-SampleUpdatedEvent
- sbt-example-sample-SampleDeletedEvent