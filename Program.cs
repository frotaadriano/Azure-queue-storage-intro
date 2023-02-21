using Azure;
using Azure.Identity;
using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Threading.Tasks;

Console.WriteLine("=======================\n" +
    "Azure Queue Storage client library - .NET quickstart sample");

// Quickstart code goes here

// Create a unique name for the queue
string queueName = "afs-queue-" + Guid.NewGuid().ToString();
string storageAccountName = "azureexercisesstorageacc"; // TODO: Replace the <storage-account-name> placeholder 

// Instantiate a QueueClient to create and interact with the queue
QueueClient queueClient = new QueueClient(
    new Uri($"https://{storageAccountName}.queue.core.windows.net/{queueName}"),
    new DefaultAzureCredential());


Console.WriteLine($"=======================\nCreating queue: {queueName}");

// Create the queue
await queueClient.CreateAsync();


Console.WriteLine("\n=======================\nAdding messages to the queue...");

// Send several messages to the queue
await queueClient.SendMessageAsync("First message");
await queueClient.SendMessageAsync("Second message");

// Save the receipt so we can update this message later
SendReceipt receipt = await queueClient.SendMessageAsync("Third message");



Console.WriteLine("\n=======================\nPeek at the messages in the queue...");

// Peek at messages in the queue
PeekedMessage[] peekedMessages = await queueClient.PeekMessagesAsync(maxMessages: 10);

foreach (PeekedMessage peekedMessage in peekedMessages)
{
    // Display the message
    Console.WriteLine($"Message: {peekedMessage.MessageText}");
}

Console.WriteLine("\n=======================\nUpdating the third message in the queue...");

// Update a message using the saved receipt from sending the message
await queueClient.UpdateMessageAsync(receipt.MessageId, receipt.PopReceipt, "Third message has been updated");


Console.WriteLine("\n=======================\nReceiving messages from the queue...");

// Get messages from the queue
QueueMessage[] messages = await queueClient.ReceiveMessagesAsync(maxMessages: 10);

Console.WriteLine($"[qtd messages = {messages.Count()}");

foreach (var item in messages)
{
    Console.WriteLine($" item.Body: {item.Body}");
}
 