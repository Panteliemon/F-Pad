using FPad.Interaction;
using System;
using System.IO;
using System.Collections.Generic;

namespace Tests.FPad;

public class InteractorTests
{
    [Fact]
    public void MemStruct_Write_Read_RoundTrip_WithEmptyLists()
    {
        // Arrange
        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>(),
            Messages = new List<MessageRec>()
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Empty(result.Instances);
        Assert.Empty(result.Messages);
    }

    [Fact]
    public void MemStruct_Write_Read_RoundTrip_WithNullLists()
    {
        // Arrange
        MemStruct memStruct = new MemStruct
        {
            Instances = null,
            Messages = null
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Empty(result.Instances);
        Assert.Empty(result.Messages);
    }

    [Fact]
    public void MemStruct_Write_Read_RoundTrip_WithSingleInstance()
    {
        // Arrange
        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>
            {
                new FPadRec
                {
                    Pid = 1234,
                    AcceptMessageEventName = "TestEvent",
                    CurrentDocumentFullPath = @"C:\Test\Document.txt"
                }
            },
            Messages = new List<MessageRec>()
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Single(result.Instances);
        Assert.Equal(1234, result.Instances[0].Pid);
        Assert.Equal("TestEvent", result.Instances[0].AcceptMessageEventName);
        Assert.Equal(@"C:\Test\Document.txt", result.Instances[0].CurrentDocumentFullPath);
        Assert.Empty(result.Messages);
    }

    [Fact]
    public void MemStruct_Write_Read_RoundTrip_WithSingleMessage()
    {
        // Arrange
        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>(),
            Messages = new List<MessageRec>
            {
                new MessageRec
                {
                    TargetPid = 5678,
                    MessageType = MessageType.Activate
                }
            }
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Empty(result.Instances);
        Assert.Single(result.Messages);
        Assert.Equal(5678, result.Messages[0].TargetPid);
        Assert.Equal(MessageType.Activate, result.Messages[0].MessageType);
    }

    [Fact]
    public void MemStruct_Write_Read_RoundTrip_WithMultipleInstancesAndMessages()
    {
        // Arrange
        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>
            {
                new FPadRec
                {
                    Pid = 1000,
                    AcceptMessageEventName = "Event1",
                    CurrentDocumentFullPath = @"C:\Path1\File1.txt"
                },
                new FPadRec
                {
                    Pid = 2000,
                    AcceptMessageEventName = "Event2",
                    CurrentDocumentFullPath = @"C:\Path2\File2.txt"
                },
                new FPadRec
                {
                    Pid = 3000,
                    AcceptMessageEventName = "Event3",
                    CurrentDocumentFullPath = @"C:\Path3\File3.txt"
                }
            },
            Messages = new List<MessageRec>
            {
                new MessageRec { TargetPid = 1000, MessageType = MessageType.Activate },
                new MessageRec { TargetPid = 2000, MessageType = MessageType.Activate }
            }
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Equal(3, result.Instances.Count);
        Assert.Equal(2, result.Messages.Count);

        // Verify instances
        Assert.Equal(1000, result.Instances[0].Pid);
        Assert.Equal("Event1", result.Instances[0].AcceptMessageEventName);
        Assert.Equal(@"C:\Path1\File1.txt", result.Instances[0].CurrentDocumentFullPath);

        Assert.Equal(2000, result.Instances[1].Pid);
        Assert.Equal("Event2", result.Instances[1].AcceptMessageEventName);
        Assert.Equal(@"C:\Path2\File2.txt", result.Instances[1].CurrentDocumentFullPath);

        Assert.Equal(3000, result.Instances[2].Pid);
        Assert.Equal("Event3", result.Instances[2].AcceptMessageEventName);
        Assert.Equal(@"C:\Path3\File3.txt", result.Instances[2].CurrentDocumentFullPath);

        // Verify messages
        Assert.Equal(1000, result.Messages[0].TargetPid);
        Assert.Equal(MessageType.Activate, result.Messages[0].MessageType);
        Assert.Equal(2000, result.Messages[1].TargetPid);
        Assert.Equal(MessageType.Activate, result.Messages[1].MessageType);
    }

    [Fact]
    public void MemStruct_Write_Read_RoundTrip_WithEmptyStrings()
    {
        // Arrange
        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>
            {
                new FPadRec
                {
                    Pid = 9999,
                    AcceptMessageEventName = "",
                    CurrentDocumentFullPath = ""
                }
            },
            Messages = new List<MessageRec>()
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Single(result.Instances);
        Assert.Equal(9999, result.Instances[0].Pid);
        Assert.Equal("", result.Instances[0].AcceptMessageEventName);
        Assert.Equal("", result.Instances[0].CurrentDocumentFullPath);
    }

    [Fact]
    public void MemStruct_Write_TruncatesInstances_WhenExceedingMaxLimit()
    {
        // Arrange - Create more than MAX_INSTANCES (1024)
        var instances = new List<FPadRec>();
        for (int i = 0; i < 1100; i++)
        {
            instances.Add(new FPadRec
            {
                Pid = i,
                AcceptMessageEventName = $"Event{i}",
                CurrentDocumentFullPath = $@"C:\Path{i}\File{i}.txt"
            });
        }

        MemStruct memStruct = new MemStruct
        {
            Instances = instances,
            Messages = new List<MessageRec>()
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Equal(1024, result.Instances.Count); // Should be truncated to MAX_INSTANCES
        Assert.Equal(0, result.Instances[0].Pid);
        Assert.Equal(1023, result.Instances[1023].Pid);
    }

    [Fact]
    public void MemStruct_Write_TruncatesMessages_WhenExceedingMaxLimit()
    {
        // Arrange - Create more than MAX_MESSAGES (2048)
        var messages = new List<MessageRec>();
        for (int i = 0; i < 2100; i++)
        {
            messages.Add(new MessageRec
            {
                TargetPid = i,
                MessageType = MessageType.Activate
            });
        }

        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>(),
            Messages = messages
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Equal(2048, result.Messages.Count); // Should be truncated to MAX_MESSAGES
        Assert.Equal(0, result.Messages[0].TargetPid);
        Assert.Equal(2047, result.Messages[2047].TargetPid);
    }

    [Fact]
    public void MemStruct_Read_ThrowsException_WhenSignatureIsInvalid()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write(0x12345678); // Invalid signature
        writer.Write(0); // instances count
        writer.Write(0); // messages count

        ms.Position = 0;
        using var reader = new BinaryReader(ms);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => MemStruct.Read(reader));
    }

    [Fact]
    public void MemStruct_Read_ThrowsException_WhenInstancesCountIsNegative()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write(0x06060600); // Valid signature
        writer.Write(-1); // Invalid instances count

        ms.Position = 0;
        using var reader = new BinaryReader(ms);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => MemStruct.Read(reader));
    }

    [Fact]
    public void MemStruct_Read_ThrowsException_WhenInstancesCountExceedsMax()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write(0x06060600); // Valid signature
        writer.Write(1025); // Exceeds MAX_INSTANCES (1024)

        ms.Position = 0;
        using var reader = new BinaryReader(ms);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => MemStruct.Read(reader));
    }

    [Fact]
    public void MemStruct_Read_ThrowsException_WhenMessagesCountIsNegative()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write(0x06060600); // Valid signature
        writer.Write(0); // Valid instances count
        writer.Write(-1); // Invalid messages count

        ms.Position = 0;
        using var reader = new BinaryReader(ms);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => MemStruct.Read(reader));
    }

    [Fact]
    public void MemStruct_Read_ThrowsException_WhenMessagesCountExceedsMax()
    {
        // Arrange
        using var ms = new MemoryStream();
        using var writer = new BinaryWriter(ms);

        writer.Write(0x06060600); // Valid signature
        writer.Write(0); // Valid instances count
        writer.Write(2049); // Exceeds MAX_MESSAGES (2048)

        ms.Position = 0;
        using var reader = new BinaryReader(ms);

        // Act & Assert
        Assert.Throws<ApplicationException>(() => MemStruct.Read(reader));
    }

    [Fact]
    public void MemStruct_Write_Read_PreservesDataIntegrity()
    {
        // Arrange - Test with special characters and edge cases
        MemStruct memStruct = new MemStruct
        {
            Instances = new List<FPadRec>
            {
                new FPadRec
                {
                    Pid = int.MaxValue,
                    AcceptMessageEventName = "Special!@#$%^&*()_+-=[]{}|;':\",./<>?",
                    CurrentDocumentFullPath = @"C:\Test\Файл.txt" // Unicode characters
                },
                new FPadRec
                {
                    Pid = int.MinValue,
                    AcceptMessageEventName = "ñ∂ƒ©˙∆˚¬",
                    CurrentDocumentFullPath = @"\\network\path\文件.txt"
                }
            },
            Messages = new List<MessageRec>
            {
                new MessageRec { TargetPid = int.MaxValue, MessageType = MessageType.Activate },
                new MessageRec { TargetPid = 0, MessageType = MessageType.Activate },
                new MessageRec { TargetPid = int.MinValue, MessageType = MessageType.Activate }
            }
        };

        // Act & Assert
        MemStruct result = WriteAndRead(memStruct);

        Assert.NotNull(result);
        Assert.Equal(2, result.Instances.Count);
        Assert.Equal(3, result.Messages.Count);

        Assert.Equal(int.MaxValue, result.Instances[0].Pid);
        Assert.Equal("Special!@#$%^&*()_+-=[]{}|;':\",./<>?", result.Instances[0].AcceptMessageEventName);
        Assert.Equal(@"C:\Test\Файл.txt", result.Instances[0].CurrentDocumentFullPath);

        Assert.Equal(int.MinValue, result.Instances[1].Pid);
        Assert.Equal("ñ∂ƒ©˙∆˚¬", result.Instances[1].AcceptMessageEventName);
        Assert.Equal(@"\\network\path\文件.txt", result.Instances[1].CurrentDocumentFullPath);

        Assert.Equal(int.MaxValue, result.Messages[0].TargetPid);
        Assert.Equal(0, result.Messages[1].TargetPid);
        Assert.Equal(int.MinValue, result.Messages[2].TargetPid);
    }

    // Helper method to write and read MemStruct
    private static MemStruct WriteAndRead(MemStruct memStruct)
    {
        using var ms = new MemoryStream();
        using (var writer = new BinaryWriter(ms, System.Text.Encoding.UTF8, leaveOpen: true))
        {
            memStruct.Write(writer);
        }

        ms.Position = 0;

        using var reader = new BinaryReader(ms);
        return MemStruct.Read(reader);
    }
}